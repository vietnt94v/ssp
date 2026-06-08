import {
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query';
import { toast } from 'sonner';
import { queryKeys } from '@/lib/query-keys';
import {
  createSchedule,
  getCalendar,
  getSchedules,
  updateSchedule,
} from '../api/schedule-api';
import type { ScheduleInput } from '../api/schedule-api';

export function useSchedules(params?: { page?: number; pageSize?: number }) {
  return useQuery({
    queryKey: queryKeys.schedules.list(params),
    queryFn: () => getSchedules(params),
  });
}

export function useScheduleCalendar(from: string, to: string) {
  return useQuery({
    queryKey: queryKeys.schedules.calendar({ from, to }),
    queryFn: () => getCalendar(from, to),
  });
}

export function useCreateSchedule() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (input: ScheduleInput) => createSchedule(input),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: queryKeys.schedules.all });
      toast.success('Schedule created');
    },
  });
}

export function useUpdateSchedule(id: string) {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (input: ScheduleInput) => updateSchedule(id, input),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: queryKeys.schedules.all });
      toast.success('Schedule updated');
    },
  });
}
