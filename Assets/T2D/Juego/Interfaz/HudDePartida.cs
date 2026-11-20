using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>
    /// Top bar: level name, water and energy credits (1748 minimal) and silo water (1750 minimal).
    /// Side columns for the demo: next event and colour legend on the left, speed control on the right.
    /// Floating texts over the map with the credits charged or refunded by a station (1687, 1766).
    /// </summary>
    [RequireComponent(typeof(Partida))]
    public sealed class HudDePartida : MonoBehaviour, IParteDePartida
    {
        private const float AltoBarra = 48f;
        private const float AnchoMedidor = 200f;
        private const float MargenLateral = 8f;
        private const float SeparacionPaneles = 8f;

        // The map leaves about 195 virtual units free on each side at 16:9, so the columns fit beside it.
        private const float AnchoColumna = 184f;

        private readonly AvisoProximoEvento _aviso = new AvisoProximoEvento();
        private readonly LeyendaDeColores _leyenda = new LeyendaDeColores();
        private readonly ControlDeVelocidad _velocidad = new ControlDeVelocidad();
        private readonly TextosFlotantes _textos = new TextosFlotantes();

        private Partida _partida;
        private ZonasDeInterfaz _zonas;
        private GeneradorDeEventos _generador;
        private bool _tieneGenerador;
        private VistaMapa _mapa;

        /// <inheritdoc />
        public void Inicializar(Partida partida)
        {
            _partida = partida;
            _zonas = GetComponent<ZonasDeInterfaz>();
            _tieneGenerador = TryGetComponent(out _generador);
            _mapa = GetComponent<VistaMapa>();
            partida.Nivel.Construccion.EstacionColocada += AlColocar;
            partida.Nivel.Construccion.EstacionDesmantelada += AlDesmantelar;
        }

        private void OnDestroy()
        {
            ControlDeVelocidad.Restablecer();
            if (_partida == null || _partida.Nivel == null)
            {
                return;
            }

            _partida.Nivel.Construccion.EstacionColocada -= AlColocar;
            _partida.Nivel.Construccion.EstacionDesmantelada -= AlDesmantelar;
        }

        private void AlColocar(Estacion estacion) =>
            _textos.Agregar(TextosFlotantes.Cobro(estacion.Definicion.Costo), _mapa.CentroDe(estacion.Posicion));

        private void AlDesmantelar(Estacion estacion) =>
            _textos.Agregar(TextosFlotantes.Reembolso(CalculadoraDeReembolso.Reembolso(estacion.Definicion.Costo)), _mapa.CentroDe(estacion.Posicion));

        private void OnGUI()
        {
            if (_partida == null)
            {
                return;
            }

            EstiloInterfaz.AplicarEscala();
            DibujarBarra();
            DibujarColumnaIzquierda();
            DibujarColumnaDerecha();
            _textos.Dibujar(_partida.Camara);
        }

        private void DibujarBarra()
        {
            var nivel = _partida.Nivel;
            var barra = new Rect(0f, 0f, EstiloInterfaz.AnchoVirtual, AltoBarra);
            _zonas.Registrar(barra);
            GUI.Box(barra, GUIContent.none, EstiloInterfaz.Caja);
            GUI.Label(new Rect(16f, 12f, 320f, 26f), $"T2D · {nivel.Nombre}", EstiloInterfaz.Titulo);
            GUI.Label(new Rect(360f, 13f, 300f, 26f), $"Agua {nivel.Creditos.Agua}   ·   Energía {nivel.Creditos.Energia}", EstiloInterfaz.Texto);
            DibujarSilo(new Rect(EstiloInterfaz.AnchoVirtual - AnchoMedidor - 150f, 14f, AnchoMedidor, 20f), nivel.Silo.Fraccion, nivel.Silo.EstaAgotado);
        }

        private void DibujarColumnaIzquierda()
        {
            var origen = new Vector2(MargenLateral, AltoBarra + SeparacionPaneles);
            if (_tieneGenerador)
            {
                var aviso = _aviso.Dibujar(origen, AnchoColumna, _generador);
                _zonas.Registrar(aviso);
                origen = new Vector2(MargenLateral, aviso.yMax + SeparacionPaneles);
            }

            var eventos = _tieneGenerador ? _generador.Eventos : System.Array.Empty<EventoDefinicionSO>();
            _zonas.Registrar(_leyenda.Dibujar(origen, AnchoColumna, _partida.Paleta, eventos));
        }

        private void DibujarColumnaDerecha()
        {
            var panel = new Rect(EstiloInterfaz.AnchoVirtual - MargenLateral - AnchoColumna, AltoBarra + SeparacionPaneles, AnchoColumna, ControlDeVelocidad.Alto);
            _zonas.Registrar(panel);
            _velocidad.Dibujar(panel);
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
