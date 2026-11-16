#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>
    /// Cell coordinates on the nursery grid: column <see cref="X"/> grows to the right and
    /// row <see cref="Y"/> grows downwards, like the mockups. Independent from UnityEngine.
    /// </summary>
    public readonly struct Posicion : IEquatable<Posicion>
    {
        /// <summary>Creates the coordinates of a cell.</summary>
        public Posicion(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>Column, from 0 at the left edge.</summary>
        public int X { get; }

        /// <summary>Row, from 0 at the top edge.</summary>
        public int Y { get; }

        /// <summary>True when both cells share a side (diagonals do not count).</summary>
        public bool EsAdyacenteA(Posicion otra) => Math.Abs(X - otra.X) + Math.Abs(Y - otra.Y) == 1;

        /// <summary>Chebyshev distance: a cell touching this one, diagonals included, is at distance 1.</summary>
        public int DistanciaA(Posicion otra) => Math.Max(Math.Abs(X - otra.X), Math.Abs(Y - otra.Y));

        /// <inheritdoc />
        public bool Equals(Posicion otra) => X == otra.X && Y == otra.Y;

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is Posicion otra && Equals(otra);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(X, Y);

        /// <inheritdoc />
        public override string ToString() => $"({X}, {Y})";

        /// <summary>Value equality.</summary>
        public static bool operator ==(Posicion a, Posicion b) => a.Equals(b);

        /// <summary>Value inequality.</summary>
        public static bool operator !=(Posicion a, Posicion b) => !a.Equals(b);
    }
}
