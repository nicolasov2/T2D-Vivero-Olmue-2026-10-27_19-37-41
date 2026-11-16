#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>UT 1689. How much damage a station can still take before going out of service.</summary>
    public sealed class Integridad
    {
        /// <summary>Starts undamaged.</summary>
        public Integridad(int maxima)
        {
            if (maxima <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxima), "La integridad máxima debe ser mayor que 0.");
            }

            Maxima = maxima;
            Actual = maxima;
        }

        /// <summary>Raised when the integrity changes.</summary>
        public event Action? Cambio;

        /// <summary>Integrity when new.</summary>
        public int Maxima { get; }

        /// <summary>Integrity left; never below 0.</summary>
        public int Actual { get; private set; }

        /// <summary>True once integrity reaches 0.</summary>
        public bool EstaFueraDeServicio => Actual == 0;

        /// <summary>Takes damage, stopping at 0. Returns the damage actually taken.</summary>
        public int RecibirDanio(int danio)
        {
            if (danio < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(danio), "El daño no puede ser negativo.");
            }

            var recibido = Math.Min(danio, Actual);
            Actual -= recibido;
            if (recibido > 0)
            {
                Cambio?.Invoke();
            }

            return recibido;
        }
    }
}
