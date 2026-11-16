using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1678. Short notice at the bottom of the screen (rejections and confirmations).</summary>
    [RequireComponent(typeof(ZonasDeInterfaz))]
    public sealed class MensajeDeRechazo : MonoBehaviour
    {
        private const float Ancho = 420f;
        private const float Alto = 40f;
        private const float DistanciaAlBorde = 190f;

        [SerializeField, Min(0.5f)] private float segundosVisible = 2.5f;

        private string _texto = string.Empty;
        private float _ocultarEn;

        /// <summary>Shows the text for a few seconds, replacing any previous one.</summary>
        public void Mostrar(string texto)
        {
            _texto = texto;
            _ocultarEn = Time.time + segundosVisible;
        }

        private void OnGUI()
        {
            if (Time.time >= _ocultarEn)
            {
                return;
            }

            EstiloInterfaz.AplicarEscala();
            var rect = new Rect((EstiloInterfaz.AnchoVirtual - Ancho) / 2f, EstiloInterfaz.AltoReferencia - DistanciaAlBorde, Ancho, Alto);
            GetComponent<ZonasDeInterfaz>().Registrar(rect);
            GUI.Box(rect, _texto, EstiloInterfaz.Boton);
        }
    }
}
