import { useQuery } from '@tanstack/react-query';
import { queryKeys } from '@/lib/query-keys';
import { getUsers } from '../api/settings-api';

export function useUsers() {
  return useQuery({
    queryKey: queryKeys.settings.users,
    queryFn: getUsers,
  });
}
