import { apiClient } from '@/lib/api-client';
import type { PaginatedResponse, Technician } from '@/types';

export interface TechnicianWorkload {
  technicianId: string;
  openWorkOrderCount: number;
  workloadPercent: number;
  byStatus: Record<string, number>;
}

export async function getTechnicians(params?: {
  page?: number;
  pageSize?: number;
}) {
  const { data } = await apiClient.get<PaginatedResponse<Technician>>(
    '/technicians',
    { params },
  );
  return data;
}

export async function getTechnician(id: string) {
  const { data } = await apiClient.get<Technician>(`/technicians/${id}`);
  return data;
}

export async function getTechnicianWorkload(id: string) {
  const { data } = await apiClient.get<TechnicianWorkload>(
    `/technicians/${id}/workload`,
  );
  return data;
}
