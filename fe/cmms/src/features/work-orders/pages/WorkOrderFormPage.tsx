import { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { PageHeader } from '@/components/shared/PageHeader';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Select } from '@/components/ui/select';
import { Textarea } from '@/components/ui/textarea';
import { Card, CardContent } from '@/components/ui/card';
import { useEquipmentList } from '@/features/equipment/hooks/use-equipment';
import { useTechnicians } from '@/features/technicians/hooks/use-technicians';
import {
  useCreateWorkOrder,
  useUpdateWorkOrder,
  useWorkOrder,
} from '../hooks/use-work-orders';
import { workOrderSchema } from '../schemas/work-order-schema';
import type { WorkOrderFormValues } from '../schemas/work-order-schema';

const types = ['Corrective', 'Preventive', 'Inspection'] as const;
const priorities = ['Low', 'Medium', 'High', 'Critical'] as const;

export default function WorkOrderFormPage() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();

  const { data: equipment } = useEquipmentList({ pageSize: 100 });
  const { data: technicians } = useTechnicians({ pageSize: 100 });
  const { data: existing } = useWorkOrder(id ?? '');

  const create = useCreateWorkOrder();
  const update = useUpdateWorkOrder(id ?? '');

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<WorkOrderFormValues>({
    resolver: zodResolver(workOrderSchema),
    defaultValues: {
      type: 'Corrective',
      priority: 'Medium',
      equipmentId: '',
      assignedTechnicianId: '',
      description: '',
      deadline: '',
    },
  });

  useEffect(() => {
    if (existing) {
      reset({
        type: existing.type,
        priority: existing.priority,
        equipmentId: existing.equipmentId,
        assignedTechnicianId: existing.assignedTechnicianId ?? '',
        description: existing.description,
        deadline: existing.deadline
          ? existing.deadline.substring(0, 16)
          : '',
      });
    }
  }, [existing, reset]);

  const onSubmit = async (values: WorkOrderFormValues) => {
    const payload = {
      type: values.type,
      priority: values.priority,
      equipmentId: values.equipmentId,
      assignedTechnicianId: values.assignedTechnicianId || null,
      description: values.description,
      deadline: values.deadline
        ? new Date(values.deadline).toISOString()
        : null,
    };
    if (isEdit) {
      await update.mutateAsync(payload);
      navigate(`/work-orders/${id}`);
    } else {
      const created = await create.mutateAsync(payload);
      navigate(`/work-orders/${created.id}`);
    }
  };

  return (
    <div className="max-w-2xl">
      <PageHeader title={isEdit ? 'Edit work order' : 'New work order'} />
      <Card>
        <CardContent className="pt-6">
          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <div className="grid gap-4 sm:grid-cols-2">
              <div className="space-y-1.5">
                <Label htmlFor="type">Type</Label>
                <Select id="type" {...register('type')}>
                  {types.map((t) => (
                    <option key={t} value={t}>
                      {t}
                    </option>
                  ))}
                </Select>
              </div>
              <div className="space-y-1.5">
                <Label htmlFor="priority">Priority</Label>
                <Select id="priority" {...register('priority')}>
                  {priorities.map((p) => (
                    <option key={p} value={p}>
                      {p}
                    </option>
                  ))}
                </Select>
              </div>
              <div className="space-y-1.5">
                <Label htmlFor="equipmentId">Equipment</Label>
                <Select id="equipmentId" {...register('equipmentId')}>
                  <option value="">Select…</option>
                  {equipment?.items.map((e) => (
                    <option key={e.id} value={e.id}>
                      {e.code} — {e.name}
                    </option>
                  ))}
                </Select>
                {errors.equipmentId ? (
                  <p className="text-xs text-destructive">
                    {errors.equipmentId.message}
                  </p>
                ) : null}
              </div>
              <div className="space-y-1.5">
                <Label htmlFor="assignedTechnicianId">Technician</Label>
                <Select
                  id="assignedTechnicianId"
                  {...register('assignedTechnicianId')}
                >
                  <option value="">Unassigned</option>
                  {technicians?.items.map((t) => (
                    <option key={t.id} value={t.id}>
                      {t.name}
                    </option>
                  ))}
                </Select>
              </div>
              <div className="space-y-1.5 sm:col-span-2">
                <Label htmlFor="deadline">Deadline</Label>
                <Input
                  id="deadline"
                  type="datetime-local"
                  {...register('deadline')}
                />
              </div>
              <div className="space-y-1.5 sm:col-span-2">
                <Label htmlFor="description">Description</Label>
                <Textarea
                  id="description"
                  rows={4}
                  {...register('description')}
                />
                {errors.description ? (
                  <p className="text-xs text-destructive">
                    {errors.description.message}
                  </p>
                ) : null}
              </div>
            </div>
            <div className="flex gap-2">
              <Button
                type="submit"
                disabled={create.isPending || update.isPending}
              >
                {isEdit ? 'Save changes' : 'Create'}
              </Button>
              <Button
                type="button"
                variant="outline"
                onClick={() => navigate('/work-orders')}
              >
                Cancel
              </Button>
            </div>
          </form>
        </CardContent>
      </Card>
    </div>
  );
}
