import { useCallback } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/stores/auth-store';
import {
  getMe,
  login as loginRequest,
  logout as logoutRequest,
} from '../api/auth-api';

export function useAuth() {
  const queryClient = useQueryClient();
  const user = useAuthStore((s) => s.user);
  const isInitialized = useAuthStore((s) => s.isInitialized);
  const setUser = useAuthStore((s) => s.setUser);
  const setInitialized = useAuthStore((s) => s.setInitialized);
  const clear = useAuthStore((s) => s.clear);

  const login = useCallback(
    async (email: string, password: string) => {
      const loggedIn = await loginRequest(email, password);
      setUser(loggedIn);
      setInitialized(true);
      return loggedIn;
    },
    [setUser, setInitialized],
  );

  const logout = useCallback(async () => {
    try {
      await logoutRequest();
    } catch {
      // Server revoke is best-effort; local session is always cleared below.
    } finally {
      clear();
      queryClient.clear();
    }
  }, [clear, queryClient]);

  const loadSession = useCallback(async () => {
    try {
      const current = await getMe();
      setUser(current);
      return current;
    } catch {
      clear();
      return null;
    } finally {
      setInitialized(true);
    }
  }, [setUser, clear, setInitialized]);

  return {
    user,
    isAuthenticated: user != null,
    isInitialized,
    login,
    logout,
    loadSession,
  };
}
