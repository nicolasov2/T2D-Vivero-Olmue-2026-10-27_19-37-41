using System;
using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>
    /// UT 1688. Pooled visual of a stress event: advances the domain event every frame and returns to
    /// the pool when it reaches the silo. A faint disc shows the reach of its effect (1689, 1690).
    /// </summary>
    [RequireComponent(typeof(SeguidorDeRuta), typeof(SpriteRenderer))]
    public sealed class VistaEventoEstres : MonoBehaviour
    {
        private const float OpacidadAlcance = 0.18f;

        private Nivel _nivel;
        private VistaMapa _mapa;
        private EventoEstres _evento;
        private Action<VistaEventoEstres> _alTerminar;
        private SpriteRenderer _alcance;

        /// <summary>Called once when the pool creates the instance.</summary>
        public void Configurar(Partida partida)
        {
            _nivel = partida.Nivel;
            _mapa = partida.GetComponent<VistaMapa>();
            if (partida.MaterialSprites != null)
            {
                GetComponent<SpriteRenderer>().sharedMaterial = partida.MaterialSprites;
            }

            _alcance = FabricaDeSprites.Crear(transform, partida.MaterialSprites, "Alcance", partida.SpriteCirculo, OrdenDeDibujo.Alcance);
        }

        /// <summary>Starts a new walk; <paramref name="alTerminar"/> receives this view when it arrives.</summary>
        public void Iniciar(EventoEstres evento, Color color, Action<VistaEventoEstres> alTerminar)
        {
            _evento = evento;
            _alTerminar = alTerminar;
            name = evento.Definicion.Nombre;
            GetComponent<SpriteRenderer>().color = color;
            MostrarAlcance(evento.Definicion.Efecto.Alcance, color);
            GetComponent<SeguidorDeRuta>().Seguir(evento.Recorrido, _mapa);
        }

        // The effect reaches every cell within Alcance (Chebyshev distance), so the disc spans
        // 2 * Alcance + 1 cells. The event object is scaled down, so the child compensates.
        private void MostrarAlcance(int alcance, Color color)
        {
            _alcance.gameObject.SetActive(alcance > 0);
            if (alcance <= 0)
            {
                return;
            }

            var diametro = ((2f * alcance) + 1f) / transform.lossyScale.x;
            _alcance.transform.localScale = new Vector3(diametro, diametro, 1f);
            _alcance.color = new Color(color.r, color.g, color.b, OpacidadAlcance);
        }

        private void Update()
        {
            if (_evento == null || _evento.Avanzar(Time.deltaTime, _nivel) != AvanceEvento.LlegoAlSilo)
            {
                return;
            }

            var alTerminar = _alTerminar;
            _evento = null;
            _alTerminar = null;
            alTerminar(this);
        }
    }
}
