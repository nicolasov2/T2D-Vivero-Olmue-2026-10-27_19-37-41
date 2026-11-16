using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1680. Draws an object where its <see cref="RecorridoDeRuta"/> is, between cells, every frame.</summary>
    [DisallowMultipleComponent]
    public sealed class SeguidorDeRuta : MonoBehaviour
    {
        private RecorridoDeRuta _recorrido;
        private VistaMapa _mapa;

        /// <summary>Starts following a route walk and snaps to its current position.</summary>
        public void Seguir(RecorridoDeRuta recorrido, VistaMapa mapa)
        {
            _recorrido = recorrido;
            _mapa = mapa;
            ActualizarPosicion();
        }

        private void LateUpdate()
        {
            if (_recorrido != null)
            {
                ActualizarPosicion();
            }
        }

        private void ActualizarPosicion()
        {
            var (x, y) = _recorrido.PosicionInterpolada;
            transform.position = _mapa.CentroDe(x, y);
        }
    }
}
