# Plan 002 · Ayudas para la demo

## Decisiones

| # | Decisión | Por qué | Alternativa descartada |
|---|---|---|---|
| D1 | Los tres paneles son clases simples (`ControlDeVelocidad`, `AvisoProximoEvento`, `LeyendaDeColores`) que dibuja `HudDePartida`. | `HudDePartida` ya está en la escena. Un componente nuevo obligaría a editar la escena o a regenerarla, y eso cambia identificadores en Git. | Un MonoBehaviour por panel. |
| D2 | La velocidad usa `Time.timeScale`. | Eventos, temporizador y recorridos ya usan `Time.deltaTime`, así que todo se acelera o se detiene sin tocarlos. IMGUI sigue respondiendo en pausa. | Multiplicar la velocidad en cada sistema. |
| D3 | `HudDePartida.OnDestroy` deja `Time.timeScale` en 1. | Evita que el editor quede en pausa al salir de Play. | — |
| D4 | El tiempo restante se calcula en el dominio, `TemporizadorDeAparicion.SegundosParaElSiguiente`. | Es lógica pura y se puede probar sin Unity. | Calcularlo en la vista. |
| D5 | `GeneradorDeEventos.TryObtenerProximo` entrega el próximo evento o `false` cuando ya no aparecerán más. | Mismo criterio que `Update`, en una sola propiedad `PuedeAparecer`. | Devolver null. |
| D6 | La leyenda toma los colores de `PaletaDeTiles` y de cada `EventoDefinicionSO`. | Si alguien cambia un color en el asset, la leyenda lo muestra igual. | Colores escritos a mano. |
| D7 | Columnas laterales de 184 unidades virtuales. | A 16:9 el mapa deja unas 195 libres a cada lado. | Paneles sobre el mapa. |

## Cambios de datos

Ninguno. Sin migraciones ni assets nuevos.

## Archivos

| Archivo | Cambio |
|---|---|
| `Dominio/Eventos/TemporizadorDeAparicion.cs` | `SegundosParaElSiguiente`. |
| `Juego/Eventos/EventoDefinicionSO.cs` | `Nombre`. |
| `Juego/Eventos/GeneradorDeEventos.cs` | `Eventos`, `PuedeAparecer` y `TryObtenerProximo`. |
| `Juego/Base/EstiloInterfaz.cs` | Estilo `TextoChico`. |
| `Juego/Interfaz/HudDePartida.cs` | Columnas laterales y `OnDestroy`. |
| `Juego/Interfaz/ControlDeVelocidad.cs` | Nuevo. |
| `Juego/Interfaz/AvisoProximoEvento.cs` | Nuevo. |
| `Juego/Interfaz/LeyendaDeColores.cs` | Nuevo. |
| `Tests/OlasDeCalorTests.cs` | Test de `SegundosParaElSiguiente`. |
| `Tests/DesmantelarTests.cs` | Reembolso de 30 agua · 20 energía. |
| `README.md` | Controles de la demo. |

## Riesgos

| Riesgo | Mitigación |
|---|---|
| Las columnas tapan el mapa en pantallas angostas. | Presentar con la vista Game en 16:9 o más ancha. |
| Quedar en pausa sin darse cuenta. | El aviso del próximo evento dice "En pausa" y el botón queda marcado. |
