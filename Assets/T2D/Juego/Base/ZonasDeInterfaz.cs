using System.Collections.Generic;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>
    /// Screen areas covered by IMGUI panels in the last drawn frame, so a touch on a button is not
    /// also read as a touch on the map below it.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ZonasDeInterfaz : MonoBehaviour
    {
        private readonly List<Rect> _zonas = new List<Rect>();
        private int _cuadroRegistrado = -1;

        /// <summary>Call from OnGUI with the panel rectangle in virtual (720p) units.</summary>
        public void Registrar(Rect rectVirtual)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            if (_cuadroRegistrado != Time.frameCount)
            {
                _zonas.Clear();
                _cuadroRegistrado = Time.frameCount;
            }

            _zonas.Add(EstiloInterfaz.APantalla(rectVirtual));
        }

        /// <summary>True when the screen point (input coordinates, origin bottom-left) is over a panel.</summary>
        public bool Contiene(Vector2 posicionEnPantalla)
        {
            var enGui = new Vector2(posicionEnPantalla.x, Screen.height - posicionEnPantalla.y);
            foreach (var zona in _zonas)
            {
                if (zona.Contains(enGui))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
