import { useEffect } from 'react';
import { RouterProvider } from 'react-router-dom';
import { AppProviders } from './providers';
import { router } from './router';
import { setAuthFailureHandler } from '@/lib/api-client';
import { useAuthStore } from '@/stores/auth-store';

export default function App() {
  useEffect(() => {
    setAuthFailureHandler(() => {
      useAuthStore.getState().clear();
      if (window.location.pathname !== '/auth/login') {
        window.location.assign('/auth/login');
      }
    });
  }, []);

  return (
    <AppProviders>
      <RouterProvider router={router} />
    </AppProviders>
  );
}
