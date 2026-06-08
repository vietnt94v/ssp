import { PageHeader } from '@/components/shared/PageHeader';
import { Card, CardContent } from '@/components/ui/card';
import { ReportsNav } from '../components/ReportsNav';
import { KpiCard } from '@/features/dashboard/components/KpiCard';
import { Clock, Gauge, Timer, TrendingUp } from 'lucide-react';
import { useKpiReport } from '../hooks/use-reports';

export default function KpiReportPage() {
  const { data } = useKpiReport({});

  return (
    <div>
      <PageHeader title="Reports" description="Maintenance KPIs" />
      <ReportsNav />
      <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
        <KpiCard
          label="MTTR (hours)"
          value={data?.mttrHours ?? '—'}
          icon={Timer}
        />
        <KpiCard
          label="MTBF (hours)"
          value={data?.mtbfHours ?? '—'}
          icon={Clock}
        />
        <KpiCard
          label="OEE impact %"
          value={data?.oeeImpactPercent ?? '—'}
          icon={Gauge}
        />
        <KpiCard
          label="Overdue rate %"
          value={data?.overdueRatePercent ?? '—'}
          icon={TrendingUp}
        />
      </div>
      <Card className="mt-4">
        <CardContent className="pt-6 text-sm text-muted-foreground">
          Period: {data?.from ?? '—'} to {data?.to ?? '—'}
        </CardContent>
      </Card>
    </div>
  );
}
