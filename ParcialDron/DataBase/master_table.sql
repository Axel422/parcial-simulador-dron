CREATE TABLE IF NOT EXISTS tb_master_control (
    id SERIAL PRIMARY KEY,
    n INT NOT NULL,
    x_inicio INT NOT NULL,
    y_inicio INT NOT NULL,
    fecha TIMESTAMP NOT NULL
);