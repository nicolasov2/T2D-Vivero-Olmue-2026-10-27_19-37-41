#nullable enable
using System;
using System.Collections.Generic;

namespace T2D.Dominio
{
    /// <summary>UT 1686. A seedling row as configured in the level: a rectangle of cells and its species.</summary>
    public sealed class DatosDePasillo
    {
        /// <summary>Rectangle that starts at <paramref name="desde"/> (top-left cell).</summary>
        public DatosDePasillo(Especie especie, Posicion desde, int ancho, int alto)
        {
            if (ancho <= 0 || alto <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ancho), $"Un pasillo debe medir al menos 1x1 (recibido {ancho}x{alto}).");
            }

            Especie = especie;
            Desde = desde;
            Ancho = ancho;
            Alto = alto;
        }

        /// <summary>Species of every seedling in the row.</summary>
        public Especie Especie { get; }

        /// <summary>Top-left cell.</summary>
        public Posicion Desde { get; }

        /// <summary>Width in cells.</summary>
        public int Ancho { get; }

        /// <summary>Height in cells.</summary>
        public int Alto { get; }

        /// <summary>Every cell covered by the rectangle.</summary>
        public IReadOnlyList<Posicion> Casillas()
        {
            var casillas = new List<Posicion>(Ancho * Alto);
            for (var x = Desde.X; x < Desde.X + Ancho; x++)
            {
                for (var y = Desde.Y; y < Desde.Y + Alto; y++)
                {
                    casillas.Add(new Posicion(x, y));
                }
            }

            return casillas;
        }
    }
}
