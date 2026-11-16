using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>
    /// Conversion between domain cells (row 0 at the top) and Unity tilemap cells (row 0 at the bottom).
    /// </summary>
    public static class Coordenadas
    {
        /// <summary>Domain cell to tilemap cell.</summary>
        public static Vector3Int ACelda(Posicion posicion, int alto) => new Vector3Int(posicion.X, alto - 1 - posicion.Y, 0);

        /// <summary>Inspector cell (x, row from the top) to tilemap cell.</summary>
        public static Vector3Int ACelda(Vector2Int celda, int alto) => new Vector3Int(celda.x, alto - 1 - celda.y, 0);

        /// <summary>Tilemap cell to domain cell.</summary>
        public static Posicion APosicion(Vector3Int celda, int alto) => new Posicion(celda.x, alto - 1 - celda.y);
    }
}
