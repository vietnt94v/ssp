import { PageHeader } from '@/components/shared/PageHeader';
import { Card, CardContent } from '@/components/ui/card';
import { SettingsNav } from '../components/SettingsNav';
import { useCategories } from '../hooks/use-lookups';

export default function CategoriesSettingsPage() {
  const { data, isLoading } = useCategories();

  return (
    <div>
      <PageHeader title="Settings" description="Equipment categories" />
      <SettingsNav />
      <Card>
        <CardContent className="space-y-2 pt-6">
          {isLoading ? (
            <p className="text-sm text-muted-foreground">Loading…</p>
          ) : data?.length ? (
            data.map((category) => (
              <div key={category.id} className="rounded-md border p-3">
                <div className="font-medium">{category.name}</div>
                {category.description ? (
                  <div className="text-xs text-muted-foreground">
                    {category.description}
                  </div>
                ) : null}
              </div>
            ))
          ) : (
            <p className="text-sm text-muted-foreground">No categories.</p>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
