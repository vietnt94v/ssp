import { apiClient } from '@/lib/api-client';
import type { PaginatedResponse, SparePart } from '@/types';

export interface SparePartInput {
  code: string;
  name: string;
  unitCost: number;
  stockQuantity: number;
  reorderLevel: number;
}

export async function getSpareParts(params?: {
  page?: number;
  pageSize?: number;
}) {
  const { data } = await apiClient.get<PaginatedResponse<SparePart>>(
    '/spare-parts',
    { params },
  );
  return data;
}

export async function getSparePart(id: string) {
  const { data } = await apiClient.get<SparePart>(`/spare-parts/${id}`);
  return data;
}

export async function createSparePart(input: SparePartInput) {
  const { data } = await apiClient.post<SparePart>('/spare-parts', input);
  return data;
}

export async function updateSparePart(id: string, input: SparePartInput) {
  const { data } = await apiClient.put<SparePart>(`/spare-parts/${id}`, input);
  return data;
}
