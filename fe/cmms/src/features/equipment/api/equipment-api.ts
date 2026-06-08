import { apiClient } from '@/lib/api-client';
import type { Equipment, PaginatedResponse } from '@/types';

export interface EquipmentListParams {
  status?: string;
  categoryId?: string;
  locationId?: string;
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortDir?: 'asc' | 'desc';
}

export interface EquipmentInput {
  code: string;
  name: string;
  categoryId: string;
  locationId: string;
  manufacturer?: string | null;
  installDate?: string | null;
  status: string;
}

export async function getEquipmentList(params: EquipmentListParams) {
  const { data } = await apiClient.get<PaginatedResponse<Equipment>>(
    '/equipment',
    { params },
  );
  return data;
}

export async function getEquipment(id: string) {
  const { data } = await apiClient.get<Equipment>(`/equipment/${id}`);
  return data;
}

export async function createEquipment(input: EquipmentInput) {
  const { data } = await apiClient.post<Equipment>('/equipment', input);
  return data;
}

export async function updateEquipment(id: string, input: EquipmentInput) {
  const { data } = await apiClient.put<Equipment>(`/equipment/${id}`, input);
  return data;
}

export async function deleteEquipment(id: string) {
  await apiClient.delete(`/equipment/${id}`);
}
