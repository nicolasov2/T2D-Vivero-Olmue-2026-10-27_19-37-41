using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1689. Greys out a station when strong winds leave it out of service.</summary>
    [RequireComponent(typeof(VistaEstacion))]
    public sealed class VistaEstadoEstacion : MonoBehaviour
    {
        [SerializeField] private Color colorFueraDeServicio = new Color(0.45f, 0.45f, 0.45f, 0.85f);

        private SpriteRenderer[] _renderers;
        private Integridad _integridad;

        private void Start()
        {
            _renderers = GetComponentsInChildren<SpriteRenderer>();
            _integridad = GetComponent<VistaEstacion>().Estacion.Integridad;
            _integridad.Cambio += Actualizar;
            Actualizar();
        }

        private void Actualizar()
        {
            if (!_integridad.EstaFueraDeServicio)
            {
                return;
            }

            foreach (var renderer in _renderers)
            {
                renderer.color = colorFueraDeServicio;
            }
        }

        private void OnDestroy()
        {
            if (_integridad != null)
            {
                _integridad.Cambio -= Actualizar;
            }
        }
    }
}
