import { z } from 'zod';

export const equipmentSchema = z.object({
  code: z.string().min(1, 'Code is required').max(64),
  name: z.string().min(1, 'Name is required').max(200),
  categoryId: z.string().uuid('Select a category'),
  locationId: z.string().uuid('Select a location'),
  manufacturer: z.string().max(200).optional().or(z.literal('')),
  installDate: z.string().optional().or(z.literal('')),
  status: z.enum(['Active', 'UnderMaintenance', 'Broken', 'Decommissioned']),
});

export type EquipmentFormValues = z.infer<typeof equipmentSchema>;
