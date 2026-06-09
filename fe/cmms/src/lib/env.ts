function normalizeApiUrl(value: string | undefined): string {
  const raw = value?.trim() || '/api';

  if (raw.startsWith('/')) {
    return raw.endsWith('/api') ? raw : `${raw.replace(/\/$/, '')}/api`;
  }

  const base = raw.replace(/\/$/, '');
  return base.endsWith('/api') ? base : `${base}/api`;
}

function normalizeSignalrUrl(value: string | undefined): string {
  const hubPath = '/hubs/work-orders';
  const raw = value?.trim() || hubPath;

  if (raw.startsWith('/')) {
    return raw;
  }

  const base = raw.replace(/\/$/, '');
  return base.endsWith(hubPath) ? base : `${base}${hubPath}`;
}

export const env = {
  apiUrl: normalizeApiUrl(import.meta.env.VITE_API_URL),
  appName: import.meta.env.VITE_APP_NAME ?? 'CMMS',
  signalrUrl: normalizeSignalrUrl(import.meta.env.VITE_SIGNALR_URL),
} as const;
