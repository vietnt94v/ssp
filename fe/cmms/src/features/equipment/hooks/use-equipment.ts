import {
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query';
import { toast } from 'sonner';
import { queryKeys } from '@/lib/query-keys';
import {
  createEquipment,
  deleteEquipment,
  getEquipment,
  getEquipmentList,
  updateEquipment,
} from '../api/equipment-api';
import type {
  EquipmentInput,
  EquipmentListParams,
} from '../api/equipment-api';

export function useEquipmentList(params: EquipmentListParams) {
  return useQuery({
    queryKey: queryKeys.equipment.list(params),
    queryFn: () => getEquipmentList(params),
  });
}

export function useEquipment(id: string) {
  return useQuery({
    queryKey: queryKeys.equipment.detail(id),
    queryFn: () => getEquipment(id),
    enabled: Boolean(id),
  });
}

export function useCreateEquipment() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (input: EquipmentInput) => createEquipment(input),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: queryKeys.equipment.all });
      toast.success('Equipment created');
    },
  });
}

export function useUpdateEquipment(id: string) {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (input: EquipmentInput) => updateEquipment(id, input),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: queryKeys.equipment.all });
      qc.invalidateQueries({ queryKey: queryKeys.equipment.detail(id) });
      toast.success('Equipment updated');
    },
  });
}

export function useDeleteEquipment() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => deleteEquipment(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: queryKeys.equipment.all });
      toast.success('Equipment deleted');
    },
  });
}
