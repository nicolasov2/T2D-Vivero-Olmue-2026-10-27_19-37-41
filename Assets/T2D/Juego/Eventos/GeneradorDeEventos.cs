using System;
using System.Collections.Generic;
using T2D.Dominio;
using UnityEngine;
using UnityEngine.Pool;

namespace T2D.Juego
{
    /// <summary>
    /// UT 1688. Spawns stress events at a fixed interval, cycling through the configured kinds and the
    /// level routes (temporary until the waves UT, 1790). Events come from a pre-warmed object pool.
    /// </summary>
    [RequireComponent(typeof(Partida))]
    public sealed class GeneradorDeEventos : MonoBehaviour, IParteDePartida
    {
        [SerializeField] private EventoDefinicionSO[] eventos = Array.Empty<EventoDefinicionSO>();
        [SerializeField] private VistaEventoEstres prefabEvento = null;
        [SerializeField, Min(0.1f)] private float intervalo = 4f;
        [SerializeField, Min(1)] private int tamanoInicialDelPool = 30;

        private Partida _partida;
        private ObjectPool<VistaEventoEstres> _pool;
        private TemporizadorDeAparicion _temporizador;
        private int _siguienteEvento;
        private int _siguienteRuta;
        private bool _precalentado;
        private bool _ampliacionAvisada;

        /// <inheritdoc />
        public void Inicializar(Partida partida)
        {
            if (eventos.Length == 0 || prefabEvento == null)
            {
                Debug.LogWarning("[T2D] GeneradorDeEventos sin eventos o sin prefab: no aparecerán eventos.", this);
                return;
            }

            _partida = partida;
            _temporizador = new TemporizadorDeAparicion(intervalo);
            _pool = new ObjectPool<VistaEventoEstres>(
                Crear,
                vista => vista.gameObject.SetActive(true),
                vista => vista.gameObject.SetActive(false),
                vista => Destroy(vista.gameObject),
                collectionCheck: true,
                defaultCapacity: tamanoInicialDelPool,
                maxSize: tamanoInicialDelPool * 2);
            Precalentar();
        }

        private void Update()
        {
            if (_pool == null || _partida.Nivel.Rutas.Count == 0 || _partida.Nivel.Silo.EstaAgotado)
            {
                return;
            }

            var debidos = _temporizador.Avanzar(Time.deltaTime);
            for (var i = 0; i < debidos; i++)
            {
                Aparecer();
            }
        }

        private void Aparecer()
        {
            var rutas = _partida.Nivel.Rutas;
            if (rutas.Count == 0)
            {
                return;
            }

            var definicion = eventos[_siguienteEvento];
            _siguienteEvento = (_siguienteEvento + 1) % eventos.Length;
            var ruta = rutas[_siguienteRuta];
            _siguienteRuta = (_siguienteRuta + 1) % rutas.Count;
            _pool.Get().Iniciar(new EventoEstres(definicion.ADefinicion(), ruta), definicion.Color, _pool.Release);
        }

        private VistaEventoEstres Crear()
        {
            if (_precalentado && !_ampliacionAvisada)
            {
                Debug.LogWarning($"[T2D] Hay más de {tamanoInicialDelPool} eventos a la vez: el pool se amplió. Sube 'tamanoInicialDelPool'.", this);
                _ampliacionAvisada = true;
            }

            var vista = Instantiate(prefabEvento, transform);
            vista.Configurar(_partida);
            vista.gameObject.SetActive(false);
            return vista;
        }

        private void Precalentar()
        {
            var instancias = new List<VistaEventoEstres>(tamanoInicialDelPool);
            for (var i = 0; i < tamanoInicialDelPool; i++)
            {
                instancias.Add(_pool.Get());
            }

            foreach (var instancia in instancias)
            {
                _pool.Release(instancia);
            }

            _precalentado = true;
        }
    }
}
