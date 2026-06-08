--liquibase formatted sql

-- Seed data for local development and Swagger examples.
-- Password for all demo users is "Passw0rd!" (BCrypt hash).

--changeset cmms:002-users
INSERT INTO users (id, email, full_name, password_hash, role, is_active, created_at, created_by) VALUES
 ('11111111-1111-1111-1111-111111111111', 'admin@cmms.local',   'System Admin',   '$2b$10$yoPTXE9c1HpgFiRNde1sYeTlC0xFG8KJWRFaZZRPlM5wjWSWQ7q66', 'Admin',      TRUE, now(), 'seed'),
 ('22222222-2222-2222-2222-222222222222', 'manager@cmms.local', 'Maria Manager',  '$2b$10$yoPTXE9c1HpgFiRNde1sYeTlC0xFG8KJWRFaZZRPlM5wjWSWQ7q66', 'Manager',    TRUE, now(), 'seed'),
 ('33333333-3333-3333-3333-333333333333', 'tech@cmms.local',    'Tom Technician', '$2b$10$yoPTXE9c1HpgFiRNde1sYeTlC0xFG8KJWRFaZZRPlM5wjWSWQ7q66', 'Technician', TRUE, now(), 'seed'),
 ('33333333-3333-3333-3333-333333333334', 'tech2@cmms.local',   'Tina Technician','$2b$10$yoPTXE9c1HpgFiRNde1sYeTlC0xFG8KJWRFaZZRPlM5wjWSWQ7q66', 'Technician', TRUE, now(), 'seed');

--changeset cmms:002-categories
INSERT INTO categories (id, name, description, created_at, created_by) VALUES
 ('a1000000-0000-0000-0000-000000000001', 'Pumps',       'Centrifugal and positive displacement pumps', now(), 'seed'),
 ('a1000000-0000-0000-0000-000000000002', 'Motors',      'Electric motors and drives',                  now(), 'seed'),
 ('a1000000-0000-0000-0000-000000000003', 'Conveyors',   'Belt and roller conveyors',                   now(), 'seed'),
 ('a1000000-0000-0000-0000-000000000004', 'Compressors', 'Air and gas compressors',                     now(), 'seed');

--changeset cmms:002-locations
INSERT INTO locations (id, name, area, created_at, created_by) VALUES
 ('b1000000-0000-0000-0000-000000000001', 'Line A', 'Production Hall 1', now(), 'seed'),
 ('b1000000-0000-0000-0000-000000000002', 'Line B', 'Production Hall 1', now(), 'seed'),
 ('b1000000-0000-0000-0000-000000000003', 'Utility Room', 'Basement',    now(), 'seed');

--changeset cmms:002-skills
INSERT INTO skills (id, name, created_at, created_by) VALUES
 ('c1000000-0000-0000-0000-000000000001', 'Mechanical',  now(), 'seed'),
 ('c1000000-0000-0000-0000-000000000002', 'Electrical',  now(), 'seed'),
 ('c1000000-0000-0000-0000-000000000003', 'Hydraulics',  now(), 'seed');

--changeset cmms:002-technicians
INSERT INTO technicians (id, user_id, name, department, rating, created_at, created_by) VALUES
 ('d1000000-0000-0000-0000-000000000001', '33333333-3333-3333-3333-333333333333', 'Tom Technician',  'Mechanical', 4.50, now(), 'seed'),
 ('d1000000-0000-0000-0000-000000000002', '33333333-3333-3333-3333-333333333334', 'Tina Technician', 'Electrical', 4.80, now(), 'seed');

--changeset cmms:002-technician-skills
INSERT INTO technician_skills (technician_id, skill_id) VALUES
 ('d1000000-0000-0000-0000-000000000001', 'c1000000-0000-0000-0000-000000000001'),
 ('d1000000-0000-0000-0000-000000000001', 'c1000000-0000-0000-0000-000000000003'),
 ('d1000000-0000-0000-0000-000000000002', 'c1000000-0000-0000-0000-000000000002');

--changeset cmms:002-equipment
INSERT INTO equipment (id, code, name, category_id, location_id, manufacturer, install_date, status, last_maintenance_at, created_at, created_by) VALUES
 ('e1000000-0000-0000-0000-000000000001', 'EQ-PUMP-001', 'Coolant Pump 1',     'a1000000-0000-0000-0000-000000000001', 'b1000000-0000-0000-0000-000000000001', 'Grundfos', '2021-03-15', 'Active',           now() - INTERVAL '20 days', now(), 'seed'),
 ('e1000000-0000-0000-0000-000000000002', 'EQ-MOTOR-001','Drive Motor A',      'a1000000-0000-0000-0000-000000000002', 'b1000000-0000-0000-0000-000000000001', 'Siemens',  '2020-06-01', 'UnderMaintenance', now() - INTERVAL '5 days',  now(), 'seed'),
 ('e1000000-0000-0000-0000-000000000003', 'EQ-CONV-001', 'Main Conveyor',      'a1000000-0000-0000-0000-000000000003', 'b1000000-0000-0000-0000-000000000002', 'Dorner',   '2019-11-20', 'Broken',           now() - INTERVAL '60 days', now(), 'seed'),
 ('e1000000-0000-0000-0000-000000000004', 'EQ-COMP-001', 'Air Compressor 1',   'a1000000-0000-0000-0000-000000000004', 'b1000000-0000-0000-0000-000000000003', 'Atlas Copco','2022-01-10','Active',           now() - INTERVAL '10 days', now(), 'seed'),
 ('e1000000-0000-0000-0000-000000000005', 'EQ-PUMP-002', 'Coolant Pump 2',     'a1000000-0000-0000-0000-000000000001', 'b1000000-0000-0000-0000-000000000002', 'Grundfos', '2018-05-05', 'Decommissioned',   now() - INTERVAL '200 days',now(), 'seed');

--changeset cmms:002-spare-parts
INSERT INTO spare_parts (id, code, name, unit_cost, stock_quantity, reorder_level, created_at, created_by) VALUES
 ('f1000000-0000-0000-0000-000000000001', 'SP-SEAL-001',    'Mechanical Seal Kit', 85.00,  12, 5,  now(), 'seed'),
 ('f1000000-0000-0000-0000-000000000002', 'SP-BEARING-001', 'Ball Bearing 6204',   12.50,  3,  10, now(), 'seed'),
 ('f1000000-0000-0000-0000-000000000003', 'SP-BELT-001',    'V-Belt A-section',    18.00,  40, 8,  now(), 'seed');

--changeset cmms:002-maintenance-schedules
INSERT INTO maintenance_schedules (id, equipment_id, title, frequency, interval_value, meter_threshold, next_due_date, is_active, created_at, created_by) VALUES
 ('a2000000-0000-0000-0000-000000000001', 'e1000000-0000-0000-0000-000000000001', 'Monthly pump inspection',  'Monthly', 1,   NULL,   CURRENT_DATE + 7,  TRUE, now(), 'seed'),
 ('a2000000-0000-0000-0000-000000000002', 'e1000000-0000-0000-0000-000000000002', 'Weekly motor check',       'Weekly',  1,   NULL,   CURRENT_DATE + 2,  TRUE, now(), 'seed'),
 ('a2000000-0000-0000-0000-000000000003', 'e1000000-0000-0000-0000-000000000004', 'Compressor 500h service',  'ByMeter', 1,   500.00, CURRENT_DATE + 14, TRUE, now(), 'seed');

--changeset cmms:002-work-orders
INSERT INTO work_orders (id, number, type, priority, status, equipment_id, assigned_technician_id, description, deadline, started_at, completed_at, created_at, created_by) VALUES
 ('b2000000-0000-0000-0000-000000000001', 'WO-2026-0001', 'Corrective', 'High',     'Draft',      'e1000000-0000-0000-0000-000000000003', NULL,                                   'Conveyor belt torn, line stopped',        now() + INTERVAL '1 day',  NULL,                       NULL,                       now(), 'seed'),
 ('b2000000-0000-0000-0000-000000000002', 'WO-2026-0002', 'Corrective', 'Critical', 'Assigned',   'e1000000-0000-0000-0000-000000000002', 'd1000000-0000-0000-0000-000000000002', 'Drive motor overheating',                  now() + INTERVAL '2 days', NULL,                       NULL,                       now(), 'seed'),
 ('b2000000-0000-0000-0000-000000000003', 'WO-2026-0003', 'Preventive', 'Medium',   'InProgress', 'e1000000-0000-0000-0000-000000000001', 'd1000000-0000-0000-0000-000000000001', 'Monthly pump inspection',                  now() + INTERVAL '3 days', now() - INTERVAL '2 hours', NULL,                       now(), 'seed'),
 ('b2000000-0000-0000-0000-000000000004', 'WO-2026-0004', 'Inspection', 'Low',      'OnHold',     'e1000000-0000-0000-0000-000000000004', 'd1000000-0000-0000-0000-000000000001', 'Compressor noise check - waiting parts',   now() + INTERVAL '5 days', now() - INTERVAL '1 day',   NULL,                       now(), 'seed'),
 ('b2000000-0000-0000-0000-000000000005', 'WO-2026-0005', 'Corrective', 'High',     'Completed',  'e1000000-0000-0000-0000-000000000001', 'd1000000-0000-0000-0000-000000000001', 'Replaced leaking seal',                    now() - INTERVAL '1 day',  now() - INTERVAL '3 days',  now() - INTERVAL '2 days',  now() - INTERVAL '3 days', 'seed'),
 ('b2000000-0000-0000-0000-000000000006', 'WO-2026-0006', 'Preventive', 'Medium',   'Closed',     'e1000000-0000-0000-0000-000000000004', 'd1000000-0000-0000-0000-000000000002', 'Quarterly compressor service',             now() - INTERVAL '10 days',now() - INTERVAL '12 days', now() - INTERVAL '11 days', now() - INTERVAL '12 days','seed'),
 ('b2000000-0000-0000-0000-000000000007', 'WO-2026-0007', 'Corrective', 'Critical', 'Assigned',   'e1000000-0000-0000-0000-000000000003', 'd1000000-0000-0000-0000-000000000001', 'Conveyor roller seized',                   now() - INTERVAL '1 day',  NULL,                       NULL,                       now() - INTERVAL '2 days', 'seed'),
 ('b2000000-0000-0000-0000-000000000008', 'WO-2026-0008', 'Inspection', 'Low',      'Draft',      'e1000000-0000-0000-0000-000000000002', NULL,                                   'Routine electrical inspection',            now() + INTERVAL '10 days',NULL,                       NULL,                       now(), 'seed'),
 ('b2000000-0000-0000-0000-000000000009', 'WO-2026-0009', 'Preventive', 'Medium',   'InProgress', 'e1000000-0000-0000-0000-000000000004', 'd1000000-0000-0000-0000-000000000002', 'Compressor 500h service',                  now() + INTERVAL '4 days', now() - INTERVAL '1 hour',  NULL,                       now(), 'seed'),
 ('b2000000-0000-0000-0000-000000000010', 'WO-2026-0010', 'Corrective', 'High',     'Completed',  'e1000000-0000-0000-0000-000000000002', 'd1000000-0000-0000-0000-000000000002', 'Bearing replacement on motor',             now() - INTERVAL '2 days', now() - INTERVAL '4 days',  now() - INTERVAL '3 days',  now() - INTERVAL '4 days', 'seed');

--changeset cmms:002-work-order-checklist
INSERT INTO work_order_checklist_items (id, work_order_id, description, is_done, sort_order) VALUES
 ('c2000000-0000-0000-0000-000000000001', 'b2000000-0000-0000-0000-000000000003', 'Lockout/tagout equipment',  TRUE,  1),
 ('c2000000-0000-0000-0000-000000000002', 'b2000000-0000-0000-0000-000000000003', 'Inspect impeller wear',     TRUE,  2),
 ('c2000000-0000-0000-0000-000000000003', 'b2000000-0000-0000-0000-000000000003', 'Check seal for leaks',      FALSE, 3),
 ('c2000000-0000-0000-0000-000000000004', 'b2000000-0000-0000-0000-000000000003', 'Record vibration reading',  FALSE, 4);

--changeset cmms:002-work-order-parts
INSERT INTO work_order_parts (id, work_order_id, spare_part_id, quantity, unit_cost) VALUES
 ('d2000000-0000-0000-0000-000000000001', 'b2000000-0000-0000-0000-000000000005', 'f1000000-0000-0000-0000-000000000001', 1, 85.00),
 ('d2000000-0000-0000-0000-000000000002', 'b2000000-0000-0000-0000-000000000010', 'f1000000-0000-0000-0000-000000000002', 2, 12.50);

--changeset cmms:002-cost-entries
INSERT INTO cost_entries (id, work_order_id, type, amount, description, created_at, created_by) VALUES
 ('e2000000-0000-0000-0000-000000000001', 'b2000000-0000-0000-0000-000000000005', 'Parts', 85.00,  'Mechanical seal kit', now(), 'seed'),
 ('e2000000-0000-0000-0000-000000000002', 'b2000000-0000-0000-0000-000000000005', 'Labor', 120.00, '2 hours labor',       now(), 'seed'),
 ('e2000000-0000-0000-0000-000000000003', 'b2000000-0000-0000-0000-000000000010', 'Parts', 25.00,  '2x ball bearings',    now(), 'seed'),
 ('e2000000-0000-0000-0000-000000000004', 'b2000000-0000-0000-0000-000000000010', 'Labor', 90.00,  '1.5 hours labor',     now(), 'seed');

--changeset cmms:002-maintenance-logs
INSERT INTO maintenance_logs (id, equipment_id, work_order_id, completed_at, summary, downtime_minutes) VALUES
 ('f2000000-0000-0000-0000-000000000001', 'e1000000-0000-0000-0000-000000000001', 'b2000000-0000-0000-0000-000000000005', now() - INTERVAL '2 days',  'Replaced mechanical seal, tested run', 90),
 ('f2000000-0000-0000-0000-000000000002', 'e1000000-0000-0000-0000-000000000004', 'b2000000-0000-0000-0000-000000000006', now() - INTERVAL '11 days', 'Quarterly service completed',          240),
 ('f2000000-0000-0000-0000-000000000003', 'e1000000-0000-0000-0000-000000000002', 'b2000000-0000-0000-0000-000000000010', now() - INTERVAL '3 days',  'Bearing replaced, vibration normal',   120);

--changeset cmms:002-alerts
INSERT INTO alerts (id, type, message, entity_id, entity_type, is_acknowledged, created_at) VALUES
 ('a3000000-0000-0000-0000-000000000001', 'WoOverdue',          'Work order WO-2026-0007 is overdue',                'b2000000-0000-0000-0000-000000000007', 'WorkOrder', FALSE, now() - INTERVAL '1 day'),
 ('a3000000-0000-0000-0000-000000000002', 'EquipmentBreakdown', 'Main Conveyor is in Broken status',                 'e1000000-0000-0000-0000-000000000003', 'Equipment', FALSE, now() - INTERVAL '2 hours'),
 ('a3000000-0000-0000-0000-000000000003', 'PmDue',              'Weekly motor check due in 2 days',                  'a2000000-0000-0000-0000-000000000002', 'Schedule',  FALSE, now() - INTERVAL '30 minutes'),
 ('a3000000-0000-0000-0000-000000000004', 'LowStock',           'Ball Bearing 6204 below reorder level (3 < 10)',    'f1000000-0000-0000-0000-000000000002', 'SparePart', FALSE, now() - INTERVAL '6 hours'),
 ('a3000000-0000-0000-0000-000000000005', 'WoOverdue',          'Work order WO-2026-0002 approaching deadline',      'b2000000-0000-0000-0000-000000000002', 'WorkOrder', TRUE,  now() - INTERVAL '1 day');
