import { apiClient } from '@/lib/api-client';
import type { Category, Location } from '@/types';

export async function getCategories() {
  const { data } = await apiClient.get<Category[]>('/settings/categories');
  return data;
}

export async function getLocations() {
  const { data } = await apiClient.get<Location[]>('/settings/locations');
  return data;
}
