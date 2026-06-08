import { apiClient } from '@/lib/api-client';
import type {
  MaintenanceSchedule,
  PaginatedResponse,
  ScheduleFrequency,
} from '@/types';

export interface CalendarEvent {
  id: string;
  title: string;
  equipmentId: string;
  equipmentName: string | null;
  frequency: ScheduleFrequency;
  start: string;
  allDay: boolean;
}

export interface ScheduleInput {
  equipmentId: string;
  title: string;
  frequency: ScheduleFrequency;
  intervalValue: number;
  meterThreshold?: number | null;
  nextDueDate: string;
  isActive: boolean;
}

export async function getSchedules(params?: {
  page?: number;
  pageSize?: number;
}) {
  const { data } = await apiClient.get<PaginatedResponse<MaintenanceSchedule>>(
    '/schedules',
    { params },
  );
  return data;
}

export async function getCalendar(from: string, to: string) {
  const { data } = await apiClient.get<CalendarEvent[]>('/schedules/calendar', {
    params: { from, to },
  });
  return data;
}

export async function createSchedule(input: ScheduleInput) {
  const { data } = await apiClient.post<MaintenanceSchedule>(
    '/schedules',
    input,
  );
  return data;
}

export async function updateSchedule(id: string, input: ScheduleInput) {
  const { data } = await apiClient.put<MaintenanceSchedule>(
    `/schedules/${id}`,
    input,
  );
  return data;
}
