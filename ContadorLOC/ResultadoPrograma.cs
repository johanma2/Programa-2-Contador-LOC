using System;
using System.Collections.Generic;

public class ResultadoPrograma
{
    private readonly List<ResultadoParte> _partes;

    public IReadOnlyList<ResultadoParte> Partes => _partes;

    public int LOCExternas { get; private set; }

    public int TotalLOC { get; private set; }

    public int CantidadArchivos { get; private set; }

    public ResultadoPrograma()
    {
        _partes = new List<ResultadoParte>();
        LOCExternas = 0;
        TotalLOC = 0;
        CantidadArchivos = 0;
    }

    public void AgregarParte(ResultadoParte parte)
    {
        ArgumentNullException.ThrowIfNull(parte);

        _partes.Add(parte);
    }

    public void AgregarLOCExternas(int cantidad)
    {
        if (cantidad < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(cantidad));
        }

        LOCExternas += cantidad;
    }

    public void RegistrarArchivo()
    {
        CantidadArchivos++;
    }

    public void IncorporarResultado(
        ResultadoPrograma resultado)
    {
        ArgumentNullException.ThrowIfNull(resultado);

        foreach (ResultadoParte parte in resultado.Partes)
        {
            AgregarParte(parte);
        }

        AgregarLOCExternas(resultado.LOCExternas);

        CantidadArchivos += resultado.CantidadArchivos;

        CalcularTotal();
    }

    public int CalcularTotal()
    {
        int total = LOCExternas;

        foreach (ResultadoParte parte in Partes)
        {
            total += parte.TotalLOC;
        }

        TotalLOC = total;

        return TotalLOC;
    }
}