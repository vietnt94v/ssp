import { useQuery } from '@tanstack/react-query';
import { queryKeys } from '@/lib/query-keys';
import {
  getTechnician,
  getTechnicianWorkload,
  getTechnicians,
} from '../api/technicians-api';

export function useTechnicians(params?: { page?: number; pageSize?: number }) {
  return useQuery({
    queryKey: queryKeys.technicians.list(params),
    queryFn: () => getTechnicians(params),
  });
}

export function useTechnician(id: string) {
  return useQuery({
    queryKey: queryKeys.technicians.detail(id),
    queryFn: () => getTechnician(id),
    enabled: Boolean(id),
  });
}

export function useTechnicianWorkload(id: string) {
  return useQuery({
    queryKey: queryKeys.technicians.workload(id),
    queryFn: () => getTechnicianWorkload(id),
    enabled: Boolean(id),
  });
}
