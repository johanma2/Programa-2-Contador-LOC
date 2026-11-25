# Pseudocódigo — Programa 2: Contador de LOC

**Fase:** Diseño  
**Proceso:** PSP0.1  
**Versión:** 1.1

## 1. Objetivo

Definir el algoritmo para identificar y contar LOC lógicas en archivos C#, asociarlas con sus partes correspondientes y presentar los resultados.

## 2. Algoritmo principal

```text
INICIO PROGRAMA

    SI existen argumentos
        ruta ← primer argumento
    SI NO
        ruta ← ConsolaUI.PedirRuta()
    FIN SI

    INTENTAR

        resultado ← AnalizadorCodigo.Analizar(ruta)

        ConsolaUI.MostrarResultado(resultado)

        RETORNAR 0

    CAPTURAR error conocido

        ConsolaUI.MostrarError(mensaje del error)

        RETORNAR 1

    CAPTURAR error inesperado

        ConsolaUI.MostrarError(
            "No se pudo completar el análisis"
        )

        RETORNAR 1

    FIN INTENTAR

FIN PROGRAMA
```

## 3. Obtención de archivos

```text
FUNCIÓN ObtenerArchivos(ruta)

    SI ruta está vacía
        GENERAR error "No se indicó una ruta"
    FIN SI

    SI ruta pertenece a directorio bin u obj
        GENERAR error "No se analizan directorios bin ni obj"
    FIN SI

    SI ruta corresponde a un archivo

        SI extensión no es .cs
            GENERAR error "Archivo no admitido"
        FIN SI

        RETORNAR colección con el archivo

    FIN SI

    SI ruta corresponde a un directorio

        archivos ← BuscarArchivos(ruta)

        SI archivos está vacía
            GENERAR error "No se encontraron archivos C#"
        FIN SI

        RETORNAR archivos

    FIN SI

    GENERAR error "La ruta no existe"

FIN FUNCIÓN
```

### 3.1. Búsqueda recursiva

```text
FUNCIÓN BuscarArchivos(directorio)

    archivos ← colección vacía

    PARA CADA archivo del directorio

        SI extensión es .cs
            agregar archivo a archivos
        FIN SI

    FIN PARA

    PARA CADA subdirectorio

        SI nombre no es bin
           Y nombre no es obj

            incorporar BuscarArchivos(subdirectorio)

        FIN SI

    FIN PARA

    RETORNAR archivos

FIN FUNCIÓN
```

La comparación de extensiones y nombres de directorios será independiente de mayúsculas y minúsculas.

## 4. Coordinación del análisis

```text
FUNCIÓN Analizar(ruta)

    archivos ← LectorArchivos.ObtenerArchivos(ruta)

    resultado ← nuevo ResultadoPrograma

    PARA CADA archivo EN archivos

        contenido ← LectorArchivos.LeerContenido(archivo)

        unidades ← ContadorLOC.AnalizarInstrucciones(contenido)

        totalArchivo ← ContadorLOC.ContarLOC(unidades)

        resultadoArchivo ← IdentificarPartes(unidades)

        SI totalArchivo es diferente de
           resultadoArchivo.CalcularTotal()

            GENERAR error "Inconsistencia de conteo"

        FIN SI

        resultadoArchivo.CantidadArchivos ← 1

        resultado.IncorporarResultado(resultadoArchivo)

    FIN PARA

    resultado.CalcularTotal()

    RETORNAR resultado

FIN FUNCIÓN
```

El análisis léxico se realiza una sola vez por archivo.

## 5. Análisis léxico y segmentación en una sola pasada

El algoritmo procesa el contenido fuente en una sola pasada carácter a carácter, conservando el texto original de los literales y los números de línea, sin crear una capa intermedia de tokens.

### 5.1. Contextos de bloque

| Contexto | Responsabilidad |
|---|---|
| Archivo | Nivel superior fuera de espacios de nombres y partes |
| Namespace | Cuerpo de un espacio de nombres con bloque |
| Clase | Cuerpo directo de una clase (identifica métodos y constructores) |
| Enumeracion | Cuerpo de una enumeración (separa miembros por coma o cierre) |
| Propiedad | Cuerpo de una propiedad (omite accesores automáticos y encabezados) |
| BloqueCodigo | Cuerpo de métodos, constructores, accesores y estructuras de control |
| Lambda | Cuerpo de bloque de expresiones lambda (`=> { ... }`) |

### 5.2. Algoritmo

```text
FUNCIÓN AnalizarInstrucciones(contenido)

    contextos ← pila con [Archivo]
    clases ← pila vacía
    acumulador ← vacío
    linea ← 1

    PARA cada caracter EN contenido

        SI caracter es salto de línea
            linea ← linea + 1
        FIN SI

        SI inicia comentario // o /* ... */
            omitir comentario conservando separación y saltos de línea
            SI comentario de bloque sin cerrar → GENERAR error

        SI NO, SI inicia directiva condicional (#if, #elif, #else, #endif) o comillas triples
            GENERAR error de construcción no admitida

        SI NO, SI inicia literal ($"...", @"...", "...", '...')
            leer literal completo conservando su contenido original en acumulador
            (en $"..." procesar huecos { ... } admitiendo literales internos)
            SI literal sin cerrar → GENERAR error

        SI NO, SI caracter es '{'
            SI acumulador termina en "=>"
                guardar estado externo y abrir bloque Lambda
            SI NO, SI corresponde a inicializador (=, new fuera de where, o dentro de ()/[])
                incrementar profundidad de inicializador y acumular
            SI NO
                emitir unidad acumulada y AperturaBloque
                apilar nuevo contexto según tipo de unidad emitida
            FIN SI

        SI NO, SI caracter es '}'
            SI existe inicializador activo
                decrementar profundidad y acumular
            SI NO
                emitir remanente (ej. último miembro de enumeración)
                desapilar contexto y emitir CierreBloque
                SI cerró Lambda → restaurar acumulador externo
                SI cerró Propiedad → verificar posible inicializador '= ...;'
            FIN SI

        SI NO
            acumular caracter y actualizar profundidad de () y []
            SI profundidad es cero
                emitir por ',' en Enumeracion, por ';', por ')' de control,
                por ':' de case/default o por palabra de control sin paréntesis
            FIN SI
        FIN SI

    FIN PARA

    SI quedan delimitadores o bloques sin cerrar
        GENERAR error de análisis
    FIN SI

    RETORNAR unidades

FIN FUNCIÓN
```

Al eliminar comentarios se conservará la separación entre los elementos del código para evitar unir identificadores diferentes.

Los literales se conservarán como elementos de la instrucción, pero sus delimitadores internos no se interpretarán como instrucciones externas.

## 6. Identificación de unidades lógicas

```text
FUNCIÓN ProcesarElemento(elemento, contexto)

    SI elemento corresponde a una declaración admitida

        generar unidad contabilizable

    SI NO, SI corresponde a una estructura de control

        generar unidad según estándar de conteo

    SI NO, SI termina una instrucción mediante ;

        SI el separador pertenece a un encabezado for
            continuar acumulando la instrucción
        SI NO
            generar unidad contabilizable
        FIN SI

    SI NO, SI corresponde a una apertura de bloque

        generar unidad no contabilizable
        actualizar contexto

    SI NO, SI corresponde a un cierre de bloque

        generar unidad no contabilizable
        actualizar contexto

    SI NO

        continuar acumulando contenido

    FIN SI

FIN FUNCIÓN
```

Las llaves de inicializadores, propiedades automáticas y expresiones admitidas deberán distinguirse de los bloques de clases, métodos y estructuras de control.

Una declaración con cuerpo de expresión se contará una sola vez.

## 7. Conteo de LOC

```text
FUNCIÓN ContarLOC(unidades)

    total ← 0

    PARA CADA unidad EN unidades

        SI unidad.EsContabilizable
            total ← total + 1
        FIN SI

    FIN PARA

    RETORNAR total

FIN FUNCIÓN
```

## 8. Identificación de métodos y constructores

```text
FUNCIÓN EsDeclaracionMetodo(unidad, contexto)

    SI contexto no corresponde directamente
       al cuerpo de una clase

        RETORNAR falso

    FIN SI

    SI unidad corresponde a campo, propiedad,
       llamada o estructura de control

        RETORNAR falso

    FIN SI

    SI unidad no presenta una firma admitida

        RETORNAR falso

    FIN SI

    SI nombre coincide con el nombre de la clase
       Y no presenta tipo de retorno
       Y presenta cuerpo de constructor admitido

        RETORNAR verdadero

    FIN SI

    SI presenta tipo de retorno
       Y nombre
       Y lista de parámetros
       Y cuerpo de método admitido

        RETORNAR verdadero

    FIN SI

    RETORNAR falso

FIN FUNCIÓN
```

Los métodos y constructores se contabilizarán una sola vez por declaración.

## 9. Identificación de partes y asignación de LOC

```text
FUNCIÓN IdentificarPartes(unidades)

    resultado ← nuevo ResultadoPrograma
    pilaBloques ← pila vacía
    pilaClases ← pila vacía

    PARA CADA unidad EN unidades

        SI unidad declara espacio de nombres

            SI unidad.EsContabilizable
                incrementar LOC externas
            FIN SI

            SI termina con ';'
                registrar namespace de ámbito de archivo
            SI NO
                registrar namespace pendiente de apertura
            FIN SI

        SI NO, SI unidad declara una clase o enumeración

            parte ← nueva ResultadoParte

            parte.Nombre ← nombre calificado (namespaces + clase contenedora + nombre)

            agregar parte al resultado

            registrar parte pendiente de apertura (indicando si es enumeración)

            SI unidad.EsContabilizable
                parte.AgregarLOC()
            FIN SI

        SI NO, SI unidad abre un bloque

            SI corresponde a parte pendiente
                incorporar parte a pilaClases y registrar Parte o Enumeracion en pilaBloques
            SI NO, SI corresponde a namespace pendiente
                incorporar namespace a pilaNamespaces y registrar Namespace en pilaBloques
            SI NO
                registrar Otro en pilaBloques
            FIN SI

        SI NO, SI unidad cierra un bloque

            bloque ← retirar elemento de pilaBloques

            SI bloque es Parte o Enumeracion
                retirar clase de pilaClases
            SI NO, SI bloque es Namespace
                retirar namespace de pilaNamespaces
            FIN SI

        SI NO

            SI unidad declara método o constructor Y bloque actual es Parte (clase directa)

                incrementar contador de métodos de la clase activa

            FIN SI

            SI unidad.EsContabilizable

                SI existe una clase activa

                    asignar LOC a la clase más interna

                SI NO

                    incrementar LOC externas

                FIN SI

            FIN SI

        FIN SI

    FIN PARA

    SI existen bloques sin cerrar
        GENERAR error de análisis
    FIN SI

    resultado.CalcularTotal()

    RETORNAR resultado

FIN FUNCIÓN
```

La declaración de una clase anidada se contabiliza exclusivamente en la nueva clase.

Los nombres calificados permitirán diferenciar clases pertenecientes a distintos espacios de nombres o clases contenedoras.

## 10. Consolidación de resultados

```text
FUNCIÓN IncorporarResultado(resultadoArchivo)

    PARA CADA parte EN resultadoArchivo.Partes

        agregar parte al resultado general

    FIN PARA

    LOCExternas ← LOCExternas
                  + resultadoArchivo.LOCExternas

    CantidadArchivos ← CantidadArchivos
                       + resultadoArchivo.CantidadArchivos

FIN FUNCIÓN
```

Las clases parciales quedan fuera del alcance inicial. No se combinarán clases diferentes únicamente porque tengan el mismo nombre simple.

## 11. Cálculo del total general

```text
FUNCIÓN CalcularTotal()

    total ← LOCExternas

    PARA CADA parte EN Partes

        total ← total + parte.TotalLOC

    FIN PARA

    TotalLOC ← total

    RETORNAR total

FIN FUNCIÓN
```

**Invariante:**

TotalLOC = suma de LOC de las partes + LOC externas.

## 12. Presentación de resultados

```text
FUNCIÓN MostrarResultado(resultado)

    MOSTRAR "CONTADOR DE LOC"

    MOSTRAR cantidad de archivos analizados

    PARA CADA parte EN resultado.Partes

        MOSTRAR nombre de la parte
        MOSTRAR cantidad de métodos y constructores
        MOSTRAR LOC de la parte

    FIN PARA

    MOSTRAR LOC externas

    MOSTRAR total de métodos y constructores

    MOSTRAR total general de LOC

FIN FUNCIÓN
```

## 13. Manejo de errores

- Ruta vacía o inexistente.
- Extensión no admitida.
- Directorio sin archivos C#.
- Errores de lectura o permisos.
- Literales o comentarios de bloque sin cerrar.
- Bloques incompletos.
- Construcciones no admitidas detectadas.
- Inconsistencias entre el conteo total y el conteo por partes.

## 14. Limitaciones

El algoritmo describe un analizador de alcance limitado y no una gramática completa de C#.

La identificación de construcciones complejas deberá comprobarse durante la implementación y las pruebas. El alcance admitido se establece en el estándar de conteo vigente.

## 15. Control de cambios

| Versión | Descripción |
|---|---|
| 1.0 | Algoritmo inicial de conteo. |
| 1.1 | Centralización del procesamiento, reutilización de unidades lógicas, estados léxicos, criterios de identificación de métodos y control de clases anidadas. |