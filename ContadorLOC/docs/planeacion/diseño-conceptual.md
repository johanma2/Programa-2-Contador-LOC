# Diseño conceptual — Programa 2: Contador de LOC

**Fase:** Planificación  
**Proceso:** PSP0.1  
**Lenguaje:** C# / .NET 10

## Objetivo

Definir los componentes principales del contador de líneas lógicas de código antes de realizar el diseño detallado.

## Componentes propuestos

1. **Program:** coordina el flujo principal y el manejo de errores.
2. **LectorArchivos:** identifica y lee los archivos fuente.
3. **ContadorLOC:** contabiliza las LOC conforme al estándar documentado.
4. **AnalizadorCodigo:** identifica clases, métodos y su relación.
5. **ResultadoParte:** representa el resultado de una clase o parte.
6. **ResultadoPrograma:** consolida los resultados del programa.
7. **ConsolaUI:** presenta los resultados y mensajes.

## Flujo conceptual

1. Obtener la ruta de entrada.
2. Validar la ruta y localizar los archivos `.cs`.
3. Leer el contenido sin modificar los archivos.
4. Identificar comentarios, cadenas e instrucciones.
5. Aplicar el estándar de LOC lógicas.
6. Identificar clases y métodos.
7. Asociar LOC y métodos con las partes correspondientes.
8. Calcular el total general.
9. Mostrar los resultados.
10. Informar los errores cuando corresponda.

## Restricciones

El contador analizará las construcciones de C# definidas en el estándar de conteo y en el alcance del proyecto.

No se pretende construir un compilador completo de C#.

Los archivos originales no serán modificados.

## Riesgos

- Clasificación incorrecta de comentarios y cadenas.
- Conteo duplicado de instrucciones.
- Identificación incorrecta de clases o métodos.
- Diferencias entre el conteo manual y automático.
- Incremento del tamaño del programa respecto a las 190 LOC estimadas.

## Verificación prevista

Se utilizarán casos pequeños con conteos manuales conocidos y los códigos fuente completos de los Programas 1 y 2.

## Estimación inicial

El tamaño previsto es de 190 LOC lógicas.

La estimación de tiempo registrada en PSP Tracker es de 8,3 horas (498 minutos).

Ambos valores deberán conservarse como referencia inicial y compararse con los resultados reales durante el Postmortem.