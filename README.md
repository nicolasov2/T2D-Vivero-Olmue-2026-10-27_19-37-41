# T2D · Tower Defense del vivero (Sprint 1)

Juego 2D en Unity del equipo T2D (IIF 6515, UPLA 2026-2). El jugador protege los plantines de paltos y cítricos de un vivero en Olmué: instala cámaras SBC mientras olas de calor, vientos fuertes y vectores de deshidratación avanzan por las rutas hacia el silo de agua.

Este repositorio trae el código de las 9 UT del Sprint 1, sus tests y un menú que arma la escena de prueba. Spec, plan y tareas están en `specs/001-sprint1-programacion/`.

## Requisitos

- **Unity 6.3 LTS** (`6000.3.27f1`), instalado desde Unity Hub. Todo el equipo usa esta misma versión; queda registrada en `ProjectSettings/ProjectVersion.txt`.
- Git con Git LFS: el repositorio guarda las imágenes con LFS. GitHub Desktop ya lo trae.

## Instalación

El proyecto ya está en el repositorio del equipo (UT 1762), con la escena y los assets generados.

1. Clona el repositorio. Con GitHub Desktop: *File → Clone repository*.
2. En Unity Hub: *Añadir → Añadir proyecto desde el disco* y elige la carpeta clonada. Si no tienes la `6000.3.27f1`, Unity Hub ofrece instalarla.
3. Abre el proyecto y espera a que compile. La consola no debe mostrar errores.
4. Abre la escena `Assets/T2D/Escenas/Nivel_Olmue` y presiona **Play**.
5. Abre *Window → General → Test Runner → EditMode → Run All*. Deben pasar los 81 tests.

No ejecutes **T2D → Crear escena de prueba** salvo que la escena se rompa: regenera `Assets/T2D/Generado` con identificadores nuevos y eso choca en Git con el trabajo de los demás.

## Cómo se juega la escena de prueba

| Acción | Resultado |
|---|---|
| Tocar una casilla | Queda marcada en amarillo (1787). |
| Elegir *Cámara SBC* en el panel inferior | Aparece la previsualización: verde si se puede construir, roja si no, con el motivo escrito (1678). |
| Tocar una casilla con la cámara elegida y luego **Confirmar** | Se instala la cámara y se descuentan los créditos (1687). **Cancelar** no cobra nada (1787). |
| Tocar una cámara instalada | Muestra su radio (1687), su integridad (1689) y el botón **Desmantelar** con el reembolso (1766). |
| Tocar un pasillo de plantines | Rótulo con la especie, los plantines y la hidratación (1686, 1690). |

Cada 4 segundos aparece un evento, en este orden: ola de calor (naranja), viento fuerte (azul) y vector de deshidratación (amarillo).

### Ayudas para la demo

| Panel | Dónde | Qué hace |
|---|---|---|
| Próximo evento | Arriba a la izquierda | Nombre y color del evento que viene y los segundos que faltan. En pausa dice "En pausa". |
| Leyenda | Bajo el próximo evento | Qué significa cada color: tipos de casilla y tipos de evento. |
| Velocidad | Arriba a la derecha | **Pausa**, **x1**, **x2** y **x3**. Al salir de Play vuelve a x1. |

Los paneles laterales caben junto al mapa con la vista Game en 16:9 o más ancha.

### Lo que se ve en el mapa

| Elemento | UT | Qué muestra |
|---|---|---|
| Plantín en cada casilla de pasillo | 1686 | Palto con hojas anchas, cítrico con hojas redondas y frutos. Se ponen café al secarse. |
| Rótulo sobre cada pasillo | 1690 | Hidratación en %. En rojo con "Seco" desde 30 % hacia abajo. |
| Disco alrededor de un evento | 1689, 1690 | Hasta dónde llega su efecto. La ola de calor no tiene, porque solo afecta al silo. |
| Barra sobre cada cámara | 1689 | Integridad, de verde a rojo. |
| Marco rojo y flechas | 1680 | Entrada de cada ruta y dirección hacia el silo. |
| Texto que sube y se desvanece | 1687, 1766 | Créditos cobrados al instalar y recuperados al desmantelar. |

## Estructura

```
Assets/T2D/
  Dominio/   reglas del juego en C# puro (sin UnityEngine)
  Juego/     MonoBehaviours, ScriptableObjects e interfaz IMGUI
  Editor/    menú T2D → Crear escena de prueba
  Tests/     tests EditMode, un archivo por UT
  Contenido/ nivel, estación y eventos (se crean una vez; edítalos aquí)
  Generado/  sprites, tiles y prefabs (el menú los regenera)
  Escenas/   Nivel_Olmue.unity
```

Todo lo configurable se edita en `Contenido/`, sin tocar código:

- `Nivel_Olmue`: rutas, pasillos, casillas bloqueadas, silo y créditos.
- `Camara_SBC`: costo, radio e integridad.
- `Ola_de_calor`, `Viento_fuerte` y `Vector_de_deshidratacion`: velocidad, daño y alcance.

## Reparto del Sprint 1

| UT | Programa | Prueba | Archivos principales |
|---|---|---|---|
| Base + 1680 Rutas | Nicolás | Pablo | `Dominio/Base`, `Dominio/Nivel`, `Dominio/Rutas`, `Juego/Base`, `Juego/Mapa`, `Juego/Rutas` |
| 1686 Puestos de plantas | Nicolás | Monserrat | `Dominio/Plantines`, `Juego/Plantines/RotuloDePasillo` |
| 1678 Validación de construcción | Benjamín | Pablo | `ValidadorDeColocacion`, `ResultadoColocacion`, `VistaPrevisualizacion`, `MensajeDeRechazo` |
| 1687 Colocar cámara fija SBC | Pablo | Benjamín | `DefinicionEstacion`, `Estacion`, `ServicioDeConstruccion`, `PanelDeConstruccion`, `ControladorDeConstruccion`, `VistaRadioCobertura` |
| 1688 Olas de calor | Pablo | Nicolás | `Dominio/Eventos` (base y `EfectoOlaDeCalor`), `GeneradorDeEventos`, `VistaEventoEstres` |
| 1766 Desmantelar o reciclar estación | Benjamín | Nicolás | `CalculadoraDeReembolso`, `ServicioDeConstruccion.Desmantelar`, `PanelDeEstacion` |
| 1787 Selección de casilla y despliegue | Monserrat | Ryan | `EstadoSeleccion`, `SelectorDeCasilla`, `VistaSeleccion`, `BotonesConfirmacion` |
| 1689 Vientos fuertes | Monserrat | Benjamín | `Integridad`, `DanioPorViento`, `EfectoViento`, `VientoDefinicion`, `VistaEstadoEstacion` |
| 1690 Deshidratación | Ryan | Monserrat | `Hidratacion`, `DanioPorDeshidratacion`, `EfectoDeshidratacion`, `DeshidratacionDefinicion`, `VistaPasillos` |

## Cómo verificar cada Prueba de Aceptación

En cada fila, **T** es un test EditMode, en el archivo de tests de la UT. **M** es una prueba manual en Play Mode.

| UT · PA | Verificación |
|---|---|
| 1680 · 1209 Ruta de la entrada al silo | T `PA1209_…` · M: la ruta café va del borde izquierdo al silo azul. |
| 1680 · 1210 Ruta distinta del suelo | T `PA1210_…` · M: café contra beige. |
| 1680 · 1211 Recorre casilla por casilla | T `PA1211_…` (3 tests) · M: los eventos siguen la ruta sin salirse. |
| 1680 · 1212 Cambiar la ruta sin código | T `PA1212_…` · M: edita un punto en `Nivel_Olmue` y presiona Play. |
| 1686 · PA1–PA4 | T `PA1_`…`PA4_` · M: 5 pasillos verdes (paltos) y amarillos (cítricos); al tocar uno aparece su rótulo. |
| 1678 · PA1–PA6 | T `PA1_`…`PA6_` · M: con la cámara elegida, recorre ruta, plantines, silo y suelo: rojo con motivo o verde. |
| 1687 · PA1 Panel con costo | T · M: el botón dice "Cámara SBC 60 agua · 40 energía". |
| 1687 · PA2–PA4 Instalar, cobrar, ocupar | T · M: con Confirmar aparece la cámara; el HUD baja 60/40; intenta otra vez en esa casilla y sale "La casilla ya está ocupada". |
| 1687 · PA5 Radio | T · M: tocar la cámara muestra el círculo azul. |
| 1787 · PA1–PA4 | T `PA1_`…`PA4_` · M: marco amarillo, previsualización con botones, Cancelar no cobra, Confirmar instala. |
| 1766 · PA1–PA4 | T `PA1_`…`PA4_` · M: el panel dice "Recuperas: 30 agua · 20 energía", la cámara desaparece, el HUD suma y la casilla vuelve a ser válida. |
| 1688 · PA1–PA4 | T `PA1_`…`PA4_` · M: la ola naranja sale de la entrada y la barra del silo baja al llegar; edita `Ola_de_calor` en Play. |
| 1688 · PA5 Pool | M: en *Hierarchy*, bajo `Partida` hay 30 `Evento` desde el inicio y no aumentan; la consola no avisa ampliación. |
| 1689 · PA1–PA5 | T `PA1_`…`PA5_` · M: instala una cámara junto a la ruta; el panel baja la integridad hasta 0 y la cámara queda gris. |
| 1690 · PA1–PA5 | T `PA1_`…`PA5_` · M: los pasillos junto a la ruta se ponen cafés y su rótulo baja la hidratación, sin pasar de 0. |

## Flujo en Worki para cada UT

1. **Diseñar:** subir el diseño y finalizar.
2. **Esperar incluir en Sprint:** no lleva tiempo. Finalízala para pasar a *Programar*.
3. **Programar** (quien programa):
   - Inicia el cronómetro en Historia → ⚡ → Iniciar/Continuar.
   - Integra, revisa y ajusta tus archivos, y corre tus tests.
   - Haz commit en tu rama `feature/<UT>-<nombre>` y abre el PR.
   - Marca tus PA en OK y finaliza.
4. **Aplicar Pruebas de Aceptación** (otra persona): ejecuta cada PA con la tabla de arriba y registra OK o KO. Con un KO, la UT vuelve a *Programar*.

El cronómetro corre solo mientras trabajas de verdad. Si se te olvida, registra el mismo día con la hora real.

## Diferencias con los diseños subidos a Worki

- **`Posicion` propia en vez de `Vector2Int`:** el dominio no usa tipos de Unity.
- **`NivelDefinicion` como fuente única del nivel:** el Tilemap se pinta desde ahí (1698 lo leía al revés).
- **`ValidadorDeColocacion` es una instancia** con la cuadrícula y los créditos, no una clase estática. Así `Validar` recibe 2 parámetros.
- **`EstadoSeleccion` tiene un cuarto estado, `EstacionElegida`:** permite elegir la estación antes de tocar la casilla, como en 1687.
- **No hay clase `CamaraSBC` por ahora:** `Estacion` + `DefinicionEstacion` bastan hasta que la cámara tenga comportamiento propio (1752).
- **Interfaz con IMGUI** para el Sprint 1.

## Solución de problemas

| Síntoma | Causa y solución |
|---|---|
| "Partida: falta asignar …" | La escena no viene del menú. Ejecuta *T2D → Crear escena de prueba*. |
| "El nivel … no es válido: …" | Un dato de `Nivel_Olmue` está mal (ruta no adyacente, pasillo sobre la ruta, etc.). El mensaje dice cuál. |
| Sprites negros o invisibles | Proyecto URP sin el material sin luz. Vuelve a ejecutar el menú. |
| No responde al mouse | Revisa *Project Settings → Player → Active Input Handling*. Cualquiera de las opciones funciona; si cambias, reinicia Unity. |
| No aparecen los tests | Instala *Test Framework* desde el Package Manager. |
