using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class ContadorLOC
{
    private enum ContextoBloque
    {
        Archivo,
        Namespace,
        Clase,
        Enumeracion,
        Propiedad,
        BloqueCodigo,
        Lambda
    }

    private const string PatronModificadores =
        @"^(?:(?:public|private|protected|internal|static|virtual|override|abstract|sealed|async|extern|unsafe|new|partial|readonly|required|file)\s+)*";

    public List<UnidadLogica> AnalizarInstrucciones(string contenido)
    {
        ArgumentNullException.ThrowIfNull(contenido);

        List<UnidadLogica> unidades = new List<UnidadLogica>();
        StringBuilder acumulador = new StringBuilder();
        Stack<ContextoBloque> contextos = new Stack<ContextoBloque>();
        Stack<string> clases = new Stack<string>();
        Stack<string> lambdaTextos = new Stack<string>();
        Stack<int[]> lambdaContadores = new Stack<int[]>();

        contextos.Push(ContextoBloque.Archivo);

        int linea = 1;
        int lineaInicio = 1;
        int parentesis = 0;
        int corchetes = 0;
        int inicializadores = 0;
        int ternarios = 0;
        bool inicioLinea = true;
        bool esperarInitPropiedad = false;
        bool enInitPropiedad = false;

        for (int i = 0; i < contenido.Length; i++)
        {
            char actual = contenido[i];
            char siguiente = i + 1 < contenido.Length ? contenido[i + 1] : '\0';

            if (actual == '\n')
            {
                linea++;
                inicioLinea = true;
            }

            if (char.IsWhiteSpace(actual))
            {
                if (acumulador.Length > 0 && acumulador[^1] != ' ')
                {
                    acumulador.Append(' ');
                }

                continue;
            }

            if (actual == '/' && siguiente == '/')
            {
                OmitirComentarioLinea(contenido, ref i, acumulador);
                continue;
            }

            if (actual == '/' && siguiente == '*')
            {
                OmitirComentarioBloque(contenido, ref i, ref linea, acumulador);
                continue;
            }

            if (inicioLinea && actual == '#')
            {
                ProcesarDirectiva(contenido, ref i, linea);
                continue;
            }

            inicioLinea = false;

            if (esperarInitPropiedad)
            {
                esperarInitPropiedad = false;

                if (actual == '=')
                {
                    enInitPropiedad = true;
                }
            }

            ValidarLiteralAdmitido(contenido, i, linea);

            if (EsInicioLiteral(actual, siguiente))
            {
                if (acumulador.Length == 0)
                {
                    lineaInicio = linea;
                }

                LeerLiteral(contenido, ref i, ref linea, acumulador);
                continue;
            }

            if (acumulador.Length == 0)
            {
                lineaInicio = linea;
            }

            if (actual == '{')
            {
                string previo = acumulador.ToString().Trim();

                if (previo.EndsWith("=>"))
                {
                    lambdaTextos.Push(previo);
                    lambdaContadores.Push(new[]
                    {
                        lineaInicio, parentesis, corchetes, inicializadores, ternarios,
                        enInitPropiedad ? 1 : 0
                    });
                    acumulador.Clear();
                    parentesis = 0;
                    corchetes = 0;
                    inicializadores = 0;
                    ternarios = 0;
                    enInitPropiedad = false;
                    unidades.Add(new UnidadLogica("{", TipoUnidad.AperturaBloque, linea, false));
                    contextos.Push(ContextoBloque.Lambda);
                    continue;
                }

                if (parentesis > 0 || corchetes > 0 || inicializadores > 0 ||
                    EsInicioInicializador(previo, enInitPropiedad, contextos.Peek()))
                {
                    inicializadores++;
                    acumulador.Append(actual);
                    continue;
                }

                TipoUnidad? tipoEmitido = AgregarUnidad(
                    unidades, previo, lineaInicio, contextos.Peek(), clases, true, enInitPropiedad);

                unidades.Add(new UnidadLogica("{", TipoUnidad.AperturaBloque, linea, false));
                contextos.Push(DeterminarContexto(contextos.Peek(), tipoEmitido, previo, clases));
                acumulador.Clear();
                ternarios = 0;
                enInitPropiedad = false;
                continue;
            }

            if (actual == '}')
            {
                if (inicializadores > 0)
                {
                    inicializadores--;
                    acumulador.Append(actual);
                    continue;
                }

                if (parentesis > 0 || corchetes > 0 || contextos.Count <= 1)
                {
                    throw new FormatException(
                        $"Cierre de bloque inesperado en la línea {linea}.");
                }

                AgregarUnidad(
                    unidades, acumulador.ToString(), lineaInicio,
                    contextos.Peek(), clases, false, enInitPropiedad);
                acumulador.Clear();
                ternarios = 0;
                enInitPropiedad = false;

                ContextoBloque cerrado = contextos.Pop();

                if (cerrado == ContextoBloque.Clase)
                {
                    clases.Pop();
                }

                unidades.Add(new UnidadLogica("}", TipoUnidad.CierreBloque, linea, false));

                if (cerrado == ContextoBloque.Lambda)
                {
                    int[] estado = lambdaContadores.Pop();
                    lineaInicio = estado[0];
                    parentesis = estado[1];
                    corchetes = estado[2];
                    inicializadores = estado[3];
                    ternarios = estado[4];
                    enInitPropiedad = estado[5] == 1;
                    acumulador.Append(lambdaTextos.Pop()).Append(" { }");
                }
                else if (cerrado == ContextoBloque.Propiedad)
                {
                    esperarInitPropiedad = true;
                }

                continue;
            }

            if (actual == '(')
            {
                parentesis++;
            }
            else if (actual == ')')
            {
                parentesis--;
            }
            else if (actual == '[')
            {
                corchetes++;
            }
            else if (actual == ']')
            {
                corchetes--;
            }
            else if (actual == '?' && parentesis == 0 && corchetes == 0 &&
                     inicializadores == 0 && EsOperadorTernario(contenido, i))
            {
                ternarios++;
            }

            if (parentesis < 0 || corchetes < 0)
            {
                throw new FormatException($"Delimitador inesperado en la línea {linea}.");
            }

            acumulador.Append(actual);

            if (parentesis != 0 || corchetes != 0 || inicializadores != 0)
            {
                continue;
            }

            if (contextos.Peek() == ContextoBloque.Enumeracion)
            {
                if (actual == ',')
                {
                    acumulador.Length--;
                    AgregarUnidad(
                        unidades, acumulador.ToString(), lineaInicio,
                        ContextoBloque.Enumeracion, clases, false, false);
                    acumulador.Clear();
                }

                continue;
            }

            if (actual == ';')
            {
                AgregarUnidad(
                    unidades, acumulador.ToString(), lineaInicio,
                    contextos.Peek(), clases, false, enInitPropiedad);
                acumulador.Clear();
                ternarios = 0;
                enInitPropiedad = false;
            }
            else if (actual == ')' && EsControlConCondicion(acumulador.ToString()))
            {
                AgregarUnidad(
                    unidades, acumulador.ToString(), lineaInicio,
                    contextos.Peek(), clases, false, false);
                acumulador.Clear();
                ternarios = 0;
            }
            else if (actual == ':')
            {
                if (ternarios > 0)
                {
                    ternarios--;
                }
                else if (EsEtiqueta(acumulador.ToString()))
                {
                    AgregarUnidad(
                        unidades, acumulador.ToString(), lineaInicio,
                        contextos.Peek(), clases, false, false);
                    acumulador.Clear();
                    ternarios = 0;
                }
            }
            else if (char.IsLetter(actual) &&
                     EsControlSinParentesisPendiente(acumulador.ToString(), contenido, i))
            {
                AgregarUnidad(
                    unidades, acumulador.ToString(), lineaInicio,
                    contextos.Peek(), clases, false, false);
                acumulador.Clear();
                ternarios = 0;
            }
        }

        if (parentesis != 0 || corchetes != 0 || inicializadores != 0 ||
            lambdaTextos.Count > 0 || contextos.Count > 1)
        {
            throw new FormatException("Se encontraron delimitadores o bloques sin cerrar.");
        }

        AgregarUnidad(
            unidades, acumulador.ToString(), lineaInicio,
            contextos.Peek(), clases, false, enInitPropiedad);

        return unidades;
    }

    public int ContarLOC(IEnumerable<UnidadLogica> unidades)
    {
        ArgumentNullException.ThrowIfNull(unidades);

        int total = 0;

        foreach (UnidadLogica unidad in unidades)
        {
            if (unidad.EsContabilizable)
            {
                total++;
            }
        }

        return total;
    }

    private static ContextoBloque DeterminarContexto(
        ContextoBloque actual,
        TipoUnidad? tipoEmitido,
        string texto,
        Stack<string> clases)
    {
        if (tipoEmitido == TipoUnidad.DeclaracionClase)
        {
            Match match = Regex.Match(texto, @"(?<!@)\bclass\s+([A-Za-z_]\w*)");
            clases.Push(match.Success ? match.Groups[1].Value : string.Empty);
            return ContextoBloque.Clase;
        }

        if (tipoEmitido == TipoUnidad.DeclaracionEnumeracion)
        {
            return ContextoBloque.Enumeracion;
        }

        if (tipoEmitido == TipoUnidad.DeclaracionNamespace)
        {
            return ContextoBloque.Namespace;
        }

        if (tipoEmitido == TipoUnidad.DeclaracionMiembro && actual == ContextoBloque.Clase)
        {
            return ContextoBloque.Propiedad;
        }

        return ContextoBloque.BloqueCodigo;
    }

    private static TipoUnidad? AgregarUnidad(
        List<UnidadLogica> unidades,
        string contenido,
        int linea,
        ContextoBloque contexto,
        Stack<string> clases,
        bool precedeLlave,
        bool enInitPropiedad)
    {
        contenido = contenido.Trim();

        if (string.IsNullOrWhiteSpace(contenido))
        {
            return null;
        }

        if (enInitPropiedad)
        {
            unidades.Add(new UnidadLogica(contenido, TipoUnidad.Otro, linea, false));
            return TipoUnidad.Otro;
        }

        string? claseActual = clases.Count > 0 ? clases.Peek() : null;
        TipoUnidad tipo = ClasificarUnidad(contenido, contexto, claseActual, precedeLlave);
        bool contabilizable = EsContabilizable(contenido, tipo, contexto);

        unidades.Add(new UnidadLogica(contenido, tipo, linea, contabilizable));
        return tipo;
    }

    private static TipoUnidad ClasificarUnidad(
        string contenido,
        ContextoBloque contexto,
        string? claseActual,
        bool precedeLlave)
    {
        if (contenido == ";")
        {
            return TipoUnidad.Otro;
        }

        if (contexto == ContextoBloque.Enumeracion)
        {
            return TipoUnidad.MiembroEnumeracion;
        }

        if (Regex.IsMatch(contenido, @"^namespace\b"))
        {
            return TipoUnidad.DeclaracionNamespace;
        }

        if (Regex.IsMatch(contenido, PatronModificadores + @"class\s+[A-Za-z_]\w*"))
        {
            return TipoUnidad.DeclaracionClase;
        }

        if (Regex.IsMatch(contenido, PatronModificadores + @"enum\s+[A-Za-z_]\w*"))
        {
            return TipoUnidad.DeclaracionEnumeracion;
        }

        if (contexto == ContextoBloque.Propiedad && EsAccesorPropiedad(contenido, out bool conExpresion))
        {
            return conExpresion ? TipoUnidad.Instruccion : TipoUnidad.Otro;
        }

        if (contexto == ContextoBloque.Clase)
        {
            if (EsDeclaracionConstructor(contenido, claseActual, precedeLlave))
            {
                return TipoUnidad.DeclaracionConstructor;
            }

            if (EsDeclaracionMetodo(contenido, precedeLlave))
            {
                return TipoUnidad.DeclaracionMetodo;
            }

            return TipoUnidad.DeclaracionMiembro;
        }

        if (EsControlConCondicion(contenido) || EsEtiqueta(contenido) ||
            EsControlSinParentesis(contenido))
        {
            return TipoUnidad.EstructuraControl;
        }

        if (contenido.EndsWith(";") || precedeLlave)
        {
            return TipoUnidad.Instruccion;
        }

        return TipoUnidad.Otro;
    }

    private static bool EsContabilizable(string contenido, TipoUnidad tipo, ContextoBloque contexto)
    {
        if (contenido == ";" || tipo == TipoUnidad.Otro ||
            tipo == TipoUnidad.AperturaBloque || tipo == TipoUnidad.CierreBloque)
        {
            return false;
        }

        if (tipo == TipoUnidad.EstructuraControl)
        {
            return !EsControlSinParentesis(contenido) && !Regex.IsMatch(contenido, @"^catch\b");
        }

        if (contexto == ContextoBloque.Propiedad && EsAccesorPropiedad(contenido, out bool conExpresion))
        {
            return conExpresion;
        }

        return true;
    }

    private static bool EsAccesorPropiedad(string contenido, out bool tieneExpresion)
    {
        tieneExpresion = false;
        string sinMod = Regex.Replace(contenido, PatronModificadores, string.Empty);

        if (Regex.IsMatch(sinMod, @"^(get|set|init)\s*;?$"))
        {
            return true;
        }

        if (Regex.IsMatch(sinMod, @"^(get|set|init)\s*=>.*;$"))
        {
            tieneExpresion = true;
            return true;
        }

        return false;
    }

    private static bool EsDeclaracionConstructor(
        string contenido,
        string? claseActual,
        bool precedeLlave)
    {
        if (string.IsNullOrEmpty(claseActual))
        {
            return false;
        }

        string sinMod = Regex.Replace(contenido, PatronModificadores, string.Empty);
        string patronCtor = $@"^{Regex.Escape(claseActual)}\s*\(";

        if (!Regex.IsMatch(sinMod, patronCtor))
        {
            return false;
        }

        int posParen = sinMod.IndexOf('(');
        int cierre = BuscarCierreParentesis(sinMod, posParen);

        if (cierre < 0)
        {
            return false;
        }

        string resto = sinMod.Substring(cierre + 1).Trim();

        if (resto.StartsWith(":"))
        {
            if (!Regex.IsMatch(resto, @"^:\s*(base|this)\s*\("))
            {
                return false;
            }

            int cierreInit = BuscarCierreParentesis(resto, resto.IndexOf('('));

            if (cierreInit < 0)
            {
                return false;
            }

            resto = resto.Substring(cierreInit + 1).Trim();
        }

        return (precedeLlave && resto.Length == 0) || Regex.IsMatch(resto, @"^=>.*;$");
    }

    private static bool EsDeclaracionMetodo(string contenido, bool precedeLlave)
    {
        string sinMod = Regex.Replace(contenido, PatronModificadores, string.Empty);
        int posParen = BuscarParentesisFirma(sinMod);

        if (posParen <= 0)
        {
            return false;
        }

        string encabezado = sinMod.Substring(0, posParen).Trim();

        if (!Regex.IsMatch(encabezado, @"^.+?\s+[A-Za-z_]\w*(?:<[^>]+>)?$"))
        {
            return false;
        }

        int cierre = BuscarCierreParentesis(sinMod, posParen);

        if (cierre < 0)
        {
            return false;
        }

        string resto = sinMod.Substring(cierre + 1).Trim();
        return (precedeLlave && (resto.Length == 0 || resto.StartsWith("where "))) ||
               Regex.IsMatch(resto, @"^(?:where\s+.+?)?=>.*;$") ||
               (!precedeLlave && resto == ";");
    }

    private static int BuscarParentesisFirma(string texto)
    {
        int angulos = 0;
        int corchetes = 0;

        for (int i = 0; i < texto.Length; i++)
        {
            char c = texto[i];
            char next = i + 1 < texto.Length ? texto[i + 1] : '\0';

            if (c == '"' || c == '\'')
            {
                return -1;
            }

            if (c == '<')
            {
                angulos++;
            }
            else if (c == '>' && angulos > 0)
            {
                angulos--;
            }
            else if (c == '[')
            {
                corchetes++;
            }
            else if (c == ']' && corchetes > 0)
            {
                corchetes--;
            }
            else if (angulos == 0 && corchetes == 0)
            {
                if (c == '=' && (next == '>' || next != '='))
                {
                    return -1;
                }

                if (c == '(')
                {
                    return i;
                }
            }
        }

        return -1;
    }

    private static int BuscarCierreParentesis(string texto, int inicio)
    {
        int profundidad = 0;

        for (int i = inicio; i < texto.Length; i++)
        {
            char c = texto[i];
            char next = i + 1 < texto.Length ? texto[i + 1] : '\0';

            if (EsInicioLiteral(c, next))
            {
                int lineaDummy = 0;
                LeerLiteral(texto, ref i, ref lineaDummy, new StringBuilder());
                continue;
            }

            if (c == '(')
            {
                profundidad++;
            }
            else if (c == ')')
            {
                profundidad--;

                if (profundidad == 0)
                {
                    return i;
                }
            }
        }

        return -1;
    }

    private static bool EsInicioInicializador(
        string texto,
        bool enInitPropiedad,
        ContextoBloque contexto)
    {
        if (enInitPropiedad)
        {
            return true;
        }

        if (contexto == ContextoBloque.Clase)
        {
            texto = Regex.Replace(texto, PatronModificadores, string.Empty);
        }

        int parentesis = 0;
        int corchetes = 0;
        bool enWhere = false;

        for (int i = 0; i < texto.Length; i++)
        {
            char c = texto[i];
            char next = i + 1 < texto.Length ? texto[i + 1] : '\0';

            if (EsInicioLiteral(c, next))
            {
                int lineaDummy = 0;
                LeerLiteral(texto, ref i, ref lineaDummy, new StringBuilder());
                continue;
            }

            if (c == '(')
            {
                parentesis++;
            }
            else if (c == ')')
            {
                parentesis--;
            }
            else if (c == '[')
            {
                corchetes++;
            }
            else if (c == ']')
            {
                corchetes--;
            }
            else if (parentesis == 0 && corchetes == 0)
            {
                char prev = i > 0 ? texto[i - 1] : '\0';

                if (c == '=' && next != '=' && next != '>' &&
                    prev != '=' && prev != '!' && prev != '<' && prev != '>')
                {
                    return true;
                }

                if (char.IsLetter(c) && (i == 0 || !char.IsLetterOrDigit(prev)))
                {
                    if (CoincidePalabra(texto, i, "where"))
                    {
                        enWhere = true;
                    }
                    else if (CoincidePalabra(texto, i, "new") && !enWhere)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private static bool CoincidePalabra(string texto, int indice, string palabra)
    {
        if (indice + palabra.Length > texto.Length)
        {
            return false;
        }

        if (string.Compare(texto, indice, palabra, 0, palabra.Length, StringComparison.Ordinal) != 0)
        {
            return false;
        }

        int fin = indice + palabra.Length;
        return fin == texto.Length || (!char.IsLetterOrDigit(texto[fin]) && texto[fin] != '_');
    }

    private static bool EsControlConCondicion(string contenido)
    {
        string limpio = contenido.Trim();
        return Regex.IsMatch(
            limpio,
            @"^(?:else\s+if|if|for|foreach|while|switch|catch|using|lock|fixed)\s*\(") &&
            BuscarCierreParentesis(limpio, limpio.IndexOf('(')) == limpio.Length - 1;
    }

    private static bool EsEtiqueta(string contenido)
    {
        return Regex.IsMatch(contenido.Trim(), @"^(?:case\b.*:|default\s*:)$");
    }

    private static bool EsControlSinParentesis(string contenido)
    {
        return Regex.IsMatch(contenido.Trim(), @"^(else|try|catch|finally|do)$");
    }

    private static bool EsControlSinParentesisPendiente(
        string acumulado,
        string contenido,
        int indice)
    {
        if (indice + 1 < contenido.Length &&
            (char.IsLetterOrDigit(contenido[indice + 1]) || contenido[indice + 1] == '_'))
        {
            return false;
        }

        string limpio = acumulado.Trim();

        if (!EsControlSinParentesis(limpio))
        {
            return false;
        }

        int j = SaltarEspaciosYComentarios(contenido, indice + 1);

        if (limpio == "else")
        {
            return !CoincidePalabra(contenido, j, "if");
        }

        if (limpio == "catch")
        {
            return j >= contenido.Length || contenido[j] != '(';
        }

        return true;
    }

    private static int SaltarEspaciosYComentarios(string contenido, int indice)
    {
        int j = indice;

        while (j < contenido.Length)
        {
            if (char.IsWhiteSpace(contenido[j]))
            {
                j++;
                continue;
            }

            if (j + 1 < contenido.Length && contenido[j] == '/' && contenido[j + 1] == '/')
            {
                j += 2;

                while (j < contenido.Length && contenido[j] != '\n')
                {
                    j++;
                }

                continue;
            }

            if (j + 1 < contenido.Length && contenido[j] == '/' && contenido[j + 1] == '*')
            {
                int fin = contenido.IndexOf("*/", j + 2, StringComparison.Ordinal);

                if (fin < 0)
                {
                    return j;
                }

                j = fin + 2;
                continue;
            }

            break;
        }

        return j;
    }

    private static bool EsOperadorTernario(string contenido, int indice)
    {
        char siguiente = indice + 1 < contenido.Length ? contenido[indice + 1] : '\0';

        if (siguiente == '.' || siguiente == '?' || siguiente == '[')
        {
            return false;
        }

        int j = SaltarEspaciosYComentarios(contenido, indice + 1);

        if (j >= contenido.Length)
        {
            return false;
        }

        char despues = contenido[j];
        char despuesSig = j + 1 < contenido.Length ? contenido[j + 1] : '\0';

        if (despues == '>' || despues == '<' || despues == ',' ||
            despues == ')' || despues == ']' || despues == ';' ||
            despues == ':' || despues == '=' || despues == '|' ||
            despues == '?')
        {
            return false;
        }

        if (despues == '!' && despuesSig == '=')
        {
            return false;
        }

        if (despues == '&' && despuesSig == '&')
        {
            return false;
        }

        return !CoincidePalabra(contenido, j, "is") &&
               !CoincidePalabra(contenido, j, "as");
    }

    private static void OmitirComentarioLinea(string contenido, ref int i, StringBuilder acumulador)
    {
        if (acumulador.Length > 0 && acumulador[^1] != ' ')
        {
            acumulador.Append(' ');
        }

        while (i + 1 < contenido.Length && contenido[i + 1] != '\n')
        {
            i++;
        }
    }

    private static void OmitirComentarioBloque(
        string contenido,
        ref int i,
        ref int linea,
        StringBuilder acumulador)
    {
        int lineaInicio = linea;
        i += 2;

        while (i < contenido.Length)
        {
            if (contenido[i] == '\n')
            {
                linea++;
            }

            if (contenido[i] == '*' && i + 1 < contenido.Length && contenido[i + 1] == '/')
            {
                i++;

                if (acumulador.Length > 0 && acumulador[^1] != ' ')
                {
                    acumulador.Append(' ');
                }

                return;
            }

            i++;
        }

        throw new FormatException($"Comentario de bloque sin cerrar en la línea {lineaInicio}.");
    }

    private static void ProcesarDirectiva(string contenido, ref int i, int linea)
    {
        int j = i + 1;

        while (j < contenido.Length && (contenido[j] == ' ' || contenido[j] == '\t'))
        {
            j++;
        }

        int inicioPalabra = j;

        while (j < contenido.Length && char.IsLetter(contenido[j]))
        {
            j++;
        }

        string directiva = contenido.Substring(inicioPalabra, j - inicioPalabra);

        if (directiva == "if" || directiva == "elif" ||
            directiva == "else" || directiva == "endif")
        {
            throw new InvalidOperationException(
                $"Construcción no admitida en la línea {linea}: directiva '#{directiva}'.");
        }

        while (i + 1 < contenido.Length && contenido[i + 1] != '\n')
        {
            i++;
        }
    }

    private static void ValidarLiteralAdmitido(string contenido, int i, int linea)
    {
        if (EsInicioComillasTriples(contenido, i))
        {
            throw new InvalidOperationException(
                $"Construcción no admitida en la línea {linea}: cadena con comillas triples.");
        }

        char actual = contenido[i];
        char siguiente = i + 1 < contenido.Length ? contenido[i + 1] : '\0';
        char tercero = i + 2 < contenido.Length ? contenido[i + 2] : '\0';

        if (actual == '$' && siguiente == '$')
        {
            throw new InvalidOperationException(
                $"Construcción no admitida en la línea {linea}: interpolación compleja.");
        }

        if (((actual == '$' && siguiente == '@') ||
             (actual == '@' && siguiente == '$')) && tercero == '"')
        {
            throw new InvalidOperationException(
                $"Construcción no admitida en la línea {linea}: cadena interpolada textual.");
        }
    }

    private static bool EsInicioComillasTriples(string contenido, int i)
    {
        return (i + 2 < contenido.Length && contenido[i] == '"' &&
                contenido[i + 1] == '"' && contenido[i + 2] == '"') ||
               (i + 3 < contenido.Length && contenido[i] == '$' &&
                contenido[i + 1] == '"' && contenido[i + 2] == '"' && contenido[i + 3] == '"');
    }

    private static bool EsInicioLiteral(char actual, char siguiente)
    {
        return actual == '"' || actual == '\'' ||
               (actual == '@' && siguiente == '"') ||
               (actual == '$' && siguiente == '"');
    }

    private static void LeerLiteral(
        string contenido,
        ref int i,
        ref int linea,
        StringBuilder acumulador)
    {
        int lineaInicio = linea;
        char actual = contenido[i];

        if (actual == '$')
        {
            acumulador.Append("$\"");
            i += 2;

            while (i < contenido.Length)
            {
                char c = contenido[i];
                char next = i + 1 < contenido.Length ? contenido[i + 1] : '\0';

                if (c == '\n' || c == '\r')
                {
                    break;
                }

                if (c == '\\')
                {
                    if (i + 1 >= contenido.Length ||
                        contenido[i + 1] == '\n' || contenido[i + 1] == '\r')
                    {
                        break;
                    }

                    acumulador.Append(c).Append(contenido[i + 1]);
                    i += 2;
                    continue;
                }

                if (c == '"')
                {
                    acumulador.Append(c);
                    return;
                }

                if (c == '{')
                {
                    if (next == '{')
                    {
                        acumulador.Append("{{");
                        i += 2;
                        continue;
                    }

                    LeerHuecoInterpolacion(contenido, ref i, ref linea, lineaInicio, acumulador);
                    i++;
                    continue;
                }

                if (c == '}')
                {
                    if (next == '}')
                    {
                        acumulador.Append("}}");
                        i += 2;
                        continue;
                    }

                    throw new FormatException(
                        $"Llave inesperada en cadena interpolada en la línea {linea}.");
                }

                acumulador.Append(c);
                i++;
            }

            throw new FormatException(
                $"Cadena interpolada sin cerrar en la línea {lineaInicio}.");
        }

        if (actual == '@')
        {
            acumulador.Append("@\"");
            i += 2;

            while (i < contenido.Length)
            {
                char c = contenido[i];

                if (c == '\n')
                {
                    linea++;
                }

                acumulador.Append(c);

                if (c == '"')
                {
                    if (i + 1 < contenido.Length && contenido[i + 1] == '"')
                    {
                        acumulador.Append('"');
                        i += 2;
                        continue;
                    }

                    return;
                }

                i++;
            }

            throw new FormatException(
                $"Literal de cadena textual sin cerrar en la línea {lineaInicio}.");
        }

        acumulador.Append(actual);
        i++;

        while (i < contenido.Length)
        {
            char c = contenido[i];

            if (c == '\n' || c == '\r')
            {
                break;
            }

            if (c == '\\')
            {
                if (i + 1 >= contenido.Length ||
                    contenido[i + 1] == '\n' || contenido[i + 1] == '\r')
                {
                    break;
                }

                acumulador.Append(c).Append(contenido[i + 1]);
                i += 2;
                continue;
            }

            acumulador.Append(c);

            if (c == actual)
            {
                return;
            }

            i++;
        }

        throw new FormatException($"Literal sin cerrar en la línea {lineaInicio}.");
    }

    private static void LeerHuecoInterpolacion(
        string contenido,
        ref int i,
        ref int linea,
        int lineaInicio,
        StringBuilder acumulador)
    {
        acumulador.Append('{');
        i++;
        int llaves = 1;

        while (i < contenido.Length && llaves > 0)
        {
            char c = contenido[i];
            char next = i + 1 < contenido.Length ? contenido[i + 1] : '\0';

            if (c == '\n')
            {
                linea++;
            }

            if (c == '/' && next == '/')
            {
                OmitirComentarioLinea(contenido, ref i, acumulador);
                i++;
                continue;
            }

            if (c == '/' && next == '*')
            {
                OmitirComentarioBloque(contenido, ref i, ref linea, acumulador);
                i++;
                continue;
            }

            ValidarLiteralAdmitido(contenido, i, linea);

            if (EsInicioLiteral(c, next))
            {
                LeerLiteral(contenido, ref i, ref linea, acumulador);
                i++;
                continue;
            }

            acumulador.Append(c);

            if (c == '{')
            {
                llaves++;
            }
            else if (c == '}')
            {
                llaves--;

                if (llaves == 0)
                {
                    return;
                }
            }

            i++;
        }

        throw new FormatException(
            $"Expresión interpolada sin cerrar en la línea {lineaInicio}.");
    }
}
