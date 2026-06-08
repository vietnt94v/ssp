import { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { PageHeader } from '@/components/shared/PageHeader';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Card, CardContent } from '@/components/ui/card';
import {
  useCreateSparePart,
  useSparePart,
  useUpdateSparePart,
} from '../hooks/use-spare-parts';

const schema = z.object({
  code: z.string().min(1, 'Code is required'),
  name: z.string().min(1, 'Name is required'),
  unitCost: z.coerce.number().min(0),
  stockQuantity: z.coerce.number().int().min(0),
  reorderLevel: z.coerce.number().int().min(0),
});

type FormValues = z.infer<typeof schema>;

export default function SparePartFormPage() {
  const { id } = useParams();
  const isEdit = Boolean(id) && id !== 'new';
  const navigate = useNavigate();
  const { data: existing } = useSparePart(isEdit ? (id ?? '') : '');
  const create = useCreateSparePart();
  const update = useUpdateSparePart(id ?? '');

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: {
      code: '',
      name: '',
      unitCost: 0,
      stockQuantity: 0,
      reorderLevel: 0,
    },
  });

  useEffect(() => {
    if (existing) {
      reset({
        code: existing.code,
        name: existing.name,
        unitCost: existing.unitCost,
        stockQuantity: existing.stockQuantity,
        reorderLevel: existing.reorderLevel,
      });
    }
  }, [existing, reset]);

  const onSubmit = async (values: FormValues) => {
    if (isEdit) {
      await update.mutateAsync(values);
    } else {
      await create.mutateAsync(values);
    }
    navigate('/spare-parts');
  };

  return (
    <div className="max-w-xl">
      <PageHeader title={isEdit ? 'Edit spare part' : 'Add spare part'} />
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
                <Label htmlFor="unitCost">Unit cost</Label>
                <Input
                  id="unitCost"
                  type="number"
                  step="0.01"
                  {...register('unitCost')}
                />
              </div>
              <div className="space-y-1.5">
                <Label htmlFor="stockQuantity">Stock quantity</Label>
                <Input
                  id="stockQuantity"
                  type="number"
                  {...register('stockQuantity')}
                />
              </div>
              <div className="space-y-1.5">
                <Label htmlFor="reorderLevel">Reorder level</Label>
                <Input
                  id="reorderLevel"
                  type="number"
                  {...register('reorderLevel')}
                />
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
                onClick={() => navigate('/spare-parts')}
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
