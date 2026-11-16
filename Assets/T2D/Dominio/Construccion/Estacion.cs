#nullable enable
namespace T2D.Dominio
{
    /// <summary>UT 1687. A station standing on a cell. A dedicated SBC camera class arrives when it gets its own behaviour (1752).</summary>
    public sealed class Estacion
    {
        internal Estacion(DefinicionEstacion definicion, Posicion posicion)
        {
            Definicion = definicion;
            Posicion = posicion;
            Integridad = new Integridad(definicion.IntegridadMaxima);
        }

        /// <summary>Kind of station.</summary>
        public DefinicionEstacion Definicion { get; }

        /// <summary>Cell it stands on.</summary>
        public Posicion Posicion { get; }

        /// <summary>UT 1689. Damage taken from strong winds.</summary>
        public Integridad Integridad { get; }

        /// <summary>UT 1689. False once integrity reaches 0.</summary>
        public bool EstaOperativa => !Integridad.EstaFueraDeServicio;
    }
}
