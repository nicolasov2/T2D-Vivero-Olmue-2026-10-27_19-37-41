using System.Collections.Generic;
using T2D.Dominio;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace T2D.Juego
{
    /// <summary>
    /// Demo aid. Explains what each colour on the map means: the kinds of cell, painted with the
    /// palette tiles, and the kinds of stress event, with the colour of their asset.
    /// </summary>
    public sealed class LeyendaDeColores
    {
        private const float Margen = 10f;
        private const float MargenSuperior = 6f;
        private const float AltoTitulo = 26f;
        private const float AltoSubtitulo = 24f;
        private const float AltoFilaMinimo = 20f;
        private const float LadoMuestra = 14f;
        private const float AnchoMuestra = 22f;

        private static readonly (string Etiqueta, TipoCasilla Tipo, Especie Especie)[] Casillas =
        {
            ("Suelo edificable", TipoCasilla.SueloEdificable, Especie.Palto),
            ("Ruta de los eventos", TipoCasilla.Ruta, Especie.Palto),
            ("Pasillo de paltos", TipoCasilla.Plantines, Especie.Palto),
            ("Pasillo de cítricos", TipoCasilla.Plantines, Especie.Citrico),
            ("Casilla bloqueada", TipoCasilla.Bloqueada, Especie.Palto),
            ("Silo de agua", TipoCasilla.Silo, Especie.Palto),
        };

        /// <summary>Draws a solid colour swatch.</summary>
        public static void DibujarMuestra(Rect muestra, Color color)
        {
            var colorAnterior = GUI.color;
            GUI.color = ComoEnElMapa(color);
            GUI.DrawTexture(muestra, Texture2D.whiteTexture);
            GUI.color = colorAnterior;
        }

        /// <summary>Draws the legend with its top-left corner at <paramref name="origen"/> and returns its rectangle.</summary>
        public Rect Dibujar(Vector2 origen, float ancho, PaletaDeTiles paleta, IReadOnlyList<EventoDefinicionSO> eventos)
        {
            var anchoTexto = ancho - (2f * Margen) - AnchoMuestra;
            var panel = new Rect(origen.x, origen.y, ancho, Medir(anchoTexto, eventos));
            GUI.Box(panel, GUIContent.none, EstiloInterfaz.Caja);
            GUI.Label(new Rect(panel.x + Margen, panel.y + MargenSuperior, ancho - (2f * Margen), AltoTitulo), "Leyenda", EstiloInterfaz.Titulo);

            var y = panel.y + MargenSuperior + AltoTitulo;
            foreach (var casilla in Casillas)
            {
                y = DibujarFila(panel.x + Margen, y, anchoTexto, casilla.Etiqueta, ColorDe(paleta, casilla.Tipo, casilla.Especie));
            }

            if (eventos.Count == 0)
            {
                return panel;
            }

            GUI.Label(new Rect(panel.x + Margen, y, ancho - (2f * Margen), AltoSubtitulo), "Eventos de estrés", EstiloInterfaz.Texto);
            y += AltoSubtitulo;
            foreach (var evento in eventos)
            {
                y = DibujarFila(panel.x + Margen, y, anchoTexto, evento.Nombre, evento.Color);
            }

            return panel;
        }

        private static float Medir(float anchoTexto, IReadOnlyList<EventoDefinicionSO> eventos)
        {
            var alto = MargenSuperior + AltoTitulo + Margen;
            foreach (var casilla in Casillas)
            {
                alto += AltoDeFila(casilla.Etiqueta, anchoTexto);
            }

            if (eventos.Count > 0)
            {
                alto += AltoSubtitulo;
            }

            foreach (var evento in eventos)
            {
                alto += AltoDeFila(evento.Nombre, anchoTexto);
            }

            return alto;
        }

        private static float DibujarFila(float x, float y, float anchoTexto, string etiqueta, Color color)
        {
            var alto = AltoDeFila(etiqueta, anchoTexto);
            DibujarMuestra(new Rect(x, y + ((AltoFilaMinimo - LadoMuestra) / 2f), LadoMuestra, LadoMuestra), color);
            GUI.Label(new Rect(x + AnchoMuestra, y, anchoTexto, alto), etiqueta, EstiloInterfaz.TextoChico);
            return y + alto;
        }

        private static float AltoDeFila(string etiqueta, float anchoTexto) =>
            Mathf.Max(AltoFilaMinimo, EstiloInterfaz.TextoChico.CalcHeight(new GUIContent(etiqueta), anchoTexto));

        // IMGUI in a linear colour space project shows colours lighter than the sprites on the map.
        // Converting them first keeps each swatch equal to the tile or event it explains.
        private static Color ComoEnElMapa(Color color) =>
            QualitySettings.activeColorSpace == ColorSpace.Linear ? color.linear : color;

        // Tilemap tiles carry their colour in Tile.color; any other tile type is shown as white.
        private static Color ColorDe(PaletaDeTiles paleta, TipoCasilla tipo, Especie especie) =>
            paleta.TileDe(tipo, especie) is Tile tile ? tile.color : Color.white;
    }
}
