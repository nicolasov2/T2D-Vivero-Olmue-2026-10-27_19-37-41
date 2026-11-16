#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace T2D.Dominio
{
    /// <summary>
    /// UT 1680. Ordered cells that stress events follow from an entrance on the edge to the silo.
    /// Every step moves to a cell that shares a side with the previous one.
    /// </summary>
    public sealed class Ruta
    {
        private const int PuntosMinimos = 2;
        private readonly Posicion[] _puntos;

        private Ruta(Posicion[] puntos) => _puntos = puntos;

        /// <summary>Cells in walking order.</summary>
        public IReadOnlyList<Posicion> Puntos => _puntos;

        /// <summary>First cell, where events appear.</summary>
        public Posicion Entrada => _puntos[0];

        /// <summary>Last cell, the silo.</summary>
        public Posicion Final => _puntos[_puntos.Length - 1];

        /// <summary>Validates the points against the map bounds and builds the route.</summary>
        public static Resultado<Ruta, string> Crear(IReadOnlyList<Posicion> puntos, LimitesDelMapa limites)
        {
            if (puntos.Count < PuntosMinimos)
            {
                return Resultado<Ruta, string>.Fallo($"La ruta necesita al menos {PuntosMinimos} puntos.");
            }

            var error = BuscarError(puntos, limites);
            return error.Length == 0
                ? Resultado<Ruta, string>.Exito(new Ruta(puntos.ToArray()))
                : Resultado<Ruta, string>.Fallo(error);
        }

        private static string BuscarError(IReadOnlyList<Posicion> puntos, LimitesDelMapa limites)
        {
            var visitadas = new HashSet<Posicion>();
            for (var i = 0; i < puntos.Count; i++)
            {
                var punto = puntos[i];
                if (!limites.Contiene(punto))
                {
                    return $"El punto {i + 1} {punto} está fuera del mapa.";
                }

                if (!visitadas.Add(punto))
                {
                    return $"La ruta pasa dos veces por {punto}.";
                }

                if (i > 0 && !puntos[i - 1].EsAdyacenteA(punto))
                {
                    return $"Los puntos {i} {puntos[i - 1]} y {i + 1} {punto} no son adyacentes.";
                }
            }

            return string.Empty;
        }
    }
}
