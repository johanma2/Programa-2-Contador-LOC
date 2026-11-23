# Estándar de codificación — Programa 2: Contador de LOC

**Lenguaje:** C#  
**Plataforma:** .NET 10  
**Entorno:** Visual Studio Code  
**Proceso:** PSP0.1  
**Versión:** 1.0

## 1. Objetivo

Definir las convenciones de programación que se aplicarán durante el desarrollo del Programa 2, para mantener un código legible, consistente, verificable y fácil de mantener.

## 2. Convenciones de nombres

| Elemento | Convención | Ejemplo |
|---|---|---|
| Clases | PascalCase | `ContadorLOC` |
| Métodos | PascalCase | `ContarLineas()` |
| Propiedades | PascalCase | `TotalLOC` |
| Variables locales | camelCase | `totalLineas` |
| Parámetros | camelCase | `rutaArchivo` |
| Campos privados | _camelCase | `_lectorArchivos` |
| Constantes | PascalCase | `ExtensionPermitida` |
| Interfaces | Prefijo I + PascalCase | `IContadorLOC` |

Los nombres deben describir claramente la responsabilidad de cada elemento.

## 3. Formato del código

- Utilizar cuatro espacios por nivel de indentación.
- Colocar las llaves de bloques en líneas independientes.
- Mantener una instrucción por línea cuando sea posible.
- Separar los métodos mediante líneas en blanco.
- Evitar instrucciones innecesariamente extensas.
- Mantener una organización consistente de los archivos.

## 4. Organización del proyecto

- Utilizar una clase por archivo, salvo excepciones justificadas.
- Mantener nombres de archivo coherentes con sus clases.
- Separar lectura, análisis, cálculo y presentación de resultados.
- Utilizar `Program.cs` para coordinar la ejecución, evitando concentrar toda la lógica en él.
- No modificar los archivos fuente que se analizan.

## 5. Tipos de datos

- Utilizar `int` para contadores de LOC y métodos, verificando posibles desbordamientos.
- Utilizar `string` para rutas, nombres y contenido textual.
- Utilizar `bool` para estados y condiciones.
- Utilizar colecciones tipadas cuando sea necesario almacenar resultados.
- Aplicar las características de nulabilidad de C# para reducir errores de referencias nulas.

## 6. Métodos y responsabilidades

- Cada método debe tener una responsabilidad principal.
- Evitar duplicar la lógica de conteo.
- Separar la identificación de clases y métodos del cálculo de LOC cuando resulte conveniente.
- Utilizar parámetros y valores de retorno claros.
- Evitar dependencias innecesarias entre componentes.

## 7. Validaciones y manejo de errores

- Comprobar que las rutas de entrada existan.
- Verificar que los archivos tengan extensiones admitidas.
- Controlar errores de acceso, lectura y permisos.
- Mostrar mensajes comprensibles al usuario.
- No ocultar excepciones inesperadas sin dejar información útil para diagnosticarlas.
- No presentar resultados parciales como resultados completos.

## 8. Comentarios y documentación

- Escribir comentarios cuando expliquen decisiones o reglas que no sean evidentes.
- Evitar comentarios que repitan literalmente lo que hace el código.
- Documentar las limitaciones del analizador.
- Mantener actualizado el estándar de conteo y sus ejemplos.
- Utilizar comentarios XML en métodos públicos cuando aporten información relevante.

## 9. Estándar de conteo

El proyecto utilizará el documento `estandar-conteo-loc.md` como referencia para determinar las LOC lógicas.

El estándar de codificación no sustituye al estándar de conteo: uno establece cómo escribir el código y el otro cómo medir su tamaño.

## 10. Revisión de código

Antes de compilar, se aplicará una lista de chequeo para verificar:

- Nombres y convenciones.
- Sintaxis y estructuras de control.
- Manejo de valores nulos.
- Validación de rutas.
- Manejo de excepciones.
- Aplicación consistente del estándar de LOC.
- Posibles duplicaciones y errores de construcción.

Esta revisión incorpora la PIP definida al finalizar el Programa 1.

## 11. Compilación y pruebas

- Compilar utilizando `dotnet build`.
- Ejecutar mediante `dotnet run`.
- Registrar los defectos reales encontrados durante revisión, compilación y pruebas.
- Verificar el contador con casos de resultado conocido.
- Comparar los resultados automáticos con conteos manuales independientes.

## 12. Control de versiones

- Utilizar Git para registrar cambios relevantes.
- Crear commits descriptivos relacionados con las fases y entregables.
- No incluir carpetas `bin/`, `obj/` ni archivos temporales.
- Mantener la documentación del proyecto junto al código fuente.