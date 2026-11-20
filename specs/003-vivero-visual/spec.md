# Spec 003 · Más vivero en las UT del Sprint 1

Fecha: 20 nov 2026 · Autor: Nicolás Ramírez · Estado: **aprobada el 20 nov 2026**

## 1. Problema y contexto

En la revisión el profesor pidió que el juego se vea más como un vivero. Las 9 UT del Sprint 1 funcionan, pero se ven como cuadros de colores. Quien mira no ve plantas, no sabe qué tan secos están los pasillos ni a qué afecta cada evento.

Estas mejoras profundizan lo que ya hacen las UT del Sprint 1. No agregan reglas ni funcionalidades del Sprint 2.

## 2. Usuarios afectados

| Usuario | Qué necesita |
|---|---|
| Profesor y quien mira la demo | Reconocer un vivero y entender qué pasa sin explicación. |
| Equipo T2D | Mostrar cada UT con algo visible. |

## 3. Alcance

| UT | Mejora |
|---|---|
| 1686 Puestos de plantas | Un plantín dibujado en cada casilla del pasillo, distinto para palto y cítrico. |
| 1690 Deshidratación | Porcentaje de hidratación siempre visible sobre cada pasillo y aviso "Seco" en rojo bajo el 30 %. Los plantines también se secan de color. |
| 1688, 1689, 1690 Eventos | Disco tenue con el alcance de cada evento, del color del evento. |
| 1689 Vientos fuertes | Barra de integridad sobre cada cámara, de verde a rojo. |
| 1680 Rutas | Marca en la entrada de cada ruta y flechas con la dirección hacia el silo. |
| 1687, 1766 Cámara y desmantelar | Texto flotante con lo que se cobra al instalar y lo que se recupera al desmantelar. |

## 4. Fuera de alcance

- Crecimiento, salud o reemplazo de plantas (1691, 1693).
- Riego, microaspersión, monitoreo, plagas y entorno (1751, 1753, 1760, 1709, 1676, 1681, 1698).
- Cambios en costos, daños, velocidades o en el nivel.
- Imágenes nuevas en el repositorio y cambios en la escena o en los prefabs.

## 5. Criterios de aceptación

- **CA-1 Plantines (1686).** Dado el juego en Play, entonces cada casilla de pasillo muestra un plantín en su bolsa. Los paltos tienen hojas anchas y oscuras y los cítricos hojas redondas con frutos naranjos.
- **CA-2 Hidratación visible (1690).** Dado el juego en Play, entonces sobre cada pasillo se ve su hidratación en %. Cuando baja a 30 % o menos, entonces el rótulo dice "Seco" y es rojo. Al bajar la hidratación, los plantines también se ponen café.
- **CA-3 Alcance (1688, 1689, 1690).** Dado un viento o un vector de deshidratación avanzando, entonces lo rodea un disco tenue de su color del tamaño de su alcance. La ola de calor, que solo afecta al silo, no lo muestra.
- **CA-4 Integridad (1689).** Dada una cámara instalada, entonces tiene una barra verde encima. Cuando el viento la daña, la barra se acorta hacia la izquierda y pasa a rojo. Sin integridad, la cámara se ve gris como antes.
- **CA-5 Rutas (1680).** Dado el juego en Play, entonces la casilla de entrada de cada ruta tiene un marco rojo y cada casilla de la ruta una flecha hacia la siguiente.
- **CA-6 Créditos (1687, 1766).** Al confirmar una cámara, entonces sube y se desvanece "−60 agua · −40 energía" sobre su casilla. Al desmantelarla, entonces sube "+30 agua · +20 energía".
- **CA-7 Sin regresiones.** Todos los tests EditMode pasan y la consola queda sin errores en Play.

## 6. Casos borde

- Un pasillo seco sigue mostrando su rótulo detallado al tocarlo.
- Con el juego en pausa, los textos flotantes igual se desvanecen.
- La barra de integridad y los discos compensan la escala de sus objetos, que son más chicos que una casilla.

## 7. Requisitos no funcionales

- Los dibujos se generan en código al iniciar la partida, una vez por tipo.
- Sin componentes nuevos en la escena ni en los prefabs: todo lo crean componentes que ya están.
