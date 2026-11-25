using System;
using System.Collections.Generic;
using System.IO;

public class LectorArchivos
{
    public List<string> ObtenerArchivos(string ruta)
    {
        if (string.IsNullOrWhiteSpace(ruta))
        {
            throw new ArgumentException("No se indicó ninguna ruta.");
        }

        ruta = Path.GetFullPath(ruta.Trim());

        if (TieneDirectorioExcluido(ruta))
        {
            throw new ArgumentException(
                "No se analizan directorios bin ni obj.");
        }

        if (File.Exists(ruta))
        {
            if (!EsArchivoCSharp(ruta))
            {
                throw new ArgumentException(
                    "El archivo debe tener extensión .cs.");
            }

            return new List<string> { ruta };
        }

        if (Directory.Exists(ruta))
        {
            List<string> archivos = new List<string>();

            BuscarArchivos(ruta, archivos);

            if (archivos.Count == 0)
            {
                throw new InvalidOperationException(
                    "No se encontraron archivos C#.");
            }

            archivos.Sort(StringComparer.Ordinal);
            return archivos;
        }

        throw new FileNotFoundException($"La ruta no existe: {ruta}");
    }

    private void BuscarArchivos(string directorio, List<string> archivos)
    {
        foreach (string archivo in Directory.EnumerateFiles(directorio))
        {
            if (EsArchivoCSharp(archivo))
            {
                archivos.Add(archivo);
            }
        }

        foreach (string subdirectorio in
            Directory.EnumerateDirectories(directorio))
        {
            string nombre = Path.GetFileName(subdirectorio);

            if (EsDirectorioExcluido(nombre))
            {
                continue;
            }

            if ((File.GetAttributes(subdirectorio)
                & FileAttributes.ReparsePoint) != 0)
            {
                continue;
            }

            BuscarArchivos(subdirectorio, archivos);
        }
    }

    private bool TieneDirectorioExcluido(string ruta)
    {
        string? directorio = Directory.Exists(ruta)
            ? Path.TrimEndingDirectorySeparator(ruta)
            : Path.GetDirectoryName(ruta);

        while (!string.IsNullOrEmpty(directorio))
        {
            string limpio = Path.TrimEndingDirectorySeparator(directorio);
            string nombre = Path.GetFileName(limpio);

            if (EsDirectorioExcluido(nombre))
            {
                return true;
            }

            directorio = Path.GetDirectoryName(limpio);
        }

        return false;
    }

    private static bool EsDirectorioExcluido(string nombre)
    {
        return nombre.Equals("bin", StringComparison.OrdinalIgnoreCase)
            || nombre.Equals("obj", StringComparison.OrdinalIgnoreCase);
    }

    private static bool EsArchivoCSharp(string ruta)
    {
        return Path.GetExtension(ruta).Equals(
            ".cs",
            StringComparison.OrdinalIgnoreCase);
    }

    public string LeerContenido(string rutaArchivo)
    {
        if (string.IsNullOrWhiteSpace(rutaArchivo))
        {
            throw new ArgumentException("No se indicó la ruta del archivo.");
        }

        string rutaCompleta = Path.GetFullPath(rutaArchivo.Trim());

        if (TieneDirectorioExcluido(rutaCompleta))
        {
            throw new ArgumentException(
                "No se analizan archivos dentro de directorios bin u obj.");
        }

        if (!EsArchivoCSharp(rutaCompleta))
        {
            throw new ArgumentException(
                "El archivo debe tener extensión .cs.");
        }

        if (!File.Exists(rutaCompleta))
        {
            throw new FileNotFoundException(
                $"El archivo no existe: {rutaCompleta}");
        }

        return File.ReadAllText(rutaCompleta);
    }
}
