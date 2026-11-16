# Tareas 001 · Sprint 1 de programación

Orden de ejecución. Cada tarea se marca `[x]` al terminar y verificar.

| # | Tarea | Cubre | Estado |
|---|---|---|---|
| T1 | Estructura: carpetas, 4 asmdef, `.gitignore`, README base y ADR 0001 | CA-1, CA-4 | [x] |
| T2 | Base del dominio: `Posicion`, `Resultado`, `TipoCasilla`, `Casilla`, `Cuadricula` + tests | CA-4, CA-5 | [x] |
| T3 | Recursos mínimos: `Costo`, `Creditos`, `Silo` + tests (saldo, clamp en 0) | CA-5 | [x] |
| T4 | 1680: `Ruta`, `RecorridoDeRuta` + tests; `SeguidorDeRuta`, `DibujoDeRuta` | PA 1209–1212 | [x] |
| T5 | 1686: `Especie`, `PasilloDePlantines`, `RegistroDePasillos` + tests; `VistaPasillo`, `RotuloDePasillo` | PA 1686 (4) | [x] |
| T6 | Nivel: `DatosDeNivel`, `ConstructorDeNivel`, `Nivel` + tests de casos borde | CA-2, spec §5 | [x] |
| T7 | 1678: `ResultadoColocacion`, `ValidadorDeColocacion` + tests; `VistaPrevisualizacion`, mensajes | PA 1678 (6) | [x] |
| T8 | 1687: `DefinicionEstacion`, `Estacion`, `ServicioDeConstruccion` + tests; panel, controlador, radio | PA 1687 (5) | [x] |
| T9 | 1787: `EstadoSeleccion`, `SelectorDeCasilla` + tests; botones Confirmar/Cancelar | PA 1787 (4) | [x] |
| T10 | 1766: `CalculadoraDeReembolso`, `Desmantelar` + tests; `PanelDeEstacion` | PA 1766 (4) | [x] |
| T11 | 1688: `DefinicionEvento`, `EventoEstres`, `TemporizadorDeAparicion`, `EfectoOlaDeCalor` + tests; generador con pool | PA 1688 (5), CA-6 | [x] |
| T12 | 1689: `Integridad`, `DanioPorViento`, `EfectoViento` + tests; `VistaEstadoEstacion` | PA 1689 (5) | [x] |
| T13 | 1690: especificación, mockup y diseño; `Hidratacion`, `DanioPorDeshidratacion`, `EfectoDeshidratacion` + tests | PA 1690 | [x] |
| T14 | Capa Unity común: `Partida` (carga el nivel), `VistaMapa`, `ControladorDeToques`, `LectorDePuntero`, HUD | CA-2 | [x] |
| T15 | Editor: `ConstructorDeEscena` (sprites, tiles, definiciones, prefabs, escena) | CA-2 | [x] |
| T16 | Verificación: compilar dominio y tests con .NET 8 / C# 9, ejecutarlos; auditoría del diff | CA-4, CA-5 | [x] |
| T17 | README: instalación, tabla PA → verificación, reparto y flujo en Worki | CA-3 | [x] |
| T18 | Entrega: zip del proyecto, guía de Worki, mensaje para el grupo | — | [x] |
| T19 | Instalación real en Unity 6.6 (`6000.6.9f1`): copiar al proyecto, compilar, generar la escena, Play y Test Runner. Corregidos `ConstructorDeEscena` (referencias vacías al nivel y a la cámara) y `VistaPasillos` (pasillos en blanco al perder hidratación) | CA-1, CA-2, CA-5 | [x] |
| T20 | Integración en el repositorio del equipo (Unity 6.3 LTS, `6000.3.27f1`): copiar al proyecto base, compilar, generar la escena, Play y Test Runner; commit y push | CA-1, CA-2, CA-5 | [x] |

Verificación (T16): dominio y tests compilados con .NET 8, C# 9 y `netstandard2.1` sin advertencias; **81/81 tests en verde**; capa Unity revisada contra stubs de la API (sin el editor de Unity, que no está disponible en el entorno de verificación).

Verificación en Unity (T19, 15 nov 2026): compila sin errores ni advertencias; *T2D → Crear escena de prueba* genera la escena; en Play se ven la cuadrícula, la ruta, los pasillos, el silo y los eventos, y se construyó una cámara (cobro 60 agua · 40 energía, radio y panel con reembolso); **81/81 tests en verde en el Test Runner**.

Verificación en el repositorio (T20, 15 nov 2026, Unity 6.3 LTS `6000.3.27f1`): compila sin errores ni advertencias del código de T2D; *T2D → Crear escena de prueba* genera la escena y los assets; en Play se ven el mapa, la ruta y los eventos; **81/81 tests en verde**.
