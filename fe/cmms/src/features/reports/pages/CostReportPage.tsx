import {
  Bar,
  BarChart,
  CartesianGrid,
  Legend,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from 'recharts';
import { PageHeader } from '@/components/shared/PageHeader';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { ExportButton } from '@/components/shared/ExportButton';
import { formatCurrency } from '@/lib/utils';
import { ReportsNav } from '../components/ReportsNav';
import { useCostReport } from '../hooks/use-reports';

export default function CostReportPage() {
  const { data } = useCostReport({});
  const rows = data?.byEquipment ?? [];

  return (
    <div>
      <PageHeader
        title="Reports"
        description="Cost breakdown"
        actions={
          <ExportButton
            rows={rows}
            filename="cost-report"
            title="Cost Report"
            columns={[
              { header: 'Equipment', accessor: (r) => r.equipmentName },
              { header: 'Labor', accessor: (r) => r.laborCost },
              { header: 'Parts', accessor: (r) => r.partsCost },
              { header: 'Total', accessor: (r) => r.totalCost },
            ]}
          />
        }
      />
      <ReportsNav />
      <Card>
        <CardHeader>
          <CardTitle>
            Total {data ? formatCurrency(data.totalCost) : '—'} (labor{' '}
            {data ? formatCurrency(data.laborCost) : '—'}, parts{' '}
            {data ? formatCurrency(data.partsCost) : '—'})
          </CardTitle>
        </CardHeader>
        <CardContent className="h-80">
          <ResponsiveContainer width="100%" height="100%">
            <BarChart data={rows}>
              <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
              <XAxis dataKey="equipmentName" fontSize={12} />
              <YAxis fontSize={12} />
              <Tooltip />
              <Legend />
              <Bar dataKey="laborCost" stackId="a" fill="#2563eb" name="Labor" />
              <Bar dataKey="partsCost" stackId="a" fill="#10b981" name="Parts" />
            </BarChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>
    </div>
  );
}
