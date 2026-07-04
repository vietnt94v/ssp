import { useWindowSize } from '../hooks/use-client';

export default function ClientDemoPage() {
  const { windowSize, isMobile } = useWindowSize();

  return (
    <div className='flex flex-col min-h-screen items-center justify-center bg-muted/40 p-4'>
      <p className='text-sm text-muted-foreground'>
        Window size: {windowSize.width}x{windowSize.height}
      </p>
      <p className='text-sm text-muted-foreground'>
        Is mobile: {isMobile ? 'Yes' : 'No'}
      </p>
    </div>
  );
}
