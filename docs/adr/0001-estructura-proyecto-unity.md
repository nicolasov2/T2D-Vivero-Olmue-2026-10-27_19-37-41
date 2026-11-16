# ADR 0001 · Estructura del proyecto Unity de T2D

Fecha: 10 nov 2026 · Estado: aceptada

## Contexto

El Sprint 1 empezó sin proyecto Unity, con 9 UT repartidas entre 5 personas que trabajan en horarios distintos y con poca coordinación. "T2D Requisitos" pide separar la lógica del juego de Unity. Los diseños subidos a Worki nombran las clases en español.

## Decisión

1. **Cuatro assemblies.**
   - `T2D.Dominio`: C# puro con `noEngineReferences`, donde viven todas las reglas.
   - `T2D.Juego`: MonoBehaviours y ScriptableObjects.
   - `T2D.Editor`: herramientas.
   - `T2D.Tests`: EditMode con NUnit.
2. **El `NivelDefinicion` (ScriptableObject) es la única fuente de verdad del nivel.** El Tilemap se pinta desde él y no al revés.
3. **Fallos esperados con `Resultado<T, TError>`.** Las excepciones quedan para errores de programación. En el dominio, la ausencia válida se tipa como `T?` (nullable activado).
4. **Interfaz del Sprint 1 con IMGUI (`OnGUI`).**
5. **Escena, tiles, prefabs y sprites generados por el menú *T2D → Crear escena de prueba*.** Las definiciones de `Contenido/` se crean una sola vez y no se pisan.
6. **Identificadores en español**, iguales a los diseños; comentarios y commits en inglés.

## Consecuencias

- Las reglas se prueban sin abrir Unity y fuera de él con .NET 8: hay 81 tests.
- La capa `T2D.Juego` no tiene nullable activado, porque los campos serializados los asigna el inspector. Se validan en `Partida.Start`.
- **Una sola persona** crea el proyecto y ejecuta el menú; el resto clona. Si cada uno genera sus assets, los GUID de los `.meta` chocan.
- Cambiar IMGUI por uGUI en el Sprint 2 no toca el dominio.
- El silo y los créditos son versiones mínimas: 1748 y 1750 las completan cuando entren a un Sprint.
