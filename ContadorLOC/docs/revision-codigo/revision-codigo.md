# Revisión de código — Programa 2: Contador de LOC

**Fase:** Revisión de código  
**Proceso:** PSP0.1 en PSP Tracker  
**Lenguaje:** C# / .NET 10  
**Tiempo estimado de fase:** 55 min (conservado sin alteraciones)

---

## 1. Objetivo de la revisión

Realizar una revisión estática integral del código fuente del Programa 2 antes de la fase de Compilación, verificando el cumplimiento de:

- Los requisitos funcionales (`RF-01` a `RF-09`) y no funcionales (`RNF-01` a `RNF-08`) en [requisitos.md](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/docs/planeacion/requisitos.md).
- El estándar de codificación en [estandar-codificacion.md](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/docs/planeacion/estandar-codificacion.md).
- El estándar de conteo de LOC lógicas v1.1 (y revisión de Código) en [estandar-conteo-loc.md](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/docs/dise%C3%B1o/estandar-conteo-loc.md).
- El diseño de clases, pseudocódigo, arquitectura, trazas de escritorio y diagramas PlantUML en `ContadorLOC/docs/diseño/`.
- La permanencia de las correcciones de los defectos `D-01` a `D-14` tras la simplificación aplicada en la fase de Código.

> [!NOTE]
> Esta intervención corresponde exclusivamente a **revisión estática**. No se ejecutaron `dotnet build`, `dotnet run` ni pruebas unitarias/de integración; la verificación en tiempo de compilación y ejecución queda pendiente para las fases de Compilación y Pruebas.

---

## 2. Archivos revisados

Se revisaron las 9 unidades de código fuente del proyecto (sin modificar el Programa 1):

| # | Archivo | Responsabilidad |
|---:|---|---|
| 1 | [ContadorLOC.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/ContadorLOC.cs) | Análisis léxico/sintáctico de una sola pasada, clasificación de `UnidadLogica` y conteo de LOC lógicas. |
| 2 | [AnalizadorCodigo.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/AnalizadorCodigo.cs) | Coordinación del análisis, identificación de partes, cálculo de nombres calificados y verificación de consistencia. |
| 3 | [LectorArchivos.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/LectorArchivos.cs) | Validación de rutas, búsqueda recursiva de `.cs`, exclusión de `bin`/`obj` y lectura de contenido. |
| 4 | [Program.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/Program.cs) | Punto de entrada, obtención de ruta, captura de excepciones y códigos de salida `0`/`1`. |
| 5 | [ConsolaUI.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/ConsolaUI.cs) | Interacción por consola, formato de tabla de resultados y salida de errores por `Console.Error`. |
| 6 | [ResultadoPrograma.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/ResultadoPrograma.cs) | Consolidación de partes, LOC externas, archivos analizados y cálculo del total general. |
| 7 | [ResultadoParte.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/ResultadoParte.cs) | Modelo de métricas por parte (`Nombre`, `CantidadMetodos`, `TotalLOC`). |
| 8 | [UnidadLogica.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/UnidadLogica.cs) | Modelo inmutable de unidad reconocida (`Contenido`, `Tipo`, `NumeroLinea`, `EsContabilizable`). |
| 9 | [TipoUnidad.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/TipoUnidad.cs) | Enumeración de clasificación de las 12 categorías de unidades lógicas. |

---

## 3. Metodología

1. **Lectura de documentación de referencia:** se contrastaron requisitos, estándar de codificación, estándar de conteo v1.1 (rev. Código), diseño de clases, pseudocódigo, arquitectura, trazas de escritorio y diagramas `.puml`.
2. **Inspección de cambios previos (`git diff` sobre la fase de Código):** se revisó el historial de simplificación de `ContadorLOC.cs`, `AnalizadorCodigo.cs` y `LectorArchivos.cs` para verificar que las correcciones de `D-01` a `D-14` permanecieran vigentes y detectar regresiones.
3. **Revisión estática línea por línea:** cada archivo se inspeccionó frente a la lista de comprobación de calidad, nulabilidad, manejo de excepciones, pilas de contexto, delimitadores, literales, comentarios, inicializadores, propiedades, lambdas, clases anidadas, enumeraciones y compatibilidad del analizador sobre su propio código fuente.
4. **Corrección en una sola intervención:** se corrigieron únicamente defectos comprobables estáticamente, sin refactorizaciones generales, sin dependencias externas (sin Roslyn) y sin ampliar el alcance.

---

## 4. Lista de comprobación

- [x] Cumplimiento del estándar de codificación (nombres PascalCase/`camelCase`/`_camelCase`, 4 espacios, llaves en líneas independientes, separación de responsabilidades).
- [x] Cumplimiento de requisitos `RF-01` a `RF-09` y `RNF-01` a `RNF-08` (con verificación en compilación/pruebas pendiente para `RNF-04`, `RNF-07` y `RNF-08`).
- [x] Coherencia entre diseño (`diseño-clases.md`, `pseudocodigo.md`, `arquitectura.md`, `.puml`) e implementación.
- [x] Validación de parámetros y nulabilidad (`<Nullable>enable</Nullable>`, corregido en `ResultadoParte` y `UnidadLogica` — `D-21`).
- [x] Manejo de excepciones sin ocultar fallos ni presentar resultados parciales.
- [x] Inicialización de variables y aislamiento de estado en pilas (corregido aislamiento de `enInitPropiedad` en lambdas — `D-18`).
- [x] Condiciones y estructuras de control (`if`, `else if`, `else`, `for`, `foreach`, `while`, `do-while`, `switch`, `case`, `default`, `try`/`catch`/`finally`, corregido `D-15`).
- [x] Equilibrio de bloques, pilas y delimitadores (`()`, `[]`, `{}`).
- [x] Conteo de LOC lógicas sin duplicaciones (invariante `TotalLOC = suma(Partes) + LOCExternas`).
- [x] Identificación de clases, métodos, constructores y enumeraciones (corregido modificador `new` en `EsInicioInicializador` — `D-17`).
- [x] Procesamiento de comentarios, literales e interpolaciones (corregidos `D-15`, `D-19` y `D-20`).
- [x] Propiedades automáticas, con cuerpo de expresión, con cuerpo de bloque e inicializadores (`D-18`).
- [x] Clases anidadas y `namespace` de bloque o ámbito de archivo (`ConstruirNombreCalificado`).
- [x] Exclusión de `bin` y `obj` en rutas directas y búsqueda recursiva, evitando enlaces simbolicos (`ReparsePoint`).
- [x] Manejo de rutas inválidas, vacías, inexistentes o con extensión distinta de `.cs`.
- [x] Detección de construcciones no admitidas (`#if`/`#elif`/`#else`/`#endif`, `"""`, `$"""`, `$$`, y corregidas `$@"`/`@$"` — `D-19`).
- [x] Compatibilidad estática del analizador con los 9 archivos de su propio código fuente.

---

## 5. Revisión de defectos anteriores (D-01 a D-14)

Se verificó la permanencia de las correcciones previas tras la simplificación realizada en Código:

| Referencia | Aspecto evaluado | Resultado de la verificación |
|---|---|---|
| Generales (`D-01`..`D-14`) | Exclusión directa y recursiva de `bin`/`obj`, `ReparsePoint`, `namespace` de archivo, exclusión de métodos en `enum` (`TipoBloque.Enumeracion`), identificadores `@class`/`@enum`, validación `ValidarSinPendientes`, rechazo de `#if`/`#elif`/`#else`/`#endif`, `"""`, `$$`, lambdas `=> { ... }`, inicializadores de propiedad y `where T : new()`. | **Vigentes** tras la simplificación, salvo las regresiones/omisiones concretas registradas como `D-15`, `D-19` y `D-20`. |
| **D-09** | Restricciones genéricas complejas. | Se determina que las restricciones simples `where T : ...` (incluido `where T : new()`) ya están cubiertas en `ContadorLOC.cs`. Las restricciones genéricas complejas (p. ej. firmas `abstract`/de interfaz con `where ... ;`, o combinación con consultas LINQ `where`/`select` e inicializadores anónimos) **no pertenecen al alcance vigente**, ya que los Programas 1 y 2 no las requieren y el estándar v1.1 excluye construcciones avanzadas innecesarias para ambos programas. No constituye defecto a corregir. |
| **D-11** | Directiva `using` sin utilizar. | Confirmado: conforme al estándar de conteo v1.1, toda directiva `using` aporta `1 LOC` independientemente de si sus símbolos se referencian o no. No constituye una LOC falsa ni debe registrarse como defecto funcional. |
| **D-13** | Ejemplo inválido `case string? s:` y clasificación de `?`. | Confirmado que `case string? s:` no es un patrón válido en C#. Sin embargo, se identificaron **casos válidos en C#** que reproducen el defecto: `case List<string?> lista:`, `case Dictionary<string, int?> d:` y `case int n when obj as int? > 0:`. En todos ellos el `?` incrementaba `ternarios` y consumía el `:` de la etiqueta `case`. Corregido como **D-16**. |
| **D-14** | Comentarios entre `else` e `if`. | **Regresión detectada tras la simplificación:** al reemplazar el tokenizador por una sola pasada, `EsControlSinParentesisPendiente` pasó a omitir únicamente espacios en blanco (`char.IsWhiteSpace`) sin saltar comentarios `//` ni `/* ... */`. Esto provoca la emisión prematura de `else` en `else /* ... */ if` y, en el caso análogo `catch /* ... */ (Exception ex)`, genera `+1 LOC` falsa al clasificar `(Exception ex)` como `Instruccion`. Corregido como **D-15**. |

---

## 6. Hallazgos por archivo

### 6.1. `ContadorLOC.cs`
Se identificaron 6 defectos reales comprobables estáticamente:
- **D-15:** `EsControlSinParentesisPendiente` no omitía comentarios `//` ni `/* ... */` al mirar adelante tras `else` o `catch`.
- **D-16:** `AnalizarInstrucciones` incrementaba `ternarios` ante `?` de tipos anulables válidos dentro de etiquetas `case` (p. ej., `case List<string?> lista:` o `case int n when obj as int? > 0:`), impidiendo reconocer el `:` final de la etiqueta.
- **D-17:** `EsInicioInicializador` interpretaba el modificador `new` en declaraciones de miembro o tipo anidado dentro de una clase (p. ej., `public new void Mostrar() { ... }`, `public new class Anidada { ... }`) como inicio de inicializador, absorbiendo el bloque completo.
- **D-18:** Al abrir un bloque lambda `=> { ... }` dentro del inicializador de una propiedad automática (p. ej., `public Action A { get; set; } = () => { Foo(); };`), no se guardaba ni reiniciaba `enInitPropiedad`, omitiendo la primera instrucción de la lambda y contabilizando falsamente el cierre `= () => { };`.
- **D-19:** Las combinaciones excluidas `$@"..."` y `@$"..."` (documentadas en el estándar v1.1 rev. Código) no se reportaban como construcciones no admitidas, ni en el bucle principal ni dentro de `LeerHuecoInterpolacion`.
- **D-20:** Regresión de simplificación en `LeerHuecoInterpolacion`: no omitía comentarios `//` ni `/* ... */` dentro de `{ ... }` de una cadena interpolada `$"..."`.

### 6.2. `ResultadoParte.cs`
- **D-21 (compartido con `UnidadLogica.cs`):** El constructor `ResultadoParte(string nombre)` no validaba `null` o cadena vacía/espacios en `nombre`, a diferencia del resto de clases públicas del proyecto.

### 6.3. `UnidadLogica.cs`
- **D-21 (compartido con `ResultadoParte.cs`):** El constructor `UnidadLogica(string contenido, ...)` no validaba `ArgumentNullException.ThrowIfNull(contenido)`.

### 6.4. `AnalizadorCodigo.cs`, `LectorArchivos.cs`, `Program.cs`, `ConsolaUI.cs`, `ResultadoPrograma.cs`, `TipoUnidad.cs`
- Sin defectos funcionales ni de nulabilidad comprobables estáticamente.
- Cumplen el estándar de codificación, la coherencia con el diseño y los requisitos asignados.
- Nota menor de estilo/documentación: el estándar de codificación sugiere comentarios XML en métodos públicos cuando aporten información relevante; actualmente no se incluyen comentarios XML en las clases públicas (no se agregan para respetar la restricción de no alterar el tamaño ni realizar refactorizaciones generales).
- Nota menor de desbordamiento (`estandar-codificacion.md` §5): los contadores `int` operan en contexto `unchecked`; dado el tamaño práctico de archivos `.cs`, el riesgo es despreciable dentro del alcance.

---

## 7. Correcciones aplicadas

Las correcciones aplicadas en esta intervención se documentan en detalle en [defectos-revision-codigo.md](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/docs/revision-codigo/defectos-revision-codigo.md):

1. **`ContadorLOC.cs` (`D-15`):** Se agregó `SaltarEspaciosYComentarios` y se utilizó en `EsControlSinParentesisPendiente` para ignorar comentarios `//` y `/* ... */` entre `else` e `if`, así como entre `catch` y `(`.
2. **`ContadorLOC.cs` (`D-16`):** Se agregó `EsOperadorTernario` para descartar `?` de nulabilidad seguidos (tras espacios/comentarios) por delimitadores u operadores binarios (`>`, `<`, `,`, `)`, `]`, `;`, `:`, `=`, `!=`, `&&`, `|`, `?`, `is`, `as`).
3. **`ContadorLOC.cs` (`D-17`):** Se pasó `ContextoBloque` a `EsInicioInicializador` para retirar los modificadores iniciales (`PatronModificadores`, incluido `new`) cuando `contexto == ContextoBloque.Clase`.
4. **`ContadorLOC.cs` (`D-18`):** Se guardó `enInitPropiedad` en `lambdaContadores`, se reinició a `false` al entrar a `ContextoBloque.Lambda` y se restauró al cerrar la lambda.
5. **`ContadorLOC.cs` (`D-19`):** Se extrajo `ValidarLiteralAdmitido` (rechazando `"""`, `$"""`, `$$`, `$@"` y `@$"`) y se invocó tanto en `AnalizarInstrucciones` como en `LeerHuecoInterpolacion`.
6. **`ContadorLOC.cs` (`D-20`):** Se restauró la omisión de comentarios `//` y `/* ... */` en `LeerHuecoInterpolacion`.
7. **`ResultadoParte.cs` y `UnidadLogica.cs` (`D-21`):** Se añadieron validaciones de parámetros (`string.IsNullOrWhiteSpace(nombre)` y `ArgumentNullException.ThrowIfNull(contenido)`).

---

## 8. Matriz de cumplimiento de requisitos

| Requisito | Estado (Revisión estática) | Evidencia en el código |
|---|---|---|
| **RF-01** Seleccionar código fuente | Cumple (estático) | `Program.Main`, `ConsolaUI.PedirRuta`, `LectorArchivos.ObtenerArchivos` |
| **RF-02** Leer archivos fuente sin modificarlos | Cumple (estático) | `LectorArchivos.LeerContenido` (`File.ReadAllText`, solo lectura) |
| **RF-03** Contar líneas de código según estándar | Cumple tras correcciones | `ContadorLOC.AnalizarInstrucciones` y `ContarLOC` (corregidos `D-15`..`D-20`) |
| **RF-04** Identificar partes del programa | Cumple (estático) | `AnalizadorCodigo.IdentificarPartes` (clases, anidadas, `enum` y `namespace`) |
| **RF-05** Contar métodos y constructores por parte | Cumple tras `D-17` | `AnalizadorCodigo.IdentificarPartes` + `ContadorLOC.EsDeclaracionMetodo`/`Constructor` |
| **RF-06** Calcular tamaño por parte sin duplicar | Cumple (estático) | `AnalizadorCodigo` asigna solo a `partesActivas.Peek()` |
| **RF-07** Calcular tamaño total e invariante | Cumple (estático) | `ResultadoPrograma.CalcularTotal` + verificación `totalEsperado != totalCalculado` |
| **RF-08** Mostrar resultados en consola | Cumple (estático) | `ConsolaUI.MostrarResultado` |
| **RF-09** Informar errores sin resultados parciales | Cumple (estático) | `LectorArchivos`, `ContadorLOC`, `AnalizadorCodigo` y `Program.Main` |
| **RNF-01** Lenguaje C# y .NET 10 consola | Cumple (estático) | `ContadorLOC.csproj` (`net10.0`, `Exe`) |
| **RNF-02** Estándar de codificación | Cumple (estático) | Nombres, indentación, llaves y separación de responsabilidades en las 9 unidades |
| **RNF-03** Estándar de conteo documentado | Cumple | `docs/diseño/estandar-conteo-loc.md` v1.1 |
| **RNF-04** Exactitud y consistencia | Pendiente de Pruebas | Requiere ejecución y comparación con conteos manuales de Programas 1 y 2 |
| **RNF-05** Manejo de errores comprensible | Cumple (estático) | Captura tipada en `Program.Main` + `ConsolaUI.MostrarError` |
| **RNF-06** Mantenibilidad y separación | Cumple (estático) | 9 unidades diferenciadas según `arquitectura.md` |
| **RNF-07** Verificación de compilación sin advertencias | Pendiente de Compilación | No autorizado ejecutar `dotnet build` en esta fase |
| **RNF-08** Compatibilidad con PSP | Parcial (en curso) | Documentación actualizada; tiempos y defectos pendientes de registro manual en PSP Tracker |

---

## 9. Limitaciones pendientes

Conforme al alcance definido en `estandar-conteo-loc.md` (v1.1 rev. Código), permanecen deliberadamente fuera de alcance:

- Cadenas de comillas triples (`"""..."""`, `$"""..."""`) e interpolaciones complejas (`$$"..."`, `$@"..."`, `@$"..."`) — detectadas e informadas como no admitidas.
- Directivas de compilación condicional (`#if`, `#elif`, `#else`, `#endif`) — detectadas e informadas como no admitidas.
- Clases parciales (consolidación entre archivos), interfaces, `struct`, `record`, constructores primarios y funciones locales.
- Atributos sobre declaraciones, tuplas de retorno, expresiones `switch`, expresiones `with`, `delegate`/`event` y restricciones genéricas complejas (`D-09`).
- Filtros `catch (...) when (...)` con cláusula `when` compleja, no contemplados en Programas 1 y 2.
- Comentarios XML públicos opcionales según `estandar-codificacion.md`.

---

## 10. Conclusión de la revisión

La revisión estática de los 9 archivos fuente del Programa 2 permitió verificar la vigencia de las correcciones de `D-01` a `D-14`, descartar falsos positivos (`D-09` fuera de alcance y `D-11` conforme al estándar), identificar un caso válido reproducible para `D-13` (`D-16`) y corregir **7 defectos reales** (`D-15` a `D-21`) concentrados principalmente en `ContadorLOC.cs` y en validaciones de constructores de `ResultadoParte.cs` y `UnidadLogica.cs`.

El proyecto queda **listo para pasar a la fase de Compilación** (`dotnet build`) para validar estáticamente la ausencia de errores y advertencias de compilación, y posteriormente a la fase de **Pruebas** (`dotnet run` y comparación con conteos manuales independientes de los Programas 1 y 2).
