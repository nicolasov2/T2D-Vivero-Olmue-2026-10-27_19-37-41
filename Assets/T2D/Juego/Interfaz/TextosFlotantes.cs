using System.Collections.Generic;
using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>
    /// UT 1687 and 1766. Short texts that rise and fade over a cell: the credits charged when a station
    /// is placed and the credits refunded when it is dismantled. They use unscaled time, so they also
    /// fade while the match is paused.
    /// </summary>
    public sealed class TextosFlotantes
    {
        private const float Duracion = 1.6f;
        private const float Subida = 40f;
        private const float Ancho = 200f;
        private const float Alto = 24f;

        private readonly List<(string Texto, Vector3 Mundo, float Inicio)> _textos = new List<(string, Vector3, float)>();

        /// <summary>Text for the credits charged.</summary>
        public static string Cobro(Costo costo) => $"−{costo.Agua} agua · −{costo.Energia} energía";

        /// <summary>Text for the credits refunded.</summary>
        public static string Reembolso(Costo costo) => $"+{costo.Agua} agua · +{costo.Energia} energía";

        /// <summary>Starts a text over a world point.</summary>
        public void Agregar(string texto, Vector3 mundo) => _textos.Add((texto, mundo, Time.unscaledTime));

        /// <summary>Draws the texts still alive; call from OnGUI after the scale is applied.</summary>
        public void Dibujar(Camera camara)
        {
            _textos.RemoveAll(texto => Time.unscaledTime - texto.Inicio > Duracion);
            var colorAnterior = GUI.color;
            foreach (var (texto, mundo, inicio) in _textos)
            {
                var avance = (Time.unscaledTime - inicio) / Duracion;
                var punto = EstiloInterfaz.MundoAVirtual(camara, mundo);
                GUI.color = new Color(colorAnterior.r, colorAnterior.g, colorAnterior.b, 1f - avance);
                GUI.Box(new Rect(punto.x - (Ancho / 2f), punto.y - Alto - (Subida * avance), Ancho, Alto), texto, EstiloInterfaz.Chip);
            }

            GUI.color = colorAnterior;
        }
    }
}
