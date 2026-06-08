import { formatDistanceToNow } from 'date-fns';
import { AlertTriangle, Bell, Check, Package, Wrench } from 'lucide-react';
import type { LucideIcon } from 'lucide-react';
import { PageHeader } from '@/components/shared/PageHeader';
import { Card, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { useAuthStore } from '@/stores/auth-store';
import {
  useAcknowledgeAlert,
  useAlerts,
  useCreateWoFromAlert,
} from '../hooks/use-alerts';
import type { AlertType } from '@/types';

const iconByType: Record<AlertType, LucideIcon> = {
  PmDue: Bell,
  EquipmentBreakdown: Wrench,
  WoOverdue: AlertTriangle,
  LowStock: Package,
};

export default function AlertsPage() {
  const canManage = useAuthStore((s) => s.hasRole(['Admin', 'Manager']));
  const { data, isLoading } = useAlerts({ pageSize: 100 });
  const acknowledge = useAcknowledgeAlert();
  const createWo = useCreateWoFromAlert();

  return (
    <div>
      <PageHeader
        title="Alerts"
        description="Live maintenance notifications"
      />
      <Card>
        <CardContent className="space-y-2 pt-6">
          {isLoading ? (
            <p className="text-sm text-muted-foreground">Loading…</p>
          ) : data?.items.length ? (
            data.items.map((alert) => {
              const Icon = iconByType[alert.type];
              return (
                <div
                  key={alert.id}
                  className="flex items-center gap-3 rounded-md border p-3"
                >
                  <div className="flex h-9 w-9 items-center justify-center rounded-md bg-muted">
                    <Icon className="h-4 w-4" />
                  </div>
                  <div className="flex-1">
                    <div className="flex items-center gap-2">
                      <span className="text-sm font-medium">
                        {alert.message}
                      </span>
                      {alert.isAcknowledged ? (
                        <Badge variant="secondary">Acknowledged</Badge>
                      ) : null}
                    </div>
                    <div className="text-xs text-muted-foreground">
                      {formatDistanceToNow(new Date(alert.createdAt), {
                        addSuffix: true,
                      })}
                    </div>
                  </div>
                  <div className="flex gap-2">
                    {!alert.isAcknowledged ? (
                      <Button
                        size="sm"
                        variant="outline"
                        onClick={() => acknowledge.mutate(alert.id)}
                      >
                        <Check className="h-4 w-4" />
                        Acknowledge
                      </Button>
                    ) : null}
                    {canManage &&
                    (alert.type === 'EquipmentBreakdown' ||
                      alert.type === 'PmDue') ? (
                      <Button
                        size="sm"
                        onClick={() => createWo.mutate(alert.id)}
                      >
                        Create WO
                      </Button>
                    ) : null}
                  </div>
                </div>
              );
            })
          ) : (
            <p className="text-sm text-muted-foreground">No alerts.</p>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
