public class UnidadLogica
{
    public string Contenido { get; }

    public TipoUnidad Tipo { get; }

    public int NumeroLinea { get; }

    public bool EsContabilizable { get; }

    public UnidadLogica(
        string contenido,
        TipoUnidad tipo,
        int numeroLinea,
        bool esContabilizable)
    {
        ArgumentNullException.ThrowIfNull(contenido);

        Contenido = contenido;
        Tipo = tipo;
        NumeroLinea = numeroLinea;
        EsContabilizable = esContabilizable;
    }
}