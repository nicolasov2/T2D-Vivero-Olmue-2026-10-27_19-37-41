#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>UT 1688. Configuration of a kind of stress event.</summary>
    public sealed class DefinicionEvento
    {
        /// <summary>Creates the definition. Speed must be positive and silo damage not negative.</summary>
        public DefinicionEvento(string nombre, float velocidad, int danioAlSilo, IEfectoDeEvento efecto)
        {
            if (velocidad <= 0f || danioAlSilo < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(velocidad), $"El evento \"{nombre}\" necesita velocidad > 0 y daño al silo >= 0.");
            }

            Nombre = nombre;
            Velocidad = velocidad;
            DanioAlSilo = danioAlSilo;
            Efecto = efecto;
        }

        /// <summary>Name for logs and the interface.</summary>
        public string Nombre { get; }

        /// <summary>Cells per second.</summary>
        public float Velocidad { get; }

        /// <summary>Water removed from the silo on arrival.</summary>
        public int DanioAlSilo { get; }

        /// <summary>What the event does on every cell it enters.</summary>
        public IEfectoDeEvento Efecto { get; }
    }
}
