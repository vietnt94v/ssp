import axios from 'axios';
import type { AxiosError, InternalAxiosRequestConfig } from 'axios';
import { toast } from 'sonner';
import type { ApiError } from '@/types';
import { useAuthStore } from '@/stores/auth-store';
import { env } from '@/lib/env';

export const apiClient = axios.create({
  baseURL: env.apiUrl,
  headers: { 'Content-Type': 'application/json' },
});

apiClient.interceptors.request.use((config: InternalAxiosRequestConfig) => {
  const token = useAuthStore.getState().token;
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

apiClient.interceptors.response.use(
  (response) => response,
  (error: AxiosError<ApiError>) => {
    const status = error.response?.status;

    if (status === 401) {
      useAuthStore.getState().logout();
      if (window.location.pathname !== '/auth/login') {
        window.location.assign('/auth/login');
      }
    } else if (status === 403) {
      toast.error('You do not have permission to perform this action.');
    } else if (status && status >= 500) {
      toast.error('A server error occurred. Please try again.');
    } else {
      const message = error.response?.data?.error ?? error.message;
      if (message) toast.error(message);
    }

    return Promise.reject(error);
  },
);
