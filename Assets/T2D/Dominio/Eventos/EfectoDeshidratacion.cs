#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>UT 1690. A dehydration vector lowers the water level of every seedling row within reach of each cell it enters.</summary>
    public sealed class EfectoDeshidratacion : IEfectoDeEvento
    {
        /// <summary>Reach in cells and water removed per cell.</summary>
        public EfectoDeshidratacion(int alcance, int danioHidratacion)
        {
            if (alcance < 0 || danioHidratacion < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(alcance), "Alcance y daño de la deshidratación no pueden ser negativos.");
            }

            Alcance = alcance;
            DanioHidratacion = danioHidratacion;
        }

        /// <summary>Reach in cells.</summary>
        public int Alcance { get; }

        /// <summary>Hydration removed from each row in reach, per cell entered.</summary>
        public int DanioHidratacion { get; }

        /// <inheritdoc />
        public int AlEntrarEnCasilla(Posicion casilla, Nivel nivel)
        {
            var alcanzados = DanioPorDeshidratacion.PasillosAlAlcance(casilla, Alcance, nivel.Pasillos.Pasillos);
            foreach (var pasillo in alcanzados)
            {
                pasillo.Hidratacion.Deshidratar(DanioHidratacion);
            }

            return alcanzados.Count;
        }
    }
}
