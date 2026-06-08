import { useQuery } from '@tanstack/react-query';
import { queryKeys } from '@/lib/query-keys';
import { getCategories, getLocations } from '../api/lookups-api';

export function useCategories() {
  return useQuery({
    queryKey: queryKeys.settings.categories,
    queryFn: getCategories,
    staleTime: 5 * 60 * 1000,
  });
}

export function useLocations() {
  return useQuery({
    queryKey: queryKeys.settings.locations,
    queryFn: getLocations,
    staleTime: 5 * 60 * 1000,
  });
}
