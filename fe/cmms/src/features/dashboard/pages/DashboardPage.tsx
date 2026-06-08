import { useNavigate } from 'react-router-dom';
import {
  CartesianGrid,
  Cell,
  Legend,
  Line,
  LineChart,
  Pie,
  PieChart,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from 'recharts';
import {
  AlertTriangle,
  ClipboardList,
  Clock,
  DollarSign,
} from 'lucide-react';
import { format } from 'date-fns';
import { PageHeader } from '@/components/shared/PageHeader';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { WorkOrderStatusBadge } from '@/components/shared/StatusBadge';
import { formatCurrency } from '@/lib/utils';
import { useAuthStore } from '@/stores/auth-store';
import { KpiCard } from '../components/KpiCard';
import {
  useDashboardKpi,
  useStatusBreakdown,
  useUrgentWorkOrders,
  useWorkOrderTrend,
} from '../hooks/use-dashboard';

const COLORS = [
  '#94a3b8',
  '#0ea5e9',
  '#f59e0b',
  '#a78bfa',
  '#10b981',
  '#2563eb',
];

export default function DashboardPage() {
  const navigate = useNavigate();
  const user = useAuthStore((s) => s.user);
  const { data: kpi } = useDashboardKpi();
  const { data: trend } = useWorkOrderTrend(30);
  const { data: urgent } = useUrgentWorkOrders();
  const { data: breakdown } = useStatusBreakdown();

  return (
    <div>
      <PageHeader
        title={`Welcome, ${user?.fullName ?? ''}`}
        description="Maintenance overview"
      />

      <div className="mb-6 grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
        <KpiCard
          label="Open work orders"
          value={kpi?.openWorkOrders ?? '—'}
          icon={ClipboardList}
        />
        <KpiCard
          label="Overdue"
          value={kpi?.overdueWorkOrders ?? '—'}
          icon={Clock}
          accent="bg-amber-500/10 text-amber-600"
        />
        <KpiCard
          label="Broken equipment"
          value={kpi?.brokenEquipment ?? '—'}
          icon={AlertTriangle}
          accent="bg-destructive/10 text-destructive"
        />
        <KpiCard
          label="Monthly cost"
          value={kpi ? formatCurrency(kpi.monthlyCost) : '—'}
          icon={DollarSign}
          accent="bg-emerald-500/10 text-emerald-600"
        />
      </div>

      <div className="grid gap-4 lg:grid-cols-3">
        <Card className="lg:col-span-2">
          <CardHeader>
            <CardTitle>Work order trend (30 days)</CardTitle>
          </CardHeader>
          <CardContent className="h-72">
            <ResponsiveContainer width="100%" height="100%">
              <LineChart data={trend ?? []}>
                <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                <XAxis
                  dataKey="date"
                  tickFormatter={(d) => format(new Date(d), 'M/d')}
                  fontSize={12}
                />
                <YAxis fontSize={12} allowDecimals={false} />
                <Tooltip />
                <Legend />
                <Line
                  type="monotone"
                  dataKey="created"
                  stroke="#2563eb"
                  name="Created"
                />
                <Line
                  type="monotone"
                  dataKey="completed"
                  stroke="#10b981"
                  name="Completed"
                />
              </LineChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>By status</CardTitle>
          </CardHeader>
          <CardContent className="h-72">
            <ResponsiveContainer width="100%" height="100%">
              <PieChart>
                <Pie
                  data={breakdown ?? []}
                  dataKey="count"
                  nameKey="status"
                  innerRadius={50}
                  outerRadius={80}
                >
                  {(breakdown ?? []).map((_, index) => (
                    <Cell key={index} fill={COLORS[index % COLORS.length]} />
                  ))}
                </Pie>
                <Tooltip />
                <Legend />
              </PieChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        <Card className="lg:col-span-3">
          <CardHeader>
            <CardTitle>Urgent work orders</CardTitle>
          </CardHeader>
          <CardContent className="space-y-2">
            {urgent?.length ? (
              urgent.map((wo) => (
                <button
                  key={wo.id}
                  type="button"
                  onClick={() => navigate(`/work-orders/${wo.id}`)}
                  className="flex w-full items-center justify-between rounded-md border p-3 text-left hover:bg-accent"
                >
                  <div>
                    <div className="font-medium">{wo.number}</div>
                    <div className="text-xs text-muted-foreground">
                      {wo.equipmentName ?? '—'}
                    </div>
                  </div>
                  <WorkOrderStatusBadge status={wo.status} />
                </button>
              ))
            ) : (
              <p className="text-sm text-muted-foreground">
                No urgent work orders.
              </p>
            )}
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
