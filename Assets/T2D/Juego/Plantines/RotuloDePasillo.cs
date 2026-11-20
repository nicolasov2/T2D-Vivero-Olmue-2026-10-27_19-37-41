using System;
using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>
    /// UT 1686 (species and seedlings) and 1690 (hydration). A chip with the hydration of every row,
    /// red with "Seco" once the row is dry, and the full label of the touched row.
    /// </summary>
    [RequireComponent(typeof(ControladorDeToques))]
    public sealed class RotuloDePasillo : MonoBehaviour, IParteDePartida
    {
        private const float Ancho = 210f;
        private const float Alto = 62f;
        private const float Separacion = 12f;
        private const float AltoChip = 22f;
        private const float AnchoChip = 56f;
        private const float AnchoChipSeco = 112f;
        private const float SeparacionChip = 2f;

        private static readonly Color ColorSeco = new Color(0.85f, 0.3f, 0.2f);

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
            if (_partida == null)
            {
                return;
            }

            EstiloInterfaz.AplicarEscala();
            var mapa = GetComponent<VistaMapa>();
            foreach (var cadaPasillo in _partida.Nivel.Pasillos.Pasillos)
            {
                DibujarChip(cadaPasillo, mapa);
            }

            var pasillo = _toques.PasilloSeleccionado;
            if (pasillo == null)
            {
                return;
            }

            var ancla = EstiloInterfaz.MundoAVirtual(_partida.Camara, mapa.CentroDe(pasillo.Casillas[0]));
            var rect = new Rect(ancla.x - (Ancho / 2f), ancla.y - Alto - Separacion, Ancho, Alto);
            _zonas.Registrar(rect);
            GUI.Box(rect, GUIContent.none, EstiloInterfaz.Caja);
            GUI.Label(new Rect(rect.x + 10f, rect.y + 6f, Ancho - 20f, 24f), $"{NombreDe(pasillo.Especie)} · {pasillo.CantidadDePlantines} plantines", EstiloInterfaz.Titulo);
            GUI.Label(new Rect(rect.x + 10f, rect.y + 32f, Ancho - 20f, 24f), $"Hidratación {pasillo.Hidratacion.Actual} %", EstiloInterfaz.Texto);
        }

        private void DibujarChip(PasilloDePlantines pasillo, VistaMapa mapa)
        {
            var hidratacion = pasillo.Hidratacion;
            var ancho = hidratacion.EstaSeca ? AnchoChipSeco : AnchoChip;
            var ancla = AnclaSobre(pasillo, mapa);
            var rect = new Rect(ancla.x - (ancho / 2f), ancla.y - AltoChip - SeparacionChip, ancho, AltoChip);
            var texto = hidratacion.EstaSeca ? $"Seco · {hidratacion.Actual} %" : $"{hidratacion.Actual} %";
            var fondoAnterior = GUI.backgroundColor;
            GUI.backgroundColor = hidratacion.EstaSeca ? ColorSeco : fondoAnterior;
            GUI.Box(rect, texto, EstiloInterfaz.Chip);
            GUI.backgroundColor = fondoAnterior;
        }

        // Middle of the row's top edge, in virtual GUI units.
        private Vector2 AnclaSobre(PasilloDePlantines pasillo, VistaMapa mapa)
        {
            var primera = mapa.CentroDe(pasillo.Casillas[0]);
            var ultima = mapa.CentroDe(pasillo.Casillas[pasillo.Casillas.Count - 1]);
            var borde = Mathf.Max(primera.y, ultima.y) + (_partida.Grid.cellSize.y / 2f);
            return EstiloInterfaz.MundoAVirtual(_partida.Camara, new Vector3((primera.x + ultima.x) / 2f, borde, primera.z));
        }
    }
}
