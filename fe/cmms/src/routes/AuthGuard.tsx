import { useEffect } from 'react';
import { Navigate, Outlet, useLocation } from 'react-router-dom';
import { Loader2 } from 'lucide-react';
import { useAuth } from '@/features/auth/hooks/use-auth';

export function AuthGuard() {
  const location = useLocation();
  const { isAuthenticated, isInitialized, loadSession } = useAuth();

  useEffect(() => {
    if (!isInitialized) {
      void loadSession();
    }
  }, [isInitialized, loadSession]);

  if (!isInitialized) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <Loader2 className="h-6 w-6 animate-spin text-muted-foreground" />
      </div>
    );
  }

  if (!isAuthenticated) {
    return <Navigate to="/auth/login" state={{ from: location }} replace />;
  }

  return <Outlet />;
}
