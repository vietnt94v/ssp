import { create } from 'zustand';
import type {
  EquipmentStatus,
  WorkOrderPriority,
  WorkOrderStatus,
  WorkOrderType,
} from '@/types';

export interface EquipmentFilters {
  search: string;
  status: EquipmentStatus | 'All';
  categoryId: string | 'All';
  locationId: string | 'All';
}

export interface WorkOrderFilters {
  search: string;
  type: WorkOrderType | 'All';
  status: WorkOrderStatus | 'All';
  priority: WorkOrderPriority | 'All';
  technicianId: string | 'All';
  equipmentId: string | 'All';
}

interface FilterState {
  equipment: EquipmentFilters;
  workOrders: WorkOrderFilters;
  setEquipmentFilters: (patch: Partial<EquipmentFilters>) => void;
  resetEquipmentFilters: () => void;
  setWorkOrderFilters: (patch: Partial<WorkOrderFilters>) => void;
  resetWorkOrderFilters: () => void;
}

const defaultEquipmentFilters: EquipmentFilters = {
  search: '',
  status: 'All',
  categoryId: 'All',
  locationId: 'All',
};

const defaultWorkOrderFilters: WorkOrderFilters = {
  search: '',
  type: 'All',
  status: 'All',
  priority: 'All',
  technicianId: 'All',
  equipmentId: 'All',
};

export const useFilterStore = create<FilterState>((set) => ({
  equipment: defaultEquipmentFilters,
  workOrders: defaultWorkOrderFilters,
  setEquipmentFilters: (patch) =>
    set((s) => ({ equipment: { ...s.equipment, ...patch } })),
  resetEquipmentFilters: () => set({ equipment: defaultEquipmentFilters }),
  setWorkOrderFilters: (patch) =>
    set((s) => ({ workOrders: { ...s.workOrders, ...patch } })),
  resetWorkOrderFilters: () => set({ workOrders: defaultWorkOrderFilters }),
}));
