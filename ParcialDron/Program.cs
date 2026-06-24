// PARTE C - CONFIGURACIÓN EXTERNA (appsettings.json)

using Microsoft.Extensions.Configuration;
// Se crea el ConfigurationBuilder para leer configuración externa
//  CONSIGNA: "Está prohibido hardcodear la cadena de conexión"
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// Se obtiene la cadena de conexión desde el JSON
//  CONSIGNA: uso obligatorio de ConnectionStrings
string conn = config.GetConnectionString("Postgres")!;
// Validación de seguridad (evita null)
//  CONSIGNA: validación de configuración dinámica
if (conn == null)
{
    Console.WriteLine("Connection string no encontrada");
    return;
}

// =======================================================
// PARTE B - ALGORITMO RECURSIVO (DRON SOLVER)
// =======================================================
// Se crea el solver del dron con tamaño N x N
DronSolver solver = new DronSolver(8);
// Se ejecuta la recursividad + backtracking
//  CONSIGNA: algoritmo recursivo obligatorio
bool ok = solver.Resolver(0, 0);

Console.WriteLine("Solución: " + ok);
// =======================================================
// PARTE E - INTERFAZ Y SALIDA POR CONSOLA
// =======================================================
if (ok)
{ // Mostrar la matriz del recorrido
    //  CONSIGNA: mostrar recorrido en consola
    solver.MostrarTablero();
    // ===================================================
    // PARTE D - PERSISTENCIA ADO.NET + POSTGRESQL
    // ===================================================

    // Se crea acceso a base de datos usando ADO.NET puro
    //  CONSIGNA: uso de Npgsql sin ORM
    var db = new Database(conn);
// Insertar cabecera (MASTER)
 //  CONSIGNA: tb_master_control + RETURNING id
    int masterId = db.InsertarMaster(8, 0, 0);
 // Insertar detalle (MASTER-DETAIL)
    //  CONSIGNA: tb_det_log con recorrido completo
    db.InsertarDetalle(masterId, solver.Tablero, 8);

    Console.WriteLine("Guardado OK. Master ID: " + masterId);
}