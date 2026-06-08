import { apiClient } from '@/lib/api-client';
import type {
  DashboardKpi,
  WorkOrderStatusCount,
  WorkOrderTrendPoint,
} from '@/types';
import type { WorkOrderListItem } from '@/features/work-orders/api/work-orders-api';

export async function getDashboardKpi() {
  const { data } = await apiClient.get<DashboardKpi>('/dashboard/kpi');
  return data;
}

export async function getWorkOrderTrend(days = 30) {
  const { data } = await apiClient.get<WorkOrderTrendPoint[]>(
    '/dashboard/wo-trend',
    { params: { days } },
  );
  return data;
}

export async function getUrgentWorkOrders() {
  const { data } = await apiClient.get<WorkOrderListItem[]>(
    '/dashboard/urgent-wo',
  );
  return data;
}

export async function getStatusBreakdown() {
  const { data } = await apiClient.get<WorkOrderStatusCount[]>(
    '/dashboard/status-breakdown',
  );
  return data;
}
