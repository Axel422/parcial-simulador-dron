public class Candidato
{
// Posición candidata a visitar
public int X;
public int Y;

// Cantidad de movimientos posibles desde esa posición
public int Grado;

}

public class DronSolver
{
// Tablero NxN
private int[,] tablero;

// Tamaño del tablero
private int n;

// Movimientos del caballo (dron)
private readonly int[] dx =
{
    -2,-2, 2, 2,
    -1,-1, 1, 1
};

private readonly int[] dy =
{
    -1, 1,-1, 1,
    -2, 2,-2, 2
};

// Constructor
public DronSolver(int n)
{
    this.n = n;

    tablero = new int[n, n];

    // Inicializar todas las casillas en -1
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            tablero[i, j] = -1;
        }
    }
}

// Verifica si una posición es válida
private bool EsValido(int x, int y)
{
    return x >= 0 &&
           x < n &&
           y >= 0 &&
           y < n &&
           tablero[x, y] == -1;
}

// Calcula el grado de una casilla
// (cantidad de movimientos válidos que tiene)
private int CalcularGrado(int x, int y)
{
    int grado = 0;

    for (int i = 0; i < 8; i++)
    {
        int nx = x + dx[i];
        int ny = y + dy[i];

        if (EsValido(nx, ny))
        {
            grado++;
        }
    }

    return grado;
}

// Obtiene candidatos ordenados por menor grado
private List<Candidato> ObtenerCandidatos(int x, int y)
{
    List<Candidato> lista = new();

    for (int i = 0; i < 8; i++)
    {
        int nx = x + dx[i];
        int ny = y + dy[i];

        if (EsValido(nx, ny))
        {
            lista.Add(new Candidato
            {
                X = nx,
                Y = ny,
                Grado = CalcularGrado(nx, ny)
            });
        }
    }

    // Heurística de Warnsdorff:
    // ordenar por menor cantidad de salidas
    lista.Sort((a, b) => a.Grado.CompareTo(b.Grado));

    return lista;
}

// Propiedad para acceder al tablero
public int[,] Tablero => tablero;


private bool ResolverRecursivo(int x, int y, int paso)
{
    tablero[x, y] = paso;

    // Si visitó todas las casillas
    if (paso == (n * n) - 1)
    {
        return true;
    }

    var candidatos = ObtenerCandidatos(x, y);

    foreach (var c in candidatos)
    {
        if (ResolverRecursivo(c.X, c.Y, paso + 1))
        {
            return true;
        }
    }

    // Backtracking
    tablero[x, y] = -1;

    return false;
}
public bool Resolver(int xInicial, int yInicial)
{
    return ResolverRecursivo(xInicial, yInicial, 0);
}
public void MostrarTablero()
{
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            Console.Write($"{tablero[i, j],4}");
        }

        Console.WriteLine();
    }
}

}