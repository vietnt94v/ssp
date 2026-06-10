import { useEffect } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { toast } from 'sonner';
import {
  startHubConnection,
  stopHubConnection,
} from '@/lib/signalr-client';
import { queryKeys } from '@/lib/query-keys';
import { useAuthStore } from '@/stores/auth-store';
import { useNotificationStore } from '@/stores/notification-store';
import type { Alert, WorkOrder } from '@/types';

export function useRealtime() {
  const queryClient = useQueryClient();
  const user = useAuthStore((s) => s.user);
  const incrementUnread = useNotificationStore((s) => s.increment);

  useEffect(() => {
    if (!user) return;

    let active = true;

    startHubConnection()
      .then((conn) => {
        if (!active) return;

        conn.on('WorkOrderStatusChanged', (workOrder: WorkOrder) => {
          queryClient.invalidateQueries({ queryKey: queryKeys.workOrders.all });
          queryClient.invalidateQueries({ queryKey: queryKeys.dashboard.kpi });
          toast.info(
            `Work order ${workOrder.number} is now ${workOrder.status}.`,
          );
        });

        conn.on('AlertCreated', (alert: Alert) => {
          queryClient.invalidateQueries({ queryKey: queryKeys.alerts.all });
          incrementUnread();
          toast.warning(alert.message);
        });

        conn.on('DashboardKpiUpdated', () => {
          queryClient.invalidateQueries({ queryKey: queryKeys.dashboard.kpi });
        });
      })
      .catch(() => {
        // connection errors are non-fatal; UI still works via polling/refetch
      });

    return () => {
      active = false;
      void stopHubConnection();
    };
  }, [user, queryClient, incrementUnread]);
}
