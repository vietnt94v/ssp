import { useNavigate } from 'react-router-dom';
import { PriorityBadge } from '@/components/shared/StatusBadge';
import type { WorkOrderStatus } from '@/types';
import type { KanbanBoard } from '../api/work-orders-api';

const columns: WorkOrderStatus[] = [
  'Draft',
  'Assigned',
  'InProgress',
  'OnHold',
  'Completed',
  'Closed',
];

export function WorkOrderKanban({ board }: { board: KanbanBoard }) {
  const navigate = useNavigate();

  return (
    <div className="grid grid-cols-1 gap-3 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-6">
      {columns.map((status) => (
        <div key={status} className="rounded-md bg-muted/40 p-2">
          <div className="mb-2 flex items-center justify-between px-1">
            <span className="text-sm font-medium">{status}</span>
            <span className="text-xs text-muted-foreground">
              {board[status]?.length ?? 0}
            </span>
          </div>
          <div className="space-y-2">
            {board[status]?.map((wo) => (
              <button
                key={wo.id}
                type="button"
                onClick={() => navigate(`/work-orders/${wo.id}`)}
                className="w-full rounded-md border bg-card p-2 text-left text-sm shadow-sm hover:bg-accent"
              >
                <div className="flex items-center justify-between">
                  <span className="font-medium">{wo.number}</span>
                  <PriorityBadge priority={wo.priority} />
                </div>
                <div className="mt-1 truncate text-xs text-muted-foreground">
                  {wo.equipmentName ?? '—'}
                </div>
              </button>
            ))}
          </div>
        </div>
      ))}
    </div>
  );
}
