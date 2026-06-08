--liquibase formatted sql

--changeset cmms:001-extensions
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

--changeset cmms:001-categories
CREATE TABLE categories (
    id          UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    name        VARCHAR(150) NOT NULL,
    description TEXT,
    created_at  TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_by  VARCHAR(256),
    updated_at  TIMESTAMPTZ,
    updated_by  VARCHAR(256)
);

--changeset cmms:001-locations
CREATE TABLE locations (
    id         UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    name       VARCHAR(150) NOT NULL,
    area       VARCHAR(150),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_by VARCHAR(256),
    updated_at TIMESTAMPTZ,
    updated_by VARCHAR(256)
);

--changeset cmms:001-skills
CREATE TABLE skills (
    id         UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    name       VARCHAR(150) NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_by VARCHAR(256),
    updated_at TIMESTAMPTZ,
    updated_by VARCHAR(256)
);

--changeset cmms:001-users
CREATE TABLE users (
    id            UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    email         VARCHAR(256) NOT NULL,
    full_name     VARCHAR(200) NOT NULL,
    password_hash TEXT NOT NULL,
    role          VARCHAR(32) NOT NULL,
    is_active     BOOLEAN NOT NULL DEFAULT TRUE,
    created_at    TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_by    VARCHAR(256),
    updated_at    TIMESTAMPTZ,
    updated_by    VARCHAR(256),
    CONSTRAINT uq_users_email UNIQUE (email)
);

--changeset cmms:001-technicians
CREATE TABLE technicians (
    id         UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    user_id    UUID NOT NULL,
    name       VARCHAR(200) NOT NULL,
    department VARCHAR(150),
    rating     NUMERIC(3,2),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_by VARCHAR(256),
    updated_at TIMESTAMPTZ,
    updated_by VARCHAR(256),
    CONSTRAINT fk_technicians_user FOREIGN KEY (user_id) REFERENCES users (id),
    CONSTRAINT uq_technicians_user UNIQUE (user_id)
);

--changeset cmms:001-technician-skills
CREATE TABLE technician_skills (
    technician_id UUID NOT NULL,
    skill_id      UUID NOT NULL,
    CONSTRAINT pk_technician_skills PRIMARY KEY (technician_id, skill_id),
    CONSTRAINT fk_ts_technician FOREIGN KEY (technician_id)
        REFERENCES technicians (id) ON DELETE CASCADE,
    CONSTRAINT fk_ts_skill FOREIGN KEY (skill_id)
        REFERENCES skills (id) ON DELETE CASCADE
);

--changeset cmms:001-equipment
CREATE TABLE equipment (
    id                  UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    code                VARCHAR(64) NOT NULL,
    name                VARCHAR(200) NOT NULL,
    category_id         UUID NOT NULL,
    location_id         UUID NOT NULL,
    manufacturer        VARCHAR(200),
    install_date        DATE,
    status              VARCHAR(32) NOT NULL DEFAULT 'Active',
    last_maintenance_at TIMESTAMPTZ,
    image_url           TEXT,
    is_deleted          BOOLEAN NOT NULL DEFAULT FALSE,
    deleted_at          TIMESTAMPTZ,
    deleted_by          VARCHAR(256),
    created_at          TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_by          VARCHAR(256),
    updated_at          TIMESTAMPTZ,
    updated_by          VARCHAR(256),
    CONSTRAINT uq_equipment_code UNIQUE (code),
    CONSTRAINT fk_equipment_category FOREIGN KEY (category_id)
        REFERENCES categories (id),
    CONSTRAINT fk_equipment_location FOREIGN KEY (location_id)
        REFERENCES locations (id)
);
CREATE INDEX ix_equipment_status ON equipment (status);
CREATE INDEX ix_equipment_category ON equipment (category_id);
CREATE INDEX ix_equipment_location ON equipment (location_id);

--changeset cmms:001-spare-parts
CREATE TABLE spare_parts (
    id             UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    code           VARCHAR(64) NOT NULL,
    name           VARCHAR(200) NOT NULL,
    unit_cost      NUMERIC(18,2) NOT NULL DEFAULT 0,
    stock_quantity INTEGER NOT NULL DEFAULT 0,
    reorder_level  INTEGER NOT NULL DEFAULT 0,
    created_at     TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_by     VARCHAR(256),
    updated_at     TIMESTAMPTZ,
    updated_by     VARCHAR(256),
    CONSTRAINT uq_spare_parts_code UNIQUE (code)
);

--changeset cmms:001-maintenance-schedules
CREATE TABLE maintenance_schedules (
    id              UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    equipment_id    UUID NOT NULL,
    title           VARCHAR(200) NOT NULL,
    frequency       VARCHAR(32) NOT NULL,
    interval_value  INTEGER NOT NULL DEFAULT 1,
    meter_threshold NUMERIC(18,2),
    next_due_date   DATE NOT NULL,
    is_active       BOOLEAN NOT NULL DEFAULT TRUE,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_by      VARCHAR(256),
    updated_at      TIMESTAMPTZ,
    updated_by      VARCHAR(256),
    CONSTRAINT fk_schedule_equipment FOREIGN KEY (equipment_id)
        REFERENCES equipment (id) ON DELETE CASCADE
);
CREATE INDEX ix_schedules_next_due ON maintenance_schedules (next_due_date);

--changeset cmms:001-work-orders
CREATE TABLE work_orders (
    id                     UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    number                 VARCHAR(32) NOT NULL,
    type                   VARCHAR(32) NOT NULL,
    priority               VARCHAR(32) NOT NULL DEFAULT 'Medium',
    status                 VARCHAR(32) NOT NULL DEFAULT 'Draft',
    equipment_id           UUID NOT NULL,
    assigned_technician_id UUID,
    schedule_id            UUID,
    description            TEXT NOT NULL,
    deadline               TIMESTAMPTZ,
    started_at             TIMESTAMPTZ,
    completed_at           TIMESTAMPTZ,
    is_deleted             BOOLEAN NOT NULL DEFAULT FALSE,
    deleted_at             TIMESTAMPTZ,
    deleted_by             VARCHAR(256),
    created_at             TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_by             VARCHAR(256),
    updated_at             TIMESTAMPTZ,
    updated_by             VARCHAR(256),
    CONSTRAINT uq_work_orders_number UNIQUE (number),
    CONSTRAINT fk_wo_equipment FOREIGN KEY (equipment_id)
        REFERENCES equipment (id),
    CONSTRAINT fk_wo_technician FOREIGN KEY (assigned_technician_id)
        REFERENCES technicians (id) ON DELETE SET NULL,
    CONSTRAINT fk_wo_schedule FOREIGN KEY (schedule_id)
        REFERENCES maintenance_schedules (id) ON DELETE SET NULL
);
CREATE INDEX ix_wo_status ON work_orders (status);
CREATE INDEX ix_wo_priority ON work_orders (priority);
CREATE INDEX ix_wo_equipment ON work_orders (equipment_id);
CREATE INDEX ix_wo_technician ON work_orders (assigned_technician_id);
CREATE INDEX ix_wo_deadline ON work_orders (deadline);

--changeset cmms:001-work-order-checklist-items
CREATE TABLE work_order_checklist_items (
    id            UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    work_order_id UUID NOT NULL,
    description   TEXT NOT NULL,
    is_done       BOOLEAN NOT NULL DEFAULT FALSE,
    sort_order    INTEGER NOT NULL DEFAULT 0,
    CONSTRAINT fk_checklist_wo FOREIGN KEY (work_order_id)
        REFERENCES work_orders (id) ON DELETE CASCADE
);
CREATE INDEX ix_checklist_wo ON work_order_checklist_items (work_order_id);

--changeset cmms:001-work-order-parts
CREATE TABLE work_order_parts (
    id            UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    work_order_id UUID NOT NULL,
    spare_part_id UUID NOT NULL,
    quantity      INTEGER NOT NULL DEFAULT 1,
    unit_cost     NUMERIC(18,2) NOT NULL DEFAULT 0,
    CONSTRAINT fk_wop_wo FOREIGN KEY (work_order_id)
        REFERENCES work_orders (id) ON DELETE CASCADE,
    CONSTRAINT fk_wop_part FOREIGN KEY (spare_part_id)
        REFERENCES spare_parts (id)
);
CREATE INDEX ix_wop_wo ON work_order_parts (work_order_id);

--changeset cmms:001-cost-entries
CREATE TABLE cost_entries (
    id            UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    work_order_id UUID NOT NULL,
    type          VARCHAR(32) NOT NULL,
    amount        NUMERIC(18,2) NOT NULL DEFAULT 0,
    description   TEXT,
    created_at    TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_by    VARCHAR(256),
    updated_at    TIMESTAMPTZ,
    updated_by    VARCHAR(256),
    CONSTRAINT fk_cost_wo FOREIGN KEY (work_order_id)
        REFERENCES work_orders (id) ON DELETE CASCADE
);
CREATE INDEX ix_cost_wo ON cost_entries (work_order_id);

--changeset cmms:001-maintenance-logs
CREATE TABLE maintenance_logs (
    id               UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    equipment_id     UUID NOT NULL,
    work_order_id    UUID NOT NULL,
    completed_at     TIMESTAMPTZ NOT NULL,
    summary          TEXT NOT NULL,
    downtime_minutes INTEGER NOT NULL DEFAULT 0,
    CONSTRAINT fk_log_equipment FOREIGN KEY (equipment_id)
        REFERENCES equipment (id) ON DELETE CASCADE,
    CONSTRAINT fk_log_wo FOREIGN KEY (work_order_id)
        REFERENCES work_orders (id) ON DELETE CASCADE
);
CREATE INDEX ix_log_equipment ON maintenance_logs (equipment_id);

--changeset cmms:001-alerts
CREATE TABLE alerts (
    id              UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    type            VARCHAR(32) NOT NULL,
    message         TEXT NOT NULL,
    entity_id       UUID,
    entity_type     VARCHAR(64),
    is_acknowledged BOOLEAN NOT NULL DEFAULT FALSE,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT now(),
    acknowledged_at TIMESTAMPTZ,
    acknowledged_by VARCHAR(256)
);
CREATE INDEX ix_alerts_ack ON alerts (is_acknowledged);
