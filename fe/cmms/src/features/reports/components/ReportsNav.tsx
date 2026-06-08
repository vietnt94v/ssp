import { NavLink } from 'react-router-dom';
import { cn } from '@/lib/utils';

const tabs = [
  { to: '/reports/kpi', label: 'KPI' },
  { to: '/reports/cost', label: 'Cost' },
  { to: '/reports/equipment-history', label: 'Equipment history' },
  { to: '/reports/downtime', label: 'Downtime' },
];

export function ReportsNav() {
  return (
    <div className="mb-4 flex flex-wrap gap-1 rounded-lg bg-muted p-1">
      {tabs.map((tab) => (
        <NavLink
          key={tab.to}
          to={tab.to}
          className={({ isActive }) =>
            cn(
              'rounded-md px-3 py-1.5 text-sm font-medium',
              isActive
                ? 'bg-background shadow'
                : 'text-muted-foreground hover:text-foreground',
            )
          }
        >
          {tab.label}
        </NavLink>
      ))}
    </div>
  );
}
