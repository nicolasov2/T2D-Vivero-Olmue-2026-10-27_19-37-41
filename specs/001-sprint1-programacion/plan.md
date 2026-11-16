# Plan 001 · Sprint 1 de programación (T2D en Unity)

Acompaña a `spec.md`. Estado: **aprobado el 10 nov 2026** (contratos de §3 actualizados durante la implementación).

## 1. Decisiones técnicas

| # | Decisión | Por qué | Alternativa descartada |
|---|---|---|---|
| D1 | **Unity 6.3 LTS** (`6000.3.27f1`), plantilla *Universal 2D*. Todos con la misma versión. | Es la LTS y la versión del proyecto base que ya está en el repositorio del equipo (UT 1762). | Unity 6.6: se probó el 15 nov 2026 y funciona, pero obligaría a todo el equipo a cambiar de versión. |
| D2 | El código se agrega en `Assets/T2D` del proyecto base del repositorio, creado en Unity Hub. **No se escribe a mano** `Packages/manifest.json` ni `ProjectSettings`. | Evita versiones de paquetes que no calcen con el editor instalado. | Entregar un proyecto completo: falla si la versión del editor difiere. |
| D3 | Cuatro assemblies: `T2D.Dominio` (C# puro), `T2D.Juego` (MonoBehaviours y ScriptableObjects), `T2D.Editor` (herramientas) y `T2D.Tests` (EditMode). | Lo pide "T2D Requisitos" y los diseños: la lógica se prueba sin abrir Unity. Además compila más rápido. | Todo en `Assembly-CSharp`: no se puede garantizar CA-4. |
| D4 | El dominio usa su propio `Posicion` (struct) en vez de `Vector2Int`. | `noEngineReferences` prohíbe tipos de Unity en el dominio. | Referenciar UnityEngine en el dominio. |
| D5 | **Una sola fuente de verdad por nivel:** `NivelDefinicion` (ScriptableObject) guarda tamaño, rutas, pasillos, casillas bloqueadas y silo. El Tilemap se **pinta** desde ahí. | Evita que la ruta pintada y la ruta lógica difieran. Cumple las PA "cambiar en la configuración sin modificar código" de 1680 y 1686. | Leer el Tilemap como fuente (diseño de 1698): hay que mantener dos cosas sincronizadas. |
| D6 | Los fallos esperados se devuelven con `Resultado<T, TError>`; las excepciones quedan para errores de programación. | Regla del equipo: nada de `null` ni vacíos como señal de error. | `bool` + `out` en todo el dominio. |
| D7 | Interfaz del Sprint 1 con **IMGUI (`OnGUI`)**: HUD, panel de construcción, rótulos y botones. | Cero assets ni Canvas que armar; se cambia por uGUI en el Sprint 2 sin tocar el dominio. | uGUI armado por código: más trabajo sin valor para las PA del Sprint 1. |
| D8 | Lectura del puntero en una sola clase, compatible con Input System y con el Input Manager antiguo (`#if ENABLE_INPUT_SYSTEM`). | Unity 6 puede venir con uno u otro activo según la plantilla. | Asumir uno: arriesga errores al dar Play. |
| D9 | Menú **T2D → Crear escena de prueba** que genera sprites, tiles, definiciones, prefabs y la escena. | Las escenas y prefabs escritos a mano se rompen con facilidad; generados por código, siempre calzan. | Escribir archivos `.unity` y `.prefab` a mano. |
| D10 | Tests EditMode (NUnit) para el dominio y una tabla de pruebas manuales en Play Mode para lo visual. Sin tests PlayMode. | El movimiento por la ruta es C# puro (`RecorridoDeRuta`), así que se prueba en EditMode. | PlayMode tests: lentos y frágiles. |
| D11 | **Identificadores en español, iguales a los diseños** subidos a Worki (`Cuadricula`, `Ruta`, `SeguidorDeRuta`…). Comentarios y commits en inglés. | El PO y el equipo comparan el código con el diseño; nombres distintos confunden. | Identificadores en inglés: obligaría a una tabla de equivalencias. |
| D12 | Sin capas ni patrones extra: no hay inyección de dependencias, eventos globales ni `CamaraSBC` como clase propia hasta que tenga comportamiento propio (1752). | YAGNI. | — |

**C# 9:** Unity 6 compila con C# 9, así que no se usan `record`, `record struct`, namespaces de archivo ni `global using`. Cada archivo del dominio activa `#nullable enable`.

## 2. Modelo de datos (dominio)

| Área | Clases | UT |
|---|---|---|
| Base | `Posicion`, `LimitesDelMapa`, `Resultado<T,TError>`, `TipoCasilla`, `Casilla`, `Cuadricula`, `Costo`, `Creditos`, `Silo`, `DatosDeNivel`, `ConstructorDeNivel`, `Nivel` | Base (1698, 1748 y 1750 mínimas) |
| Rutas | `Ruta`, `RecorridoDeRuta` | 1680 |
| Plantines | `Especie`, `DatosDePasillo`, `PasilloDePlantines`, `RegistroDePasillos` | 1686 |
| Construcción | `ResultadoColocacion`, `ValidadorDeColocacion` | 1678 |
| | `DefinicionEstacion`, `Estacion`, `ServicioDeConstruccion` | 1687 |
| | `EstadoSeleccion` (+4 subtipos: se agregó `EstacionElegida`), `SelectorDeCasilla` | 1787 |
| | `CalculadoraDeReembolso`, `ServicioDeConstruccion.Desmantelar` | 1766 |
| Eventos | `DefinicionEvento`, `EventoEstres`, `IEfectoDeEvento`, `TemporizadorDeAparicion`, `EfectoOlaDeCalor` | 1688 |
| | `Integridad`, `DanioPorViento`, `EfectoViento` | 1689 |
| | `Hidratacion`, `DanioPorDeshidratacion`, `EfectoDeshidratacion` | 1690 |

Cada tipo de evento es una clase `IEfectoDeEvento` distinta. Así, cada persona trabaja en su archivo sin tocar el de otra UT.

## 3. Contratos principales (API pública del dominio)

```csharp
Resultado<Nivel, string>                  ConstructorDeNivel.Construir(DatosDeNivel datos)
Resultado<Ruta, string>                   Ruta.Crear(IReadOnlyList<Posicion> puntos, LimitesDelMapa limites)
IReadOnlyList<Posicion>                   RecorridoDeRuta.Avanzar(float segundos)        // casillas nuevas
PasilloDePlantines?                       RegistroDePasillos.PasilloEn(Posicion)         // null: casilla sin plantines
ResultadoColocacion                       ValidadorDeColocacion.Validar(Costo, Posicion) // instancia con Cuadricula y Creditos
Resultado<Estacion, ResultadoColocacion>  ServicioDeConstruccion.IntentarColocar(DefinicionEstacion, Posicion)
Estacion?                                 ServicioDeConstruccion.EstacionEn(Posicion)    // null: casilla sin estación
Resultado<Costo, string>                  ServicioDeConstruccion.Desmantelar(Estacion)
Resultado<Estacion, ResultadoColocacion>  SelectorDeCasilla.Confirmar(ServicioDeConstruccion)
int                                       Silo.RecibirDanio(int agua)                   // agua descontada; nunca baja de 0
```

`ResultadoColocacion`: `Valida`, `FueraDelMapa`, `EsRuta`, `Ocupada`, `NoEdificable`, `CreditosInsuficientes`, `SinEstacionElegida`.

## 4. Archivos a crear

```
Assets/T2D/
  Dominio/   T2D.Dominio.asmdef  Base/  Rutas/  Plantines/  Construccion/  Eventos/
  Juego/     T2D.Juego.asmdef    Mapa/  Rutas/  Plantines/  Construccion/  Eventos/  Interfaz/  Partida.cs
  Editor/    T2D.Editor.asmdef   ConstructorDeEscena.cs
  Tests/     T2D.Tests.asmdef    (un archivo de tests por UT)
.gitignore   README.md   specs/001-sprint1-programacion/{spec,plan,tasks}.md
docs/adr/0001-estructura-proyecto-unity.md
```

Además, fuera del proyecto: documentos de la 1690 (especificación, mockup y diseño), una guía de Worki para *Programar* y *Aplicar PA*, y el mensaje para el grupo.

## 5. Reparto propuesto de *Programar* y *Aplicar PA*

Nadie pasa de 8 HIP y nadie prueba lo que programó.

| UT | HIP | Programa | Prueba | Depende de |
|---|---|---|---|---|
| Base (mapa) + 1680 | 4 | Nicolás | Pablo | — |
| 1686 | 4 | Nicolás | Monserrat | Base |
| 1678 | 4 | Benjamín | Pablo | Base |
| 1687 (+ créditos) | 4 | Pablo | Benjamín | 1678 |
| 1688 (+ silo) | 4 | Pablo | Nicolás | 1680 |
| 1766 | 3 | Benjamín | Nicolás | 1687 |
| 1787 | 2 | Monserrat | Ryan | 1678, 1687 |
| 1689 | 5 | Monserrat | Benjamín | 1688, 1687 |
| 1690 | 5 | Ryan | Monserrat | 1688, 1686 |

**Programar por persona:** Nicolás 8, Pablo 8, Benjamín 7, Monserrat 7 y Ryan 5.

## 6. Verificación

- **Antes de abrir Unity:** compilar `T2D.Dominio` y los tests con .NET 8 y C# 9 (`LangVersion 9`) y ejecutarlos con un ejecutor mínimo compatible con los atributos de NUnit que usa Unity.
- **En Unity:** la capa `T2D.Juego` y `T2D.Editor` se verifican al abrir el proyecto: consola sin errores, escena de prueba en Play y Test Runner.
- Auditoría final con la checklist de terminado: sin `null` como error, sin código muerto, sin `TODO`, sin `Debug.Log` de depuración, con constantes con nombre.

## 7. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| Errores de compilación en la capa Unity que no puedo ver | Capa Unity delgada, solo API estable de Unity 6; README con solución de problemas; corrección rápida con la consola. |
| 35 HIP contra ~22 de capacidad; Evaluación integral 1 el jueves 12 | El código llega listo para integrar y probar. Seguir pidiendo al PO pasar 1689 y 1766 al Sprint 2. |
| La mayoría de las UT siguen en *Especificar* (incluida la 1680) | No se puede *Programar* en Worki sin pasar por *Diseñar*: el mensaje al grupo lo prioriza. |
| Tiempo en Worki que no refleje el trabajo real | Cada uno cronometra su propio trabajo: integrar, revisar, probar y ajustar. |
