import { useState } from 'react';
import { format } from 'date-fns';
import { PageHeader } from '@/components/shared/PageHeader';
import { Card, CardContent } from '@/components/ui/card';
import { Select } from '@/components/ui/select';
import { ReportsNav } from '../components/ReportsNav';
import { useEquipmentList } from '@/features/equipment/hooks/use-equipment';
import { useEquipmentHistory } from '../hooks/use-reports';

export default function EquipmentHistoryPage() {
  const { data: equipment } = useEquipmentList({ pageSize: 100 });
  const [equipmentId, setEquipmentId] = useState('');
  const { data: history } = useEquipmentHistory(equipmentId);

  return (
    <div>
      <PageHeader title="Reports" description="Equipment maintenance history" />
      <ReportsNav />

      <div className="mb-4 max-w-sm">
        <Select
          value={equipmentId}
          onChange={(e) => setEquipmentId(e.target.value)}
        >
          <option value="">Select equipment…</option>
          {equipment?.items.map((e) => (
            <option key={e.id} value={e.id}>
              {e.code} — {e.name}
            </option>
          ))}
        </Select>
      </div>

      <Card>
        <CardContent className="space-y-3 pt-6">
          {!equipmentId ? (
            <p className="text-sm text-muted-foreground">
              Select an equipment to view its history.
            </p>
          ) : history?.events.length ? (
            <ol className="relative space-y-4 border-l pl-4">
              {history.events.map((event) => (
                <li key={event.workOrderNumber} className="relative">
                  <span className="absolute -left-[1.4rem] top-1 h-3 w-3 rounded-full bg-primary" />
                  <div className="text-sm font-medium">
                    {event.workOrderNumber} — {event.type}
                  </div>
                  <div className="text-xs text-muted-foreground">
                    {format(new Date(event.completedAt), 'PPp')} ·{' '}
                    {event.downtimeMinutes} min downtime
                  </div>
                  <p className="text-sm">{event.summary}</p>
                </li>
              ))}
            </ol>
          ) : (
            <p className="text-sm text-muted-foreground">
              No history for this equipment.
            </p>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
