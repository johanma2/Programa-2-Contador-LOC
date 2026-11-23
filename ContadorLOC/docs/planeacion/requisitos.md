# Requisitos del software — Programa 2: Contador de líneas de código (LOC)

**Lenguaje:** C#  
**Entorno:** Visual Studio Code y .NET 10  
**Proceso:** PSP0.1 en PSP Tracker  
**Fase:** Planificación  
**Estado:** Borrador para revisión

## 1. Objetivo

Desarrollar una aplicación de consola en C# que permita medir el tamaño de programas escritos en C#, utilizando un estándar de conteo de líneas de código (LOC) previamente definido.

El programa deberá presentar el tamaño total del código analizado, el tamaño de cada parte y la cantidad de métodos o elementos contenidos en cada parte.

## 2. Alcance

El contador analizará archivos fuente `.cs` y producirá un informe organizado por clases o partes del programa.

Se utilizarán como conjuntos de verificación el código fuente del Programa 1 (Media y desviación estándar) y el propio Programa 2 (Contador de LOC).

La herramienta no pretende ser un compilador ni un analizador completo de toda la sintaxis de C#.

## 3. Requisitos funcionales

### RF-01. Seleccionar el código fuente

El programa permitirá indicar la ruta de un archivo `.cs` o de una carpeta que contenga archivos fuente C#.

**Criterios de aceptación:**
- Acepta una ruta proporcionada por el usuario.
- Comprueba que la ruta existe.
- Si se indica una carpeta, identifica los archivos `.cs` que contiene.
- Informa cuando no existen archivos fuente válidos.

### RF-02. Leer los archivos fuente

El programa leerá el contenido de los archivos seleccionados sin modificarlos.

**Criterios de aceptación:**
- Procesa los archivos fuente como texto.
- Conserva el orden de lectura.
- Reconoce las líneas vacías y los comentarios según el estándar definido.
- No modifica los archivos originales.

### RF-03. Contar las líneas de código

El programa calculará las LOC de acuerdo con un estándar de conteo documentado y aplicado consistentemente.

**Criterios de aceptación:**
- Distingue el código contabilizable de los comentarios y líneas vacías.
- Aplica las reglas definidas para declaraciones, instrucciones y estructuras de control.
- Produce resultados reproducibles sobre los mismos archivos.
- No contabiliza automáticamente una línea física como una LOC lógica.

### RF-04. Identificar las partes del programa

El programa identificará las clases o partes del código fuente que se encuentren dentro del alcance sintáctico definido.

**Criterios de aceptación:**
- Obtiene el nombre de cada clase identificada.
- Asocia el código contabilizado con su clase correspondiente.
- Permite diferenciar las partes del programa analizado.

### RF-05. Contar los métodos de cada parte

El programa calculará la cantidad de métodos identificados en cada clase o parte.

**Criterios de aceptación:**
- Identifica los métodos definidos en las clases analizadas.
- Cuenta cada método una sola vez.
- Presenta la cantidad de métodos correspondiente a cada parte.
- El criterio de inclusión de constructores y otros miembros debe quedar definido antes de implementar.

### RF-06. Calcular el tamaño por parte

El programa calculará las LOC de cada clase o parte identificada.

**Criterios de aceptación:**
- Cada parte tiene un nombre y un tamaño.
- El tamaño se obtiene utilizando el mismo estándar de conteo general.
- Las LOC no se duplican entre partes.

### RF-07. Calcular el tamaño total

El programa calculará el tamaño total del código fuente analizado.

**Criterios de aceptación:**
- Obtiene un total general de LOC.
- Evita contabilizar dos veces el mismo código.
- El total corresponde a las LOC contabilizables de los archivos procesados, incluidas las que no pertenezcan a una clase, si las hubiera.

### RF-08. Mostrar los resultados

El programa mostrará un informe con los resultados del análisis.

**Criterios de aceptación:**
- Muestra el nombre de cada parte.
- Muestra la cantidad de métodos o elementos de cada parte.
- Muestra las LOC correspondientes a cada parte.
- Muestra el tamaño total del programa.
- Presenta los resultados de manera comprensible en consola.

### RF-09. Informar errores de entrada y lectura

El programa informará los errores que impidan realizar el análisis.

**Criterios de aceptación:**
- Informa cuando la ruta no existe.
- Informa cuando no encuentra archivos `.cs`.
- Controla los errores de lectura y permisos.
- No presenta resultados incompletos como si fueran un análisis exitoso.

## 4. Requisitos no funcionales

### RNF-01. Lenguaje y plataforma

La aplicación se desarrollará en C# con .NET 10 y se ejecutará desde consola.

### RNF-02. Estándar de codificación

Se utilizarán nombres descriptivos, convenciones de C#, indentación consistente y separación de responsabilidades entre clases y métodos.

### RNF-03. Estándar de conteo

Antes de implementar el contador se documentará un estándar de LOC que especifique qué instrucciones, declaraciones y estructuras cuentan y cuáles se excluyen.

### RNF-04. Exactitud y consistencia

El programa deberá producir resultados consistentes con el estándar documentado. Los casos de prueba incluirán conteos manuales independientes para comprobar su exactitud.

### RNF-05. Manejo de errores

Los errores de entrada y lectura deberán comunicarse mediante mensajes comprensibles, sin provocar cierres inesperados.

### RNF-06. Mantenibilidad

La lectura de archivos, la clasificación del código, el cálculo de resultados y la presentación deberán estar organizados en responsabilidades diferenciadas.

### RNF-07. Verificación de compilación

El proyecto deberá compilar correctamente sin errores y sin introducir advertencias nuevas atribuibles a su implementación.

### RNF-08. Compatibilidad con PSP

Se registrarán los tiempos reales por fase, defectos encontrados, tamaño estimado y real, y propuestas de mejora mediante PSP Tracker.

## 5. Pruebas obligatorias

Se verificará el contador, como mínimo, con:

- **Programa 1:** Media y desviación estándar.
- **Programa 2:** Código fuente del propio contador de LOC.

También se prepararán casos controlados con resultados conocidos para validar el estándar de conteo y el manejo de errores.

## 6. Restricciones y decisiones pendientes

Antes de comenzar el diseño detallado se deben definir:

1. El estándar exacto de conteo de LOC lógicas.
2. El tratamiento de constructores, propiedades y otros miembros de C#.
3. Las estructuras sintácticas admitidas para identificar clases y métodos.
4. La forma de contabilizar código que se encuentre fuera de las clases.
5. Si las carpetas se analizarán únicamente en su nivel inmediato o también en subcarpetas.

Estas decisiones deberán documentarse para evitar que el alcance aumente durante la codificación.

## 7. Fuentes

Software Engineering Institute, Carnegie Mellon University. (2012). *PSP Assignment Kit: Program 2*.

Manual del Aprendiz de PSP Tracker, versión 1.4.

Documentación del Programa 1: Media y desviación estándar.