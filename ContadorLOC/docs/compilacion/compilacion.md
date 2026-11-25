# Compilación — Programa 2: Contador de LOC

**Fase PSP:** Compilación  
**Programa:** 2 — Contador de Líneas de Código Lógicas (C#)  
**Fecha:** 2026-11-25  
**Autor:** Johan (registro PSP manual pendiente)

---

## 1. Objetivo de la Fase

Verificar que los nueve archivos fuente del Programa 2 compilan sin errores ni advertencias bajo el SDK y configuración de proyecto establecidos. Esta fase cubre únicamente la compilación estática; la validación funcional se realiza en la fase de Pruebas.

---

## 2. Entorno de Desarrollo

| Parámetro               | Valor                                              |
|-------------------------|----------------------------------------------------|
| Sistema operativo       | Windows 10.0.19045 (Windows 10 22H2)               |
| SDK de .NET             | 11.0.100                                           |
| MSBuild                 | 18.12.2-1.26529.118+297acf930                      |
| SDK adicional instalado | 9.0.304                                            |
| Runtime net10.0         | Microsoft.NETCore.App 10.0.15 ✓                    |
| Runtime net11.0         | Microsoft.NETCore.App 11.0.0 ✓                     |
| Base Path del SDK       | C:\Program Files\dotnet\sdk\11.0.100               |
| Directorio del proyecto | C:\Users\johan\Programa-2-Contador-LOC\ContadorLOC |

> [!NOTE]  
> El SDK 11.0.100 soporta el TFM `net10.0` directamente. El runtime `Microsoft.NETCore.App 10.0.15` está instalado, por lo que el ejecutable producido puede ejecutarse sin necesidad del SDK de desarrollo.

---

## 3. Configuración del Proyecto

Archivo: `ContadorLOC.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>
```

### Archivos fuente incluidos (9 archivos)

Todos los archivos fuente se incluyen de forma implícita mediante el glob `**/*.cs` del SDK de Microsoft.NET:

| N.° | Archivo              | Estado en workspace |
|-----|----------------------|---------------------|
| 1   | AnalizadorCodigo.cs  | Presente ✓          |
| 2   | ConsolaUI.cs         | Presente ✓          |
| 3   | ContadorLOC.cs       | Presente ✓          |
| 4   | LectorArchivos.cs    | Presente ✓          |
| 5   | Program.cs           | Presente ✓          |
| 6   | ResultadoParte.cs    | Presente ✓          |
| 7   | ResultadoPrograma.cs | Presente ✓          |
| 8   | TipoUnidad.cs        | Presente ✓          |
| 9   | UnidadLogica.cs      | Presente ✓          |

### Dependencias externas

**Ninguna.** El proyecto no referencia paquetes NuGet ni dependencias externas. Solo se utiliza el SDK implícito `Microsoft.NET.Sdk`.

---

## 4. Comandos Ejecutados

### Comando 1 — Primera compilación

```powershell
cd C:\Users\johan\Programa-2-Contador-LOC\ContadorLOC
dotnet build ContadorLOC.csproj
```

### Comando 2 — Compilación final sin incremental

```powershell
cd C:\Users\johan\Programa-2-Contador-LOC\ContadorLOC
dotnet build ContadorLOC.csproj --no-incremental
```

---

## 5. Resultado de la Primera Compilación

**Comando:** `dotnet build ContadorLOC.csproj`  
**Fecha/Hora:** 2026-11-25 (fase de Compilación PSP0.1)

### Salida completa del compilador

```
  Determinando los proyectos que se van a restaurar...
  Todos los proyectos están actualizados para la restauración.
  ContadorLOC -> C:\Users\johan\Programa-2-Contador-LOC\ContadorLOC\bin\Debug\net10.0\ContadorLOC.dll

Compilación correcta.
    0 Advertencia(s)
    0 Errores

Tiempo transcurrido 00:01:08.73
```

| Métrica            | Valor        |
|--------------------|--------------|
| Código de salida   | 0            |
| Errores            | 0            |
| Advertencias       | 0            |
| Tiempo transcurrido | 1 m 08.73 s |
| Artefacto producido | `bin\Debug\net10.0\ContadorLOC.dll` |

> [!NOTE]
> El tiempo de 1 m 08 s corresponde a la compilación completa del proyecto. En compilaciones subsiguientes sin modificaciones el tiempo se reduce significativamente por el caché de MSBuild.

---

## 6. Errores y Advertencias Encontrados

**No se detectaron errores ni advertencias en ninguna de las dos compilaciones.**

La primera compilación produjo directamente un resultado limpio (`Compilación correcta. 0 Advertencia(s) 0 Errores`), lo que confirma que:

- Las correcciones aplicadas en las fases de Código (D-01 a D-14) y Revisión de Código (D-15 a D-21) fueron suficientes para garantizar la compilación limpia.
- Los `using` explícitos presentes en varios archivos (`using System;`, `using System.Collections.Generic;`, `using System.Text;`) no generan CS8019 con la configuración actual del SDK 11.0.100 bajo `ImplicitUsings=enable`. El compilador no los reporta como innecesarios en este entorno.
- La habilitación de `<Nullable>enable</Nullable>` no produjo advertencias CS8600/CS8601/CS8602, lo que indica que la gestión de nulabilidad del código es consistente con el modo nullable.

---

## 7. Correcciones Realizadas

**No se realizaron correcciones durante la fase de Compilación.**

La compilación fue exitosa en el primer intento, sin requerir modificación de ningún archivo fuente. No se registran defectos de compilación (D-22 en adelante no fue necesario utilizar).

---

## 8. Resultado de la Compilación Final

**Comando:** `dotnet build ContadorLOC.csproj --no-incremental`

### Salida completa del compilador

```
  Determinando los proyectos que se van a restaurar...
  Todos los proyectos están actualizados para la restauración.
  ContadorLOC -> C:\Users\johan\Programa-2-Contador-LOC\ContadorLOC\bin\Debug\net10.0\ContadorLOC.dll

Compilación correcta.
    0 Advertencia(s)
    0 Errores

Tiempo transcurrido 00:00:08.59
```

| Métrica            | Valor        |
|--------------------|--------------|
| Código de salida   | 0            |
| Errores            | 0            |
| Advertencias       | 0            |
| Tiempo transcurrido | 8.59 s      |
| Artefacto producido | `bin\Debug\net10.0\ContadorLOC.dll` |

> [!IMPORTANT]
> La compilación `--no-incremental` fuerza la recompilación completa de todos los archivos fuente sin utilizar caché de salidas anteriores. El resultado de 0 errores y 0 advertencias confirma que el proyecto es compilable de forma reproducible e independiente del estado previo.

---

## 9. Verificación del Requisito RNF-07

**RNF-07:** El programa debe compilar sin errores ni advertencias bajo .NET con `Nullable=enable`.

| Criterio                                | Resultado |
|-----------------------------------------|-----------|
| Compilación sin errores                 | ✓ Cumplido |
| Compilación sin advertencias            | ✓ Cumplido |
| `Nullable=enable` activo                | ✓ Verificado en csproj |
| SDK compatible con TFM `net10.0`        | ✓ SDK 11.0.100 + Runtime 10.0.15 |
| Sin dependencias externas               | ✓ Solo Microsoft.NET.Sdk |
| Artefacto ejecutable generado           | ✓ `ContadorLOC.dll` en bin\Debug\net10.0 |

**Estado de RNF-07:** ✅ **CUMPLIDO**

---

## 10. Número de Compilaciones Realizadas

| N.° | Comando                                        | Resultado       | Tiempo      |
|-----|------------------------------------------------|-----------------|-------------|
| 1   | `dotnet build ContadorLOC.csproj`              | ✅ 0E / 0W / Exit 0 | 1 m 08.73 s |
| 2   | `dotnet build ContadorLOC.csproj --no-incremental` | ✅ 0E / 0W / Exit 0 | 8.59 s   |

**Total compilaciones:** 2  
**Compilaciones fallidas:** 0  
**Rondas de corrección:** 0

---

## 11. Conclusiones

1. **La compilación fue exitosa en el primer intento.** No se requirió ninguna iteración de corrección.
2. **Los nueve archivos fuente compilaron sin errores ni advertencias**, confirmando la consistencia sintáctica, semántica y de tipos del código producido en las fases anteriores.
3. **El requisito RNF-07 está cumplido** con evidencia real del compilador.
4. **Esta fase cubre únicamente la validación estática del compilador.** La corrección funcional del algoritmo de conteo LOC, el comportamiento ante casos límite y el cumplimiento de los requisitos RF-01 a RF-09 se verificarán en la fase de Pruebas.
5. **No se modificaron archivos fuente** durante esta fase. Las estimaciones iniciales (190 LOC, 498 minutos) permanecen inalteradas.
6. **El proyecto está listo para la fase de Pruebas.**

---

*Documento generado en la fase de Compilación del Programa 2 — PSP0.1*  
*Tiempo de esta fase: pendiente de registro manual en el Tracker PSP*
