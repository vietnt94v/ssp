import { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { format } from 'date-fns';
import { Pencil, Trash2 } from 'lucide-react';
import { PageHeader } from '@/components/shared/PageHeader';
import { EquipmentStatusBadge } from '@/components/shared/StatusBadge';
import { ConfirmDialog } from '@/components/shared/ConfirmDialog';
import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { useAuthStore } from '@/stores/auth-store';
import { useWorkOrders } from '@/features/work-orders/hooks/use-work-orders';
import { WorkOrderStatusBadge } from '@/components/shared/StatusBadge';
import { useDeleteEquipment, useEquipment } from '../hooks/use-equipment';

function Field({ label, value }: { label: string; value: string }) {
  return (
    <div>
      <div className="text-xs text-muted-foreground">{label}</div>
      <div className="text-sm font-medium">{value}</div>
    </div>
  );
}

export default function EquipmentDetailPage() {
  const { id = '' } = useParams();
  const navigate = useNavigate();
  const canManage = useAuthStore((s) => s.hasRole(['Admin', 'Manager']));
  const { data: equipment, isLoading } = useEquipment(id);
  const { data: workOrders } = useWorkOrders({ equipmentId: id, pageSize: 50 });
  const deleteEquipment = useDeleteEquipment();
  const [confirmOpen, setConfirmOpen] = useState(false);

  if (isLoading || !equipment) {
    return <div className="text-muted-foreground">Loading…</div>;
  }

  return (
    <div>
      <PageHeader
        title={equipment.name}
        description={equipment.code}
        actions={
          canManage ? (
            <>
              <Button
                variant="outline"
                onClick={() => navigate(`/equipment/${id}/edit`)}
              >
                <Pencil className="h-4 w-4" />
                Edit
              </Button>
              <Button
                variant="destructive"
                onClick={() => setConfirmOpen(true)}
              >
                <Trash2 className="h-4 w-4" />
                Delete
              </Button>
            </>
          ) : undefined
        }
      />

      <div className="mb-4">
        <EquipmentStatusBadge status={equipment.status} />
      </div>

      <Tabs defaultValue="specs">
        <TabsList>
          <TabsTrigger value="specs">Specifications</TabsTrigger>
          <TabsTrigger value="history">Work order history</TabsTrigger>
        </TabsList>

        <TabsContent value="specs">
          <Card>
            <CardContent className="grid gap-4 pt-6 sm:grid-cols-2 md:grid-cols-3">
              <Field label="Category" value={equipment.categoryName ?? '—'} />
              <Field label="Location" value={equipment.locationName ?? '—'} />
              <Field
                label="Manufacturer"
                value={equipment.manufacturer ?? '—'}
              />
              <Field
                label="Install date"
                value={
                  equipment.installDate
                    ? format(new Date(equipment.installDate), 'PP')
                    : '—'
                }
              />
              <Field
                label="Last maintenance"
                value={
                  equipment.lastMaintenanceAt
                    ? format(new Date(equipment.lastMaintenanceAt), 'PP')
                    : '—'
                }
              />
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="history">
          <Card>
            <CardContent className="space-y-2 pt-6">
              {workOrders?.items.length ? (
                workOrders.items.map((wo) => (
                  <button
                    key={wo.id}
                    type="button"
                    onClick={() => navigate(`/work-orders/${wo.id}`)}
                    className="flex w-full items-center justify-between rounded-md border p-3 text-left hover:bg-accent"
                  >
                    <div>
                      <div className="font-medium">{wo.number}</div>
                      <div className="text-xs text-muted-foreground">
                        {wo.type}
                      </div>
                    </div>
                    <WorkOrderStatusBadge status={wo.status} />
                  </button>
                ))
              ) : (
                <p className="text-sm text-muted-foreground">
                  No work orders for this equipment.
                </p>
              )}
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>

      <ConfirmDialog
        open={confirmOpen}
        onOpenChange={setConfirmOpen}
        title="Delete equipment?"
        description="This will soft-delete the equipment record."
        destructive
        confirmLabel="Delete"
        onConfirm={async () => {
          await deleteEquipment.mutateAsync(id);
          navigate('/equipment');
        }}
      />
    </div>
  );
}
