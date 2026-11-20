# Plan 003 · Más vivero en las UT del Sprint 1

## Decisiones

| # | Decisión | Por qué | Alternativa descartada |
|---|---|---|---|
| D1 | Plantín y flecha se dibujan en código (`SpritesProcedurales`) al iniciar la partida. | Sin imágenes nuevas en Git ni archivos LFS, y cada sprite se dibuja una sola vez. | PNG nuevos en `Generado`. |
| D2 | Todo lo crean componentes que ya están en la escena o en los prefabs (`VistaPasillos`, `RotuloDePasillo`, `VistaEventoEstres`, `VistaEstadoEstacion`, `VistaMapa`, `HudDePartida`). | Editar la escena o los prefabs cambia identificadores y choca en Git. | Componentes nuevos. |
| D3 | `Hidratacion.EstaSeca` con `UmbralSeco = 30` en el dominio. | Es una regla que se puede probar sin Unity. | El umbral en la vista. |
| D4 | `IEfectoDeEvento.Alcance`, 0 para la ola de calor. | La vista dibuja el alcance real del efecto, sin conocer cada tipo. | Preguntar el tipo de efecto en la vista. |
| D5 | El disco de alcance mide `2 × Alcance + 1` casillas. | El daño usa distancia de Chebyshev, igual que el radio de cobertura de la cámara. | Un círculo de radio Alcance. |
| D6 | Los textos flotantes usan tiempo sin escala. | Se desvanecen aunque el juego esté en pausa. | `Time.time`. |
| D7 | `VistaPasillos` ubica los plantines con la grilla y no con `VistaMapa`. | `VistaMapa` puede iniciarse después que `VistaPasillos`. | — |

## Archivos

| Archivo | Cambio |
|---|---|
| `Dominio/Plantines/Hidratacion.cs` | `UmbralSeco` y `EstaSeca`. |
| `Dominio/Eventos/IEfectoDeEvento.cs`, `EfectoOlaDeCalor.cs` | `Alcance`. |
| `Juego/Base/SpritesProcedurales.cs` | Nuevo. Plantín por especie y flecha. |
| `Juego/Base/FabricaDeSprites.cs` | Sobrecarga con cualquier transform como padre. |
| `Juego/Base/OrdenDeDibujo.cs` | Órdenes de plantín, flecha, entrada, barra y alcance. |
| `Juego/Base/EstiloInterfaz.cs` | Estilo `Chip`. |
| `Juego/Plantines/VistaPasillos.cs` | Plantines y su color según la hidratación. |
| `Juego/Plantines/RotuloDePasillo.cs` | Rótulo de hidratación sobre cada pasillo. |
| `Juego/Eventos/VistaEventoEstres.cs` | Disco de alcance. |
| `Juego/Construccion/VistaEstacion.cs`, `VistaEstadoEstacion.cs` | Barra de integridad. |
| `Juego/Mapa/VistaMapa.cs` | Entrada y flechas de las rutas. |
| `Juego/Interfaz/TextosFlotantes.cs`, `HudDePartida.cs` | Textos flotantes de créditos. |
| `Tests/DeshidratacionTests.cs` | Tests de `EstaSeca` y `Alcance`. |

## Riesgos

| Riesgo | Mitigación |
|---|---|
| Se cambia una UT ya terminada (retrabajo). | Registrar una UT por mejora con el aviso "Detalle que queda por resolver detectado en revisión de Sprint 1". |
| Muchos sprites en el mapa. | Son unos 60 sprites estáticos y comparten 3 texturas. |
