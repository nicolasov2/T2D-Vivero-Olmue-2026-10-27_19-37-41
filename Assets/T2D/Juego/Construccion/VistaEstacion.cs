using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1687. Visual of a station placed on the map.</summary>
    [DisallowMultipleComponent]
    public sealed class VistaEstacion : MonoBehaviour
    {
        /// <summary>Domain station drawn by this object.</summary>
        public Estacion Estacion { get; private set; }

        /// <summary>Binds the view to its station and places it on its cell.</summary>
        public void Inicializar(Estacion estacion, VistaMapa mapa)
        {
            Estacion = estacion;
            transform.position = mapa.CentroDe(estacion.Posicion);
        }
    }
}
