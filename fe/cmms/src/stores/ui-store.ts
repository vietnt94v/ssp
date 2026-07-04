import { create } from 'zustand';
import { persist, subscribeWithSelector } from 'zustand/middleware';

type Theme = 'light' | 'dark';

interface UiState {
  sidebarOpen: boolean;
  activeModal: string | null;
  theme: Theme;
  toggleSidebar: () => void;
  setSidebarOpen: (open: boolean) => void;
  openModal: (id: string) => void;
  closeModal: () => void;
  setTheme: (theme: Theme) => void;
  toggleTheme: () => void;
}

export function getSystemTheme(): Theme {
  if (typeof window === 'undefined' || !window.matchMedia) {
    return 'light';
  }
  return window.matchMedia('(prefers-color-scheme: dark)').matches
    ? 'dark'
    : 'light';
}

export function applyTheme(theme: Theme) {
  if (typeof document === 'undefined') return;
  document.documentElement.classList.toggle('dark', theme === 'dark');
}

export const useUiStore = create<UiState>()(
  subscribeWithSelector(
    persist(
      (set, get) => ({
        sidebarOpen: true,
        activeModal: null,
        theme: getSystemTheme(),
        toggleSidebar: () => set((s) => ({ sidebarOpen: !s.sidebarOpen })),
        setSidebarOpen: (open) => set({ sidebarOpen: open }),
        openModal: (id) => set({ activeModal: id }),
        closeModal: () => set({ activeModal: null }),
        setTheme: (theme) => set({ theme }),
        toggleTheme: () =>
          set({ theme: get().theme === 'dark' ? 'light' : 'dark' }),
      }),
      {
        name: 'cmms-ui',
        partialize: (state) => ({ theme: state.theme }),
      },
    ),
  ),
);

useUiStore.subscribe((s) => s.theme, applyTheme, { fireImmediately: true });
