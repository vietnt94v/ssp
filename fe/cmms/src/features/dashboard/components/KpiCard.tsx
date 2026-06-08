import type { LucideIcon } from 'lucide-react';
import { Card, CardContent } from '@/components/ui/card';

interface KpiCardProps {
  label: string;
  value: string | number;
  icon: LucideIcon;
  accent?: string;
}

export function KpiCard({ label, value, icon: Icon, accent }: KpiCardProps) {
  return (
    <Card>
      <CardContent className="flex items-center gap-4 pt-6">
        <div
          className={`flex h-11 w-11 items-center justify-center rounded-lg ${
            accent ?? 'bg-primary/10 text-primary'
          }`}
        >
          <Icon className="h-5 w-5" />
        </div>
        <div>
          <div className="text-2xl font-semibold">{value}</div>
          <div className="text-xs text-muted-foreground">{label}</div>
        </div>
      </CardContent>
    </Card>
  );
}
