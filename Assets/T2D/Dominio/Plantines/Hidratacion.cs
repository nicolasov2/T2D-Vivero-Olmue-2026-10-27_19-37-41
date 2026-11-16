#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>
    /// UT 1690. Water level of a seedling row, from 0 (dry) to <see cref="Maxima"/>.
    /// Full plant health and the Crop Water Stress Index arrive with UT 1691.
    /// </summary>
    public sealed class Hidratacion
    {
        /// <summary>Fully watered.</summary>
        public const int Maxima = 100;

        /// <summary>Raised when the level changes.</summary>
        public event Action? Cambio;

        /// <summary>Current level; never below 0.</summary>
        public int Actual { get; private set; } = Maxima;

        /// <summary>Level from 0 to 1.</summary>
        public float Fraccion => (float)Actual / Maxima;

        /// <summary>Lowers the level, stopping at 0. Returns the amount actually removed.</summary>
        public int Deshidratar(int cantidad)
        {
            if (cantidad < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cantidad), "La deshidratación no puede ser negativa.");
            }

            var descontada = Math.Min(cantidad, Actual);
            Actual -= descontada;
            if (descontada > 0)
            {
                Cambio?.Invoke();
            }

            return descontada;
        }
    }
}
