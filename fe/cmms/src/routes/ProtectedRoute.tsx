import { Navigate, Outlet } from 'react-router-dom';
import type { Role } from '@/types';
import { useAuthStore } from '@/stores/auth-store';

interface ProtectedRouteProps {
  allowed?: Role[];
}

export function ProtectedRoute({ allowed }: ProtectedRouteProps) {
  const hasAccess = useAuthStore((s) => !allowed || s.hasRole(allowed));

  if (!hasAccess) {
    return <Navigate to="/dashboard" replace />;
  }

  return <Outlet />;
}
