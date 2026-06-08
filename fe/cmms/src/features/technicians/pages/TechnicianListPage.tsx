import { useNavigate } from 'react-router-dom';
import { PageHeader } from '@/components/shared/PageHeader';
import { Card, CardContent } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { useTechnicians } from '../hooks/use-technicians';

export default function TechnicianListPage() {
  const navigate = useNavigate();
  const { data, isLoading } = useTechnicians({ pageSize: 100 });

  return (
    <div>
      <PageHeader
        title="Technicians"
        description="Team workload and skills"
      />
      {isLoading ? (
        <p className="text-sm text-muted-foreground">Loading…</p>
      ) : (
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {data?.items.map((tech) => (
            <Card
              key={tech.id}
              className="cursor-pointer hover:bg-accent/40"
              onClick={() => navigate(`/technicians/${tech.id}`)}
            >
              <CardContent className="space-y-3 pt-6">
                <div className="flex items-center justify-between">
                  <div>
                    <div className="font-medium">{tech.name}</div>
                    <div className="text-xs text-muted-foreground">
                      {tech.department ?? '—'}
                    </div>
                  </div>
                  <Badge variant="secondary">
                    {tech.openWorkOrderCount} open
                  </Badge>
                </div>
                <div>
                  <div className="mb-1 flex justify-between text-xs text-muted-foreground">
                    <span>Workload</span>
                    <span>{tech.workloadPercent}%</span>
                  </div>
                  <div className="h-2 w-full rounded-full bg-muted">
                    <div
                      className="h-2 rounded-full bg-primary"
                      style={{
                        width: `${Math.min(tech.workloadPercent, 100)}%`,
                      }}
                    />
                  </div>
                </div>
                <div className="flex flex-wrap gap-1">
                  {tech.skills.map((skill) => (
                    <Badge key={skill} variant="outline">
                      {skill}
                    </Badge>
                  ))}
                </div>
              </CardContent>
            </Card>
          ))}
        </div>
      )}
    </div>
  );
}
