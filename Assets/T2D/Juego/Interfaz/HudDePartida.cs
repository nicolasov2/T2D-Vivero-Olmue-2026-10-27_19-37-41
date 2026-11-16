using UnityEngine;

namespace T2D.Juego
{
    /// <summary>Top bar: level name, water and energy credits (1748 minimal) and silo water (1750 minimal).</summary>
    [RequireComponent(typeof(Partida))]
    public sealed class HudDePartida : MonoBehaviour, IParteDePartida
    {
        private const float AltoBarra = 48f;
        private const float AnchoMedidor = 200f;

        private Partida _partida;
        private ZonasDeInterfaz _zonas;

        /// <inheritdoc />
        public void Inicializar(Partida partida)
        {
            _partida = partida;
            _zonas = GetComponent<ZonasDeInterfaz>();
        }

        private void OnGUI()
        {
            if (_partida == null)
            {
                return;
            }

            EstiloInterfaz.AplicarEscala();
            var nivel = _partida.Nivel;
            var barra = new Rect(0f, 0f, EstiloInterfaz.AnchoVirtual, AltoBarra);
            _zonas.Registrar(barra);
            GUI.Box(barra, GUIContent.none, EstiloInterfaz.Caja);
            GUI.Label(new Rect(16f, 12f, 320f, 26f), $"T2D · {nivel.Nombre}", EstiloInterfaz.Titulo);
            GUI.Label(new Rect(360f, 13f, 300f, 26f), $"Agua {nivel.Creditos.Agua}   ·   Energía {nivel.Creditos.Energia}", EstiloInterfaz.Texto);
            DibujarSilo(new Rect(EstiloInterfaz.AnchoVirtual - AnchoMedidor - 150f, 14f, AnchoMedidor, 20f), nivel.Silo.Fraccion, nivel.Silo.EstaAgotado);
        }

        private static void DibujarSilo(Rect medidor, float fraccion, bool agotado)
        {
            GUI.Label(new Rect(medidor.x - 44f, medidor.y - 2f, 44f, 24f), "Silo", EstiloInterfaz.Texto);
            GUI.Box(medidor, GUIContent.none);
            var colorAnterior = GUI.color;
            GUI.color = new Color(0.35f, 0.66f, 0.9f);
            GUI.DrawTexture(new Rect(medidor.x, medidor.y, medidor.width * fraccion, medidor.height), Texture2D.whiteTexture);
            GUI.color = colorAnterior;
            var texto = agotado ? "Silo agotado" : $"{Mathf.RoundToInt(fraccion * 100f)} %";
            GUI.Label(new Rect(medidor.xMax + 10f, medidor.y - 2f, 130f, 24f), texto, EstiloInterfaz.Texto);
        }
    }
}
