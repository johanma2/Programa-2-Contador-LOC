# Revisión del diseño — Programa 2: Contador de LOC

**Fase:** Diseño  
**Proceso:** PSP0.1  
**Lenguaje:** C# / .NET 10  
**Versión:** 1.1

## 1. Objetivo

Revisar el diseño del Programa 2 antes de iniciar la codificación, verificando su correspondencia con los requisitos funcionales y no funcionales, el estándar de conteo de LOC lógicas y el estándar de codificación.

La revisión busca identificar inconsistencias, omisiones y posibles errores que puedan afectar la implementación y los resultados del contador.

## 2. Documentos revisados

- Requisitos funcionales y no funcionales.
- Estándar de conteo de LOC.
- Estándar de codificación.
- Diseño conceptual.
- Diseño de clases.
- Pseudocódigo.
- Traza de escritorio.
- Arquitectura del sistema.
- Diagrama de clases.
- Diagrama de arquitectura.
- Diagrama de flujo.

## 3. Criterios de revisión

- Correspondencia entre los requisitos y el diseño.
- Separación de responsabilidades.
- Coherencia entre el pseudocódigo y la arquitectura.
- Aplicación del estándar de conteo.
- Identificación de partes, métodos y constructores.
- Prevención del doble conteo.
- Validación de rutas y manejo de errores.
- Consistencia de las trazas de escritorio.
- Identificación de limitaciones técnicas.

## 4. Trazabilidad de requisitos

| Requisito | Elemento del diseño asociado |
|---|---|
| RF-01: Selección de archivos | Program, ConsolaUI y LectorArchivos |
| RF-02: Lectura de archivos | LectorArchivos |
| RF-03: Conteo de LOC lógicas | ContadorLOC y UnidadLogica |
| RF-04: Identificación de partes | AnalizadorCodigo |
| RF-05: Conteo de métodos | AnalizadorCodigo |
| RF-06: LOC por parte | AnalizadorCodigo y ResultadoParte |
| RF-07: Total general | ResultadoPrograma |
| RF-08: Presentación de resultados | ConsolaUI |
| RF-09: Manejo de errores | Program, LectorArchivos y AnalizadorCodigo |

## 5. Hallazgos de la revisión

### H-01. Responsabilidades de lectura de archivos

**Hallazgo:** existía una inconsistencia entre las responsabilidades asignadas a Program y AnalizadorCodigo.

**Corrección:** se centralizó la coordinación de la búsqueda, lectura y análisis en AnalizadorCodigo. Program se limita a obtener la ruta, solicitar el análisis y presentar los resultados.

**Documentos afectados:** diseño de clases, pseudocódigo y diagramas.

### H-02. Duplicación del procesamiento

**Hallazgo:** realizar análisis independientes para contar LOC e identificar partes podía producir inconsistencias.

**Corrección:** se incorporaron UnidadLogica y TipoUnidad como representación común de los elementos reconocidos. Las unidades generadas se reutilizarán durante el conteo y la identificación de partes.

**Documentos afectados:** diseño de clases, pseudocódigo, arquitectura y diagramas.

### H-03. Identificación de métodos y constructores

**Hallazgo:** la presencia de paréntesis no permite diferenciar por sí sola las declaraciones de métodos, llamadas y estructuras de control.

**Corrección:** se definieron criterios basados en el contexto de la clase, el nombre, el tipo de retorno, los parámetros y el cuerpo de la declaración.

**Documentos afectados:** pseudocódigo y estándar de conteo.

### H-04. Comentarios y cadenas

**Hallazgo:** los comentarios y literales pueden contener caracteres que se confundan con delimitadores de código.

**Corrección:** se definieron estados léxicos diferenciados y se delimitaron los formatos de literales admitidos.

Se incorporó una traza adicional para verificar el tratamiento de cadenas convencionales, textuales, interpoladas y literales de carácter.

**Documentos afectados:** pseudocódigo, estándar de conteo y traza de escritorio.

### H-05. Clases anidadas y parciales

**Hallazgo:** la identificación incorrecta de las partes podía generar duplicaciones o resultados ambiguos.

**Corrección:** se estableció que cada LOC pertenecerá únicamente a su clase propietaria inmediata. Se definió el uso de nombres calificados y se excluyeron inicialmente las clases parciales.

Se agregó una traza de escritorio para verificar las clases anidadas.

**Documentos afectados:** estándar de conteo, pseudocódigo y traza de escritorio.

### H-06. Archivos generados automáticamente

**Hallazgo:** los directorios bin y obj pueden contener archivos fuente generados durante la compilación.

**Corrección:** se estableció su exclusión durante la búsqueda recursiva en cualquier nivel del directorio.

**Documentos afectados:** pseudocódigo, arquitectura y estándar de conteo.

### H-07. Comparación con el Programa 1

**Hallazgo:** el conteo anterior del Programa 1 podría haberse realizado utilizando criterios diferentes.

**Corrección:** se estableció la realización de conteos manuales independientes de los Programas 1 y 2 utilizando el estándar vigente.

**Documentos afectados:** estándar de conteo y criterios de verificación.

### H-08. Reconocimiento de enumeraciones

**Hallazgo:** la incorporación de TipoUnidad introdujo una enumeración que deberá ser reconocida al analizar el propio código fuente del Programa 2.

**Corrección:** se amplió el estándar para contabilizar las declaraciones de enumeraciones y sus miembros. Se incorporó un caso de traza de escritorio.

**Documentos afectados:** estándar de conteo, diseño de clases, pseudocódigo y traza de escritorio.

## 6. Verificación de trazas de escritorio

| Caso | LOC esperadas | Métodos y constructores |
|---|---:|---:|
| Clase con un método | 4 | 1 |
| Instrucciones multilínea | 7 | 1 |
| Comentarios, cadenas y ciclo for | 5 | 1 |
| Múltiples clases y constructores | 11 | 3 |
| Ruta inexistente | No aplica | No aplica |
| Clases anidadas | 7 | 2 |
| Propiedades y métodos de expresión | 3 | 1 |
| Cadenas y literales | 6 | 1 |
| Enumeraciones | 3 | 0 |

Los resultados corresponden a conteos manuales previstos. Su correspondencia con la implementación se verificará durante Pruebas.

## 7. Control de correcciones

| Hallazgo | Corrección definida |
|---|---|
| H-01 | Centralización de responsabilidades |
| H-02 | Representación común de unidades |
| H-03 | Criterios de identificación de métodos |
| H-04 | Estados léxicos y alcance de literales |
| H-05 | Asignación exclusiva de LOC |
| H-06 | Exclusión recursiva de archivos generados |
| H-07 | Conteo manual independiente |
| H-08 | Reconocimiento de enumeraciones |

## 8. Lista de chequeo

- [ ] Los requisitos funcionales están representados en el diseño.
- [ ] Los requisitos no funcionales se han considerado.
- [ ] Las responsabilidades de las clases están definidas.
- [ ] Las dependencias entre componentes son coherentes.
- [ ] El pseudocódigo coincide con el diseño de clases.
- [ ] El algoritmo contempla las reglas del estándar de conteo.
- [ ] Los formatos de cadenas admitidos están definidos.
- [ ] La identificación de métodos y constructores está definida.
- [ ] El tratamiento de clases anidadas y parciales está definido.
- [ ] El tratamiento de enumeraciones está definido.
- [ ] Se establecieron mecanismos para evitar el doble conteo.
- [ ] Se excluyen los archivos generados automáticamente.
- [ ] Las trazas presentan resultados manuales coherentes.
- [ ] El manejo de errores está documentado.
- [ ] Los diagramas corresponden a la arquitectura.
- [ ] Las limitaciones del analizador están documentadas.
- [ ] Las correcciones fueron incorporadas en los documentos correspondientes.

## 9. Impacto sobre la estimación

La revisión incorporó una clase adicional, una enumeración y mecanismos de control del análisis.

Estos cambios pueden incrementar el tamaño y el esfuerzo de implementación respecto a la estimación inicial.

Se conservarán las 190 LOC y los 498 minutos como referencia de planificación. Las diferencias se analizarán durante el Postmortem.

## 10. Resultado de la revisión

Se definieron correcciones para los hallazgos identificados durante la revisión del diseño.

La arquitectura propuesta establece responsabilidades independientes, una representación común de las unidades lógicas y mecanismos de control de consistencia.

La aprobación documental queda sujeta a la incorporación y verificación de las correcciones.

La precisión del análisis sintáctico y la exactitud del conteo deberán comprobarse durante la implementación y las pruebas.

## 11. Control de cambios

| Versión | Descripción |
|---|---|
| 1.0 | Identificación inicial de hallazgos. |
| 1.1 | Consolidación de correcciones, ampliación de trazas y reconocimiento de enumeraciones. |