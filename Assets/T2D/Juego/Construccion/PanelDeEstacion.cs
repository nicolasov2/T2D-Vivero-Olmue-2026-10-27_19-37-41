using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1766. Panel of the selected station: integrity, refund and the Dismantle button.</summary>
    [RequireComponent(typeof(ControladorDeToques), typeof(MensajeDeRechazo))]
    public sealed class PanelDeEstacion : MonoBehaviour, IParteDePartida
    {
        private const float Ancho = 290f;
        private const float Alto = 132f;
        private const float Separacion = 40f;

        private Partida _partida;
        private VistaMapa _mapa;
        private ZonasDeInterfaz _zonas;
        private ControladorDeToques _toques;
        private MensajeDeRechazo _mensaje;

        /// <inheritdoc />
        public void Inicializar(Partida partida)
        {
            _partida = partida;
            _mapa = GetComponent<VistaMapa>();
            _zonas = GetComponent<ZonasDeInterfaz>();
            _toques = GetComponent<ControladorDeToques>();
            _mensaje = GetComponent<MensajeDeRechazo>();
        }

        private void OnGUI()
        {
            var estacion = _toques == null ? null : _toques.EstacionSeleccionada;
            if (estacion == null)
            {
                return;
            }

            EstiloInterfaz.AplicarEscala();
            var ancla = EstiloInterfaz.MundoAVirtual(_partida.Camara, _mapa.CentroDe(estacion.Posicion));
            var rect = new Rect(ancla.x + Separacion, ancla.y - (Alto / 2f), Ancho, Alto);
            _zonas.Registrar(rect);
            GUI.Box(rect, GUIContent.none, EstiloInterfaz.Caja);
            var reembolso = CalculadoraDeReembolso.Reembolso(estacion.Definicion.Costo);
            GUI.Label(new Rect(rect.x + 14f, rect.y + 8f, Ancho - 28f, 24f), estacion.Definicion.Nombre, EstiloInterfaz.Titulo);
            GUI.Label(new Rect(rect.x + 14f, rect.y + 34f, Ancho - 28f, 22f), $"Integridad {estacion.Integridad.Actual}/{estacion.Integridad.Maxima}", EstiloInterfaz.Texto);
            GUI.Label(new Rect(rect.x + 14f, rect.y + 56f, Ancho - 28f, 22f), $"Recuperas: {reembolso}", EstiloInterfaz.Texto);
            if (GUI.Button(new Rect(rect.x + 14f, rect.y + 84f, 160f, 36f), "Desmantelar", EstiloInterfaz.Boton))
            {
                Desmantelar(estacion);
            }
        }

        private void Desmantelar(Estacion estacion)
        {
            var resultado = _partida.Nivel.Construccion.Desmantelar(estacion);
            _mensaje.Mostrar(resultado.EsExito ? $"Recuperaste {resultado.Valor}" : resultado.Error);
            _toques.LimpiarSeleccion();
        }
    }
}
