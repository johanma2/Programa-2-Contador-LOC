public class ResultadoParte
{
    public string Nombre { get; }

    public int CantidadMetodos { get; private set; }

    public int TotalLOC { get; private set; }

    public ResultadoParte(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException(
                "El nombre de la parte no puede estar vacío.",
                nameof(nombre));
        }

        Nombre = nombre;
        CantidadMetodos = 0;
        TotalLOC = 0;
    }

    public void AgregarLOC()
    {
        TotalLOC++;
    }

    public void AgregarMetodo()
    {
        CantidadMetodos++;
    }
}