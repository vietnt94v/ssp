import { apiClient } from '@/lib/api-client';
import type { Alert, PaginatedResponse } from '@/types';

export async function getAlerts(params?: {
  acknowledged?: boolean;
  page?: number;
  pageSize?: number;
}) {
  const { data } = await apiClient.get<PaginatedResponse<Alert>>('/alerts', {
    params,
  });
  return data;
}

export async function acknowledgeAlert(id: string) {
  const { data } = await apiClient.patch<Alert>(`/alerts/${id}/acknowledge`);
  return data;
}

export async function createWorkOrderFromAlert(id: string) {
  const { data } = await apiClient.post<{ workOrderId: string }>(
    `/alerts/${id}/create-wo`,
  );
  return data;
}
