import { useNavigate } from 'react-router-dom';
import { Bell, LogOut, Menu, User as UserIcon } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { useAuthStore } from '@/stores/auth-store';
import { useUiStore } from '@/stores/ui-store';
import { useNotificationStore } from '@/stores/notification-store';

export function TopBar() {
  const navigate = useNavigate();
  const { user, logout } = useAuthStore();
  const toggleSidebar = useUiStore((s) => s.toggleSidebar);
  const unreadCount = useNotificationStore((s) => s.unreadCount);

  const handleLogout = () => {
    logout();
    navigate('/auth/login');
  };

  return (
    <header className="flex h-14 items-center justify-between border-b bg-card px-4">
      <Button
        variant="ghost"
        size="icon"
        className="md:inline-flex"
        onClick={toggleSidebar}
      >
        <Menu className="h-5 w-5" />
      </Button>

      <div className="flex items-center gap-2">
        <Button
          variant="ghost"
          size="icon"
          className="relative"
          onClick={() => navigate('/alerts')}
        >
          <Bell className="h-5 w-5" />
          {unreadCount > 0 ? (
            <Badge
              variant="destructive"
              className="absolute -right-1 -top-1 h-5 min-w-5 justify-center px-1 text-[10px]"
            >
              {unreadCount}
            </Badge>
          ) : null}
        </Button>

        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="ghost" size="icon">
              <UserIcon className="h-5 w-5" />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuLabel>
              {user?.fullName}
              <div className="text-xs font-normal text-muted-foreground">
                {user?.role}
              </div>
            </DropdownMenuLabel>
            <DropdownMenuSeparator />
            <DropdownMenuItem onClick={handleLogout}>
              <LogOut className="h-4 w-4" />
              Sign out
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
    </header>
  );
}
