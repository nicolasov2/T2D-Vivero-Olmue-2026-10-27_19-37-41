using UnityEngine;

namespace T2D.Juego
{
    /// <summary>Demo aid. Says which stress event comes next and in how many seconds.</summary>
    public sealed class AvisoProximoEvento
    {
        private const float Margen = 10f;
        private const float MargenSuperior = 6f;
        private const float AltoTitulo = 26f;
        private const float AltoLinea = 22f;
        private const float LadoMuestra = 14f;
        private const float AnchoMuestra = 22f;

        /// <summary>Draws the panel with its top-left corner at <paramref name="origen"/> and returns its rectangle.</summary>
        public Rect Dibujar(Vector2 origen, float ancho, GeneradorDeEventos generador)
        {
            var hayProximo = generador.TryObtenerProximo(out var proximo, out var segundos);
            var nombre = hayProximo ? proximo.Nombre : "Sin eventos";
            var anchoTexto = ancho - (2f * Margen) - AnchoMuestra;
            var altoNombre = Mathf.Max(AltoLinea, EstiloInterfaz.Texto.CalcHeight(new GUIContent(nombre), anchoTexto));
            var alto = MargenSuperior + AltoTitulo + altoNombre + (hayProximo ? AltoLinea : 0f) + Margen;
            var panel = new Rect(origen.x, origen.y, ancho, alto);

            GUI.Box(panel, GUIContent.none, EstiloInterfaz.Caja);
            GUI.Label(new Rect(panel.x + Margen, panel.y + MargenSuperior, ancho - (2f * Margen), AltoTitulo), "Próximo evento", EstiloInterfaz.Titulo);
            var yNombre = panel.y + MargenSuperior + AltoTitulo;
            if (!hayProximo)
            {
                GUI.Label(new Rect(panel.x + Margen, yNombre, ancho - (2f * Margen), altoNombre), nombre, EstiloInterfaz.Texto);
                return panel;
            }

            LeyendaDeColores.DibujarMuestra(new Rect(panel.x + Margen, yNombre + ((AltoLinea - LadoMuestra) / 2f), LadoMuestra, LadoMuestra), proximo.Color);
            GUI.Label(new Rect(panel.x + Margen + AnchoMuestra, yNombre, anchoTexto, altoNombre), nombre, EstiloInterfaz.Texto);
            var cuenta = ControlDeVelocidad.EstaEnPausa ? "En pausa" : $"En {Mathf.CeilToInt(segundos)} s";
            GUI.Label(new Rect(panel.x + Margen + AnchoMuestra, yNombre + altoNombre, anchoTexto, AltoLinea), cuenta, EstiloInterfaz.Texto);
            return panel;
        }
    }
}
