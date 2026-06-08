import { NavLink } from 'react-router-dom';
import { cn } from '@/lib/utils';
import { navItems } from '@/routes/nav-items';
import { useAuthStore } from '@/stores/auth-store';

export function MobileNav() {
  const user = useAuthStore((s) => s.user);
  if (!user) return null;

  const mobileItems = navItems.filter(
    (item) => item.mobile && item.roles.includes(user.role),
  );

  return (
    <nav className="fixed inset-x-0 bottom-0 z-40 flex border-t bg-card md:hidden">
      {mobileItems.map((item) => (
        <NavLink
          key={item.to}
          to={item.to}
          className={({ isActive }) =>
            cn(
              'flex flex-1 flex-col items-center gap-1 py-2 text-xs',
              isActive ? 'text-primary' : 'text-muted-foreground',
            )
          }
        >
          <item.icon className="h-5 w-5" />
          <span>{item.label}</span>
        </NavLink>
      ))}
    </nav>
  );
}
