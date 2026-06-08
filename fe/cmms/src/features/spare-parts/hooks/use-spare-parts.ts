import {
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query';
import { toast } from 'sonner';
import { queryKeys } from '@/lib/query-keys';
import {
  createSparePart,
  getSparePart,
  getSpareParts,
  updateSparePart,
} from '../api/spare-parts-api';
import type { SparePartInput } from '../api/spare-parts-api';

export function useSpareParts(params?: { page?: number; pageSize?: number }) {
  return useQuery({
    queryKey: queryKeys.spareParts.list(params),
    queryFn: () => getSpareParts(params),
  });
}

export function useSparePart(id: string) {
  return useQuery({
    queryKey: queryKeys.spareParts.detail(id),
    queryFn: () => getSparePart(id),
    enabled: Boolean(id),
  });
}

export function useCreateSparePart() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (input: SparePartInput) => createSparePart(input),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: queryKeys.spareParts.all });
      toast.success('Spare part created');
    },
  });
}

export function useUpdateSparePart(id: string) {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (input: SparePartInput) => updateSparePart(id, input),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: queryKeys.spareParts.all });
      toast.success('Spare part updated');
    },
  });
}
