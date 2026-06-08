import { useQuery } from '@tanstack/react-query';
import { queryKeys } from '@/lib/query-keys';
import {
  getCostReport,
  getDowntimeReport,
  getEquipmentHistory,
  getKpiReport,
} from '../api/reports-api';

interface DateRange {
  from?: string;
  to?: string;
}

export function useKpiReport(range: DateRange) {
  return useQuery({
    queryKey: queryKeys.reports.kpi(range),
    queryFn: () => getKpiReport(range),
  });
}

export function useCostReport(range: DateRange) {
  return useQuery({
    queryKey: queryKeys.reports.cost(range),
    queryFn: () => getCostReport(range),
  });
}

export function useDowntimeReport(range: DateRange) {
  return useQuery({
    queryKey: queryKeys.reports.downtime(range),
    queryFn: () => getDowntimeReport(range),
  });
}

export function useEquipmentHistory(equipmentId: string) {
  return useQuery({
    queryKey: queryKeys.reports.equipmentHistory(equipmentId),
    queryFn: () => getEquipmentHistory(equipmentId),
    enabled: Boolean(equipmentId),
  });
}
