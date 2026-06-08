import { PageHeader } from '@/components/shared/PageHeader';
import { Card, CardContent } from '@/components/ui/card';
import { SettingsNav } from '../components/SettingsNav';
import { useLocations } from '../hooks/use-lookups';

export default function LocationsSettingsPage() {
  const { data, isLoading } = useLocations();

  return (
    <div>
      <PageHeader title="Settings" description="Plant locations" />
      <SettingsNav />
      <Card>
        <CardContent className="space-y-2 pt-6">
          {isLoading ? (
            <p className="text-sm text-muted-foreground">Loading…</p>
          ) : data?.length ? (
            data.map((location) => (
              <div key={location.id} className="rounded-md border p-3">
                <div className="font-medium">{location.name}</div>
                {location.area ? (
                  <div className="text-xs text-muted-foreground">
                    {location.area}
                  </div>
                ) : null}
              </div>
            ))
          ) : (
            <p className="text-sm text-muted-foreground">No locations.</p>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
