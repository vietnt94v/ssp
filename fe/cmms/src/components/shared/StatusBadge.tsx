import { Badge } from '@/components/ui/badge';
import type { BadgeProps } from '@/components/ui/badge';
import type {
  EquipmentStatus,
  WorkOrderPriority,
  WorkOrderStatus,
} from '@/types';

type BadgeVariant = NonNullable<BadgeProps['variant']>;

const equipmentVariant: Record<EquipmentStatus, BadgeVariant> = {
  Active: 'success',
  UnderMaintenance: 'warning',
  Broken: 'destructive',
  Decommissioned: 'secondary',
};

const workOrderVariant: Record<WorkOrderStatus, BadgeVariant> = {
  Draft: 'secondary',
  Assigned: 'info',
  InProgress: 'warning',
  OnHold: 'outline',
  Completed: 'success',
  Closed: 'default',
};

const priorityVariant: Record<WorkOrderPriority, BadgeVariant> = {
  Low: 'secondary',
  Medium: 'info',
  High: 'warning',
  Critical: 'destructive',
};

const labelize = (value: string) =>
  value.replace(/([a-z])([A-Z])/g, '$1 $2');

export function EquipmentStatusBadge({ status }: { status: EquipmentStatus }) {
  return <Badge variant={equipmentVariant[status]}>{labelize(status)}</Badge>;
}

export function WorkOrderStatusBadge({ status }: { status: WorkOrderStatus }) {
  return <Badge variant={workOrderVariant[status]}>{labelize(status)}</Badge>;
}

export function PriorityBadge({ priority }: { priority: WorkOrderPriority }) {
  return <Badge variant={priorityVariant[priority]}>{priority}</Badge>;
}
