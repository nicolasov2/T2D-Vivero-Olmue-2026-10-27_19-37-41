#nullable enable
using System;
using System.Collections.Generic;

namespace T2D.Dominio
{
    /// <summary>The nursery as a rectangular grid of cells (base of UT 1698).</summary>
    public sealed class Cuadricula
    {
        private readonly Casilla[,] _casillas;

        /// <summary>Builds a grid asking <paramref name="tipoEn"/> for the type of every cell.</summary>
        public Cuadricula(int ancho, int alto, Func<Posicion, TipoCasilla> tipoEn)
        {
            if (ancho <= 0 || alto <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ancho), $"El mapa debe medir al menos 1x1 (recibido {ancho}x{alto}).");
            }

            Ancho = ancho;
            Alto = alto;
            _casillas = new Casilla[ancho, alto];
            for (var x = 0; x < ancho; x++)
            {
                for (var y = 0; y < alto; y++)
                {
                    var posicion = new Posicion(x, y);
                    _casillas[x, y] = new Casilla(posicion, tipoEn(posicion));
                }
            }
        }

        /// <summary>Number of columns.</summary>
        public int Ancho { get; }

        /// <summary>Number of rows.</summary>
        public int Alto { get; }

        /// <summary>True when the position lies inside the grid.</summary>
        public bool Contiene(Posicion posicion) => EstaDentro(posicion, Ancho, Alto);

        /// <summary>Cell at a position inside the grid. Asking outside the grid is a bug: check <see cref="Contiene"/> first.</summary>
        public Casilla Obtener(Posicion posicion)
        {
            if (!Contiene(posicion))
            {
                throw new ArgumentOutOfRangeException(nameof(posicion), $"La posición {posicion} está fuera del mapa de {Ancho}x{Alto}.");
            }

            return _casillas[posicion.X, posicion.Y];
        }

        /// <summary>Every cell, column by column.</summary>
        public IEnumerable<Casilla> Casillas()
        {
            for (var x = 0; x < Ancho; x++)
            {
                for (var y = 0; y < Alto; y++)
                {
                    yield return _casillas[x, y];
                }
            }
        }

        /// <summary>Bounds check shared with the level validation, before a grid exists.</summary>
        public static bool EstaDentro(Posicion posicion, int ancho, int alto) =>
            posicion.X >= 0 && posicion.Y >= 0 && posicion.X < ancho && posicion.Y < alto;
    }
}
