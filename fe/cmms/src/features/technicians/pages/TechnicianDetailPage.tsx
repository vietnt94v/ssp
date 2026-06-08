import { useParams } from 'react-router-dom';
import { PageHeader } from '@/components/shared/PageHeader';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { useTechnician, useTechnicianWorkload } from '../hooks/use-technicians';
import { useWorkOrders } from '@/features/work-orders/hooks/use-work-orders';
import { WorkOrderStatusBadge } from '@/components/shared/StatusBadge';

export default function TechnicianDetailPage() {
  const { id = '' } = useParams();
  const { data: tech } = useTechnician(id);
  const { data: workload } = useTechnicianWorkload(id);
  const { data: workOrders } = useWorkOrders({ technicianId: id, pageSize: 50 });

  if (!tech) {
    return <div className="text-muted-foreground">Loading…</div>;
  }

  return (
    <div>
      <PageHeader title={tech.name} description={tech.department ?? undefined} />

      <div className="grid gap-4 lg:grid-cols-3">
        <Card>
          <CardHeader>
            <CardTitle>Profile</CardTitle>
          </CardHeader>
          <CardContent className="space-y-3">
            <div>
              <div className="text-xs text-muted-foreground">Rating</div>
              <div className="text-sm font-medium">{tech.rating ?? '—'}</div>
            </div>
            <div>
              <div className="text-xs text-muted-foreground">Skills</div>
              <div className="mt-1 flex flex-wrap gap-1">
                {tech.skills.map((s) => (
                  <Badge key={s} variant="outline">
                    {s}
                  </Badge>
                ))}
              </div>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Workload</CardTitle>
          </CardHeader>
          <CardContent className="space-y-2 text-sm">
            <div className="flex justify-between">
              <span className="text-muted-foreground">Open work orders</span>
              <span>{workload?.openWorkOrderCount ?? '—'}</span>
            </div>
            <div className="flex justify-between">
              <span className="text-muted-foreground">Utilization</span>
              <span>{workload?.workloadPercent ?? '—'}%</span>
            </div>
          </CardContent>
        </Card>

        <Card className="lg:col-span-3">
          <CardHeader>
            <CardTitle>Assigned work orders</CardTitle>
          </CardHeader>
          <CardContent className="space-y-2">
            {workOrders?.items.length ? (
              workOrders.items.map((wo) => (
                <div
                  key={wo.id}
                  className="flex items-center justify-between rounded-md border p-3"
                >
                  <div>
                    <div className="font-medium">{wo.number}</div>
                    <div className="text-xs text-muted-foreground">
                      {wo.equipmentName ?? '—'}
                    </div>
                  </div>
                  <WorkOrderStatusBadge status={wo.status} />
                </div>
              ))
            ) : (
              <p className="text-sm text-muted-foreground">
                No assigned work orders.
              </p>
            )}
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
