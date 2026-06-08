import { useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import type { ColumnDef } from '@tanstack/react-table';
import { LayoutGrid, List, Plus } from 'lucide-react';
import { format } from 'date-fns';
import { PageHeader } from '@/components/shared/PageHeader';
import { DataTable } from '@/components/shared/DataTable';
import {
  PriorityBadge,
  WorkOrderStatusBadge,
} from '@/components/shared/StatusBadge';
import { ExportButton } from '@/components/shared/ExportButton';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Select } from '@/components/ui/select';
import { useAuthStore } from '@/stores/auth-store';
import { useFilterStore } from '@/stores/filter-store';
import { useWorkOrders, useWorkOrdersKanban } from '../hooks/use-work-orders';
import { WorkOrderKanban } from '../components/WorkOrderKanban';
import type { WorkOrderListItem } from '../api/work-orders-api';
import type { WorkOrderStatus } from '@/types';

const statuses: (WorkOrderStatus | 'All')[] = [
  'All',
  'Draft',
  'Assigned',
  'InProgress',
  'OnHold',
  'Completed',
  'Closed',
];

export default function WorkOrderListPage() {
  const navigate = useNavigate();
  const user = useAuthStore((s) => s.user);
  const canManage = useAuthStore((s) => s.hasRole(['Admin', 'Manager']));
  const { workOrders: filters, setWorkOrderFilters } = useFilterStore();
  const [view, setView] = useState<'table' | 'kanban'>('table');

  const params = {
    status: filters.status === 'All' ? undefined : filters.status,
    pageSize: 100,
  };

  const { data, isLoading } = useWorkOrders(params);
  const { data: board } = useWorkOrdersKanban();

  const rows = useMemo(() => {
    const items = data?.items ?? [];
    const search = filters.search.toLowerCase();
    return items.filter(
      (w) =>
        !search ||
        w.number.toLowerCase().includes(search) ||
        (w.equipmentName ?? '').toLowerCase().includes(search),
    );
  }, [data, filters.search]);

  const columns = useMemo<ColumnDef<WorkOrderListItem>[]>(
    () => [
      { accessorKey: 'number', header: 'Number' },
      { accessorKey: 'type', header: 'Type' },
      {
        accessorKey: 'priority',
        header: 'Priority',
        cell: ({ row }) => <PriorityBadge priority={row.original.priority} />,
      },
      {
        accessorKey: 'status',
        header: 'Status',
        cell: ({ row }) => (
          <WorkOrderStatusBadge status={row.original.status} />
        ),
      },
      {
        accessorKey: 'equipmentName',
        header: 'Equipment',
        cell: ({ row }) => row.original.equipmentName ?? '—',
      },
      {
        accessorKey: 'assignedTechnicianName',
        header: 'Technician',
        cell: ({ row }) => row.original.assignedTechnicianName ?? '—',
      },
      {
        accessorKey: 'deadline',
        header: 'Deadline',
        cell: ({ row }) =>
          row.original.deadline
            ? format(new Date(row.original.deadline), 'PP')
            : '—',
      },
    ],
    [],
  );

  return (
    <div>
      <PageHeader
        title="Work Orders"
        description={
          user?.role === 'Technician'
            ? 'Your assigned tasks'
            : 'All maintenance work orders'
        }
        actions={
          <>
            <div className="flex rounded-md border">
              <Button
                variant={view === 'table' ? 'secondary' : 'ghost'}
                size="icon"
                onClick={() => setView('table')}
              >
                <List className="h-4 w-4" />
              </Button>
              <Button
                variant={view === 'kanban' ? 'secondary' : 'ghost'}
                size="icon"
                onClick={() => setView('kanban')}
              >
                <LayoutGrid className="h-4 w-4" />
              </Button>
            </div>
            <ExportButton
              rows={rows}
              filename="work-orders"
              title="Work Orders"
              columns={[
                { header: 'Number', accessor: (w) => w.number },
                { header: 'Type', accessor: (w) => w.type },
                { header: 'Priority', accessor: (w) => w.priority },
                { header: 'Status', accessor: (w) => w.status },
                { header: 'Equipment', accessor: (w) => w.equipmentName ?? '' },
              ]}
            />
            {canManage ? (
              <Button onClick={() => navigate('/work-orders/new')}>
                <Plus className="h-4 w-4" />
                New work order
              </Button>
            ) : null}
          </>
        }
      />

      {view === 'table' ? (
        <>
          <div className="mb-4 flex flex-col gap-2 sm:flex-row">
            <Input
              placeholder="Search by number or equipment…"
              value={filters.search}
              onChange={(e) => setWorkOrderFilters({ search: e.target.value })}
              className="sm:max-w-xs"
            />
            <Select
              value={filters.status}
              onChange={(e) =>
                setWorkOrderFilters({
                  status: e.target.value as WorkOrderStatus | 'All',
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
            onRowClick={(row) => navigate(`/work-orders/${row.id}`)}
          />
        </>
      ) : board ? (
        <WorkOrderKanban board={board} />
      ) : (
        <div className="text-muted-foreground">Loading…</div>
      )}
    </div>
  );
}
