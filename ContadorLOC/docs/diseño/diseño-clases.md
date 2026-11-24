# Diseño de clases — Programa 2: Contador de LOC

**Fase:** Diseño  
**Proceso:** PSP0.1  
**Lenguaje:** C# / .NET 10  
**Versión:** 1.1

## 1. Objetivo

Definir las clases, atributos, métodos y relaciones necesarios para implementar un contador de LOC lógicas en archivos fuente C#.

## 2. Clases del sistema

### 2.1. Program

**Responsabilidad:** coordinar la ejecución del programa.

| Método | Retorno | Responsabilidad |
|---|---|---|
| Main(string[] args) | int | Obtener la ruta, solicitar el análisis y presentar los resultados o errores. |

**Dependencias:** ConsolaUI y AnalizadorCodigo.

Program no realizará directamente la búsqueda, lectura ni análisis de archivos.

### 2.2. LectorArchivos

**Responsabilidad:** localizar y leer archivos fuente C#.

| Método | Retorno |
|---|---|
| ObtenerArchivos(string ruta) | List<string> |
| LeerContenido(string rutaArchivo) | string |

**Validaciones:**

- Ruta vacía o inexistente.
- Extensión diferente de .cs.
- Directorio sin archivos fuente.
- Errores de lectura y permisos.

La búsqueda será recursiva y excluirá los directorios bin y obj en cualquier nivel.

### 2.3. ContadorLOC

**Responsabilidad:** reconocer y clasificar las unidades de código según el estándar de conteo.

| Método | Retorno |
|---|---|
| AnalizarInstrucciones(string contenido) | List<UnidadLogica> |
| ContarLOC(IEnumerable<UnidadLogica> unidades) | int |

**Responsabilidades específicas:**

- Diferenciar código, comentarios y literales.
- Reconocer instrucciones y declaraciones contabilizables.
- Identificar delimitadores y estructuras de control.
- Generar una representación común de las unidades analizadas.
- Contar exclusivamente las unidades contabilizables.

El contenido se analizará una sola vez por archivo. Las unidades generadas se utilizarán posteriormente para determinar las LOC y sus partes propietarias.

### 2.4. AnalizadorCodigo

**Responsabilidad:** coordinar la lectura, el conteo y la identificación de partes.

**Atributos:**

| Atributo | Tipo |
|---|---|
| _lectorArchivos | LectorArchivos |
| _contadorLOC | ContadorLOC |

**Métodos:**

| Método | Retorno |
|---|---|
| Analizar(string ruta) | ResultadoPrograma |
| IdentificarPartes(IEnumerable<UnidadLogica> unidades) | ResultadoPrograma |

**Responsabilidades específicas:**

- Obtener los archivos mediante LectorArchivos.
- Solicitar el análisis de cada archivo a ContadorLOC.
- Identificar clases, métodos y constructores.
- Mantener el contexto de los bloques.
- Asignar cada LOC a su parte propietaria.
- Consolidar los resultados.
- Verificar que no existan duplicaciones en la contabilización.

### 2.5. ResultadoParte

**Responsabilidad:** representar el resultado de una clase o parte.

| Propiedad | Tipo |
|---|---|
| Nombre | string |
| CantidadMetodos | int |
| TotalLOC | int |

**Métodos:**

- Constructor.
- AgregarLOC(): incrementa el contador de LOC.
- AgregarMetodo(): incrementa el contador de métodos y constructores.

No realiza directamente el análisis del código fuente.

### 2.6. ResultadoPrograma

**Responsabilidad:** almacenar y consolidar los resultados generales.

| Propiedad | Tipo |
|---|---|
| Partes | List<ResultadoParte> |
| LOCExternas | int |
| TotalLOC | int |
| CantidadArchivos | int |

**Métodos:**

- Constructor.
- AgregarParte(ResultadoParte parte).
- AgregarLOCExternas(int cantidad).
- IncorporarResultado(ResultadoPrograma resultado).
- CalcularTotal(): int.

El total general será la suma de las LOC de todas las partes y las LOC externas.

### 2.7. ConsolaUI

**Responsabilidad:** gestionar la interacción con el usuario.

| Método | Retorno |
|---|---|
| PedirRuta() | string |
| MostrarResultado(ResultadoPrograma resultado) | void |
| MostrarError(string mensaje) | void |

La presentación de resultados será independiente del procesamiento del código fuente.

### 2.8. UnidadLogica

**Responsabilidad:** representar un elemento reconocido durante el análisis.

| Propiedad | Tipo |
|---|---|
| Contenido | string |
| Tipo | TipoUnidad |
| NumeroLinea | int |
| EsContabilizable | bool |

**Métodos:**

- Constructor para inicializar las propiedades.

Las unidades podrán representar declaraciones, instrucciones, estructuras de control y delimitadores necesarios para mantener el contexto del análisis.

## 3. Enumeración TipoUnidad

La enumeración permitirá clasificar los elementos reconocidos.

| Valor | Descripción |
|---|---|
| DeclaracionClase | Declaración de una clase |
| DeclaracionMetodo | Declaración de un método |
| DeclaracionConstructor | Declaración de un constructor |
| DeclaracionMiembro | Declaración de campo o propiedad |
| EstructuraControl | Estructura de control |
| Instruccion | Instrucción lógica |
| AperturaBloque | Inicio de un bloque |
| CierreBloque | Final de un bloque |
| Otro | Otro elemento reconocido |

La propiedad EsContabilizable determinará si la unidad aporta una LOC conforme al estándar.

## 4. Relaciones

- Program utiliza ConsolaUI y AnalizadorCodigo.
- AnalizadorCodigo utiliza LectorArchivos y ContadorLOC.
- ContadorLOC genera objetos UnidadLogica.
- UnidadLogica utiliza TipoUnidad.
- AnalizadorCodigo procesa las unidades y construye los resultados.
- ResultadoPrograma contiene una colección de ResultadoParte.
- ConsolaUI utiliza ResultadoPrograma para presentar la información.

## 5. Decisiones de diseño

1. Separar la entrada, lectura, procesamiento y presentación.
2. Analizar léxicamente cada archivo una sola vez.
3. Reutilizar las unidades reconocidas para calcular las LOC e identificar partes.
4. Utilizar una pila para mantener el contexto de los bloques.
5. Asignar las LOC de clases anidadas únicamente a su clase propietaria inmediata.
6. Contabilizar los constructores dentro de la cantidad de métodos.
7. Excluir archivos generados automáticamente.
8. No modificar los archivos fuente analizados.
9. Informar las construcciones no admitidas cuando se detecten.
10. Evitar presentar resultados parciales como resultados completos.

## 6. Restricciones

El analizador se limitará a las construcciones definidas en la versión vigente del estándar de conteo.

No se pretende implementar un compilador completo de C#.

## 7. Control de cambios

| Versión | Descripción |
|---|---|
| 1.0 | Diseño inicial de siete clases. |
| 1.1 | Centralización del análisis, incorporación de UnidadLogica y TipoUnidad y definición de responsabilidades corregidas. |