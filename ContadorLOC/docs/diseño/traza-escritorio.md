# Traza de escritorio — Programa 2: Contador de LOC

**Fase:** Diseño  
**Proceso:** PSP0.1  
**Lenguaje:** C# / .NET 10

## 1. Objetivo

Verificar manualmente el comportamiento del algoritmo propuesto para contar líneas lógicas de código, identificar clases y métodos y consolidar los resultados conforme al estándar de conteo.

## 2. Caso 1: clase con un método

### Código de entrada

```csharp
using System;

public class Calculadora
{
    public int Sumar(int a, int b)
    {
        // Retornar la suma
        return a + b;
    }
}
```

### Traza

| Línea | Contenido | Clasificación | LOC añadidas | Acumulado |
|---:|---|---|---:|---:|
| 1 | using System; | Directiva using | 1 | 1 |
| 2 | Vacía | Línea vacía | 0 | 1 |
| 3 | public class Calculadora | Declaración de clase | 1 | 2 |
| 4 | Llave de apertura | Delimitador | 0 | 2 |
| 5 | public int Sumar(int a, int b) | Declaración de método | 1 | 3 |
| 6 | Llave de apertura | Delimitador | 0 | 3 |
| 7 | Comentario | Comentario | 0 | 3 |
| 8 | return a + b; | Instrucción | 1 | 4 |
| 9 | Llave de cierre | Delimitador | 0 | 4 |
| 10 | Llave de cierre | Delimitador | 0 | 4 |

### Resultado esperado

| Parte | Métodos | LOC |
|---|---:|---:|
| Calculadora | 1 | 3 |
| LOC externas | — | 1 |
| **Total** | **1** | **4** |

La directiva using pertenece al total general, pero no a la clase Calculadora.

## 3. Caso 2: instrucciones multilínea y múltiples instrucciones físicas

### Código de entrada

```csharp
public class Operaciones
{
    public void Ejecutar()
    {
        int a = 5; int b = 10;

        int resultado =
            a + b;

        if (resultado > 0)
        {
            System.Console.WriteLine(resultado);
        }
    }
}
```

### Traza

| Elemento | Clasificación | LOC añadidas | Acumulado |
|---|---|---:|---:|
| Declaración Operaciones | Clase | 1 | 1 |
| Declaración Ejecutar | Método | 1 | 2 |
| int a = 5; | Instrucción | 1 | 3 |
| int b = 10; | Instrucción | 1 | 4 |
| int resultado = a + b; | Instrucción multilínea | 1 | 5 |
| if (resultado > 0) | Estructura de control | 1 | 6 |
| System.Console.WriteLine(resultado); | Instrucción | 1 | 7 |
| Llaves y líneas vacías | Excluidas | 0 | 7 |

### Resultado esperado

| Parte | Métodos | LOC |
|---|---:|---:|
| Operaciones | 1 | 7 |
| LOC externas | — | 0 |
| **Total** | **1** | **7** |

Se verifica que una instrucción distribuida en varias líneas físicas cuenta como una LOC y que dos instrucciones en una misma línea física cuentan por separado.

## 4. Caso 3: comentarios, cadenas y ciclo for

### Código de entrada

```csharp
public class Ejemplo
{
    public void Mostrar()
    {
        // int x = 10;

        /* int y = 20;
           int z = 30; */

        string mensaje = "Hola; mundo;";

        for (int i = 0; i < 3; i++)
        {
            System.Console.WriteLine(mensaje);
        }
    }
}
```

### Traza

| Elemento | Clasificación | LOC añadidas | Acumulado |
|---|---|---:|---:|
| Declaración Ejemplo | Clase | 1 | 1 |
| Declaración Mostrar | Método | 1 | 2 |
| Comentario de línea | Excluido | 0 | 2 |
| Comentario de bloque | Excluido | 0 | 2 |
| Declaración mensaje | Instrucción | 1 | 3 |
| Puntos y coma dentro de la cadena | Excluidos | 0 | 3 |
| Encabezado for | Estructura de control | 1 | 4 |
| Separadores del encabezado for | Excluidos | 0 | 4 |
| System.Console.WriteLine(mensaje); | Instrucción | 1 | 5 |
| Llaves y líneas vacías | Excluidas | 0 | 5 |

### Resultado esperado

| Parte | Métodos | LOC |
|---|---:|---:|
| Ejemplo | 1 | 5 |
| LOC externas | — | 0 |
| **Total** | **1** | **5** |

Se verifica que los comentarios, los puntos y coma internos de las cadenas y los separadores del encabezado for no generan LOC adicionales.

## 5. Caso 4: múltiples clases y constructores

### Código de entrada

```csharp
using System;

public class Persona
{
    private string _nombre;

    public Persona(string nombre)
    {
        _nombre = nombre;
    }

    public void Mostrar()
    {
        Console.WriteLine(_nombre);
    }
}

public class Principal
{
    public static void Main()
    {
        Persona persona = new Persona("Ana");
        persona.Mostrar();
    }
}
```

### Traza

| Elemento | Parte | LOC añadidas | Acumulado |
|---|---|---:|---:|
| using System; | Externa | 1 | 1 |
| Declaración Persona | Persona | 1 | 2 |
| Declaración _nombre | Persona | 1 | 3 |
| Constructor Persona | Persona | 1 | 4 |
| Asignación _nombre | Persona | 1 | 5 |
| Declaración Mostrar | Persona | 1 | 6 |
| Console.WriteLine | Persona | 1 | 7 |
| Declaración Principal | Principal | 1 | 8 |
| Declaración Main | Principal | 1 | 9 |
| Creación de persona | Principal | 1 | 10 |
| persona.Mostrar(); | Principal | 1 | 11 |

### Resultado esperado

| Parte | Métodos y constructores | LOC |
|---|---:|---:|
| Persona | 2 | 6 |
| Principal | 1 | 4 |
| LOC externas | — | 1 |
| **Total** | **3** | **11** |

El constructor se contabiliza como un elemento ejecutable de la clase, conforme al criterio de conteo establecido.

## 6. Caso 5: ruta inexistente

### Entrada

Ruta de un archivo que no existe.

### Traza

| Paso | Operación | Resultado |
|---:|---|---|
| 1 | Obtener ruta | Ruta recibida |
| 2 | Verificar existencia | Falso |
| 3 | Validar directorio | Falso |
| 4 | Generar error | La ruta no existe |
| 5 | Mostrar mensaje | Error informado |
| 6 | Terminar ejecución | Código de salida 1 |

### Resultado esperado

El programa informa el error y termina sin presentar resultados de conteo.

## Caso 6: clases anidadas

### Código de entrada

```csharp
public class Exterior
{
    private int _valor;

    public class Interior
    {
        public int Obtener()
        {
            return 10;
        }
    }

    public void Mostrar()
    {
        System.Console.WriteLine(_valor);
    }
}
```

### Traza

| Elemento | Parte propietaria | LOC |
|---|---|---:|
| Declaración Exterior | Exterior | 1 |
| Declaración _valor | Exterior | 1 |
| Declaración Interior | Exterior.Interior | 1 |
| Declaración Obtener | Exterior.Interior | 1 |
| return 10; | Exterior.Interior | 1 |
| Declaración Mostrar | Exterior | 1 |
| Console.WriteLine | Exterior | 1 |

### Resultado esperado

| Parte | Métodos | LOC |
|---|---:|---:|
| Exterior | 1 | 4 |
| Exterior.Interior | 1 | 3 |
| LOC externas | — | 0 |
| Total | 2 | 7 |

Se verifica que las LOC de la clase anidada no se contabilicen nuevamente en la clase contenedora.

## Caso 7: propiedades y métodos con cuerpo de expresión

### Código de entrada

```csharp
public class Producto
{
    public int Precio { get; set; }

    public int ObtenerPrecio() => Precio;
}
```

### Traza

| Elemento | Clasificación | LOC |
|---|---|---:|
| Declaración Producto | Clase | 1 |
| Declaración Precio | Propiedad automática | 1 |
| Declaración ObtenerPrecio | Método con cuerpo de expresión | 1 |

### Resultado esperado

| Parte | Métodos | LOC |
|---|---:|---:|
| Producto | 1 | 3 |
| Total | 1 | 3 |

Se verifica que los accesores automáticos y el punto y coma final del método no generen LOC duplicadas.

## Caso 8: cadenas y literales

### Código de entrada

```csharp
public class Mensajes
{
    public void Mostrar()
    {
        string normal = "Hola; mundo";
        string ruta = @"C:\Datos\archivo.cs";
        char separador = ';';
        string texto = $"Resultado: {10}";
    }
}
```

### Traza

| Elemento | Clasificación | LOC |
|---|---|---:|
| Declaración Mensajes | Clase | 1 |
| Declaración Mostrar | Método | 1 |
| Declaración normal | Instrucción | 1 |
| Declaración ruta | Instrucción | 1 |
| Declaración separador | Instrucción | 1 |
| Declaración texto | Instrucción | 1 |

### Resultado esperado

| Parte | Métodos | LOC |
|---|---:|---:|
| Mensajes | 1 | 6 |
| Total | 1 | 6 |

Se verifica que los delimitadores internos de los literales no generen instrucciones adicionales.

## Caso 9: enumeración

### Código de entrada

```csharp
public enum Estado
{
    Activo,
    Inactivo
}
```

### Traza

| Elemento | Clasificación | LOC |
|---|---|---:|
| Declaración Estado | Enumeración | 1 |
| Activo | Miembro de enumeración | 1 |
| Inactivo | Miembro de enumeración | 1 |

### Resultado esperado

| Parte | Métodos | LOC |
|---|---:|---:|
| Estado | 0 | 3 |
| Total | 0 | 3 |

## Consolidación de casos

| Caso | LOC esperadas | Métodos y constructores |
|---|---:|---:|
| 1. Clase sencilla | 4 | 1 |
| 2. Instrucciones multilínea | 7 | 1 |
| 3. Comentarios, cadenas y for | 5 | 1 |
| 4. Múltiples clases | 11 | 3 |
| 5. Ruta inexistente | No aplica | No aplica |
| 6. Clases anidadas | 7 | 2 |
| 7. Propiedades y métodos de expresión | 3 | 1 |
| 8. Cadenas y literales | 6 | 1 |
| 9. Enumeración | 3 | 0 |

## Conclusión

Las trazas establecen los resultados manuales esperados para las principales construcciones contempladas en el estándar de conteo.

Los casos adicionales permiten comprobar las decisiones adoptadas durante la revisión del diseño.

Los resultados automáticos deberán compararse con estos valores durante la fase de Pruebas. La revisión conceptual no sustituye la ejecución del programa.

## 7. Consolidación de casos

| Caso | LOC esperadas | Métodos y constructores | Resultado esperado |
|---|---:|---:|---|
| Clase sencilla | 4 | 1 | Conteo correcto |
| Instrucciones multilínea | 7 | 1 | Conteo correcto |
| Comentarios, cadenas y for | 5 | 1 | Conteo correcto |
| Múltiples clases | 11 | 3 | Conteo correcto |
| Ruta inexistente | No aplica | No aplica | Error controlado |

## 8. Conclusión

Las trazas establecen los resultados esperados para las principales reglas del estándar de conteo.

Durante la fase de Pruebas se compararán estos resultados manuales con los obtenidos mediante la ejecución del programa.

La verificación realizada en esta fase es conceptual y no demuestra todavía el funcionamiento de la implementación.