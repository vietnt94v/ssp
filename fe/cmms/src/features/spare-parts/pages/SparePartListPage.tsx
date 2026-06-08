import { useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import type { ColumnDef } from '@tanstack/react-table';
import { Plus } from 'lucide-react';
import { PageHeader } from '@/components/shared/PageHeader';
import { DataTable } from '@/components/shared/DataTable';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { formatCurrency } from '@/lib/utils';
import { useSpareParts } from '../hooks/use-spare-parts';
import type { SparePart } from '@/types';

export default function SparePartListPage() {
  const navigate = useNavigate();
  const { data, isLoading } = useSpareParts({ pageSize: 100 });

  const columns = useMemo<ColumnDef<SparePart>[]>(
    () => [
      { accessorKey: 'code', header: 'Code' },
      { accessorKey: 'name', header: 'Name' },
      {
        accessorKey: 'unitCost',
        header: 'Unit cost',
        cell: ({ row }) => formatCurrency(row.original.unitCost),
      },
      {
        accessorKey: 'stockQuantity',
        header: 'In stock',
        cell: ({ row }) => {
          const part = row.original;
          const low = part.stockQuantity <= part.reorderLevel;
          return (
            <span className="flex items-center gap-2">
              {part.stockQuantity}
              {low ? <Badge variant="destructive">Low</Badge> : null}
            </span>
          );
        },
      },
      { accessorKey: 'reorderLevel', header: 'Reorder level' },
    ],
    [],
  );

  return (
    <div>
      <PageHeader
        title="Spare Parts"
        description="Inventory and stock levels"
        actions={
          <Button onClick={() => navigate('/spare-parts/new')}>
            <Plus className="h-4 w-4" />
            Add part
          </Button>
        }
      />
      <DataTable
        columns={columns}
        data={data?.items ?? []}
        isLoading={isLoading}
        onRowClick={(row) => navigate(`/spare-parts/${row.id}`)}
      />
    </div>
  );
}
