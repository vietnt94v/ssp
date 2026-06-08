import type { Role } from '@/types';

export const routePermissions: Record<string, Role[]> = {
  '/dashboard': ['Admin', 'Manager', 'Technician'],
  '/equipment': ['Admin', 'Manager', 'Technician'],
  '/equipment/new': ['Admin', 'Manager'],
  '/equipment/:id': ['Admin', 'Manager', 'Technician'],
  '/equipment/:id/edit': ['Admin', 'Manager'],
  '/work-orders': ['Admin', 'Manager', 'Technician'],
  '/work-orders/new': ['Admin', 'Manager'],
  '/work-orders/:id': ['Admin', 'Manager', 'Technician'],
  '/work-orders/:id/edit': ['Admin', 'Manager', 'Technician'],
  '/schedule': ['Admin', 'Manager', 'Technician'],
  '/schedule/new': ['Admin', 'Manager'],
  '/schedule/:id/edit': ['Admin', 'Manager'],
  '/technicians': ['Admin', 'Manager'],
  '/technicians/:id': ['Admin', 'Manager'],
  '/spare-parts': ['Admin', 'Manager'],
  '/reports/*': ['Admin', 'Manager'],
  '/alerts': ['Admin', 'Manager', 'Technician'],
  '/settings/*': ['Admin'],
};

export function canAccess(role: Role, allowed: Role[]): boolean {
  return allowed.includes(role);
}
