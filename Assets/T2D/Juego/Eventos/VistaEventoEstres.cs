using System;
using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>
    /// UT 1688. Pooled visual of a stress event: advances the domain event every frame and returns to
    /// the pool when it reaches the silo.
    /// </summary>
    [RequireComponent(typeof(SeguidorDeRuta), typeof(SpriteRenderer))]
    public sealed class VistaEventoEstres : MonoBehaviour
    {
        private Nivel _nivel;
        private VistaMapa _mapa;
        private EventoEstres _evento;
        private Action<VistaEventoEstres> _alTerminar;

        /// <summary>Called once when the pool creates the instance.</summary>
        public void Configurar(Partida partida)
        {
            _nivel = partida.Nivel;
            _mapa = partida.GetComponent<VistaMapa>();
            if (partida.MaterialSprites != null)
            {
                GetComponent<SpriteRenderer>().sharedMaterial = partida.MaterialSprites;
            }
        }

        /// <summary>Starts a new walk; <paramref name="alTerminar"/> receives this view when it arrives.</summary>
        public void Iniciar(EventoEstres evento, Color color, Action<VistaEventoEstres> alTerminar)
        {
            _evento = evento;
            _alTerminar = alTerminar;
            name = evento.Definicion.Nombre;
            GetComponent<SpriteRenderer>().color = color;
            GetComponent<SeguidorDeRuta>().Seguir(evento.Recorrido, _mapa);
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
