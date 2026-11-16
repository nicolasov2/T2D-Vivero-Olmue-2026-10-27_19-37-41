using T2D.Dominio;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace T2D.Juego
{
    /// <summary>
    /// Composition root of a match: builds the domain <see cref="Nivel"/> from the level asset,
    /// frames the camera and starts every <see cref="IParteDePartida"/> on this object.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VistaMapa), typeof(ZonasDeInterfaz), typeof(ControladorDeToques))]
    public sealed class Partida : MonoBehaviour
    {
        private const float MargenVertical = 2f;
        private const float MargenHorizontal = 0.5f;
        private const float DesplazamientoVertical = 0.6f;

        [SerializeField] private NivelDefinicion nivel = null;
        [SerializeField] private PaletaDeTiles paleta = null;
        [SerializeField] private Grid grid = null;
        [SerializeField] private Tilemap tilemap = null;
        [SerializeField] private Sprite spriteCuadrado = null;
        [SerializeField] private Sprite spriteCirculo = null;
        [SerializeField] private Sprite spriteMarco = null;
        [Tooltip("Unlit sprite material for URP. Leave empty in the built-in pipeline.")]
        [SerializeField] private Material materialSprites = null;

        /// <summary>Playable level; set in Start when the configuration is valid.</summary>
        public Nivel Nivel { get; private set; }

        /// <summary>UT 1787. Selection before building, shared by the input, the preview and the buttons.</summary>
        public SelectorDeCasilla Selector { get; } = new SelectorDeCasilla();

        /// <summary>Camera that renders the map.</summary>
        public Camera Camara { get; private set; }

        /// <summary>Level asset (also read in edit mode to draw the routes).</summary>
        public NivelDefinicion Definicion => nivel;

        /// <summary>Tiles by cell type.</summary>
        public PaletaDeTiles Paleta => paleta;

        /// <summary>Grid that places the tilemap in the world.</summary>
        public Grid Grid => grid;

        /// <summary>Tilemap painted from the level.</summary>
        public Tilemap Tilemap => tilemap;

        /// <summary>White square used for previews.</summary>
        public Sprite SpriteCuadrado => spriteCuadrado;

        /// <summary>White disk used for coverage radius.</summary>
        public Sprite SpriteCirculo => spriteCirculo;

        /// <summary>White outline used to highlight a cell.</summary>
        public Sprite SpriteMarco => spriteMarco;

        /// <summary>Material for sprites created at runtime; empty in the built-in pipeline.</summary>
        public Material MaterialSprites => materialSprites;

        private void Start()
        {
            var faltante = BuscarReferenciaFaltante();
            if (faltante.Length > 0)
            {
                Debug.LogError($"[T2D] Partida: falta asignar \"{faltante}\". Ejecuta el menú T2D → Crear escena de prueba.", this);
                enabled = false;
                return;
            }

            var resultado = ConstructorDeNivel.Construir(nivel.ADatos());
            if (!resultado.EsExito)
            {
                Debug.LogError($"[T2D] El nivel \"{nivel.name}\" no es válido: {resultado.Error}", nivel);
                enabled = false;
                return;
            }

            Nivel = resultado.Valor;
            Camara = Camera.main;
            EncuadrarCamara();
            foreach (var parte in GetComponents<IParteDePartida>())
            {
                parte.Inicializar(this);
            }
        }

        private string BuscarReferenciaFaltante()
        {
            if (nivel == null) return nameof(nivel);
            if (paleta == null) return nameof(paleta);
            if (grid == null) return nameof(grid);
            if (tilemap == null) return nameof(tilemap);
            if (spriteCuadrado == null || spriteCirculo == null || spriteMarco == null) return "sprites";
            return Camera.main == null ? "una cámara con la etiqueta MainCamera" : string.Empty;
        }

        private void EncuadrarCamara()
        {
            var ancho = Nivel.Cuadricula.Ancho;
            var alto = Nivel.Cuadricula.Alto;
            Camara.orthographic = true;
            Camara.orthographicSize = Mathf.Max((alto / 2f) + MargenVertical, ((ancho / 2f) + MargenHorizontal) / Camara.aspect);
            var centro = grid.LocalToWorld(Vector3.Scale(new Vector3(ancho / 2f, (alto / 2f) - DesplazamientoVertical, 0f), grid.cellSize));
            Camara.transform.position = new Vector3(centro.x, centro.y, Camara.transform.position.z);
        }
    }
}
