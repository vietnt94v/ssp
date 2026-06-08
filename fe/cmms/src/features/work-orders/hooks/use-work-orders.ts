import {
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query';
import { toast } from 'sonner';
import { queryKeys } from '@/lib/query-keys';
import type { WorkOrder, WorkOrderStatus } from '@/types';
import {
  changeWorkOrderStatus,
  createWorkOrder,
  deleteWorkOrder,
  getWorkOrder,
  getWorkOrders,
  getWorkOrdersKanban,
  updateWorkOrder,
} from '../api/work-orders-api';
import type {
  WorkOrderInput,
  WorkOrderListParams,
} from '../api/work-orders-api';

export function useWorkOrders(params: WorkOrderListParams) {
  return useQuery({
    queryKey: queryKeys.workOrders.list(params),
    queryFn: () => getWorkOrders(params),
  });
}

export function useWorkOrdersKanban(technicianId?: string) {
  return useQuery({
    queryKey: queryKeys.workOrders.kanban(technicianId),
    queryFn: () => getWorkOrdersKanban(technicianId),
  });
}

export function useWorkOrder(id: string) {
  return useQuery({
    queryKey: queryKeys.workOrders.detail(id),
    queryFn: () => getWorkOrder(id),
    enabled: Boolean(id),
  });
}

export function useCreateWorkOrder() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (input: WorkOrderInput) => createWorkOrder(input),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: queryKeys.workOrders.all });
      toast.success('Work order created');
    },
  });
}

export function useUpdateWorkOrder(id: string) {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (input: WorkOrderInput) => updateWorkOrder(id, input),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: queryKeys.workOrders.all });
      qc.invalidateQueries({ queryKey: queryKeys.workOrders.detail(id) });
      toast.success('Work order updated');
    },
  });
}

export function useChangeWorkOrderStatus(id: string) {
  const qc = useQueryClient();
  const detailKey = queryKeys.workOrders.detail(id);

  return useMutation({
    mutationFn: (status: WorkOrderStatus) => changeWorkOrderStatus(id, status),
    onMutate: async (status) => {
      await qc.cancelQueries({ queryKey: detailKey });
      const previous = qc.getQueryData<WorkOrder>(detailKey);
      if (previous) {
        qc.setQueryData<WorkOrder>(detailKey, { ...previous, status });
      }
      return { previous };
    },
    onError: (_err, _status, context) => {
      if (context?.previous) {
        qc.setQueryData(detailKey, context.previous);
      }
    },
    onSettled: () => {
      qc.invalidateQueries({ queryKey: queryKeys.workOrders.all });
      qc.invalidateQueries({ queryKey: queryKeys.dashboard.kpi });
    },
  });
}

export function useDeleteWorkOrder() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => deleteWorkOrder(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: queryKeys.workOrders.all });
      toast.success('Work order deleted');
    },
  });
}
