#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace T2D.Dominio
{
    /// <summary>UT 1689. Which stations a strong wind reaches.</summary>
    public static class DanioPorViento
    {
        /// <summary>Stations at a distance less than or equal to <paramref name="alcance"/> (diagonals included).</summary>
        public static IReadOnlyList<Estacion> EstacionesAlAlcance(Posicion posicion, int alcance, IEnumerable<Estacion> estaciones) =>
            estaciones.Where(estacion => estacion.Posicion.DistanciaA(posicion) <= alcance).ToList();
    }
}
