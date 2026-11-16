#nullable enable
namespace T2D.Dominio
{
    /// <summary>
    /// UT 1688. What a kind of stress event does while it walks the route. Each kind lives in its
    /// own class (heat wave 1688, wind 1689, dehydration 1690) so each UT has its own file.
    /// </summary>
    public interface IEfectoDeEvento
    {
        /// <summary>Applies the effect when the event enters a cell. Returns how many things it affected.</summary>
        int AlEntrarEnCasilla(Posicion casilla, Nivel nivel);
    }
}
