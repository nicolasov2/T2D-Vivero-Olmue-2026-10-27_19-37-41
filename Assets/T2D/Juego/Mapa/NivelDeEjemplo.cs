using System.Collections.Generic;
using System.Linq;
using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>The 16x9 Olmué nursery drawn in the Sprint 1 mockups, used as the default level.</summary>
    public static class NivelDeEjemplo
    {
        private static readonly int[,] PuntosDeRuta =
        {
            { 0, 4 }, { 1, 4 }, { 2, 4 }, { 3, 4 }, { 3, 3 }, { 3, 2 }, { 4, 2 }, { 5, 2 }, { 6, 2 }, { 7, 2 }, { 8, 2 }, { 8, 3 },
            { 8, 4 }, { 8, 5 }, { 8, 6 }, { 9, 6 }, { 10, 6 }, { 11, 6 }, { 12, 6 }, { 13, 6 }, { 13, 5 }, { 13, 4 }, { 14, 4 }, { 15, 4 },
        };

        /// <summary>One route from the west edge to the silo at (15, 4).</summary>
        public static List<RutaSerializada> Rutas()
        {
            var puntos = Enumerable.Range(0, PuntosDeRuta.GetLength(0)).Select(i => new Vector2Int(PuntosDeRuta[i, 0], PuntosDeRuta[i, 1]));
            return new List<RutaSerializada> { new RutaSerializada(puntos) };
        }

        /// <summary>Five rows of palto and citrus seedlings.</summary>
        public static List<PasilloSerializado> Pasillos() => new List<PasilloSerializado>
        {
            new PasilloSerializado(Especie.Palto, new Vector2Int(5, 4), new Vector2Int(3, 2)),
            new PasilloSerializado(Especie.Citrico, new Vector2Int(10, 2), new Vector2Int(3, 2)),
            new PasilloSerializado(Especie.Palto, new Vector2Int(1, 6), new Vector2Int(2, 2)),
            new PasilloSerializado(Especie.Citrico, new Vector2Int(14, 0), new Vector2Int(2, 2)),
            new PasilloSerializado(Especie.Palto, new Vector2Int(10, 8), new Vector2Int(3, 1)),
        };

        /// <summary>The shed and four trees.</summary>
        public static List<Vector2Int> Bloqueadas() => new List<Vector2Int>
        {
            new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(5, 8), new Vector2Int(6, 8), new Vector2Int(14, 8), new Vector2Int(15, 8),
        };
    }
}
