import { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { PageHeader } from '@/components/shared/PageHeader';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Select } from '@/components/ui/select';
import { Card, CardContent } from '@/components/ui/card';
import { useCategories, useLocations } from '@/features/settings/hooks/use-lookups';
import {
  useCreateEquipment,
  useEquipment,
  useUpdateEquipment,
} from '../hooks/use-equipment';
import { equipmentSchema } from '../schemas/equipment-schema';
import type { EquipmentFormValues } from '../schemas/equipment-schema';

const statuses = ['Active', 'UnderMaintenance', 'Broken', 'Decommissioned'];

export default function EquipmentFormPage() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();

  const { data: categories } = useCategories();
  const { data: locations } = useLocations();
  const { data: existing } = useEquipment(id ?? '');

  const create = useCreateEquipment();
  const update = useUpdateEquipment(id ?? '');

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<EquipmentFormValues>({
    resolver: zodResolver(equipmentSchema),
    defaultValues: {
      code: '',
      name: '',
      categoryId: '',
      locationId: '',
      manufacturer: '',
      installDate: '',
      status: 'Active',
    },
  });

  useEffect(() => {
    if (existing) {
      reset({
        code: existing.code,
        name: existing.name,
        categoryId: existing.categoryId,
        locationId: existing.locationId,
        manufacturer: existing.manufacturer ?? '',
        installDate: existing.installDate ?? '',
        status: existing.status,
      });
    }
  }, [existing, reset]);

  const onSubmit = async (values: EquipmentFormValues) => {
    const payload = {
      ...values,
      manufacturer: values.manufacturer || null,
      installDate: values.installDate || null,
    };
    if (isEdit) {
      await update.mutateAsync(payload);
    } else {
      await create.mutateAsync(payload);
    }
    navigate('/equipment');
  };

  return (
    <div className="max-w-2xl">
      <PageHeader title={isEdit ? 'Edit equipment' : 'Add equipment'} />
      <Card>
        <CardContent className="pt-6">
          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <div className="grid gap-4 sm:grid-cols-2">
              <div className="space-y-1.5">
                <Label htmlFor="code">Code</Label>
                <Input id="code" {...register('code')} />
                {errors.code ? (
                  <p className="text-xs text-destructive">
                    {errors.code.message}
                  </p>
                ) : null}
              </div>
              <div className="space-y-1.5">
                <Label htmlFor="name">Name</Label>
                <Input id="name" {...register('name')} />
                {errors.name ? (
                  <p className="text-xs text-destructive">
                    {errors.name.message}
                  </p>
                ) : null}
              </div>
              <div className="space-y-1.5">
                <Label htmlFor="categoryId">Category</Label>
                <Select id="categoryId" {...register('categoryId')}>
                  <option value="">Select…</option>
                  {categories?.map((c) => (
                    <option key={c.id} value={c.id}>
                      {c.name}
                    </option>
                  ))}
                </Select>
                {errors.categoryId ? (
                  <p className="text-xs text-destructive">
                    {errors.categoryId.message}
                  </p>
                ) : null}
              </div>
              <div className="space-y-1.5">
                <Label htmlFor="locationId">Location</Label>
                <Select id="locationId" {...register('locationId')}>
                  <option value="">Select…</option>
                  {locations?.map((l) => (
                    <option key={l.id} value={l.id}>
                      {l.name}
                    </option>
                  ))}
                </Select>
                {errors.locationId ? (
                  <p className="text-xs text-destructive">
                    {errors.locationId.message}
                  </p>
                ) : null}
              </div>
              <div className="space-y-1.5">
                <Label htmlFor="manufacturer">Manufacturer</Label>
                <Input id="manufacturer" {...register('manufacturer')} />
              </div>
              <div className="space-y-1.5">
                <Label htmlFor="installDate">Install date</Label>
                <Input id="installDate" type="date" {...register('installDate')} />
              </div>
              <div className="space-y-1.5">
                <Label htmlFor="status">Status</Label>
                <Select id="status" {...register('status')}>
                  {statuses.map((s) => (
                    <option key={s} value={s}>
                      {s}
                    </option>
                  ))}
                </Select>
              </div>
            </div>
            <div className="flex gap-2">
              <Button type="submit" disabled={create.isPending || update.isPending}>
                {isEdit ? 'Save changes' : 'Create'}
              </Button>
              <Button
                type="button"
                variant="outline"
                onClick={() => navigate('/equipment')}
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
