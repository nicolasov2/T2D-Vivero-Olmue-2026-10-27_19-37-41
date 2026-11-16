#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace T2D.Dominio
{
    /// <summary>UT 1690. Which seedling rows a dehydration vector reaches.</summary>
    public static class DanioPorDeshidratacion
    {
        /// <summary>Rows with at least one cell at a distance less than or equal to <paramref name="alcance"/>.</summary>
        public static IReadOnlyList<PasilloDePlantines> PasillosAlAlcance(Posicion posicion, int alcance, IEnumerable<PasilloDePlantines> pasillos) =>
            pasillos.Where(pasillo => pasillo.DistanciaA(posicion) <= alcance).ToList();
    }
}
