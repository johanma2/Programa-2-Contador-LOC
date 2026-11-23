# Estándar de conteo de líneas lógicas de código (LOC)

**Proyecto:** Programa 2 — Contador de LOC  
**Lenguaje analizado:** C#  
**Proceso:** PSP0.1  
**Versión:** 1.0  
**Fase de definición:** Planificación

## 1. Objetivo

Establecer reglas para contar líneas lógicas de código C# de forma consistente, independientemente de cuántas líneas físicas ocupe cada instrucción.

Este estándar se utilizará para desarrollar el contador, verificar los resultados de los Programas 1 y 2 y registrar el tamaño real en PSP Tracker.

## 2. Unidad de conteo

Una LOC lógica representa una instrucción, declaración o estructura de control contabilizable según las reglas siguientes.

Una línea física puede contener cero, una o varias LOC lógicas.

## 3. Reglas de conteo

| Elemento | Regla |
|---|---|
| Línea vacía o solo espacios | 0 LOC |
| Comentario `//` | 0 LOC |
| Comentario `/* ... */` | 0 LOC |
| Comentario XML `///` | 0 LOC |
| Directiva `using` | 1 LOC |
| Declaración de `namespace` | 1 LOC |
| Declaración de clase, interfaz, estructura o enumeración | 1 LOC |
| Declaración de método o constructor | 1 LOC |
| Declaración de propiedad o campo | 1 LOC |
| Instrucción terminada en `;` | 1 LOC |
| `if`, `for`, `foreach`, `while`, `switch` | 1 LOC por estructura |
| `case` y `default` | 1 LOC por etiqueta |
| `else`, `try`, `catch`, `finally`, `do` | 0 LOC adicionales |
| Llaves `{` y `}` aisladas | 0 LOC |
| Declaración o instrucción repartida en varias líneas | 1 LOC por elemento lógico |
| Varias instrucciones en una línea física | 1 LOC por instrucción |
| Punto y coma dentro de cadenas de texto | No genera LOC adicionales |
| Punto y coma separador en el encabezado de `for` | No genera LOC adicionales |
| Instrucciones dentro de expresiones lambda | Se cuentan según las reglas anteriores |

## 4. Reglas de interpretación

1. Los comentarios no generan LOC, aunque contengan palabras clave o puntos y coma.
2. Los símbolos dentro de cadenas de texto no se interpretan como instrucciones.
3. Los encabezados de estructuras de control se cuentan una vez, aunque ocupen varias líneas.
4. Las llaves delimitan bloques, pero no suman LOC.
5. Una declaración de método cuenta una LOC y las instrucciones de su cuerpo se contabilizan por separado.
6. Las propiedades de expresión cuentan una LOC por declaración; sus accesores con cuerpo se contabilizan según las instrucciones que contienen.
7. Los inicializadores de objetos y colecciones se contabilizan como parte de la declaración o instrucción que los contiene.
8. Las estructuras sintácticas no reconocidas no deben producir un conteo aparentemente exitoso sin advertencia.

## 5. Conteo por partes

Para este proyecto, una parte corresponde principalmente a una clase C#.

- Cada clase identificada tendrá un nombre, un número de métodos y un tamaño en LOC.
- Los constructores se incluirán en el número de elementos, identificándolos como constructores.
- Las LOC de los miembros se atribuirán a su clase.
- Las declaraciones e instrucciones externas a las clases se contabilizarán en el total general y se informarán separadamente.
- Las clases anidadas se contabilizarán como partes independientes, evitando duplicar sus LOC.
- Las clases parciales se consolidarán por nombre completamente calificado cuando sea posible identificarlas correctamente.

## 6. Alcance sintáctico

La primera versión analizará archivos `.cs` con estructuras convencionales de C#, como las utilizadas en los Programas 1 y 2.

No se exige interpretar todas las características del lenguaje C#. Las construcciones no soportadas deberán identificarse como limitaciones del análisis.

El contador no modificará los archivos examinados.

## 7. Ejemplos de referencia

### Ejemplo A — Declaración e instrucción

```csharp
int numero = 10;
numero++;
```

Resultado: **2 LOC**.

### Ejemplo B — Condicional

```csharp
if (numero > 0)
{
    Console.WriteLine(numero);
}
```

Resultado: **2 LOC**: una por `if` y una por `Console.WriteLine`.

### Ejemplo C — Instrucción multilínea

```csharp
double resultado = Math.Sqrt(
    suma / cantidad
);
```

Resultado: **1 LOC**.

### Ejemplo D — Comentarios

```csharp
// Comentario
/* Otro comentario */
```

Resultado: **0 LOC**.

### Ejemplo E — Método

```csharp
public int Sumar(int a, int b)
{
    return a + b;
}
```

Resultado: **2 LOC**: declaración del método y retorno.

### Ejemplo F — Bucle for

```csharp
for (int i = 0; i < 10; i++)
{
    Console.WriteLine(i);
}
```

Resultado: **2 LOC**: estructura `for` e instrucción de salida.

### Ejemplo G — Clase

```csharp
public class Calculadora
{
    public int Sumar(int a, int b)
    {
        return a + b;
    }
}
```

Resultado: **3 LOC**: clase, método y retorno. Número de métodos: **1**.

### Ejemplo H — Cadena con punto y coma

```csharp
string mensaje = "a;b;c";
```

Resultado: **1 LOC**.

## 8. Verificación

El contador deberá verificarse con:

- Archivos que contengan únicamente comentarios o líneas vacías.
- Instrucciones en una o varias líneas físicas.
- Estructuras de control.
- Clases con métodos y constructores.
- Cadenas con caracteres especiales.
- Código fuente del Programa 1.
- Código fuente del Programa 2.

Los resultados automáticos se compararán con conteos manuales realizados bajo este mismo estándar.

## 9. Restricciones

No se debe modificar el estándar para conseguir que el contador produzca un resultado esperado.

Si se detecta una ambigüedad, deberá documentarse, corregirse el estándar antes de continuar y actualizar los casos de prueba correspondientes.

## 10. Referencias

- *Guía de estudio PSP*, sección 7.2, Ejemplo de estándar de conteo lógico.
- Software Engineering Institute. (2012). *PSP Assignment Kit: Program 2*.