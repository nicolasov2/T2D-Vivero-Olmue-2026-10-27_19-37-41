#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>An amount of water and energy credits (a price, a refund or a starting balance).</summary>
    public readonly struct Costo : IEquatable<Costo>
    {
        /// <summary>Creates an amount. Negative amounts are a bug.</summary>
        public Costo(int agua, int energia)
        {
            if (agua < 0 || energia < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(agua), $"Un costo no puede ser negativo (agua {agua}, energía {energia}).");
            }

            Agua = agua;
            Energia = energia;
        }

        /// <summary>Water credits.</summary>
        public int Agua { get; }

        /// <summary>Energy credits.</summary>
        public int Energia { get; }

        /// <inheritdoc />
        public bool Equals(Costo otro) => Agua == otro.Agua && Energia == otro.Energia;

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is Costo otro && Equals(otro);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Agua, Energia);

        /// <summary>Text for the interface, for example "60 agua · 40 energía".</summary>
        public override string ToString() => $"{Agua} agua · {Energia} energía";
    }
}
