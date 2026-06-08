import {
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query';
import { toast } from 'sonner';
import { queryKeys } from '@/lib/query-keys';
import {
  acknowledgeAlert,
  createWorkOrderFromAlert,
  getAlerts,
} from '../api/alerts-api';
import { useNotificationStore } from '@/stores/notification-store';

export function useAlerts(params?: {
  acknowledged?: boolean;
  page?: number;
  pageSize?: number;
}) {
  return useQuery({
    queryKey: queryKeys.alerts.list(params),
    queryFn: () => getAlerts(params),
  });
}

export function useAcknowledgeAlert() {
  const qc = useQueryClient();
  const decrement = useNotificationStore((s) => s.decrement);
  return useMutation({
    mutationFn: (id: string) => acknowledgeAlert(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: queryKeys.alerts.all });
      decrement();
      toast.success('Alert acknowledged');
    },
  });
}

export function useCreateWoFromAlert() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => createWorkOrderFromAlert(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: queryKeys.alerts.all });
      qc.invalidateQueries({ queryKey: queryKeys.workOrders.all });
      toast.success('Work order created from alert');
    },
  });
}
