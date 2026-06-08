import {
  AlertTriangle,
  CalendarDays,
  ClipboardList,
  LayoutDashboard,
  Package,
  Settings,
  TrendingUp,
  Users,
  Wrench,
} from 'lucide-react';
import type { LucideIcon } from 'lucide-react';
import type { Role } from '@/types';

export interface NavItem {
  label: string;
  to: string;
  icon: LucideIcon;
  roles: Role[];
  mobile?: boolean;
}

export const navItems: NavItem[] = [
  {
    label: 'Dashboard',
    to: '/dashboard',
    icon: LayoutDashboard,
    roles: ['Admin', 'Manager', 'Technician'],
    mobile: true,
  },
  {
    label: 'Equipment',
    to: '/equipment',
    icon: Wrench,
    roles: ['Admin', 'Manager', 'Technician'],
  },
  {
    label: 'Work Orders',
    to: '/work-orders',
    icon: ClipboardList,
    roles: ['Admin', 'Manager', 'Technician'],
    mobile: true,
  },
  {
    label: 'Schedule',
    to: '/schedule',
    icon: CalendarDays,
    roles: ['Admin', 'Manager', 'Technician'],
    mobile: true,
  },
  {
    label: 'Technicians',
    to: '/technicians',
    icon: Users,
    roles: ['Admin', 'Manager'],
  },
  {
    label: 'Spare Parts',
    to: '/spare-parts',
    icon: Package,
    roles: ['Admin', 'Manager'],
  },
  {
    label: 'Reports',
    to: '/reports/kpi',
    icon: TrendingUp,
    roles: ['Admin', 'Manager'],
  },
  {
    label: 'Alerts',
    to: '/alerts',
    icon: AlertTriangle,
    roles: ['Admin', 'Manager', 'Technician'],
    mobile: true,
  },
  {
    label: 'Settings',
    to: '/settings/users',
    icon: Settings,
    roles: ['Admin'],
  },
];
