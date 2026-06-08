export const queryKeys = {
  equipment: {
    all: ['equipment'] as const,
    list: (filters?: unknown) => ['equipment', 'list', filters] as const,
    detail: (id: string) => ['equipment', 'detail', id] as const,
  },
  workOrders: {
    all: ['work-orders'] as const,
    list: (filters?: unknown) => ['work-orders', 'list', filters] as const,
    kanban: (filters?: unknown) => ['work-orders', 'kanban', filters] as const,
    detail: (id: string) => ['work-orders', 'detail', id] as const,
  },
  schedules: {
    all: ['schedules'] as const,
    list: (filters?: unknown) => ['schedules', 'list', filters] as const,
    calendar: (range?: unknown) => ['schedules', 'calendar', range] as const,
    detail: (id: string) => ['schedules', 'detail', id] as const,
  },
  technicians: {
    all: ['technicians'] as const,
    list: (filters?: unknown) => ['technicians', 'list', filters] as const,
    detail: (id: string) => ['technicians', 'detail', id] as const,
    workload: (id: string) => ['technicians', 'workload', id] as const,
  },
  spareParts: {
    all: ['spare-parts'] as const,
    list: (filters?: unknown) => ['spare-parts', 'list', filters] as const,
    detail: (id: string) => ['spare-parts', 'detail', id] as const,
  },
  dashboard: {
    kpi: ['dashboard', 'kpi'] as const,
    trend: (days: number) => ['dashboard', 'trend', days] as const,
    urgent: ['dashboard', 'urgent'] as const,
    statusBreakdown: ['dashboard', 'status-breakdown'] as const,
  },
  reports: {
    kpi: (range?: unknown) => ['reports', 'kpi', range] as const,
    cost: (range?: unknown) => ['reports', 'cost', range] as const,
    equipmentHistory: (id: string) =>
      ['reports', 'equipment-history', id] as const,
    downtime: (range?: unknown) => ['reports', 'downtime', range] as const,
  },
  alerts: {
    all: ['alerts'] as const,
    list: (filters?: unknown) => ['alerts', 'list', filters] as const,
  },
  settings: {
    users: ['settings', 'users'] as const,
    categories: ['settings', 'categories'] as const,
    locations: ['settings', 'locations'] as const,
  },
} as const;
