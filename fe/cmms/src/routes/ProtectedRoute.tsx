import { Navigate, Outlet } from 'react-router-dom';
import type { Role } from '@/types';
import { useAuthStore } from '@/stores/auth-store';

interface ProtectedRouteProps {
  allowed?: Role[];
}

export function ProtectedRoute({ allowed }: ProtectedRouteProps) {
  const user = useAuthStore((s) => s.user);

  if (!user) {
    return <Navigate to="/auth/login" replace />;
  }

  if (allowed && !allowed.includes(user.role)) {
    return <Navigate to="/dashboard" replace />;
  }

  return <Outlet />;
}
