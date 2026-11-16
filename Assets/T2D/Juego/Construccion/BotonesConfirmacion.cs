using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1787. Confirm and Cancel buttons under the previewed station.</summary>
    [RequireComponent(typeof(MensajeDeRechazo))]
    public sealed class BotonesConfirmacion : MonoBehaviour, IParteDePartida
    {
        private const float AnchoBoton = 110f;
        private const float AltoBoton = 36f;
        private const float Separacion = 8f;
        private const float DistanciaBajoCasilla = 40f;

        private Partida _partida;
        private VistaMapa _mapa;
        private ZonasDeInterfaz _zonas;
        private MensajeDeRechazo _mensaje;

        /// <inheritdoc />
        public void Inicializar(Partida partida)
        {
            _partida = partida;
            _mapa = GetComponent<VistaMapa>();
            _zonas = GetComponent<ZonasDeInterfaz>();
            _mensaje = GetComponent<MensajeDeRechazo>();
        }

        private void OnGUI()
        {
            if (_partida == null || _partida.Selector.Estado is not Previsualizando previa)
            {
                return;
            }

            EstiloInterfaz.AplicarEscala();
            var ancla = EstiloInterfaz.MundoAVirtual(_partida.Camara, _mapa.CentroDe(previa.Posicion));
            var zona = new Rect(ancla.x - AnchoBoton - (Separacion / 2f), ancla.y + DistanciaBajoCasilla, (AnchoBoton * 2f) + Separacion, AltoBoton);
            _zonas.Registrar(zona);
            if (GUI.Button(new Rect(zona.x, zona.y, AnchoBoton, AltoBoton), "Confirmar", EstiloInterfaz.Boton))
            {
                Confirmar();
            }

            if (GUI.Button(new Rect(zona.x + AnchoBoton + Separacion, zona.y, AnchoBoton, AltoBoton), "Cancelar", EstiloInterfaz.Boton))
            {
                _partida.Selector.Cancelar();
            }
        }

        private void Confirmar()
        {
            var resultado = _partida.Selector.Confirmar(_partida.Nivel.Construccion);
            _mensaje.Mostrar(resultado.EsExito ? $"{resultado.Valor.Definicion.Nombre} instalada" : resultado.Error.Mensaje());
        }
    }
}
