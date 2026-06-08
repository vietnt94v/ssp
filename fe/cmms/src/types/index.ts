export type Role = 'Admin' | 'Manager' | 'Technician';

export type EquipmentStatus =
  | 'Active'
  | 'UnderMaintenance'
  | 'Broken'
  | 'Decommissioned';

export type WorkOrderType = 'Corrective' | 'Preventive' | 'Inspection';

export type WorkOrderStatus =
  | 'Draft'
  | 'Assigned'
  | 'InProgress'
  | 'OnHold'
  | 'Completed'
  | 'Closed';

export type WorkOrderPriority = 'Low' | 'Medium' | 'High' | 'Critical';

export type ScheduleFrequency = 'Daily' | 'Weekly' | 'Monthly' | 'ByMeter';

export type AlertType =
  | 'PmDue'
  | 'EquipmentBreakdown'
  | 'WoOverdue'
  | 'LowStock';

export type CostEntryType = 'Labor' | 'Parts';

export interface AuditFields {
  createdAt: string;
  createdBy: string | null;
  updatedAt: string | null;
  updatedBy: string | null;
}

export interface User {
  id: string;
  email: string;
  fullName: string;
  role: Role;
}

export interface Category {
  id: string;
  name: string;
  description: string | null;
}

export interface Location {
  id: string;
  name: string;
  area: string | null;
}

export interface Equipment extends AuditFields {
  id: string;
  code: string;
  name: string;
  categoryId: string;
  categoryName: string | null;
  locationId: string;
  locationName: string | null;
  manufacturer: string | null;
  installDate: string | null;
  status: EquipmentStatus;
  lastMaintenanceAt: string | null;
  imageUrl: string | null;
}

export interface WorkOrderChecklistItem {
  id: string;
  description: string;
  isDone: boolean;
}

export interface WorkOrderPart {
  id: string;
  partId: string;
  partName: string;
  quantity: number;
  unitCost: number;
}

export interface CostEntry extends AuditFields {
  id: string;
  workOrderId: string;
  type: CostEntryType;
  amount: number;
  description: string | null;
}

export interface WorkOrder extends AuditFields {
  id: string;
  number: string;
  type: WorkOrderType;
  priority: WorkOrderPriority;
  status: WorkOrderStatus;
  equipmentId: string;
  equipmentName: string | null;
  assignedTechnicianId: string | null;
  assignedTechnicianName: string | null;
  description: string;
  deadline: string | null;
  checklist: WorkOrderChecklistItem[];
  parts: WorkOrderPart[];
  costs: CostEntry[];
  totalCost: number;
}

export interface MaintenanceSchedule extends AuditFields {
  id: string;
  equipmentId: string;
  equipmentName: string | null;
  title: string;
  frequency: ScheduleFrequency;
  intervalValue: number;
  meterThreshold: number | null;
  nextDueDate: string;
  isActive: boolean;
}

export interface Technician extends AuditFields {
  id: string;
  userId: string;
  name: string;
  department: string | null;
  skills: string[];
  openWorkOrderCount: number;
  workloadPercent: number;
  rating: number | null;
}

export interface MaintenanceLog {
  id: string;
  equipmentId: string;
  workOrderId: string;
  completedAt: string;
  summary: string;
  downtimeMinutes: number;
}

export interface SparePart extends AuditFields {
  id: string;
  code: string;
  name: string;
  unitCost: number;
  stockQuantity: number;
  reorderLevel: number;
}

export interface Alert {
  id: string;
  type: AlertType;
  message: string;
  entityId: string | null;
  entityType: string | null;
  isAcknowledged: boolean;
  createdAt: string;
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface PaginationParams {
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortDir?: 'asc' | 'desc';
}

export interface ApiError {
  error: string;
  code: string;
  details?: string[];
}

export interface DashboardKpi {
  openWorkOrders: number;
  overdueWorkOrders: number;
  brokenEquipment: number;
  monthlyCost: number;
  mttrHours: number;
  mtbfHours: number;
}

export interface WorkOrderTrendPoint {
  date: string;
  created: number;
  completed: number;
}

export interface WorkOrderStatusCount {
  status: WorkOrderStatus;
  count: number;
}
