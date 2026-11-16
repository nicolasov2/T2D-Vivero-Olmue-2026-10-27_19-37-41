using System.Collections.Generic;
using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1687 (and 1766). Creates the visual of each placed station and removes it when dismantled.</summary>
    [RequireComponent(typeof(Partida))]
    public sealed class ControladorDeConstruccion : MonoBehaviour, IParteDePartida
    {
        [SerializeField] private VistaEstacion prefabEstacion = null;

        private readonly Dictionary<Estacion, VistaEstacion> _vistas = new Dictionary<Estacion, VistaEstacion>();
        private ServicioDeConstruccion _construccion;
        private VistaMapa _mapa;

        /// <inheritdoc />
        public void Inicializar(Partida partida)
        {
            if (prefabEstacion == null)
            {
                Debug.LogError("[T2D] ControladorDeConstruccion: falta el prefab de estación.", this);
                return;
            }

            _mapa = GetComponent<VistaMapa>();
            _construccion = partida.Nivel.Construccion;
            _construccion.EstacionColocada += CrearVista;
            _construccion.EstacionDesmantelada += QuitarVista;
        }

        private void CrearVista(Estacion estacion)
        {
            var vista = Instantiate(prefabEstacion, transform);
            vista.name = $"{estacion.Definicion.Nombre} {estacion.Posicion}";
            vista.Inicializar(estacion, _mapa);
            _vistas.Add(estacion, vista);
        }

        private void QuitarVista(Estacion estacion)
        {
            if (_vistas.Remove(estacion, out var vista))
            {
                Destroy(vista.gameObject);
            }
        }

        private void OnDestroy()
        {
            if (_construccion == null)
            {
                return;
            }

            _construccion.EstacionColocada -= CrearVista;
            _construccion.EstacionDesmantelada -= QuitarVista;
        }
    }
}
