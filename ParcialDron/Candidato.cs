namespace ParcialDron
{
public class Candidato
{
    // Posición candidata a visitar
    public int X { get; set; }

    public int Y { get; set; }

    // Cantidad de movimientos posibles desde esa posición
    public int Grado { get; set; }
}
}