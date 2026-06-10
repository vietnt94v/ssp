import { lazy } from 'react';
import { createBrowserRouter, Navigate } from 'react-router-dom';
import { AppShell } from '@/components/layout/AppShell';
import { AuthGuard } from '@/routes/AuthGuard';
import { ProtectedRoute } from '@/routes/ProtectedRoute';

const LoginPage = lazy(() => import('@/features/auth/pages/LoginPage'));
const ForgotPasswordPage = lazy(
  () => import('@/features/auth/pages/ForgotPasswordPage'),
);

const DashboardPage = lazy(
  () => import('@/features/dashboard/pages/DashboardPage'),
);

const EquipmentListPage = lazy(
  () => import('@/features/equipment/pages/EquipmentListPage'),
);
const EquipmentDetailPage = lazy(
  () => import('@/features/equipment/pages/EquipmentDetailPage'),
);
const EquipmentFormPage = lazy(
  () => import('@/features/equipment/pages/EquipmentFormPage'),
);

const WorkOrderListPage = lazy(
  () => import('@/features/work-orders/pages/WorkOrderListPage'),
);
const WorkOrderDetailPage = lazy(
  () => import('@/features/work-orders/pages/WorkOrderDetailPage'),
);
const WorkOrderFormPage = lazy(
  () => import('@/features/work-orders/pages/WorkOrderFormPage'),
);

const SchedulePage = lazy(
  () => import('@/features/schedule/pages/SchedulePage'),
);
const ScheduleFormPage = lazy(
  () => import('@/features/schedule/pages/ScheduleFormPage'),
);

const TechnicianListPage = lazy(
  () => import('@/features/technicians/pages/TechnicianListPage'),
);
const TechnicianDetailPage = lazy(
  () => import('@/features/technicians/pages/TechnicianDetailPage'),
);

const SparePartListPage = lazy(
  () => import('@/features/spare-parts/pages/SparePartListPage'),
);
const SparePartFormPage = lazy(
  () => import('@/features/spare-parts/pages/SparePartFormPage'),
);

const KpiReportPage = lazy(
  () => import('@/features/reports/pages/KpiReportPage'),
);
const CostReportPage = lazy(
  () => import('@/features/reports/pages/CostReportPage'),
);
const EquipmentHistoryPage = lazy(
  () => import('@/features/reports/pages/EquipmentHistoryPage'),
);
const DowntimeReportPage = lazy(
  () => import('@/features/reports/pages/DowntimeReportPage'),
);

const AlertsPage = lazy(() => import('@/features/alerts/pages/AlertsPage'));

const UsersSettingsPage = lazy(
  () => import('@/features/settings/pages/UsersSettingsPage'),
);
const RolesSettingsPage = lazy(
  () => import('@/features/settings/pages/RolesSettingsPage'),
);
const CategoriesSettingsPage = lazy(
  () => import('@/features/settings/pages/CategoriesSettingsPage'),
);
const LocationsSettingsPage = lazy(
  () => import('@/features/settings/pages/LocationsSettingsPage'),
);

export const router = createBrowserRouter([
  { path: '/', element: <Navigate to="/dashboard" replace /> },
  { path: '/auth/login', element: <LoginPage /> },
  { path: '/auth/forgot-password', element: <ForgotPasswordPage /> },
  {
    element: <AuthGuard />,
    children: [
      {
        element: <ProtectedRoute />,
        children: [
          {
            element: <AppShell />,
            children: [
              { path: '/dashboard', element: <DashboardPage /> },

              { path: '/equipment', element: <EquipmentListPage /> },
              { path: '/equipment/:id', element: <EquipmentDetailPage /> },

              { path: '/work-orders', element: <WorkOrderListPage /> },
              {
                path: '/work-orders/:id',
                element: <WorkOrderDetailPage />,
              },
              {
                path: '/work-orders/:id/edit',
                element: <WorkOrderFormPage />,
              },

              { path: '/schedule', element: <SchedulePage /> },

              { path: '/alerts', element: <AlertsPage /> },
            ],
          },
        ],
      },
      {
        element: <ProtectedRoute allowed={['Admin', 'Manager']} />,
        children: [
          {
            element: <AppShell />,
            children: [
              { path: '/equipment/new', element: <EquipmentFormPage /> },
              { path: '/equipment/:id/edit', element: <EquipmentFormPage /> },

              { path: '/work-orders/new', element: <WorkOrderFormPage /> },

              { path: '/schedule/new', element: <ScheduleFormPage /> },
              { path: '/schedule/:id/edit', element: <ScheduleFormPage /> },

              { path: '/technicians', element: <TechnicianListPage /> },
              { path: '/technicians/:id', element: <TechnicianDetailPage /> },

              { path: '/spare-parts', element: <SparePartListPage /> },
              { path: '/spare-parts/new', element: <SparePartFormPage /> },
              { path: '/spare-parts/:id', element: <SparePartFormPage /> },

              { path: '/reports/kpi', element: <KpiReportPage /> },
              { path: '/reports/cost', element: <CostReportPage /> },
              {
                path: '/reports/equipment-history',
                element: <EquipmentHistoryPage />,
              },
              { path: '/reports/downtime', element: <DowntimeReportPage /> },
            ],
          },
        ],
      },
      {
        element: <ProtectedRoute allowed={['Admin']} />,
        children: [
          {
            element: <AppShell />,
            children: [
              { path: '/settings/users', element: <UsersSettingsPage /> },
              { path: '/settings/roles', element: <RolesSettingsPage /> },
              {
                path: '/settings/categories',
                element: <CategoriesSettingsPage />,
              },
              {
                path: '/settings/locations',
                element: <LocationsSettingsPage />,
              },
            ],
          },
        ],
      },
    ],
  },
  { path: '*', element: <Navigate to="/dashboard" replace /> },
]);
