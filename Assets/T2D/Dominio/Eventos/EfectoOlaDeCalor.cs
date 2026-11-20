#nullable enable
namespace T2D.Dominio
{
    /// <summary>
    /// UT 1688. In Sprint 1 the heat wave only drains the silo when it arrives: its effect on
    /// plants (1691) and its neutralisation by micro-sprinklers (1753) come later.
    /// </summary>
    public sealed class EfectoOlaDeCalor : IEfectoDeEvento
    {
        /// <inheritdoc />
        public int Alcance => 0;

        /// <inheritdoc />
        public int AlEntrarEnCasilla(Posicion casilla, Nivel nivel) => 0;
    }
}
