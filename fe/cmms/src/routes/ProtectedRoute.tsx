import { Navigate, Outlet, useLocation } from 'react-router-dom';
import type { Role } from '@/types';
import { useAuthStore } from '@/stores/auth-store';

interface ProtectedRouteProps {
  allowed?: Role[];
}

export function ProtectedRoute({ allowed }: ProtectedRouteProps) {
  const location = useLocation();
  const { token, user } = useAuthStore();

  if (!token || !user) {
    return <Navigate to="/auth/login" state={{ from: location }} replace />;
  }

  if (allowed && !allowed.includes(user.role)) {
    return <Navigate to="/dashboard" replace />;
  }

  return <Outlet />;
}
