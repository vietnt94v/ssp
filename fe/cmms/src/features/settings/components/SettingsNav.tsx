import { NavLink } from 'react-router-dom';
import { cn } from '@/lib/utils';

const tabs = [
  { to: '/settings/users', label: 'Users' },
  { to: '/settings/roles', label: 'Roles' },
  { to: '/settings/categories', label: 'Categories' },
  { to: '/settings/locations', label: 'Locations' },
];

export function SettingsNav() {
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
