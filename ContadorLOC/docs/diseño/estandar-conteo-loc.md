# Estándar de conteo de LOC — Programa 2

**Proceso:** PSP0.1  
**Lenguaje:** C#  
**Versión:** 1.1

## 1. Objetivo

Establecer reglas uniformes para medir el tamaño lógico de programas escritos en C#, permitiendo comparar los conteos manuales con los resultados del contador automático.

## 2. Unidad de medida

Se utilizarán líneas lógicas de código (LOC).

Una LOC representa una instrucción, declaración o estructura contabilizable según las reglas de este estándar, independientemente de la cantidad de líneas físicas que ocupe.

## 3. Reglas generales

| Elemento | LOC |
|---|---:|
| Línea vacía | 0 |
| Comentario de línea | 0 |
| Comentario de bloque | 0 |
| Directiva using | 1 |
| Declaración de namespace | 1 |
| Declaración de clase | 1 |
| Declaración de método | 1 |
| Declaración de constructor | 1 |
| Declaración de campo | 1 |
| Declaración de propiedad | 1 |
| Instrucción terminada en punto y coma | 1 |
| Encabezado if | 1 |
| Encabezado else if | 1 |
| Encabezado for | 1 |
| Encabezado foreach | 1 |
| Encabezado while | 1 |
| Encabezado switch | 1 |
| Etiqueta case | 1 |
| Etiqueta default | 1 |
| else independiente | 0 |
| try | 0 |
| catch | 0 |
| finally | 0 |
| do | 0 |
| Llave aislada de apertura o cierre | 0 |

Cada elemento se clasificará una sola vez.

## 4. Instrucciones y declaraciones

Una instrucción distribuida en varias líneas físicas se contabilizará como una LOC.

Varias instrucciones independientes ubicadas en una misma línea física se contabilizarán individualmente.

Una declaración de campo con múltiples variables en la misma instrucción se contabilizará como una LOC.

Los puntos y coma internos del encabezado for no se contabilizarán individualmente.

En un ciclo do-while se contabilizará el encabezado while final como una LOC, sin contar adicionalmente su punto y coma.

## 5. Comentarios y literales

Los comentarios no aportarán LOC.

Los puntos y coma, llaves y otros delimitadores ubicados dentro de literales admitidos no se interpretarán como instrucciones externas.

Una declaración que contenga un literal se contabilizará normalmente.

## 6. Métodos y constructores

Cada declaración de método o constructor admitida aporta una LOC.

Los constructores se incluirán en el indicador de cantidad de métodos y constructores de la clase.

Las llamadas a métodos no incrementarán la cantidad de métodos declarados.

Las declaraciones con cuerpo de expresión se contabilizarán una sola vez, sin contar adicionalmente el punto y coma que las finaliza.

## 7. Propiedades

Una propiedad automática se contabilizará como una LOC.

Los accesores automáticos get, set e init no aportarán LOC adicionales.

En propiedades con cuerpos de bloque, las instrucciones internas se contabilizarán según las reglas generales.

Los encabezados de los accesores no aportarán LOC adicionales.

Las propiedades con cuerpo de expresión se contabilizarán como una sola declaración.

## 8. Inicializadores y expresiones

Los inicializadores de objetos y colecciones forman parte de la instrucción o declaración que los contiene.

Sus llaves no se interpretarán como bloques de clases o métodos.

Las expresiones lambda admitidas se contabilizarán como parte de la instrucción que las contiene. Si poseen un cuerpo de bloque admitido, las instrucciones internas se contabilizarán conforme al estándar.

## 9. Conteo por partes

Las clases serán las partes principales del programa.

Cada parte tendrá:

- Nombre calificado.
- Cantidad de métodos y constructores.
- Cantidad de LOC lógicas.

La declaración de una clase forma parte de sus propias LOC.

Las LOC externas a las clases se incluirán en el total general.

## 10. Clases anidadas

Cada clase anidada se contabilizará como una parte independiente.

Su declaración y las instrucciones de su cuerpo pertenecerán únicamente a dicha clase.

No se duplicarán estas LOC en la clase contenedora.

## 11. Alcance sintáctico

### Construcciones previstas

- Comentarios de línea y de bloque.
- Cadenas convencionales con escapes.
- Cadenas textuales con prefijo @.
- Cadenas interpoladas convencionales con expresiones admitidas.
- Literales de carácter.
- Directivas using.
- Espacios de nombres con bloque o ámbito de archivo.
- Clases convencionales y anidadas.
- Campos y propiedades.
- Métodos y constructores.
- Cuerpos de expresión admitidos.
- Estructuras de control convencionales.
- Inicializadores convencionales.
- Instrucciones multilínea.

### Construcciones excluidas inicialmente

- Cadenas delimitadas mediante comillas triples.
- Interpolaciones complejas o combinaciones de literales no admitidas (como `$@"..."`, `@$"..."` o `$$"..."`).
- Directivas de compilación condicional (`#if`, `#elif`, `#else`, `#endif`).
- Clases parciales.
- Constructores primarios.
- Funciones locales.
- Tuplas de retorno, expresiones `switch` y expresiones `with`.
- Atributos sobre declaraciones y otras construcciones avanzadas que no sean necesarias para los Programas 1 y 2.

Las interfaces, estructuras y enumeraciones podrán incorporarse mediante una ampliación documentada del alcance cuando sean necesarias.

**Excepción necesaria para el Programa 2:** se deberá reconocer la declaración de enum y contabilizarla como una parte, debido a la incorporación de TipoUnidad en el diseño. Cada miembro declarado de la enumeración contará como una LOC.

Las construcciones no admitidas que puedan afectar el conteo deberán informarse cuando sean detectadas.

El analizador no garantiza la identificación de todas las construcciones posibles de C#.

## 12. Archivos incluidos

Se analizarán archivos con extensión .cs.

La búsqueda en directorios será recursiva.

Se excluirán los directorios bin y obj en cualquier nivel para evitar contabilizar archivos generados automáticamente.

## 13. Ejemplos

| Código | LOC |
|---|---:|
| int numero = 10; | 1 |
| int a = 5; int b = 10; | 2 |
| // Comentario | 0 |
| string mensaje = "Hola; mundo;"; | 1 |
| public int Valor { get; set; } | 1 |
| public int Obtener() => 10; | 1 |
| public enum Estado { Activo, Inactivo } | 3 |

Ejemplo de método:

```csharp
public int Sumar(int a, int b)
{
    return a + b;
}
```

Resultado: 2 LOC.

Ejemplo de ciclo:

```csharp
for (int i = 0; i < 5; i++)
{
    Console.WriteLine(i);
}
```

Resultado: 2 LOC.

## 14. Verificación

Se realizarán conteos manuales independientes de los Programas 1 y 2 utilizando este estándar.

Los resultados automáticos se compararán con los valores manuales.

Las LOC registradas previamente en el Programa 1 no se utilizarán directamente como resultado esperado sin comprobar la equivalencia de los criterios de conteo.

## 15. Control de cambios

| Versión | Fase | Descripción |
|---|---|---|
| 1.0 | Planificación | Definición inicial del estándar. |
| 1.1 | Diseño | Delimitación sintáctica, precisión de propiedades, clases anidadas, enumeraciones y exclusión de archivos generados. |
| 1.1 (rev. Código) | Código | Precisión en exclusiones de alcance académico (tuplas de retorno, expresiones `switch` sin asignación, `with`, atributos, `delegate`/`event` y combinaciones `$@"`/`@$"`) manteniendo lambdas con cuerpo de bloque `=> { ... }` y exclusión directa de `bin`/`obj`. |