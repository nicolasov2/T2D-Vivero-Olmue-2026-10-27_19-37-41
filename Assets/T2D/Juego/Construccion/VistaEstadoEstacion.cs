using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>
    /// UT 1689. Integrity bar above a station, from green to red as strong winds damage it, and the
    /// station greyed out once it is out of service.
    /// </summary>
    [RequireComponent(typeof(VistaEstacion))]
    public sealed class VistaEstadoEstacion : MonoBehaviour
    {
        private const float AnchoBarra = 0.8f;
        private const float AltoBarra = 0.1f;
        private const float AlturaBarra = 0.5f;

        [SerializeField] private Color colorFueraDeServicio = new Color(0.45f, 0.45f, 0.45f, 0.85f);
        [SerializeField] private Color colorIntegro = new Color(0.3f, 0.75f, 0.3f);
        [SerializeField] private Color colorDaniado = new Color(0.85f, 0.25f, 0.2f);
        [SerializeField] private Color colorFondoBarra = new Color(0.15f, 0.15f, 0.15f, 0.8f);

        private SpriteRenderer[] _renderers;
        private Integridad _integridad;
        private SpriteRenderer _relleno;

        private void Start()
        {
            // Collected before the bar exists, so only the station itself turns grey.
            _renderers = GetComponentsInChildren<SpriteRenderer>();
            var vista = GetComponent<VistaEstacion>();
            _integridad = vista.Estacion.Integridad;
            CrearBarra(vista.Partida);
            _integridad.Cambio += Actualizar;
            Actualizar();
        }

        private void CrearBarra(Partida partida)
        {
            // The station object is scaled down, so the bar divides by its scale to keep world sizes.
            var escala = transform.lossyScale.x;
            var fondo = FabricaDeSprites.Crear(transform, partida.MaterialSprites, "Barra de integridad", partida.SpriteCuadrado, OrdenDeDibujo.BarraFondo);
            fondo.transform.localPosition = new Vector3(0f, AlturaBarra / escala, 0f);
            fondo.transform.localScale = new Vector3(AnchoBarra / escala, AltoBarra / escala, 1f);
            fondo.color = colorFondoBarra;
            fondo.gameObject.SetActive(true);

            _relleno = FabricaDeSprites.Crear(transform, partida.MaterialSprites, "Integridad", partida.SpriteCuadrado, OrdenDeDibujo.BarraRelleno);
            _relleno.transform.localPosition = fondo.transform.localPosition;
            _relleno.gameObject.SetActive(true);
        }

        private void Actualizar()
        {
            var fraccion = (float)_integridad.Actual / _integridad.Maxima;
            var escala = transform.lossyScale.x;
            var ancho = AnchoBarra * fraccion;
            _relleno.transform.localScale = new Vector3(ancho / escala, AltoBarra / escala, 1f);
            // Anchored on the left edge, so the bar shrinks towards the left.
            _relleno.transform.localPosition = new Vector3((ancho - AnchoBarra) / (2f * escala), AlturaBarra / escala, 0f);
            _relleno.color = Color.Lerp(colorDaniado, colorIntegro, fraccion);
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
