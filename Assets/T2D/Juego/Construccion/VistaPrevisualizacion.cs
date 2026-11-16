using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>
    /// UT 1678. Ghost of the station being placed: green on a valid cell, red on an invalid one, always
    /// with the reason in text so color is not the only cue.
    /// </summary>
    [RequireComponent(typeof(ZonasDeInterfaz))]
    public sealed class VistaPrevisualizacion : MonoBehaviour, IParteDePartida
    {
        private const float AnchoTexto = 280f;
        private const float AltoTexto = 30f;

        [SerializeField] private Color colorValida = new Color(0.18f, 0.66f, 0.31f, 0.6f);
        [SerializeField] private Color colorInvalida = new Color(0.9f, 0.2f, 0.18f, 0.6f);

        private Partida _partida;
        private VistaMapa _mapa;
        private ZonasDeInterfaz _zonas;
        private SpriteRenderer _fantasma;
        private bool _visible;
        private Posicion _posicion;
        private ResultadoColocacion _resultado;

        /// <inheritdoc />
        public void Inicializar(Partida partida)
        {
            _partida = partida;
            _mapa = GetComponent<VistaMapa>();
            _zonas = GetComponent<ZonasDeInterfaz>();
            _fantasma = FabricaDeSprites.Crear(partida, "Previsualización", partida.SpriteCuadrado, OrdenDeDibujo.Previsualizacion);
            _fantasma.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
        }

        private void Update()
        {
            if (_fantasma == null)
            {
                return;
            }

            _visible = IntentarObtenerObjetivo(out _posicion, out var definicion);
            _fantasma.gameObject.SetActive(_visible);
            if (!_visible)
            {
                return;
            }

            _resultado = _partida.Nivel.Construccion.Validar(definicion, _posicion);
            _fantasma.color = _resultado == ResultadoColocacion.Valida ? colorValida : colorInvalida;
            _fantasma.transform.position = _mapa.CentroDe(_posicion);
        }

        private bool IntentarObtenerObjetivo(out Posicion posicion, out DefinicionEstacion definicion)
        {
            switch (_partida.Selector.Estado)
            {
                case Previsualizando previa:
                    posicion = previa.Posicion;
                    definicion = previa.Definicion;
                    return true;
                case EstacionElegida elegida:
                    definicion = elegida.Definicion;
                    var pantalla = LectorDePuntero.PosicionEnPantalla();
                    return _mapa.IntentarObtenerPosicion(pantalla, out posicion) && !_zonas.Contiene(pantalla);
                default:
                    posicion = default;
                    definicion = null;
                    return false;
            }
        }

        private void OnGUI()
        {
            if (!_visible)
            {
                return;
            }

            EstiloInterfaz.AplicarEscala();
            var ancla = EstiloInterfaz.MundoAVirtual(_partida.Camara, _mapa.CentroDe(_posicion));
            var rect = new Rect(ancla.x - (AnchoTexto / 2f), ancla.y - 70f, AnchoTexto, AltoTexto);
            var colorAnterior = GUI.backgroundColor;
            GUI.backgroundColor = _resultado == ResultadoColocacion.Valida ? colorValida : colorInvalida;
            GUI.Box(rect, _resultado.Mensaje(), EstiloInterfaz.Boton);
            GUI.backgroundColor = colorAnterior;
        }
    }
}
