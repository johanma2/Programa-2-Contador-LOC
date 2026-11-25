using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class AnalizadorCodigo
{
    private enum TipoBloque
    {
        Parte,
        Enumeracion,
        Namespace,
        Otro
    }

    private readonly LectorArchivos _lectorArchivos;
    private readonly ContadorLOC _contadorLOC;

    public AnalizadorCodigo()
    {
        _lectorArchivos = new LectorArchivos();
        _contadorLOC = new ContadorLOC();
    }

    public ResultadoPrograma Analizar(string ruta)
    {
        List<string> archivos = _lectorArchivos.ObtenerArchivos(ruta);
        ResultadoPrograma resultadoGeneral = new ResultadoPrograma();

        foreach (string archivo in archivos)
        {
            string contenido = _lectorArchivos.LeerContenido(archivo);
            List<UnidadLogica> unidades = _contadorLOC.AnalizarInstrucciones(contenido);
            int totalEsperado = _contadorLOC.ContarLOC(unidades);
            ResultadoPrograma resultadoArchivo = IdentificarPartes(unidades);
            int totalCalculado = resultadoArchivo.CalcularTotal();

            if (totalEsperado != totalCalculado)
            {
                throw new InvalidOperationException(
                    $"Inconsistencia en el conteo del archivo: {archivo}");
            }

            resultadoArchivo.RegistrarArchivo();
            resultadoGeneral.IncorporarResultado(resultadoArchivo);
        }

        resultadoGeneral.CalcularTotal();
        return resultadoGeneral;
    }

    public ResultadoPrograma IdentificarPartes(IEnumerable<UnidadLogica> unidades)
    {
        ArgumentNullException.ThrowIfNull(unidades);

        ResultadoPrograma resultado = new ResultadoPrograma();
        Stack<ResultadoParte> partesActivas = new Stack<ResultadoParte>();
        Stack<string> namespacesBloque = new Stack<string>();
        Stack<TipoBloque> bloques = new Stack<TipoBloque>();

        string? namespaceArchivo = null;
        string? namespacePendiente = null;
        ResultadoParte? partePendiente = null;
        bool partePendienteEsEnum = false;

        foreach (UnidadLogica unidad in unidades)
        {
            if (unidad.Tipo == TipoUnidad.DeclaracionNamespace)
            {
                ValidarSinPendientes(partePendiente, namespacePendiente, unidad.NumeroLinea);
                string nombreNs = ExtraerNombreNamespace(unidad.Contenido);

                if (unidad.EsContabilizable)
                {
                    resultado.AgregarLOCExternas(1);
                }

                if (unidad.Contenido.EndsWith(";"))
                {
                    namespaceArchivo = nombreNs;
                }
                else
                {
                    namespacePendiente = nombreNs;
                }

                continue;
            }

            if (unidad.Tipo == TipoUnidad.DeclaracionClase ||
                unidad.Tipo == TipoUnidad.DeclaracionEnumeracion)
            {
                ValidarSinPendientes(partePendiente, namespacePendiente, unidad.NumeroLinea);
                string nombre = ConstruirNombreCalificado(
                    ExtraerNombreParte(unidad.Contenido),
                    partesActivas,
                    namespacesBloque,
                    namespaceArchivo);

                ResultadoParte parte = new ResultadoParte(nombre);
                resultado.AgregarParte(parte);

                if (unidad.EsContabilizable)
                {
                    parte.AgregarLOC();
                }

                partePendiente = parte;
                partePendienteEsEnum = unidad.Tipo == TipoUnidad.DeclaracionEnumeracion;
                continue;
            }

            if (unidad.Tipo == TipoUnidad.AperturaBloque)
            {
                if (partePendiente != null)
                {
                    partesActivas.Push(partePendiente);
                    bloques.Push(partePendienteEsEnum ? TipoBloque.Enumeracion : TipoBloque.Parte);
                    partePendiente = null;
                    partePendienteEsEnum = false;
                }
                else if (namespacePendiente != null)
                {
                    namespacesBloque.Push(namespacePendiente);
                    bloques.Push(TipoBloque.Namespace);
                    namespacePendiente = null;
                }
                else
                {
                    bloques.Push(TipoBloque.Otro);
                }

                continue;
            }

            if (unidad.Tipo == TipoUnidad.CierreBloque)
            {
                if (partePendiente != null || namespacePendiente != null || bloques.Count == 0)
                {
                    throw new FormatException(
                        $"Cierre de bloque inesperado en la línea {unidad.NumeroLinea}.");
                }

                TipoBloque tipoBloque = bloques.Pop();

                if (tipoBloque == TipoBloque.Parte || tipoBloque == TipoBloque.Enumeracion)
                {
                    partesActivas.Pop();
                }
                else if (tipoBloque == TipoBloque.Namespace)
                {
                    namespacesBloque.Pop();
                }

                continue;
            }

            ValidarSinPendientes(partePendiente, namespacePendiente, unidad.NumeroLinea);

            if (partesActivas.Count > 0)
            {
                ResultadoParte parteActual = partesActivas.Peek();

                if (unidad.EsContabilizable)
                {
                    parteActual.AgregarLOC();
                }

                bool cuerpoClaseDirecto = bloques.Count > 0 && bloques.Peek() == TipoBloque.Parte;

                if (cuerpoClaseDirecto &&
                    (unidad.Tipo == TipoUnidad.DeclaracionMetodo ||
                     unidad.Tipo == TipoUnidad.DeclaracionConstructor))
                {
                    parteActual.AgregarMetodo();
                }
            }
            else if (unidad.EsContabilizable)
            {
                resultado.AgregarLOCExternas(1);
            }
        }

        if (bloques.Count > 0 || partesActivas.Count > 0 ||
            partePendiente != null || namespacePendiente != null)
        {
            throw new FormatException("Se encontraron declaraciones o bloques sin cerrar.");
        }

        resultado.CalcularTotal();
        return resultado;
    }

    private static void ValidarSinPendientes(
        ResultadoParte? partePendiente,
        string? namespacePendiente,
        int numeroLinea)
    {
        if (partePendiente != null || namespacePendiente != null)
        {
            throw new FormatException(
                $"Se esperaba apertura de bloque en la línea {numeroLinea}.");
        }
    }

    private static string ConstruirNombreCalificado(
        string nombreSimple,
        Stack<ResultadoParte> partesActivas,
        Stack<string> namespacesBloque,
        string? namespaceArchivo)
    {
        if (partesActivas.Count > 0)
        {
            return $"{partesActivas.Peek().Nombre}.{nombreSimple}";
        }

        List<string> segmentos = new List<string>();

        if (!string.IsNullOrEmpty(namespaceArchivo))
        {
            segmentos.Add(namespaceArchivo);
        }

        string[] bloquesNs = namespacesBloque.ToArray();

        for (int i = bloquesNs.Length - 1; i >= 0; i--)
        {
            segmentos.Add(bloquesNs[i]);
        }

        segmentos.Add(nombreSimple);
        return string.Join(".", segmentos);
    }

    private static string ExtraerNombreNamespace(string contenido)
    {
        Match coincidencia = Regex.Match(contenido, @"\bnamespace\s+([A-Za-z_][\w.]*)");

        if (!coincidencia.Success)
        {
            throw new FormatException("No se pudo identificar el espacio de nombres.");
        }

        return coincidencia.Groups[1].Value;
    }

    private static string ExtraerNombreParte(string contenido)
    {
        Match coincidencia = Regex.Match(contenido, @"(?<!@)\b(?:class|enum)\s+([A-Za-z_]\w*)");

        if (!coincidencia.Success)
        {
            throw new FormatException("No se pudo identificar el nombre de la parte.");
        }

        return coincidencia.Groups[1].Value;
    }
}
