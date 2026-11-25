using System;
using System.IO;

public class Program
{
    public static int Main(string[] args)
    {
        ConsolaUI consola = new ConsolaUI();

        try
        {
            string ruta = args.Length > 0
                ? args[0]
                : consola.PedirRuta();

            AnalizadorCodigo analizador =
                new AnalizadorCodigo();

            ResultadoPrograma resultado =
                analizador.Analizar(ruta);

            consola.MostrarResultado(resultado);

            return 0;
        }
        catch (ArgumentException ex)
        {
            consola.MostrarError(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            consola.MostrarError(ex.Message);
        }
        catch (IOException ex)
        {
            consola.MostrarError(ex.Message);
        }
        catch (FormatException ex)
        {
            consola.MostrarError(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            consola.MostrarError(ex.Message);
        }
        catch (NotSupportedException ex)
        {
            consola.MostrarError(ex.Message);
        }
        catch (Exception ex)
        {
            consola.MostrarError(
                $"No se pudo completar el análisis: {ex.Message}");
        }

        return 1;
    }
}