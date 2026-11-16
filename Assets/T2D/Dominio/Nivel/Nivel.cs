#nullable enable
using System.Collections.Generic;

namespace T2D.Dominio
{
    /// <summary>A validated, playable level: map, routes, seedling rows, silo, credits and stations.</summary>
    public sealed class Nivel
    {
        internal Nivel(string nombre, Cuadricula cuadricula, IReadOnlyList<Ruta> rutas, RegistroDePasillos pasillos, Silo silo, Creditos creditos)
        {
            Nombre = nombre;
            Cuadricula = cuadricula;
            Rutas = rutas;
            Pasillos = pasillos;
            Silo = silo;
            Creditos = creditos;
            Construccion = new ServicioDeConstruccion(cuadricula, creditos);
        }

        /// <summary>Level name.</summary>
        public string Nombre { get; }

        /// <summary>The nursery grid.</summary>
        public Cuadricula Cuadricula { get; }

        /// <summary>UT 1680. Routes of the stress events.</summary>
        public IReadOnlyList<Ruta> Rutas { get; }

        /// <summary>UT 1686. Seedling rows.</summary>
        public RegistroDePasillos Pasillos { get; }

        /// <summary>Central water tank.</summary>
        public Silo Silo { get; }

        /// <summary>Player balance.</summary>
        public Creditos Creditos { get; }

        /// <summary>UT 1687 and 1766. Places and dismantles stations.</summary>
        public ServicioDeConstruccion Construccion { get; }
    }
}
