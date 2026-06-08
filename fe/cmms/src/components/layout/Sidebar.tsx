import { NavLink } from 'react-router-dom';
import { Wrench } from 'lucide-react';
import { cn } from '@/lib/utils';
import { navItems } from '@/routes/nav-items';
import { useAuthStore } from '@/stores/auth-store';
import { useUiStore } from '@/stores/ui-store';

export function Sidebar() {
  const user = useAuthStore((s) => s.user);
  const sidebarOpen = useUiStore((s) => s.sidebarOpen);

  if (!user) return null;

  const visibleItems = navItems.filter((item) =>
    item.roles.includes(user.role),
  );

  return (
    <aside
      className={cn(
        'hidden border-r bg-card md:flex md:flex-col',
        sidebarOpen ? 'md:w-60' : 'md:w-16',
      )}
    >
      <div className="flex h-14 items-center gap-2 border-b px-4">
        <Wrench className="h-6 w-6 text-primary" />
        {sidebarOpen ? <span className="font-semibold">CMMS</span> : null}
      </div>
      <nav className="flex-1 space-y-1 p-2">
        {visibleItems.map((item) => (
          <NavLink
            key={item.to}
            to={item.to}
            className={({ isActive }) =>
              cn(
                'flex items-center gap-3 rounded-md px-3 py-2 text-sm font-medium transition-colors',
                isActive
                  ? 'bg-primary text-primary-foreground'
                  : 'text-muted-foreground hover:bg-accent hover:text-accent-foreground',
              )
            }
          >
            <item.icon className="h-4 w-4 shrink-0" />
            {sidebarOpen ? <span>{item.label}</span> : null}
          </NavLink>
        ))}
      </nav>
    </aside>
  );
}
