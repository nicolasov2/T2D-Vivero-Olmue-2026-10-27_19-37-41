using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>
    /// Reads every touch on the map and decides what it means: move the preview (1787), select a
    /// station (1687, 1766), show a seedling label (1686) or select a cell (1787).
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ControladorDeToques : MonoBehaviour, IParteDePartida
    {
        private Partida _partida;
        private ZonasDeInterfaz _zonas;
        private VistaMapa _mapa;

        /// <summary>UT 1687 and 1766. Station touched last; shows its radius and its panel.</summary>
        public Estacion EstacionSeleccionada { get; private set; }

        /// <summary>UT 1686. Seedling row touched last; shows its label.</summary>
        public PasilloDePlantines PasilloSeleccionado { get; private set; }

        /// <inheritdoc />
        public void Inicializar(Partida partida)
        {
            _partida = partida;
            _zonas = GetComponent<ZonasDeInterfaz>();
            _mapa = GetComponent<VistaMapa>();
        }

        /// <summary>Clears every selection, including the preview.</summary>
        public void LimpiarSeleccion()
        {
            EstacionSeleccionada = null;
            PasilloSeleccionado = null;
            _partida.Selector.Cancelar();
        }

        /// <summary>UT 1687. Called by the build panel: choosing a station hides the station panel.</summary>
        public void OlvidarEstacionSeleccionada() => EstacionSeleccionada = null;

        private void Update()
        {
            if (_partida == null || !LectorDePuntero.SePresionoEsteFrame())
            {
                return;
            }

            var pantalla = LectorDePuntero.PosicionEnPantalla();
            if (_zonas.Contiene(pantalla))
            {
                return;
            }

            if (_mapa.IntentarObtenerPosicion(pantalla, out var posicion))
            {
                Tocar(posicion);
                return;
            }

            LimpiarSeleccion();
        }

        private void Tocar(Posicion posicion)
        {
            var selector = _partida.Selector;
            EstacionSeleccionada = null;
            PasilloSeleccionado = null;
            if (selector.Estado is EstacionElegida || selector.Estado is Previsualizando)
            {
                selector.Seleccionar(posicion);
                return;
            }

            var estacion = _partida.Nivel.Construccion.EstacionEn(posicion);
            if (estacion != null)
            {
                selector.Cancelar();
                EstacionSeleccionada = estacion;
                return;
            }

            PasilloSeleccionado = _partida.Nivel.Pasillos.PasilloEn(posicion);
            selector.Seleccionar(posicion);
        }
    }
}
