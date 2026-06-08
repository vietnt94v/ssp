import { apiClient } from '@/lib/api-client';
import type { Role, User } from '@/types';

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
  user: {
    id: string;
    email: string;
    fullName: string;
    role: Role;
  };
}

export async function login(email: string, password: string) {
  const { data } = await apiClient.post<LoginResponse>('/auth/login', {
    email,
    password,
  });
  return data;
}

export async function forgotPassword(email: string) {
  await apiClient.post('/auth/forgot-password', { email });
}

export function toUser(response: LoginResponse): User {
  return {
    id: response.user.id,
    email: response.user.email,
    fullName: response.user.fullName,
    role: response.user.role,
  };
}
