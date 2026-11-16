using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1687. Semi-transparent disk with the coverage radius of the selected station.</summary>
    [RequireComponent(typeof(ControladorDeToques))]
    public sealed class VistaRadioCobertura : MonoBehaviour, IParteDePartida
    {
        [SerializeField] private Color color = new Color(0.18f, 0.5f, 0.98f, 0.15f);

        private ControladorDeToques _toques;
        private VistaMapa _mapa;
        private SpriteRenderer _disco;

        /// <inheritdoc />
        public void Inicializar(Partida partida)
        {
            _toques = GetComponent<ControladorDeToques>();
            _mapa = GetComponent<VistaMapa>();
            _disco = FabricaDeSprites.Crear(partida, "Radio de cobertura", partida.SpriteCirculo, OrdenDeDibujo.Radio);
            _disco.color = color;
        }

        private void LateUpdate()
        {
            if (_disco == null)
            {
                return;
            }

            var estacion = _toques.EstacionSeleccionada;
            _disco.gameObject.SetActive(estacion != null);
            if (estacion == null)
            {
                return;
            }

            var diametro = (estacion.Definicion.RadioCobertura * 2f) + 1f;
            _disco.transform.position = _mapa.CentroDe(estacion.Posicion);
            _disco.transform.localScale = new Vector3(diametro, diametro, 1f);
        }
    }
}
