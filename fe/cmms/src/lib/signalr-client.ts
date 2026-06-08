import {
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from '@microsoft/signalr';
import type { HubConnection } from '@microsoft/signalr';
import { useAuthStore } from '@/stores/auth-store';

const HUB_URL = import.meta.env.VITE_SIGNALR_URL ?? '/hubs/work-orders';

let connection: HubConnection | null = null;

export function getHubConnection(): HubConnection {
  if (!connection) {
    connection = new HubConnectionBuilder()
      .withUrl(HUB_URL, {
        accessTokenFactory: () => useAuthStore.getState().token ?? '',
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Warning)
      .build();
  }
  return connection;
}

export async function startHubConnection(): Promise<HubConnection> {
  const conn = getHubConnection();
  if (conn.state === HubConnectionState.Disconnected) {
    await conn.start();
  }
  return conn;
}

export async function stopHubConnection(): Promise<void> {
  if (connection && connection.state !== HubConnectionState.Disconnected) {
    await connection.stop();
  }
}
