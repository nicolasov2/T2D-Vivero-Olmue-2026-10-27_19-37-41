using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1680. Draws the routes of the level in the Scene view (editor only) to check them while editing.</summary>
    [RequireComponent(typeof(Partida))]
    public sealed class DibujoDeRuta : MonoBehaviour
    {
        private const float RadioEntrada = 0.25f;

        [SerializeField] private Color color = new Color(0.36f, 0.23f, 0.09f);

        private void OnDrawGizmos()
        {
            var partida = GetComponent<Partida>();
            if (partida.Definicion == null || partida.Grid == null)
            {
                return;
            }

            Gizmos.color = color;
            var alto = partida.Definicion.Alto;
            foreach (var ruta in partida.Definicion.Rutas)
            {
                for (var i = 0; i < ruta.Puntos.Count; i++)
                {
                    var punto = partida.Grid.GetCellCenterWorld(Coordenadas.ACelda(ruta.Puntos[i], alto));
                    if (i == 0)
                    {
                        Gizmos.DrawSphere(punto, RadioEntrada);
                        continue;
                    }

                    Gizmos.DrawLine(partida.Grid.GetCellCenterWorld(Coordenadas.ACelda(ruta.Puntos[i - 1], alto)), punto);
                }
            }
        }
    }
}
