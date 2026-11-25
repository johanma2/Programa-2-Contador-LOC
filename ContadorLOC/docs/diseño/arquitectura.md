# Arquitectura y flujo de ejecución — Programa 2

**Fase:** Diseño  
**Proceso:** PSP0.1  
**Versión:** 1.1

## 1. Objetivo

Definir la organización modular, las dependencias y el flujo de información del contador de LOC lógicas.

## 2. Arquitectura

El programa utilizará una arquitectura modular basada en la separación de responsabilidades.

| Componente | Elementos |
|---|---|
| Coordinación | Program |
| Entrada y salida | ConsolaUI |
| Acceso a archivos | LectorArchivos |
| Procesamiento | AnalizadorCodigo y ContadorLOC |
| Representación de unidades | UnidadLogica y TipoUnidad |
| Modelo de resultados | ResultadoPrograma y ResultadoParte |

## 3. Estructura prevista del código fuente

```text
ContadorLOC/
├── Program.cs
├── ContadorLOC.csproj
├── ConsolaUI.cs
├── LectorArchivos.cs
├── AnalizadorCodigo.cs
├── ContadorLOC.cs
├── UnidadLogica.cs
├── TipoUnidad.cs
├── ResultadoParte.cs
├── ResultadoPrograma.cs
├── README.md
└── docs/
```

Los archivos se implementarán durante la fase de Código.

## 4. Dependencias

- Program utiliza ConsolaUI y AnalizadorCodigo.
- AnalizadorCodigo utiliza LectorArchivos y ContadorLOC.
- ContadorLOC genera objetos UnidadLogica.
- UnidadLogica utiliza TipoUnidad para representar su clasificación.
- AnalizadorCodigo utiliza las unidades reconocidas para construir los resultados.
- ResultadoPrograma contiene una colección de ResultadoParte.
- ConsolaUI recibe ResultadoPrograma para presentar la información.

## 5. Flujo de ejecución

1. Obtener la ruta mediante argumentos o consola.
2. Solicitar el análisis a AnalizadorCodigo.
3. Validar la ruta.
4. Localizar los archivos C#.
5. Excluir directorios bin y obj (en rutas directas y búsqueda recursiva).
6. Leer cada archivo.
7. Reconocer comentarios, literales y delimitadores.
8. Generar las unidades lógicas.
9. Contabilizar las unidades.
10. Identificar las partes y sus métodos.
11. Asociar cada LOC con su parte propietaria.
12. Verificar la consistencia del conteo.
13. Consolidar los resultados.
14. Calcular el total general.
15. Presentar los resultados.
16. Finalizar la ejecución.

## 6. Entrada

La ruta podrá proporcionarse mediante un argumento o solicitarse directamente por consola.

```text
CONTADOR DE LOC

Ingrese la ruta del archivo o directorio:
```

## 7. Salida

Ejemplo basado en las trazas de escritorio:

```text
CONTADOR DE LOC

Archivos analizados: 1

Parte         Metodos     LOC
--------------------------------
Persona             2       6
Principal           1       4
--------------------------------

LOC externas: 1
Total de metodos y constructores: 3
TOTAL LOC: 11
```

## 8. Manejo de errores

| Situación | Comportamiento |
|---|---|
| Ruta vacía | Informar que no se indicó una ruta |
| Ruta inexistente | Informar que la ruta no existe |
| Extensión incorrecta | Informar que el archivo no es admitido |
| Directorio sin archivos | Informar que no se encontraron archivos C# |
| Error de lectura | Informar el problema de acceso |
| Construcción no admitida detectada | Informar la limitación |
| Inconsistencia de conteo | Interrumpir el análisis |
| Error inesperado | Informar que no se pudo completar el análisis |

La ejecución exitosa finalizará con código 0.

Las ejecuciones que no puedan completar el análisis finalizarán con código 1.

## 9. Diagramas

Los diagramas se encuentran en:

- diagrama-clases.puml
- diagrama-arquitectura.puml
- diagrama-flujo.puml

El diagrama de clases muestra la estructura de las nueve unidades públicas y sus tipos internos.

El diagrama de arquitectura representa las dependencias entre componentes.

El diagrama de flujo representa las validaciones, el procesamiento de archivos, el conteo y los caminos de error.

## 10. Justificación

La arquitectura permite mantener responsabilidades independientes y reutilizar las unidades reconocidas para evitar inconsistencias.

No se introduce infraestructura adicional que no sea necesaria para cumplir los objetivos del programa.

## 11. Control de cambios

| Versión | Descripción |
|---|---|
| 1.0 | Arquitectura modular inicial. |
| 1.1 | Incorporación de UnidadLogica y TipoUnidad, centralización del análisis y verificación de consistencia. |