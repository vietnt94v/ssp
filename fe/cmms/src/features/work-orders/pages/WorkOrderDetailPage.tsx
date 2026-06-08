import { useNavigate, useParams } from 'react-router-dom';
import { format } from 'date-fns';
import { Check } from 'lucide-react';
import { toast } from 'sonner';
import { PageHeader } from '@/components/shared/PageHeader';
import {
  PriorityBadge,
  WorkOrderStatusBadge,
} from '@/components/shared/StatusBadge';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { formatCurrency } from '@/lib/utils';
import { WORK_ORDER_TRANSITIONS } from '../work-order-transitions';
import {
  useChangeWorkOrderStatus,
  useWorkOrder,
} from '../hooks/use-work-orders';

export default function WorkOrderDetailPage() {
  const { id = '' } = useParams();
  const navigate = useNavigate();
  const { data: wo, isLoading } = useWorkOrder(id);
  const changeStatus = useChangeWorkOrderStatus(id);

  if (isLoading || !wo) {
    return <div className="text-muted-foreground">Loading…</div>;
  }

  const nextStatuses = WORK_ORDER_TRANSITIONS[wo.status];

  const handleStatus = async (status: typeof wo.status) => {
    try {
      await changeStatus.mutateAsync(status);
      toast.success(`Status changed to ${status}`);
    } catch {
      // interceptor surfaces the error
    }
  };

  return (
    <div>
      <PageHeader
        title={wo.number}
        description={wo.equipmentName ?? undefined}
        actions={
          <Button variant="outline" onClick={() => navigate(`/work-orders/${id}/edit`)}>
            Edit
          </Button>
        }
      />

      <div className="mb-4 flex flex-wrap items-center gap-2">
        <WorkOrderStatusBadge status={wo.status} />
        <PriorityBadge priority={wo.priority} />
        <span className="text-sm text-muted-foreground">{wo.type}</span>
      </div>

      {nextStatuses.length ? (
        <div className="mb-6 flex flex-wrap gap-2">
          <span className="text-sm text-muted-foreground">Move to:</span>
          {nextStatuses.map((status) => (
            <Button
              key={status}
              size="sm"
              variant="secondary"
              disabled={changeStatus.isPending}
              onClick={() => handleStatus(status)}
            >
              {status}
            </Button>
          ))}
        </div>
      ) : null}

      <div className="grid gap-4 lg:grid-cols-3">
        <Card className="lg:col-span-2">
          <CardHeader>
            <CardTitle>Details</CardTitle>
          </CardHeader>
          <CardContent className="space-y-3">
            <p className="text-sm">{wo.description}</p>
            <div className="grid gap-3 sm:grid-cols-2">
              <div>
                <div className="text-xs text-muted-foreground">Technician</div>
                <div className="text-sm font-medium">
                  {wo.assignedTechnicianName ?? 'Unassigned'}
                </div>
              </div>
              <div>
                <div className="text-xs text-muted-foreground">Deadline</div>
                <div className="text-sm font-medium">
                  {wo.deadline
                    ? format(new Date(wo.deadline), 'PPp')
                    : '—'}
                </div>
              </div>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Cost</CardTitle>
          </CardHeader>
          <CardContent className="space-y-2">
            {wo.costs.length ? (
              wo.costs.map((c) => (
                <div key={c.id} className="flex justify-between text-sm">
                  <span className="text-muted-foreground">
                    {c.type} — {c.description ?? ''}
                  </span>
                  <span>{formatCurrency(c.amount)}</span>
                </div>
              ))
            ) : (
              <p className="text-sm text-muted-foreground">No cost entries.</p>
            )}
            <div className="flex justify-between border-t pt-2 text-sm font-semibold">
              <span>Total</span>
              <span>{formatCurrency(wo.totalCost)}</span>
            </div>
          </CardContent>
        </Card>

        <Card className="lg:col-span-2">
          <CardHeader>
            <CardTitle>Checklist</CardTitle>
          </CardHeader>
          <CardContent className="space-y-2">
            {wo.checklist.length ? (
              wo.checklist.map((item) => (
                <div key={item.id} className="flex items-center gap-2 text-sm">
                  <span
                    className={
                      item.isDone
                        ? 'flex h-4 w-4 items-center justify-center rounded-sm bg-emerald-500 text-white'
                        : 'h-4 w-4 rounded-sm border'
                    }
                  >
                    {item.isDone ? <Check className="h-3 w-3" /> : null}
                  </span>
                  <span
                    className={
                      item.isDone ? 'text-muted-foreground line-through' : ''
                    }
                  >
                    {item.description}
                  </span>
                </div>
              ))
            ) : (
              <p className="text-sm text-muted-foreground">No checklist items.</p>
            )}
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Parts used</CardTitle>
          </CardHeader>
          <CardContent className="space-y-2">
            {wo.parts.length ? (
              wo.parts.map((p) => (
                <div key={p.id} className="flex justify-between text-sm">
                  <span className="text-muted-foreground">
                    {p.partName ?? p.partId} × {p.quantity}
                  </span>
                  <span>{formatCurrency(p.unitCost * p.quantity)}</span>
                </div>
              ))
            ) : (
              <p className="text-sm text-muted-foreground">No parts logged.</p>
            )}
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
