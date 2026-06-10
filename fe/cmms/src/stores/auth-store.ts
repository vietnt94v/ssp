import { create } from 'zustand';
import type { Role, User } from '@/types';

interface AuthState {
  user: User | null;
  isInitialized: boolean;
  setUser: (user: User | null) => void;
  setInitialized: (value: boolean) => void;
  clear: () => void;
  hasRole: (roles: Role[]) => boolean;
}

export const useAuthStore = create<AuthState>()((set, get) => ({
  user: null,
  isInitialized: false,
  setUser: (user) => set({ user }),
  setInitialized: (value) => set({ isInitialized: value }),
  clear: () => set({ user: null }),
  hasRole: (roles) => {
    const user = get().user;
    return user ? roles.includes(user.role) : false;
  },
}));
