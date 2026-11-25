# Registro de defectos — Revisión de código (Programa 2)

**Fase:** Revisión de código  
**Proceso:** PSP0.1 en PSP Tracker  
**Nota de numeración:** Se continúa la secuencia a partir de `D-15`, evitando duplicar los defectos `D-01` a `D-14` previamente registrados en la fase de Código.

---

## Resumen de defectos nuevos detectados

| ID | Archivo | Línea(s) | Tipo PSP | Inyección | Eliminación | Estado | Tiempo de corrección |
|---|---|---:|---|---|---|---|---|
| D-15 | `ContadorLOC.cs` | 762–836 | 80 - Function | Código | Revisión de código | Corregido (pendiente compilación/pruebas) | Pendiente de registro manual |
| D-16 | `ContadorLOC.cs` | 222–226, 838–877 | 80 - Function | Código | Revisión de código | Corregido (pendiente compilación/pruebas) | Pendiente de registro manual |
| D-17 | `ContadorLOC.cs` | 137–139, 653–666 | 80 - Function | Código | Revisión de código | Corregido (pendiente compilación/pruebas) | Pendiente de registro manual |
| D-18 | `ContadorLOC.cs` | 118–135, 187–197 | 70 - Data | Código | Revisión de código | Corregido (pendiente compilación/pruebas) | Pendiente de registro manual |
| D-19 | `ContadorLOC.cs` | 96, 957–981, 1184 | 60 - Checking | Código | Revisión de código | Corregido (pendiente compilación/pruebas) | Pendiente de registro manual |
| D-20 | `ContadorLOC.cs` | 1170–1182 | 80 - Function | Código | Revisión de código | Corregido (pendiente compilación/pruebas) | Pendiente de registro manual |
| D-21 | `ResultadoParte.cs`, `UnidadLogica.cs` | 9–16 / 11–18 | 60 - Checking | Código | Revisión de código | Corregido (pendiente compilación/pruebas) | Pendiente de registro manual |

---

## Detalle de defectos

### D-15
- **Identificador del defecto:** D-15
- **Archivo y línea:** [ContadorLOC.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/ContadorLOC.cs#L762-L836) (`EsControlSinParentesisPendiente`, `SaltarEspaciosYComentarios`)
- **Descripción:** Regresión tras la simplificación de Código (relacionada con `D-14`): cuando hay un comentario `//` o `/* ... */` entre `else` e `if`, o entre `catch` y `(`, `EsControlSinParentesisPendiente` emite prematuramente `else` o `catch` como unidad aislada. En `else /* c */ if (...)` separa indebidamente `else if`; en `catch /* c */ (Exception ex) { ... }`, el encabezado `(Exception ex)` posterior queda clasificado como `TipoUnidad.Instruccion` contabilizable y suma `+1` LOC falsa.
- **Causa:** Al sustituir el tokenizador previo por una lectura de una sola pasada, la búsqueda anticipada en `EsControlSinParentesisPendiente` únicamente avanzaba sobre `char.IsWhiteSpace` e ignoraba comentarios.
- **Tipo PSP:** 80 - Function
- **Fase de inyección:** Código
- **Fase de eliminación:** Revisión de código
- **Corrección aplicada:** Se implementó el método auxiliar `SaltarEspaciosYComentarios` para omitir espacios en blanco y comentarios `//` y `/* ... */` antes de verificar `if` o `(` en `EsControlSinParentesisPendiente`.
- **Estado:** Corregido (verificado estáticamente; pendiente de compilación y pruebas).
- **Tiempo de corrección:** Pendiente de registro manual en PSP Tracker.

---

### D-16
- **Identificador del defecto:** D-16
- **Archivo y línea:** [ContadorLOC.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/ContadorLOC.cs#L222-L226) y [ContadorLOC.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/ContadorLOC.cs#L838-L877) (`EsOperadorTernario`)
- **Descripción:** En casos válidos de C# dentro de etiquetas `case` que contienen `?` de tipo anulable fuera de paréntesis y corchetes —por ejemplo `case List<string?> lista:`, `case Dictionary<string, int?> d:` o `case int n when obj as int? > 0:` (reproducción válida del escenario planteado en `D-13`, dado que `case string? s:` no es sintaxis C# válida)— el carácter `?` incrementa `ternarios`, haciendo que el carácter `:` de la etiqueta `case` se consuma como cierre de ternario en lugar de emitir la unidad `case`.
- **Causa:** La detección de operador ternario solo descartaba `?.`, `??` y `?[`, asumiendo que cualquier otro `?` fuera de `()`/`[]`/`{}` abría un operador condicional `?:`.
- **Tipo PSP:** 80 - Function
- **Fase de inyección:** Código
- **Fase de eliminación:** Revisión de código
- **Corrección aplicada:** Se extrajo `EsOperadorTernario` reutilizando `SaltarEspaciosYComentarios` para descartar los `?` seguidos por delimitadores u operadores que no pueden iniciar la rama verdadera de un ternario (`>`, `<`, `,`, `)`, `]`, `;`, `:`, `=`, `!=`, `&&`, `|`, `?`, `is`, `as`).
- **Estado:** Corregido (verificado estáticamente; pendiente de compilación y pruebas).
- **Tiempo de corrección:** Pendiente de registro manual en PSP Tracker.

---

### D-17
- **Identificador del defecto:** D-17
- **Archivo y línea:** [ContadorLOC.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/ContadorLOC.cs#L137-L139) y [ContadorLOC.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/ContadorLOC.cs#L653-L666) (`EsInicioInicializador`)
- **Descripción:** Una declaración de método, propiedad o clase anidada dentro de una clase que utilice el modificador de ocultamiento `new` (por ejemplo, `public new void Mostrar() { ... }`, `public new int Valor { get; set; }` o `public new class Anidada { ... }`) es interpretada por `EsInicioInicializador` como un inicializador (`new ... { ... }`), incrementando `inicializadores` y omitiendo el reconocimiento de la declaración, sus métodos y sus instrucciones internas.
- **Causa:** `EsInicioInicializador` buscaba la palabra clave `new` en todo el texto acumulado sin descontar los modificadores de acceso/declaración (`PatronModificadores`, que incluye `new`) al inicio de miembros o tipos dentro de `ContextoBloque.Clase`.
- **Tipo PSP:** 80 - Function
- **Fase de inyección:** Código
- **Fase de eliminación:** Revisión de código
- **Corrección aplicada:** Se pasó `ContextoBloque` a `EsInicioInicializador` y, cuando `contexto == ContextoBloque.Clase`, se retiró el prefijo `PatronModificadores` antes de buscar `=` o `new`.
- **Estado:** Corregido (verificado estáticamente; pendiente de compilación y pruebas).
- **Tiempo de corrección:** Pendiente de registro manual en PSP Tracker.

---

### D-18
- **Identificador del defecto:** D-18
- **Archivo y línea:** [ContadorLOC.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/ContadorLOC.cs#L118-L135) y [ContadorLOC.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/ContadorLOC.cs#L187-L197) (`AnalizarInstrucciones`)
- **Descripción:** Si un inicializador de propiedad automática contiene una expresión lambda con cuerpo de bloque (`public Action Accion { get; set; } = () => { Foo(); };`), la primera instrucción interna de la lambda (`Foo();`) se descarta como `TipoUnidad.Otro` (`0 LOC`) y el cierre externo `= () => { };` se contabiliza erróneamente como `TipoUnidad.DeclaracionMiembro` (`+1 LOC` falsa).
- **Causa:** Al apilar `ContextoBloque.Lambda` en `lambdaContadores`, no se guardaba ni reiniciaba la bandera `enInitPropiedad`.
- **Tipo PSP:** 70 - Data
- **Fase de inyección:** Código
- **Fase de eliminación:** Revisión de código
- **Corrección aplicada:** Se incluyó `enInitPropiedad ? 1 : 0` en el arreglo de estado guardado en `lambdaContadores`, se reinició `enInitPropiedad = false` al abrir el contexto `Lambda` y se restauró su valor al cerrarlo.
- **Estado:** Corregido (verificado estáticamente; pendiente de compilación y pruebas).
- **Tiempo de corrección:** Pendiente de registro manual en PSP Tracker.

---

### D-19
- **Identificador del defecto:** D-19
- **Archivo y línea:** [ContadorLOC.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/ContadorLOC.cs#L96), [ContadorLOC.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/ContadorLOC.cs#L957-L981) y [ContadorLOC.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/ContadorLOC.cs#L1184) (`ValidarLiteralAdmitido`)
- **Descripción:** Las cadenas interpoladas textuales `$@"..."` y `@$"..."` —excluidas explícitamente en el estándar v1.1 (rev. Código)— no eran rechazadas como construcciones no admitidas (ni en el bucle principal ni dentro de `LeerHuecoInterpolacion`), por lo que el analizador interpretaba el carácter `"` siguiente como inicio de cadena convencional y fallaba o desalineaba el conteo ante saltos de línea o comillas `""`. Tampoco se validaban `"""` ni `$$` dentro de huecos de interpolación.
- **Causa:** Al retirar el soporte de `$@"..."` y `@$"..."` en la simplificación de Código, se omitió agregar la verificación explícita de construcción no admitida requerida por el estándar.
- **Tipo PSP:** 60 - Checking
- **Fase de inyección:** Código
- **Fase de eliminación:** Revisión de código
- **Corrección aplicada:** Se centralizó la validación en `ValidarLiteralAdmitido`, añadiendo el rechazo de `$@"` y `@$"` mediante `InvalidOperationException`, e invocándola también desde `LeerHuecoInterpolacion`.
- **Estado:** Corregido (verificado estáticamente; pendiente de compilación y pruebas).
- **Tiempo de corrección:** Pendiente de registro manual en PSP Tracker.

---

### D-20
- **Identificador del defecto:** D-20
- **Archivo y línea:** [ContadorLOC.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/ContadorLOC.cs#L1170-L1182) (`LeerHuecoInterpolacion`)
- **Descripción:** Regresión tras la simplificación de Código: dentro de un hueco de interpolación `{ ... }` de una cadena `$"..."`, los comentarios `//` y `/* ... */` no se omiten. Si un comentario interno contiene `}`, `"` o `'` (p. ej., `$"x: { /* } */ x }"`), cierra prematuramente el hueco o provoca un `FormatException` falso.
- **Causa:** Durante la simplificación de `OmitirHuecoInterpolacion` hacia `LeerHuecoInterpolacion` se suprimió la omisión de comentarios de línea y bloque.
- **Tipo PSP:** 80 - Function
- **Fase de inyección:** Código
- **Fase de eliminación:** Revisión de código
- **Corrección aplicada:** Se restauró la detección y omisión de comentarios `//` y `/* ... */` en `LeerHuecoInterpolacion` reutilizando `OmitirComentarioLinea` y `OmitirComentarioBloque`.
- **Estado:** Corregido (verificado estáticamente; pendiente de compilación y pruebas).
- **Tiempo de corrección:** Pendiente de registro manual en PSP Tracker.

---

### D-21
- **Identificador del defecto:** D-21
- **Archivo y línea:** [ResultadoParte.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/ResultadoParte.cs#L9-L16) y [UnidadLogica.cs](file:///C:/Users/johan/Programa-2-Contador-LOC/ContadorLOC/UnidadLogica.cs#L11-L18)
- **Descripción:** Los constructores públicos `ResultadoParte(string nombre)` y `UnidadLogica(string contenido, ...)` no validaban parámetros nulos (ni nombre vacío/en blanco en `ResultadoParte`), permitiendo construir instancias con propiedades no anulables en estado nulo o inválido si se invocan desde código externo o pruebas.
- **Causa:** Se confió únicamente en la anotación estática de nulabilidad sin añadir guardas en tiempo de ejecución coherentes con el resto de clases públicas.
- **Tipo PSP:** 60 - Checking
- **Fase de inyección:** Código
- **Fase de eliminación:** Revisión de código
- **Corrección aplicada:** Se agregó la validación de `string.IsNullOrWhiteSpace(nombre)` con `ArgumentException` en `ResultadoParte` y `ArgumentNullException.ThrowIfNull(contenido)` en `UnidadLogica`.
- **Estado:** Corregido (verificado estáticamente; pendiente de compilación y pruebas).
- **Tiempo de corrección:** Pendiente de registro manual en PSP Tracker.
