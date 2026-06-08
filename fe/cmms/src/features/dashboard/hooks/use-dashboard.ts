import { useQuery } from '@tanstack/react-query';
import { queryKeys } from '@/lib/query-keys';
import {
  getDashboardKpi,
  getStatusBreakdown,
  getUrgentWorkOrders,
  getWorkOrderTrend,
} from '../api/dashboard-api';

export function useDashboardKpi() {
  return useQuery({
    queryKey: queryKeys.dashboard.kpi,
    queryFn: getDashboardKpi,
  });
}

export function useWorkOrderTrend(days = 30) {
  return useQuery({
    queryKey: queryKeys.dashboard.trend(days),
    queryFn: () => getWorkOrderTrend(days),
  });
}

export function useUrgentWorkOrders() {
  return useQuery({
    queryKey: queryKeys.dashboard.urgent,
    queryFn: getUrgentWorkOrders,
  });
}

export function useStatusBreakdown() {
  return useQuery({
    queryKey: queryKeys.dashboard.statusBreakdown,
    queryFn: getStatusBreakdown,
  });
}
