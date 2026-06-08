import { z } from 'zod';

export const workOrderSchema = z.object({
  type: z.enum(['Corrective', 'Preventive', 'Inspection']),
  priority: z.enum(['Low', 'Medium', 'High', 'Critical']),
  equipmentId: z.string().uuid('Select equipment'),
  assignedTechnicianId: z.string().uuid().optional().or(z.literal('')),
  description: z.string().min(1, 'Description is required').max(2000),
  deadline: z.string().optional().or(z.literal('')),
});

export type WorkOrderFormValues = z.infer<typeof workOrderSchema>;
