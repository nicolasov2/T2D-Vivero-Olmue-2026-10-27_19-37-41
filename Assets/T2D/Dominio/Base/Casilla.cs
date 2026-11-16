#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>One cell of the nursery grid.</summary>
    public sealed class Casilla
    {
        /// <summary>Creates a free cell of the given type.</summary>
        public Casilla(Posicion posicion, TipoCasilla tipo)
        {
            Posicion = posicion;
            Tipo = tipo;
        }

        /// <summary>Where the cell is.</summary>
        public Posicion Posicion { get; }

        /// <summary>What the cell is made of.</summary>
        public TipoCasilla Tipo { get; }

        /// <summary>True while a station stands on the cell.</summary>
        public bool EstaOcupada { get; private set; }

        /// <summary>True when stations can be built on this kind of cell.</summary>
        public bool EsEdificable => Tipo == TipoCasilla.SueloEdificable;

        /// <summary>Marks the cell as taken. Callers validate first, so a taken cell here is a bug.</summary>
        public void Ocupar()
        {
            if (EstaOcupada)
            {
                throw new InvalidOperationException($"La casilla {Posicion} ya está ocupada.");
            }

            EstaOcupada = true;
        }

        /// <summary>Frees the cell. Freeing a free cell is a bug.</summary>
        public void Liberar()
        {
            if (!EstaOcupada)
            {
                throw new InvalidOperationException($"La casilla {Posicion} ya está libre.");
            }

            EstaOcupada = false;
        }
    }
}
