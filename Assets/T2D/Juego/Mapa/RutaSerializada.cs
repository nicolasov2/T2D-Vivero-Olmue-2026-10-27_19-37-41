using System;
using System.Collections.Generic;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1680. A route as edited in the inspector: cells from the entrance to the silo (x = column, y = row from the top).</summary>
    [Serializable]
    public sealed class RutaSerializada
    {
        [SerializeField] private List<Vector2Int> puntos = new List<Vector2Int>();

        /// <summary>Empty route, required by the Unity serializer.</summary>
        public RutaSerializada()
        {
        }

        /// <summary>Route with the given points.</summary>
        public RutaSerializada(IEnumerable<Vector2Int> puntos) => this.puntos = new List<Vector2Int>(puntos);

        /// <summary>Cells in walking order.</summary>
        public IReadOnlyList<Vector2Int> Puntos => puntos;
    }
}
