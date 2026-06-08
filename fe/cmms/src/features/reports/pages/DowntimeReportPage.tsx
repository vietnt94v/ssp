import {
  Bar,
  BarChart,
  CartesianGrid,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from 'recharts';
import { PageHeader } from '@/components/shared/PageHeader';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { ExportButton } from '@/components/shared/ExportButton';
import { ReportsNav } from '../components/ReportsNav';
import { useDowntimeReport } from '../hooks/use-reports';

export default function DowntimeReportPage() {
  const { data } = useDowntimeReport({});
  const rows = data?.byEquipment ?? [];

  return (
    <div>
      <PageHeader
        title="Reports"
        description="Downtime by equipment"
        actions={
          <ExportButton
            rows={rows}
            filename="downtime-report"
            title="Downtime Report"
            columns={[
              { header: 'Equipment', accessor: (r) => r.equipmentName },
              { header: 'Downtime (min)', accessor: (r) => r.downtimeMinutes },
              { header: 'Incidents', accessor: (r) => r.incidents },
            ]}
          />
        }
      />
      <ReportsNav />
      <Card>
        <CardHeader>
          <CardTitle>Downtime minutes</CardTitle>
        </CardHeader>
        <CardContent className="h-80">
          <ResponsiveContainer width="100%" height="100%">
            <BarChart data={rows}>
              <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
              <XAxis dataKey="equipmentName" fontSize={12} />
              <YAxis fontSize={12} />
              <Tooltip />
              <Bar dataKey="downtimeMinutes" fill="#f59e0b" name="Downtime" />
            </BarChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>
    </div>
  );
}
