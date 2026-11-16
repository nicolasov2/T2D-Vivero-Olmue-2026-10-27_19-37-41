#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace T2D.Dominio
{
    /// <summary>UT 1686. Every seedling row of a level, validated against the map.</summary>
    public sealed class RegistroDePasillos
    {
        private readonly PasilloDePlantines[] _pasillos;

        private RegistroDePasillos(PasilloDePlantines[] pasillos) => _pasillos = pasillos;

        /// <summary>Rows of the level.</summary>
        public IReadOnlyList<PasilloDePlantines> Pasillos => _pasillos;

        /// <summary>
        /// Builds the rows, rejecting cells outside the map, cells already used by a route, the silo
        /// or a blocked cell (<paramref name="reservadas"/>) and rows that overlap.
        /// </summary>
        public static Resultado<RegistroDePasillos, string> Crear(
            IReadOnlyList<DatosDePasillo> datos, LimitesDelMapa limites, IReadOnlyCollection<Posicion> reservadas)
        {
            var usadas = new HashSet<Posicion>(reservadas);
            var pasillos = new List<PasilloDePlantines>(datos.Count);
            for (var i = 0; i < datos.Count; i++)
            {
                var casillas = datos[i].Casillas();
                var error = BuscarError(casillas, limites, usadas, i + 1);
                if (error.Length > 0)
                {
                    return Resultado<RegistroDePasillos, string>.Fallo(error);
                }

                usadas.UnionWith(casillas);
                pasillos.Add(new PasilloDePlantines(datos[i].Especie, casillas));
            }

            return Resultado<RegistroDePasillos, string>.Exito(new RegistroDePasillos(pasillos.ToArray()));
        }

        /// <summary>Row that contains the cell; <c>null</c> when the cell has no seedlings.</summary>
        public PasilloDePlantines? PasilloEn(Posicion posicion) =>
            _pasillos.FirstOrDefault(pasillo => pasillo.Contiene(posicion));

        private static string BuscarError(IReadOnlyList<Posicion> casillas, LimitesDelMapa limites, HashSet<Posicion> usadas, int numero)
        {
            foreach (var casilla in casillas)
            {
                if (!limites.Contiene(casilla))
                {
                    return $"El pasillo {numero} tiene la casilla {casilla} fuera del mapa.";
                }

                if (usadas.Contains(casilla))
                {
                    return $"El pasillo {numero} usa la casilla {casilla}, que ya es ruta, silo, bloqueada u otro pasillo.";
                }
            }

            return string.Empty;
        }
    }
}
