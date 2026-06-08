export const env = {
  apiUrl: import.meta.env.VITE_API_URL ?? '/api',
  appName: import.meta.env.VITE_APP_NAME ?? 'CMMS',
  signalrUrl: import.meta.env.VITE_SIGNALR_URL ?? '/hubs/work-orders',
} as const;
