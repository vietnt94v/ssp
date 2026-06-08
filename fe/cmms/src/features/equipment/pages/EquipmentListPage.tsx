import { useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import type { ColumnDef } from '@tanstack/react-table';
import { Plus } from 'lucide-react';
import { format } from 'date-fns';
import { PageHeader } from '@/components/shared/PageHeader';
import { DataTable } from '@/components/shared/DataTable';
import { EquipmentStatusBadge } from '@/components/shared/StatusBadge';
import { ExportButton } from '@/components/shared/ExportButton';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Select } from '@/components/ui/select';
import { useAuthStore } from '@/stores/auth-store';
import { useFilterStore } from '@/stores/filter-store';
import { useEquipmentList } from '../hooks/use-equipment';
import type { Equipment, EquipmentStatus } from '@/types';

const statuses: (EquipmentStatus | 'All')[] = [
  'All',
  'Active',
  'UnderMaintenance',
  'Broken',
  'Decommissioned',
];

export default function EquipmentListPage() {
  const navigate = useNavigate();
  const canManage = useAuthStore((s) => s.hasRole(['Admin', 'Manager']));
  const { equipment: filters, setEquipmentFilters } = useFilterStore();

  const { data, isLoading } = useEquipmentList({
    status: filters.status === 'All' ? undefined : filters.status,
    pageSize: 100,
  });

  const rows = useMemo(() => {
    const items = data?.items ?? [];
    const search = filters.search.toLowerCase();
    return items.filter(
      (e) =>
        !search ||
        e.name.toLowerCase().includes(search) ||
        e.code.toLowerCase().includes(search),
    );
  }, [data, filters.search]);

  const columns = useMemo<ColumnDef<Equipment>[]>(
    () => [
      { accessorKey: 'code', header: 'Code' },
      { accessorKey: 'name', header: 'Name' },
      {
        accessorKey: 'locationName',
        header: 'Location',
        cell: ({ row }) => row.original.locationName ?? '—',
      },
      {
        accessorKey: 'status',
        header: 'Status',
        cell: ({ row }) => (
          <EquipmentStatusBadge status={row.original.status} />
        ),
      },
      {
        accessorKey: 'lastMaintenanceAt',
        header: 'Last maintenance',
        cell: ({ row }) =>
          row.original.lastMaintenanceAt
            ? format(new Date(row.original.lastMaintenanceAt), 'PP')
            : '—',
      },
    ],
    [],
  );

  return (
    <div>
      <PageHeader
        title="Equipment"
        description="Asset registry across the plant"
        actions={
          <>
            <ExportButton
              rows={rows}
              filename="equipment"
              title="Equipment"
              columns={[
                { header: 'Code', accessor: (e) => e.code },
                { header: 'Name', accessor: (e) => e.name },
                { header: 'Location', accessor: (e) => e.locationName ?? '' },
                { header: 'Status', accessor: (e) => e.status },
              ]}
            />
            {canManage ? (
              <Button onClick={() => navigate('/equipment/new')}>
                <Plus className="h-4 w-4" />
                Add equipment
              </Button>
            ) : null}
          </>
        }
      />

      <div className="mb-4 flex flex-col gap-2 sm:flex-row">
        <Input
          placeholder="Search by name or code…"
          value={filters.search}
          onChange={(e) => setEquipmentFilters({ search: e.target.value })}
          className="sm:max-w-xs"
        />
        <Select
          value={filters.status}
          onChange={(e) =>
            setEquipmentFilters({
              status: e.target.value as EquipmentStatus | 'All',
            })
          }
          className="sm:max-w-[200px]"
        >
          {statuses.map((s) => (
            <option key={s} value={s}>
              {s === 'All' ? 'All statuses' : s}
            </option>
          ))}
        </Select>
      </div>

      <DataTable
        columns={columns}
        data={rows}
        isLoading={isLoading}
        onRowClick={(row) => navigate(`/equipment/${row.id}`)}
      />
    </div>
  );
}
