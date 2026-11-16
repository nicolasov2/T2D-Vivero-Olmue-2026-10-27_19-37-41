using System;
using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1686 (species and seedlings) and 1690 (hydration). Label of the touched seedling row.</summary>
    [RequireComponent(typeof(ControladorDeToques))]
    public sealed class RotuloDePasillo : MonoBehaviour, IParteDePartida
    {
        private const float Ancho = 210f;
        private const float Alto = 62f;
        private const float Separacion = 12f;

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

        /// <summary>Plural name shown in the label.</summary>
        public static string NombreDe(Especie especie) => especie switch
        {
            Especie.Palto => "Paltos",
            Especie.Citrico => "Cítricos",
            _ => throw new ArgumentOutOfRangeException(nameof(especie), especie, "Especie sin nombre."),
        };

        private void OnGUI()
        {
            var pasillo = _toques == null ? null : _toques.PasilloSeleccionado;
            if (pasillo == null)
            {
                return;
            }

            EstiloInterfaz.AplicarEscala();
            var ancla = EstiloInterfaz.MundoAVirtual(_partida.Camara, GetComponent<VistaMapa>().CentroDe(pasillo.Casillas[0]));
            var rect = new Rect(ancla.x - (Ancho / 2f), ancla.y - Alto - Separacion, Ancho, Alto);
            _zonas.Registrar(rect);
            GUI.Box(rect, GUIContent.none, EstiloInterfaz.Caja);
            GUI.Label(new Rect(rect.x + 10f, rect.y + 6f, Ancho - 20f, 24f), $"{NombreDe(pasillo.Especie)} · {pasillo.CantidadDePlantines} plantines", EstiloInterfaz.Titulo);
            GUI.Label(new Rect(rect.x + 10f, rect.y + 32f, Ancho - 20f, 24f), $"Hidratación {pasillo.Hidratacion.Actual} %", EstiloInterfaz.Texto);
        }
    }
}
