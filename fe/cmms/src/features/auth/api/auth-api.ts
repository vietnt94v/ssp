import { apiClient } from '@/lib/api-client';
import type { Role, User } from '@/types';

export interface AuthUserResponse {
  id: string;
  email: string;
  fullName: string;
  role: Role;
}

export async function login(email: string, password: string): Promise<User> {
  const { data } = await apiClient.post<AuthUserResponse>('/auth/login', {
    email,
    password,
  });
  return toUser(data);
}

export async function getMe(): Promise<User> {
  const { data } = await apiClient.get<AuthUserResponse>('/auth/me');
  return toUser(data);
}

export async function logout(): Promise<void> {
  await apiClient.post('/auth/logout');
}

export async function forgotPassword(email: string): Promise<void> {
  await apiClient.post('/auth/forgot-password', { email });
}

export function toUser(response: AuthUserResponse): User {
  return {
    id: response.id,
    email: response.email,
    fullName: response.fullName,
    role: response.role,
  };
}
