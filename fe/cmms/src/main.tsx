import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import App from './app/App';
import { env } from '@/lib/env';
import { applyTheme, getSystemTheme } from '@/stores/ui-store';

function resolveInitialTheme(): 'light' | 'dark' {
  try {
    const raw = localStorage.getItem('cmms-ui');
    if (raw) {
      const persisted = JSON.parse(raw) as { state?: { theme?: 'light' | 'dark' } };
      if (persisted.state?.theme) {
        return persisted.state.theme;
      }
    }
  } catch {
    // ignore malformed storage and fall back to the system preference
  }
  return getSystemTheme();
}

applyTheme(resolveInitialTheme());

document.title = `${env.appName} — Maintenance Management`;

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
);
