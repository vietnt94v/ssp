import axios from 'axios';
import type { AxiosError, InternalAxiosRequestConfig } from 'axios';
import { toast } from 'sonner';
import type { ApiError } from '@/types';
import { env } from '@/lib/env';

export const apiClient = axios.create({
  baseURL: env.apiUrl,
  headers: { 'Content-Type': 'application/json' },
  withCredentials: true,
});

type RetriableConfig = InternalAxiosRequestConfig & { _retry?: boolean };

const REFRESH_PATH = '/auth/refresh-token';
const NO_REFRESH_PATHS = ['/auth/login', '/auth/logout', REFRESH_PATH];

let isRefreshing = false;
let refreshWaiters: Array<{
  resolve: () => void;
  reject: (error: unknown) => void;
}> = [];

function flushWaiters(error: unknown) {
  const waiters = refreshWaiters;
  refreshWaiters = [];
  for (const waiter of waiters) {
    if (error) {
      waiter.reject(error);
    } else {
      waiter.resolve();
    }
  }
}

let onAuthFailure: (() => void) | null = null;

export function setAuthFailureHandler(handler: () => void) {
  onAuthFailure = handler;
}

async function refreshSession(): Promise<void> {
  await apiClient.post(REFRESH_PATH);
}

apiClient.interceptors.response.use(
  (response) => response,
  async (error: AxiosError<ApiError>) => {
    const status = error.response?.status;
    const original = error.config as RetriableConfig | undefined;

    const isAuthEndpoint =
      original?.url != null &&
      NO_REFRESH_PATHS.some((path) => original.url?.includes(path));

    if (status === 401 && original && !original._retry && !isAuthEndpoint) {
      original._retry = true;

      if (isRefreshing) {
        try {
          await new Promise<void>((resolve, reject) => {
            refreshWaiters.push({ resolve, reject });
          });
          return apiClient(original);
        } catch (queueError) {
          return Promise.reject(queueError);
        }
      }

      isRefreshing = true;
      try {
        await refreshSession();
        flushWaiters(null);
        return apiClient(original);
      } catch (refreshError) {
        flushWaiters(refreshError);
        onAuthFailure?.();
        return Promise.reject(refreshError);
      } finally {
        isRefreshing = false;
      }
    }

    if (status === 403) {
      toast.error('You do not have permission to perform this action.');
    } else if (status && status >= 500) {
      toast.error('A server error occurred. Please try again.');
    } else if (status !== 401) {
      const message = error.response?.data?.error ?? error.message;
      if (message) toast.error(message);
    }

    return Promise.reject(error);
  },
);
