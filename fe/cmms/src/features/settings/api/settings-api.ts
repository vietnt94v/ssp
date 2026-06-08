import { apiClient } from '@/lib/api-client';
import type { User } from '@/types';

export async function getUsers() {
  const { data } = await apiClient.get<User[]>('/settings/users');
  return data;
}
