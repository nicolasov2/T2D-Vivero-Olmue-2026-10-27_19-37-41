# Tareas 002 · Ayudas para la demo

| # | Tarea | Criterios | Estado |
|---|---|---|---|
| T1 | Corregir los 2 tests de la 1766 al costo 60 agua · 40 energía | CA-6 | [x] |
| T2 | `TemporizadorDeAparicion.SegundosParaElSiguiente` con su test | CA-4 | [x] |
| T3 | `GeneradorDeEventos.TryObtenerProximo` y `EventoDefinicionSO.Nombre` | CA-4 | [x] |
| T4 | `ControlDeVelocidad` y reinicio de la velocidad al salir | CA-1, CA-2 | [x] |
| T5 | `LeyendaDeColores` | CA-3 | [x] |
| T6 | `AvisoProximoEvento` | CA-4 | [x] |
| T7 | Columnas laterales en `HudDePartida` y registro en `ZonasDeInterfaz` | CA-5 | [x] |
| T8 | README con los controles de la demo | — | [x] |
| T9 | Verificación en Unity: compila, Play, paneles visibles | CA-1 a CA-5 | [x] |

Verificación fuera de Unity (19 nov 2026): dominio, tests y código de juego compilan con advertencias como errores. Pasan 83 de 83 tests con el ejecutor mínimo.

Verificación en Unity 6.3 LTS (19 nov 2026): compila sin errores. En Play se ven el próximo evento, la leyenda con los mismos colores del mapa y la velocidad. Pausa detiene los eventos y el aviso dice "En pausa". x2 y x3 los aceleran y el botón activo queda marcado en verde. Consola sin errores ni advertencias en Play. El Test Runner de Unity queda pendiente de correr antes de mergear.
