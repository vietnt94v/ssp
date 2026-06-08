import { Suspense } from 'react';
import { Outlet } from 'react-router-dom';
import { Sidebar } from './Sidebar';
import { TopBar } from './TopBar';
import { MobileNav } from './MobileNav';
import { useRealtime } from '@/hooks/use-realtime';

function PageFallback() {
  return (
    <div className="flex h-full items-center justify-center py-20 text-muted-foreground">
      Loading…
    </div>
  );
}

export function AppShell() {
  useRealtime();

  return (
    <div className="flex h-screen overflow-hidden">
      <Sidebar />
      <div className="flex flex-1 flex-col overflow-hidden">
        <TopBar />
        <main className="flex-1 overflow-y-auto p-4 pb-20 md:p-6 md:pb-6">
          <Suspense fallback={<PageFallback />}>
            <Outlet />
          </Suspense>
        </main>
        <MobileNav />
      </div>
    </div>
  );
}
