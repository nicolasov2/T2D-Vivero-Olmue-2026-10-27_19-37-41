using System;
using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1687. Bottom build panel: one button per station with its price; the chosen one is marked.</summary>
    [RequireComponent(typeof(ControladorDeToques))]
    public sealed class PanelDeConstruccion : MonoBehaviour, IParteDePartida
    {
        private const float AltoPanel = 120f;
        private const float AnchoBoton = 230f;
        private const float AltoBoton = 64f;
        private const float Margen = 16f;

        [SerializeField] private EstacionDefinicionSO[] estaciones = Array.Empty<EstacionDefinicionSO>();
        [SerializeField] private Color colorElegida = new Color(0.18f, 0.5f, 0.98f);

        private Partida _partida;
        private ControladorDeToques _toques;
        private ZonasDeInterfaz _zonas;

        /// <inheritdoc />
        public void Inicializar(Partida partida)
        {
            _partida = partida;
            _toques = GetComponent<ControladorDeToques>();
            _zonas = GetComponent<ZonasDeInterfaz>();
        }

        private void OnGUI()
        {
            if (_partida == null)
            {
                return;
            }

            EstiloInterfaz.AplicarEscala();
            var panel = new Rect(0f, EstiloInterfaz.AltoReferencia - AltoPanel, EstiloInterfaz.AnchoVirtual, AltoPanel);
            _zonas.Registrar(panel);
            GUI.Box(panel, GUIContent.none, EstiloInterfaz.Caja);
            GUI.Label(new Rect(Margen, panel.y + 8f, 300f, 24f), "Construcción", EstiloInterfaz.Titulo);
            for (var i = 0; i < estaciones.Length; i++)
            {
                var boton = new Rect(Margen + (i * (AnchoBoton + Margen)), panel.y + 38f, AnchoBoton, AltoBoton);
                DibujarBoton(estaciones[i], boton);
            }
        }

        private void DibujarBoton(EstacionDefinicionSO estacion, Rect boton)
        {
            var definicion = estacion.Definicion;
            var colorAnterior = GUI.backgroundColor;
            if (EstaElegida(estacion))
            {
                GUI.backgroundColor = colorElegida;
            }

            if (GUI.Button(boton, $"{definicion.Nombre}\n{definicion.Costo}", EstiloInterfaz.Boton))
            {
                _toques.OlvidarEstacionSeleccionada();
                _partida.Selector.ElegirEstacion(definicion);
            }

            GUI.backgroundColor = colorAnterior;
        }

        private bool EstaElegida(EstacionDefinicionSO estacion) => _partida.Selector.Estado switch
        {
            EstacionElegida elegida => elegida.Definicion == estacion.Definicion,
            Previsualizando previa => previa.Definicion == estacion.Definicion,
            _ => false,
        };
    }
}
