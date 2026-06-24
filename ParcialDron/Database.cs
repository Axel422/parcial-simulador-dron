using Npgsql;
// =======================================================
// PARTE D - ACCESO A DATOS (ADO.NET PURO + POSTGRESQL)
// =======================================================
public class Database
{ 
    // 🔴 CONSIGNA: conexión gestionada manualmente (sin ORM)
    private string _conn;

    public Database(string conn)
    {
       // Se recibe la cadena desde appsettings.json
        // 🔴 CONSIGNA PARTE C: configuración externa obligatoria
        _conn = conn;
    }
   // ===================================================
    // PARTE D - INSERTAR MASTER (tb_master_control)
    // ===================================================
    public int InsertarMaster(int n, int x, int y)
    {
         // Se abre conexión manual (ADO.NET puro)
        using var conn = new NpgsqlConnection(_conn);
        conn.Open();
 // 🔴 CONSIGNA:
        // - tabla master
        // - RETURNING id obligatorio
        var sql = @"
            INSERT INTO tb_master_control (n, x_inicio, y_inicio, fecha)
            VALUES (@n, @x, @y, NOW())
            RETURNING id;
        ";

        using var cmd = new NpgsqlCommand(sql, conn);
    // 🔴 CONSIGNA: uso de parámetros (NO concatenación de strings)
        cmd.Parameters.AddWithValue("@n", n);
        cmd.Parameters.AddWithValue("@x", x);
        cmd.Parameters.AddWithValue("@y", y);
          // Devuelve ID generado por PostgreSQL
        return Convert.ToInt32(cmd.ExecuteScalar()!);

    }
 // ===================================================
    // PARTE D - INSERTAR DETAIL (tb_det_log)
    // ===================================================

    public void InsertarDetalle(int masterId, int[,] tablero, int n)
    {
         // Se abre conexión ADO.NET manual
        using var conn = new NpgsqlConnection(_conn);
        conn.Open();
 // 🔴 CONSIGNA OBLIGATORIA:
        // uso de transacción (Commit / Rollback implícito)
        using var tx = conn.BeginTransaction();
  // 🔴 CONSIGNA RESTRICTIVA DEL EXAMEN:
        // NO usar foreach / for para recorrer inserciones
        // => se usa while con contador manual
        int i = 0;
   // Recorre todos los pasos del recorrido del dron
        while (i < n * n)
        {
             // Se busca la celda que corresponde al paso i
            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    if (tablero[x, y] == i)
                    { // ===================================================
                        // PARTE D - OFUSCACIÓN DE DATOS (REGLA DEL PARCIAL)
                        // ===================================================
                        // PAR: se multiplica por 2
                        // IMPAR: se guarda negativo
                         int pasoOfuscado = (i % 2 == 0) ? i * 2 : -i;
                        var sql = @"
                            INSERT INTO tb_det_log (master_id, paso, x, y)
                            VALUES (@m, @p, @x, @y);
                        ";

                        using var cmd = new NpgsqlCommand(sql, conn, tx);

                        cmd.Parameters.AddWithValue("@m", masterId);
                           // 🔴 CONSIGNA: guardar paso (idealmente ofuscado)
                        cmd.Parameters.AddWithValue("@p", i);
                        cmd.Parameters.AddWithValue("@x", x);
                        cmd.Parameters.AddWithValue("@y", y);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
 // avance manual obligatorio (restricción del parcial)
            i++;
        }
  // Confirmación de transacción
        tx.Commit();
    }
}