# Spec 002 · Ayudas para la demo del 20 nov 2026

Fecha: 19 nov 2026 · Autor: Nicolás Ramírez · Estado: **aprobada el 19 nov 2026**

## 1. Problema y contexto

El 20 nov se presenta el juego al profesor. Hoy cuesta mostrarlo por tres motivos.

- Hay que esperar los eventos en tiempo real para ver una ola de calor, un viento o la deshidratación.
- Quien mira no sabe qué significa cada color del mapa.
- No se sabe cuándo ni qué evento viene.

Además, el PR #2 cambió el costo de la cámara de prueba a 60 agua · 40 energía y dejó 2 tests de la 1766 esperando el reembolso del costo anterior.

## 2. Usuarios afectados

| Usuario | Qué necesita |
|---|---|
| Equipo T2D en la presentación | Acelerar, pausar y explicar el juego mientras lo muestra. |
| Profesor | Entender el mapa y los eventos sin que se lo expliquen celda por celda. |

## 3. Alcance

1. Control de velocidad con Pausa, x1, x2 y x3.
2. Leyenda de colores con los tipos de casilla y los tipos de evento.
3. Aviso del próximo evento con su nombre y los segundos que faltan.
4. Corregir los 2 tests de la 1766 que rompió el PR #2.

## 4. Fuera de alcance

- Pantalla de fin de partida. Corresponde a la UT 1756 Gestionar Derrota.
- Oleadas y dificultad. Corresponden a la UT 1790.
- Cambios en las reglas del juego, en los costos o en el nivel.
- Cambios en la escena Nivel_Olmue. Todo se dibuja desde componentes que ya están en ella.

## 5. Criterios de aceptación

- **CA-1 Velocidad.** Dado el juego en Play, cuando elijo x2 o x3, entonces los eventos avanzan y aparecen 2 o 3 veces más rápido. Cuando elijo Pausa, entonces todo lo que se mueve se detiene y la interfaz sigue respondiendo. Cuando elijo x1, entonces vuelve a la velocidad normal.
- **CA-2 Velocidad al salir.** Dado el juego en pausa o acelerado, cuando salgo de Play, entonces la velocidad vuelve a x1.
- **CA-3 Leyenda.** Dado el juego en Play, entonces a la izquierda se ve una leyenda con un color por cada tipo de casilla (suelo edificable, ruta, paltos, cítricos, bloqueada, silo) y por cada tipo de evento configurado, con su nombre.
- **CA-4 Próximo evento.** Dado el juego en Play, entonces se ve el nombre y el color del próximo evento y cuántos segundos faltan. En pausa dice "En pausa". Cuando el silo se agota, entonces dice "Sin eventos".
- **CA-5 Toques.** Dado un toque sobre cualquiera de los paneles nuevos, entonces no se selecciona la casilla del mapa que está debajo.
- **CA-6 Tests.** Todos los tests EditMode pasan, incluidos los 2 de la 1766.

## 6. Casos borde

- Sin generador de eventos en la escena, no se muestra el aviso y la leyenda muestra solo las casillas.
- Un nombre de evento largo, como "Vector de deshidratación", se ajusta en dos líneas sin salirse del panel.
- Pantallas más angostas que 16:9 pueden tapar el borde del mapa con las columnas laterales.

## 7. Requisitos no funcionales

- Sin escenas ni assets nuevos. Los paneles se dibujan desde `HudDePartida`, que ya está en la escena.
- El dominio sigue sin referencias a Unity.
