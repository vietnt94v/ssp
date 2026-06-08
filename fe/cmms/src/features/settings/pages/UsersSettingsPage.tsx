import { useMemo } from 'react';
import type { ColumnDef } from '@tanstack/react-table';
import { PageHeader } from '@/components/shared/PageHeader';
import { DataTable } from '@/components/shared/DataTable';
import { Badge } from '@/components/ui/badge';
import { SettingsNav } from '../components/SettingsNav';
import { useUsers } from '../hooks/use-settings';
import type { User } from '@/types';

export default function UsersSettingsPage() {
  const { data, isLoading } = useUsers();

  const columns = useMemo<ColumnDef<User>[]>(
    () => [
      { accessorKey: 'fullName', header: 'Name' },
      { accessorKey: 'email', header: 'Email' },
      {
        accessorKey: 'role',
        header: 'Role',
        cell: ({ row }) => <Badge variant="secondary">{row.original.role}</Badge>,
      },
    ],
    [],
  );

  return (
    <div>
      <PageHeader title="Settings" description="Manage users" />
      <SettingsNav />
      <DataTable
        columns={columns}
        data={data ?? []}
        isLoading={isLoading}
      />
    </div>
  );
}
