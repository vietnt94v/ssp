import { apiClient } from '@/lib/api-client';
import type {
  PaginatedResponse,
  WorkOrder,
  WorkOrderPriority,
  WorkOrderStatus,
  WorkOrderType,
} from '@/types';

export interface WorkOrderListItem {
  id: string;
  number: string;
  type: WorkOrderType;
  priority: WorkOrderPriority;
  status: WorkOrderStatus;
  equipmentName: string | null;
  assignedTechnicianName: string | null;
  deadline: string | null;
}

export interface WorkOrderListParams {
  type?: string;
  status?: string;
  priority?: string;
  technicianId?: string;
  equipmentId?: string;
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortDir?: 'asc' | 'desc';
}

export interface WorkOrderInput {
  type: WorkOrderType;
  priority: WorkOrderPriority;
  equipmentId: string;
  assignedTechnicianId?: string | null;
  description: string;
  deadline?: string | null;
  checklist?: { description: string; isDone: boolean }[];
}

export type KanbanBoard = Record<WorkOrderStatus, WorkOrderListItem[]>;

export async function getWorkOrders(params: WorkOrderListParams) {
  const { data } = await apiClient.get<PaginatedResponse<WorkOrderListItem>>(
    '/work-orders',
    { params },
  );
  return data;
}

export async function getWorkOrdersKanban(technicianId?: string) {
  const { data } = await apiClient.get<KanbanBoard>('/work-orders/kanban', {
    params: { technicianId },
  });
  return data;
}

export async function getWorkOrder(id: string) {
  const { data } = await apiClient.get<WorkOrder>(`/work-orders/${id}`);
  return data;
}

export async function createWorkOrder(input: WorkOrderInput) {
  const { data } = await apiClient.post<WorkOrder>('/work-orders', input);
  return data;
}

export async function updateWorkOrder(id: string, input: WorkOrderInput) {
  const { data } = await apiClient.put<WorkOrder>(`/work-orders/${id}`, input);
  return data;
}

export async function changeWorkOrderStatus(
  id: string,
  status: WorkOrderStatus,
) {
  const { data } = await apiClient.patch<WorkOrder>(
    `/work-orders/${id}/status`,
    { status },
  );
  return data;
}

export async function deleteWorkOrder(id: string) {
  await apiClient.delete(`/work-orders/${id}`);
}
