import { useNavigate, useParams } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { PageHeader } from '@/components/shared/PageHeader';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Select } from '@/components/ui/select';
import { Card, CardContent } from '@/components/ui/card';
import { useEquipmentList } from '@/features/equipment/hooks/use-equipment';
import { useCreateSchedule, useUpdateSchedule } from '../hooks/use-schedule';

const scheduleSchema = z.object({
  equipmentId: z.string().uuid('Select equipment'),
  title: z.string().min(1, 'Title is required'),
  frequency: z.enum(['Daily', 'Weekly', 'Monthly', 'ByMeter']),
  intervalValue: z.coerce.number().int().min(1),
  meterThreshold: z.coerce.number().optional(),
  nextDueDate: z.string().min(1, 'Next due date is required'),
  isActive: z.boolean(),
});

type ScheduleFormValues = z.infer<typeof scheduleSchema>;

const frequencies = ['Daily', 'Weekly', 'Monthly', 'ByMeter'] as const;

export default function ScheduleFormPage() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();
  const { data: equipment } = useEquipmentList({ pageSize: 100 });
  const create = useCreateSchedule();
  const update = useUpdateSchedule(id ?? '');

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ScheduleFormValues>({
    resolver: zodResolver(scheduleSchema),
    defaultValues: {
      equipmentId: '',
      title: '',
      frequency: 'Monthly',
      intervalValue: 1,
      nextDueDate: '',
      isActive: true,
    },
  });

  const onSubmit = async (values: ScheduleFormValues) => {
    const payload = {
      ...values,
      meterThreshold: values.meterThreshold ?? null,
    };
    if (isEdit) {
      await update.mutateAsync(payload);
    } else {
      await create.mutateAsync(payload);
    }
    navigate('/schedule');
  };

  return (
    <div className="max-w-2xl">
      <PageHeader title={isEdit ? 'Edit schedule' : 'New schedule'} />
      <Card>
        <CardContent className="pt-6">
          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <div className="grid gap-4 sm:grid-cols-2">
              <div className="space-y-1.5 sm:col-span-2">
                <Label htmlFor="title">Title</Label>
                <Input id="title" {...register('title')} />
                {errors.title ? (
                  <p className="text-xs text-destructive">
                    {errors.title.message}
                  </p>
                ) : null}
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
                <Label htmlFor="frequency">Frequency</Label>
                <Select id="frequency" {...register('frequency')}>
                  {frequencies.map((f) => (
                    <option key={f} value={f}>
                      {f}
                    </option>
                  ))}
                </Select>
              </div>
              <div className="space-y-1.5">
                <Label htmlFor="intervalValue">Interval</Label>
                <Input
                  id="intervalValue"
                  type="number"
                  min={1}
                  {...register('intervalValue')}
                />
              </div>
              <div className="space-y-1.5">
                <Label htmlFor="meterThreshold">Meter threshold</Label>
                <Input
                  id="meterThreshold"
                  type="number"
                  {...register('meterThreshold')}
                />
              </div>
              <div className="space-y-1.5">
                <Label htmlFor="nextDueDate">Next due date</Label>
                <Input
                  id="nextDueDate"
                  type="date"
                  {...register('nextDueDate')}
                />
                {errors.nextDueDate ? (
                  <p className="text-xs text-destructive">
                    {errors.nextDueDate.message}
                  </p>
                ) : null}
              </div>
              <label className="flex items-center gap-2 text-sm">
                <input type="checkbox" {...register('isActive')} />
                Active
              </label>
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
                onClick={() => navigate('/schedule')}
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
