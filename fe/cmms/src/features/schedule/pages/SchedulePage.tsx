import { useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import listPlugin from '@fullcalendar/list';
import interactionPlugin from '@fullcalendar/interaction';
import { Plus } from 'lucide-react';
import { addMonths, format, subMonths } from 'date-fns';
import { PageHeader } from '@/components/shared/PageHeader';
import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';
import { useAuthStore } from '@/stores/auth-store';
import { useScheduleCalendar } from '../hooks/use-schedule';

export default function SchedulePage() {
  const navigate = useNavigate();
  const canManage = useAuthStore((s) => s.hasRole(['Admin', 'Manager']));
  const [anchor] = useState(new Date());

  const from = format(subMonths(anchor, 1), 'yyyy-MM-dd');
  const to = format(addMonths(anchor, 2), 'yyyy-MM-dd');
  const { data: events } = useScheduleCalendar(from, to);

  const calendarEvents = useMemo(
    () =>
      (events ?? []).map((e) => ({
        id: e.id,
        title: `${e.title}${e.equipmentName ? ` — ${e.equipmentName}` : ''}`,
        start: e.start,
        allDay: e.allDay,
      })),
    [events],
  );

  return (
    <div>
      <PageHeader
        title="Maintenance Schedule"
        description="Preventive maintenance calendar"
        actions={
          canManage ? (
            <Button onClick={() => navigate('/schedule/new')}>
              <Plus className="h-4 w-4" />
              New schedule
            </Button>
          ) : undefined
        }
      />
      <Card>
        <CardContent className="pt-6">
          <FullCalendar
            plugins={[
              dayGridPlugin,
              timeGridPlugin,
              listPlugin,
              interactionPlugin,
            ]}
            initialView="dayGridMonth"
            headerToolbar={{
              left: 'prev,next today',
              center: 'title',
              right: 'dayGridMonth,timeGridWeek,listWeek',
            }}
            events={calendarEvents}
            height="auto"
            eventClick={(info) => {
              if (canManage) navigate(`/schedule/${info.event.id}/edit`);
            }}
          />
        </CardContent>
      </Card>
    </div>
  );
}
