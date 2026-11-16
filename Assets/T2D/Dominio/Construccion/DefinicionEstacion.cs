#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>UT 1687. Static data of a kind of station (for example, the fixed SBC camera).</summary>
    public sealed class DefinicionEstacion
    {
        /// <summary>Creates the definition. Radius and integrity must be positive.</summary>
        public DefinicionEstacion(string nombre, Costo costo, int radioCobertura, int integridadMaxima)
        {
            if (radioCobertura <= 0 || integridadMaxima <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radioCobertura), $"La estación \"{nombre}\" necesita radio e integridad mayores que 0.");
            }

            Nombre = nombre;
            Costo = costo;
            RadioCobertura = radioCobertura;
            IntegridadMaxima = integridadMaxima;
        }

        /// <summary>Name shown in the build panel.</summary>
        public string Nombre { get; }

        /// <summary>Price in water and energy.</summary>
        public Costo Costo { get; }

        /// <summary>Coverage radius in cells.</summary>
        public int RadioCobertura { get; }

        /// <summary>UT 1689. Integrity when new.</summary>
        public int IntegridadMaxima { get; }
    }
}
