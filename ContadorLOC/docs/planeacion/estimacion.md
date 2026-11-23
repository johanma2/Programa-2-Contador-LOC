# Estimación inicial — Programa 2: Contador de LOC

**Proceso:** PSP0.1 en PSP Tracker  
**Lenguaje:** C# / .NET 10  
**Fase:** Planificación  
**Estado:** Estimación inicial

## 1. Método de estimación

La estimación se realiza mediante descomposición del programa en componentes funcionales y utilización de los datos históricos disponibles del Programa 1.

Se toma como referencia el procedimiento de estimación de tamaño y tiempo del kit PSP, pero no se realiza una regresión estadística debido a que solamente se dispone de un programa anterior.

La estimación se basa en el juicio técnico, la complejidad prevista de cada componente y la productividad histórica aproximada.

## 2. Datos históricos

| Indicador | Programa 1 |
|---|---:|
| LOC reales | 128 |
| Tiempo real | 336 minutos |
| Productividad aproximada | 22,86 LOC/hora |

La productividad se calcula utilizando el tiempo total registrado del Programa 1.

## 3. Estimación de tamaño

| Componente | LOC estimadas |
|---|---:|
| Lectura de archivos y validaciones | 30 |
| Clasificación de líneas | 45 |
| Identificación de clases y métodos | 45 |
| Acumulación de resultados | 25 |
| Presentación de resultados | 25 |
| Coordinación y manejo de errores | 20 |
| **Total estimado** | **190 LOC** |

Se considera un programa nuevo, sin código base previo ni reutilización contabilizada.

La estimación podrá diferir del tamaño real. Esa diferencia se analizará en el Postmortem sin alterar retrospectivamente la estimación inicial.

## 4. Estimación de tiempo

Productividad histórica aproximada:

128 LOC / 5,6 horas = 22,86 LOC/hora.

Tiempo previsto para 190 LOC:

190 / 22,86 ≈ 8,31 horas.

El valor registrado en PSP Tracker es de 8,3 horas, equivalentes a 498 minutos.

## 5. Distribución por fases

| Fase | Tiempo estimado |
|---|---:|
| Planificación | 65 min |
| Diseño | 80 min |
| Código | 190 min |
| Revisión de código | 55 min |
| Compilación | 25 min |
| Pruebas | 60 min |
| Postmortem | 23 min |
| **Total** | **498 min** |

## 6. Riesgos de estimación

**R-01. Complejidad del análisis sintáctico:** reconocer clases, métodos, comentarios y cadenas puede requerir más código del previsto.

**R-02. Diferencias de conteo:** el contador podría producir resultados distintos de los conteos manuales, lo que exigiría revisar su implementación.

**R-03. Alcance del lenguaje:** algunas construcciones de C# podrían no estar contempladas por la primera versión.

**R-04. Datos históricos limitados:** una única observación anterior no permite establecer una productividad estadísticamente estable.

## 7. Estrategias de control

- Mantener el alcance definido en los requisitos.
- Aplicar el estándar de conteo documentado.
- Preparar pruebas con resultados conocidos.
- Revisar el código antes de compilar, aplicando la PIP del Programa 1.
- Registrar los defectos cuando se detecten.
- Conservar las estimaciones iniciales para compararlas con los datos reales.

## 8. Control PSP

Los tiempos reales se registrarán mediante el cronómetro de PSP Tracker.

Los defectos se documentarán indicando su tipo, fase de inyección, fase de remoción y tiempo de corrección.

Durante el Postmortem se registrarán las LOC reales, se analizarán las diferencias entre lo estimado y lo ejecutado y se formularán las propuestas de mejora correspondientes.