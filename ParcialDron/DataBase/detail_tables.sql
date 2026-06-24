CREATE TABLE IF NOT EXISTS tb_det_log (
    id SERIAL PRIMARY KEY,
    master_id INT NOT NULL REFERENCES tb_master_control(id),
    paso INT NOT NULL,
    x INT NOT NULL,
    y INT NOT NULL
);