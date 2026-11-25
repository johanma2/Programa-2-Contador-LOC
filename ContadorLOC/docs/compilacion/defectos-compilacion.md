# Defectos — Fase de Compilación

**Fase PSP:** Compilación  
**Programa:** 2 — Contador de Líneas de Código Lógicas (C#)  
**Fecha:** 2026-11-25  
**Continuación de:** `docs/revision-codigo/defectos-revision-codigo.md` (D-01 a D-21)

---

## Resultado de la Fase

**No se detectaron defectos durante la fase de Compilación.**

Ambas compilaciones (`dotnet build` y `dotnet build --no-incremental`) produjeron directamente:

```
Compilación correcta.
    0 Advertencia(s)
    0 Errores
```

El rango de identificadores **D-22 en adelante no fue utilizado** en esta fase.

---

## Resumen de Defectos Acumulados

| Rango   | Fase              | Defectos | Estado        |
|---------|-------------------|----------|---------------|
| D-01 a D-14 | Código       | 14       | Corregidos ✓  |
| D-15 a D-21 | Revisión de Código | 7   | Corregidos ✓  |
| D-22+   | Compilación       | 0        | No aplicable  |
| **Total** |                 | **21**   | **21 corregidos** |

---

## Observaciones

1. **CS8019 (unnecessary using):** Se anticipaba que los `using` explícitos (`using System;`, `using System.Collections.Generic;`, `using System.Text;`) pudieran generar advertencias con `ImplicitUsings=enable`. El compilador (SDK 11.0.100) no los reportó como innecesarios. No se requirió intervención.

2. **Nullable (CS8600/CS8601/CS8602):** Con `<Nullable>enable</Nullable>` activo, no se produjeron advertencias de nulabilidad. La gestión de referencias nulas en el código es consistente con el modo de análisis nullable.

3. **TFM net10.0 con SDK 11.0.100:** El SDK 11.0.100 compiló correctamente el TFM `net10.0` sin advertencias de compatibilidad ni de preview.

---

*Documento generado en la fase de Compilación del Programa 2 — PSP0.1*  
*Si en fases posteriores (Pruebas) se detectan defectos con origen en Código o Diseño, se registrarán en el documento de defectos correspondiente a esa fase, continuando desde D-22.*
