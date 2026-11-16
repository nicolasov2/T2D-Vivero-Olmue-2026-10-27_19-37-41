#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>
    /// Farmer's central water tank at the end of every route. Minimal version of UT 1750 so that
    /// stress events (1688, 1689, 1690) can drain it.
    /// </summary>
    public sealed class Silo
    {
        /// <summary>Creates a full silo.</summary>
        public Silo(Posicion posicion, int capacidad)
        {
            if (capacidad <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacidad), "La capacidad del silo debe ser mayor que 0.");
            }

            Posicion = posicion;
            Capacidad = capacidad;
            Agua = capacidad;
        }

        /// <summary>Raised when the stored water changes.</summary>
        public event Action? Cambio;

        /// <summary>Cell of the silo.</summary>
        public Posicion Posicion { get; }

        /// <summary>Maximum water.</summary>
        public int Capacidad { get; }

        /// <summary>Water left; never below 0.</summary>
        public int Agua { get; private set; }

        /// <summary>True when no water is left.</summary>
        public bool EstaAgotado => Agua == 0;

        /// <summary>Water left from 0 to 1.</summary>
        public float Fraccion => (float)Agua / Capacidad;

        /// <summary>Removes water, stopping at 0. Returns the water actually removed.</summary>
        public int RecibirDanio(int agua)
        {
            if (agua < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(agua), "El daño no puede ser negativo.");
            }

            var descontada = Math.Min(agua, Agua);
            Agua -= descontada;
            if (descontada > 0)
            {
                Cambio?.Invoke();
            }

            return descontada;
        }
    }
}
