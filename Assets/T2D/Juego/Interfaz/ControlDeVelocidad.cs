using UnityEngine;

namespace T2D.Juego
{
    /// <summary>
    /// Demo aid. Pause, x1, x2 and x3 buttons that change <see cref="Time.timeScale"/>, so the stress
    /// events can be shown without waiting. Everything that moves uses scaled time, so pause stops it.
    /// </summary>
    public sealed class ControlDeVelocidad
    {
        /// <summary>Height the panel needs.</summary>
        public const float Alto = 112f;

        private const float Margen = 10f;
        private const float MargenSuperior = 6f;
        private const float AltoTitulo = 26f;
        private const float EspacioBajoTitulo = 4f;
        private const float AltoBoton = 30f;
        private const float Separacion = 6f;
        private const int Columnas = 2;
        private const float VelocidadNormal = 1f;

        private static readonly Color ColorActivo = new Color(0.45f, 0.8f, 0.45f);

        private static readonly float[] Velocidades = { 0f, 1f, 2f, 3f };
        private static readonly string[] Etiquetas = { "Pausa", "x1", "x2", "x3" };

        /// <summary>True while the match clock is stopped.</summary>
        public static bool EstaEnPausa => Time.timeScale <= 0f;

        /// <summary>Back to normal speed, so the editor never keeps a stopped clock after Play.</summary>
        public static void Restablecer() => Time.timeScale = VelocidadNormal;

        /// <summary>Draws the panel inside <paramref name="panel"/> (virtual units).</summary>
        public void Dibujar(Rect panel)
        {
            GUI.Box(panel, GUIContent.none, EstiloInterfaz.Caja);
            GUI.Label(new Rect(panel.x + Margen, panel.y + MargenSuperior, panel.width - (2f * Margen), AltoTitulo), "Velocidad", EstiloInterfaz.Titulo);
            var ancho = (panel.width - (2f * Margen) - ((Columnas - 1) * Separacion)) / Columnas;
            var primeraFila = panel.y + MargenSuperior + AltoTitulo + EspacioBajoTitulo;
            for (var i = 0; i < Velocidades.Length; i++)
            {
                var columna = i % Columnas;
                var fila = i / Columnas;
                var boton = new Rect(panel.x + Margen + (columna * (ancho + Separacion)), primeraFila + (fila * (AltoBoton + Separacion)), ancho, AltoBoton);
                DibujarBoton(boton, Velocidades[i], Etiquetas[i]);
            }
        }

        private static void DibujarBoton(Rect boton, float velocidad, string etiqueta)
        {
            var activa = Mathf.Approximately(Time.timeScale, velocidad);
            var fondoAnterior = GUI.backgroundColor;
            GUI.backgroundColor = activa ? ColorActivo : fondoAnterior;
            var elegida = GUI.Toggle(boton, activa, etiqueta, EstiloInterfaz.Boton);
            GUI.backgroundColor = fondoAnterior;
            if (elegida && !activa)
            {
                Time.timeScale = velocidad;
            }
        }
    }
}
