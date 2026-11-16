using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1787. Yellow outline on the selected cell.</summary>
    [RequireComponent(typeof(Partida))]
    public sealed class VistaSeleccion : MonoBehaviour, IParteDePartida
    {
        [SerializeField] private Color color = new Color(0.95f, 0.79f, 0.3f);

        private Partida _partida;
        private VistaMapa _mapa;
        private SpriteRenderer _marco;

        /// <inheritdoc />
        public void Inicializar(Partida partida)
        {
            _partida = partida;
            _mapa = GetComponent<VistaMapa>();
            _marco = FabricaDeSprites.Crear(partida, "Casilla seleccionada", partida.SpriteMarco, OrdenDeDibujo.Marco);
            _marco.color = color;
        }

        private void LateUpdate()
        {
            if (_marco == null)
            {
                return;
            }

            var hayCasilla = IntentarObtenerCasilla(_partida.Selector.Estado, out var posicion);
            _marco.gameObject.SetActive(hayCasilla);
            if (hayCasilla)
            {
                _marco.transform.position = _mapa.CentroDe(posicion);
            }
        }

        private static bool IntentarObtenerCasilla(EstadoSeleccion estado, out Posicion posicion)
        {
            switch (estado)
            {
                case CasillaSeleccionada seleccionada:
                    posicion = seleccionada.Posicion;
                    return true;
                case Previsualizando previa:
                    posicion = previa.Posicion;
                    return true;
                default:
                    posicion = default;
                    return false;
            }
        }
    }
}
