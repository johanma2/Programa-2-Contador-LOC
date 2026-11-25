using System;

public class ConsolaUI
{
    public string PedirRuta()
    {
        Console.WriteLine("CONTADOR DE LINEAS DE CODIGO");
        Console.WriteLine("---------------------------");
        Console.WriteLine();

        Console.WriteLine(
            "Ingrese la ruta de un archivo .cs o de una carpeta:");

        Console.Write("> ");

        return (Console.ReadLine() ?? string.Empty)
            .Trim()
            .Trim('"');
    }

    public void MostrarResultado(ResultadoPrograma resultado)
    {
        ArgumentNullException.ThrowIfNull(resultado);

        int totalMetodos = 0;

        Console.WriteLine();
        Console.WriteLine("RESULTADOS DEL ANALISIS");
        Console.WriteLine(new string('-', 75));

        Console.WriteLine(
            $"{"Nombre de la parte",-45} {"Metodos",12} {"LOC",12}");

        Console.WriteLine(new string('-', 75));

        foreach (ResultadoParte parte in resultado.Partes)
        {
            Console.WriteLine(
                $"{parte.Nombre,-45} " +
                $"{parte.CantidadMetodos,12} " +
                $"{parte.TotalLOC,12}");

            totalMetodos += parte.CantidadMetodos;
        }

        Console.WriteLine(new string('-', 75));

        Console.WriteLine(
            $"LOC externas: {resultado.LOCExternas}");

        Console.WriteLine(
            $"Total de partes: {resultado.Partes.Count}");

        Console.WriteLine(
            $"Total de metodos y constructores: {totalMetodos}");

        Console.WriteLine(
            $"Total de archivos analizados: {resultado.CantidadArchivos}");

        Console.WriteLine(
            $"Total de LOC logicas: {resultado.CalcularTotal()}");
    }

    public void MostrarError(string mensaje)
    {
        Console.Error.WriteLine();
        Console.Error.WriteLine($"Error: {mensaje}");
    }
}