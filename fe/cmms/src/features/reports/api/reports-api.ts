import { apiClient } from '@/lib/api-client';

export interface KpiReport {
  from: string;
  to: string;
  mttrHours: number;
  mtbfHours: number;
  oeeImpactPercent: number;
  overdueRatePercent: number;
}

export interface CostReport {
  from: string;
  to: string;
  totalCost: number;
  laborCost: number;
  partsCost: number;
  byEquipment: {
    equipmentId: string;
    equipmentName: string;
    laborCost: number;
    partsCost: number;
    totalCost: number;
  }[];
}

export interface DowntimeReport {
  from: string;
  to: string;
  byEquipment: {
    equipmentId: string;
    equipmentName: string;
    downtimeMinutes: number;
    incidents: number;
  }[];
}

export interface EquipmentHistoryReport {
  equipmentId: string;
  equipmentName: string;
  events: {
    workOrderNumber: string;
    type: string;
    completedAt: string;
    summary: string;
    downtimeMinutes: number;
  }[];
}

interface DateRange {
  from?: string;
  to?: string;
}

export async function getKpiReport(range: DateRange) {
  const { data } = await apiClient.get<KpiReport>('/reports/kpi', {
    params: range,
  });
  return data;
}

export async function getCostReport(range: DateRange) {
  const { data } = await apiClient.get<CostReport>('/reports/cost', {
    params: range,
  });
  return data;
}

export async function getDowntimeReport(range: DateRange) {
  const { data } = await apiClient.get<DowntimeReport>('/reports/downtime', {
    params: range,
  });
  return data;
}

export async function getEquipmentHistory(equipmentId: string) {
  const { data } = await apiClient.get<EquipmentHistoryReport>(
    `/reports/equipment-history/${equipmentId}`,
  );
  return data;
}
