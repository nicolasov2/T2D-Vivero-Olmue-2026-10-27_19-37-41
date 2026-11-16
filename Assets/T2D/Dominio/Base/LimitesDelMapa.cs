#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>Width and height of a map, used to validate a level before its grid exists.</summary>
    public readonly struct LimitesDelMapa
    {
        /// <summary>Creates the bounds. A map smaller than 1x1 is a bug.</summary>
        public LimitesDelMapa(int ancho, int alto)
        {
            if (ancho <= 0 || alto <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ancho), $"El mapa debe medir al menos 1x1 (recibido {ancho}x{alto}).");
            }

            Ancho = ancho;
            Alto = alto;
        }

        /// <summary>Number of columns.</summary>
        public int Ancho { get; }

        /// <summary>Number of rows.</summary>
        public int Alto { get; }

        /// <summary>True when the position lies inside the map.</summary>
        public bool Contiene(Posicion posicion) => Cuadricula.EstaDentro(posicion, Ancho, Alto);
    }
}
