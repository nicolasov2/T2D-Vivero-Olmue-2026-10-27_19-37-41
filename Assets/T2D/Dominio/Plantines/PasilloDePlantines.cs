#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace T2D.Dominio
{
    /// <summary>UT 1686. A row of palto or citrus seedlings that the player protects. One seedling per cell.</summary>
    public sealed class PasilloDePlantines
    {
        private readonly HashSet<Posicion> _casillas;

        /// <summary>Creates a row covering the given cells.</summary>
        public PasilloDePlantines(Especie especie, IReadOnlyList<Posicion> casillas)
        {
            if (casillas.Count == 0)
            {
                throw new ArgumentException("Un pasillo necesita al menos una casilla.", nameof(casillas));
            }

            Especie = especie;
            Casillas = casillas.ToArray();
            _casillas = new HashSet<Posicion>(casillas);
        }

        /// <summary>Species of the seedlings.</summary>
        public Especie Especie { get; }

        /// <summary>Cells of the row.</summary>
        public IReadOnlyList<Posicion> Casillas { get; }

        /// <summary>Number of seedlings shown in the label.</summary>
        public int CantidadDePlantines => Casillas.Count;

        /// <summary>UT 1690. Water level of the whole row.</summary>
        public Hidratacion Hidratacion { get; } = new Hidratacion();

        /// <summary>True when the cell belongs to the row.</summary>
        public bool Contiene(Posicion posicion) => _casillas.Contains(posicion);

        /// <summary>Distance from the closest cell of the row to <paramref name="posicion"/>.</summary>
        public int DistanciaA(Posicion posicion) => Casillas.Min(casilla => casilla.DistanciaA(posicion));
    }
}
