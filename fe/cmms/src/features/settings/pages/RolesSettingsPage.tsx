import { PageHeader } from '@/components/shared/PageHeader';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { SettingsNav } from '../components/SettingsNav';

const roles = [
  {
    name: 'Admin',
    description: 'Full access including settings, users, categories, locations.',
  },
  {
    name: 'Manager',
    description:
      'Work orders, equipment, schedule, technicians, spare parts, reports.',
  },
  {
    name: 'Technician',
    description: 'View and update assigned work orders, read-only schedule.',
  },
];

export default function RolesSettingsPage() {
  return (
    <div>
      <PageHeader title="Settings" description="Roles and permissions" />
      <SettingsNav />
      <div className="grid gap-4 sm:grid-cols-3">
        {roles.map((role) => (
          <Card key={role.name}>
            <CardHeader>
              <CardTitle>{role.name}</CardTitle>
            </CardHeader>
            <CardContent className="text-sm text-muted-foreground">
              {role.description}
            </CardContent>
          </Card>
        ))}
      </div>
    </div>
  );
}
