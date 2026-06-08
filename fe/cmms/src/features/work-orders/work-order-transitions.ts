import type { WorkOrderStatus } from '@/types';

export const WORK_ORDER_TRANSITIONS: Record<
  WorkOrderStatus,
  WorkOrderStatus[]
> = {
  Draft: ['Assigned'],
  Assigned: ['InProgress', 'Draft'],
  InProgress: ['OnHold', 'Completed'],
  OnHold: ['InProgress'],
  Completed: ['Closed'],
  Closed: [],
};
