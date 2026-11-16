# Spec 001 · Sprint 1 de programación (T2D en Unity)

Fecha: 10 nov 2026 · Autor: Nicolás Ramírez · Estado: **aprobada el 10 nov 2026** · actualizada el 15 nov 2026 (versión de Unity en CA-1)

## 1. Problema y contexto

T2D es un Tower Defense 2D sobre el monitoreo de un vivero (plantines de palto y cítricos en Olmué). El Sprint 1 ya tiene especificación y diseño por UT en Worki, pero:

- El equipo **no tiene proyecto Unity**: la UT 1762 (repositorio) no tiene link y no hay código.
- El Sprint 1 tiene **9 UT (35 HIP)**: la capacidad real del equipo es de unas 22.
- Tres piezas de las que dependen varias UT quedaron fuera del Sprint: el mapa (1698, ahora tarea), los créditos (1748) y el silo (1750), las dos últimas en Backlog.
- La 1690 entró al Sprint el 7 nov sin especificación ni diseño.

## 2. Usuarios afectados

| Usuario | Qué necesita |
|---|---|
| Jugador | Ver el vivero, construir cámaras, ver avanzar los eventos de estrés y perder agua en el silo. |
| Equipo T2D (5) | Un proyecto que compile al primer intento, con el código de su UT separado y sus tests. |
| Profesor (PO) | Poder ejecutar cada Prueba de Aceptación (PA) de cada UT en *Aplicar PA*. |

## 3. Alcance

1. Proyecto Unity base: carpetas, assemblies, `.gitignore`, README y un menú de editor que arma la escena de prueba.
2. Base común mínima: cuadrícula del vivero, créditos y silo.
3. Código y tests de las 9 UT del Sprint 1:

| UT | Nombre | HIP |
|---|---|---|
| 1680 | Rutas | 4 |
| 1686 | Puestos de plantas | 4 |
| 1678 | Validación de construcción | 4 |
| 1687 | Colocar cámara fija SBC | 4 |
| 1787 | Selección de casilla y despliegue de infraestructura | 2 |
| 1766 | Desmantelar o reciclar estación | 3 |
| 1688 | Olas de calor | 4 |
| 1689 | Vientos fuertes | 5 |
| 1690 | Deshidratación | 5 |

4. Especificación, mockup y diseño de la 1690 para Ryan, con el mismo formato que las demás.
5. Guía para pasar cada UT por *Esperar incluir en Sprint → Programar → Aplicar PA* en Worki.

### Fuera de alcance

- UT del Sprint 2 o posteriores (oleadas 1790, plantas 1691, microaspersión 1753, etc.).
- Arte final, sonido, menús, guardado y build para móvil.
- Registrar tiempo en Worki: lo hace cada persona, con su cronómetro, mientras trabaja.

## 4. Criterios de aceptación

**CA-1 · Compila al primer intento**
- Dado un proyecto nuevo creado en Unity Hub con la plantilla *Universal 2D* y la versión acordada (Unity 6.3 LTS, `6000.3.27f1`),
- cuando se copia la carpeta `Assets/T2D`,
- entonces Unity compila sin errores y sin advertencias en los scripts de T2D.

**CA-2 · Escena de prueba en un clic**
- Dado el proyecto compilado,
- cuando se ejecuta el menú *T2D → Crear escena de prueba*,
- entonces se crean la escena `Nivel_Olmue`, los tiles, las definiciones y los prefabs, y al presionar Play se ven la cuadrícula, la ruta, los pasillos de plantines y el silo.

**CA-3 · Cada PA tiene cómo verificarse**
- Dada cualquier PA de las 9 UT,
- cuando se busca en la tabla de verificación del README,
- entonces tiene al menos un test EditMode o un paso de prueba manual en Play Mode.

**CA-4 · Lógica separada de Unity**
- Dado el assembly `T2D.Dominio`,
- cuando se revisan sus referencias,
- entonces no usa `UnityEngine` (`noEngineReferences: true`), y toda regla del juego vive ahí.

**CA-5 · Tests en verde**
- Dado el Test Runner de Unity en modo EditMode,
- cuando se ejecutan todos los tests,
- entonces pasan todos.

**CA-6 · Sin crear ni destruir en partida**
- Dada una partida en curso,
- cuando aparecen y llegan eventos de estrés,
- entonces se reutilizan desde un pool: no hay `Instantiate` ni `Destroy` de eventos después de cargar el nivel.

## 5. Casos borde y de error

| Caso | Comportamiento esperado |
|---|---|
| Ruta con dos puntos no adyacentes o fuera del mapa | La definición se rechaza al cargar el nivel, con un mensaje que nombra el punto. |
| Nivel sin rutas | No se carga; error claro en consola. |
| Pasillo de plantines sobre una ruta | Se rechaza al cargar el nivel. |
| Créditos insuficientes | No se construye ni se descuenta nada; aparece "Créditos insuficientes". |
| Reembolso con costo impar (45 de energía) | Se redondea hacia abajo (22). |
| Daño mayor que la integridad o el agua restante | Queda en 0, nunca negativo. |
| Pool de eventos agotado | Crece una vez de forma controlada y lo informa en consola; nunca falla en silencio. |
| Confirmar sin estación elegida | Se ignora; el estado de selección no cambia. |

## 6. Requisitos no funcionales

- **Rendimiento:** 60 FPS en los PC del laboratorio con 30 eventos en pantalla.
- **Mantenibilidad:** una carpeta por área del juego; clases cortas; las reglas del juego se prueban sin abrir Unity.
- **Errores:** los fallos esperados (validación, saldo, transición inválida) se devuelven como resultados explícitos; nada de `null` ni valores vacíos como señal de error.
- **Accesibilidad:** casilla válida e inválida se distinguen por color **y** por texto, no solo por color.
- **Versión:** todo el equipo usa la misma versión de Unity, registrada en `ProjectSettings/ProjectVersion.txt`.
