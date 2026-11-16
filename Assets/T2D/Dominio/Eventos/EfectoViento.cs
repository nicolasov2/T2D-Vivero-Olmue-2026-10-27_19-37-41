#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>UT 1689. A strong wind damages every station within reach of each cell it enters.</summary>
    public sealed class EfectoViento : IEfectoDeEvento
    {
        /// <summary>Reach in cells and integrity removed per cell.</summary>
        public EfectoViento(int alcance, int danioAEstaciones)
        {
            if (alcance < 0 || danioAEstaciones < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(alcance), "Alcance y daño del viento no pueden ser negativos.");
            }

            Alcance = alcance;
            DanioAEstaciones = danioAEstaciones;
        }

        /// <summary>Reach in cells.</summary>
        public int Alcance { get; }

        /// <summary>Integrity removed from each station in reach, per cell entered.</summary>
        public int DanioAEstaciones { get; }

        /// <inheritdoc />
        public int AlEntrarEnCasilla(Posicion casilla, Nivel nivel)
        {
            var alcanzadas = DanioPorViento.EstacionesAlAlcance(casilla, Alcance, nivel.Construccion.Estaciones);
            foreach (var estacion in alcanzadas)
            {
                estacion.Integridad.RecibirDanio(DanioAEstaciones);
            }

            return alcanzadas.Count;
        }
    }
}
