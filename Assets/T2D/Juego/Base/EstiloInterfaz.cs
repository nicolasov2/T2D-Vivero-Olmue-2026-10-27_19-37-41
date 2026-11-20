using UnityEngine;

namespace T2D.Juego
{
    /// <summary>
    /// Shared IMGUI scale and styles. Panels draw in a virtual 720-pixel-high space so the interface
    /// keeps its size on any screen. Styles can only be built inside OnGUI.
    /// </summary>
    public static class EstiloInterfaz
    {
        /// <summary>Virtual height of the interface.</summary>
        public const float AltoReferencia = 720f;

        private const int TamanoTexto = 16;
        private const int TamanoTextoChico = 14;
        private const int TamanoTitulo = 18;

        private static GUIStyle _caja;
        private static GUIStyle _texto;
        private static GUIStyle _textoChico;
        private static GUIStyle _titulo;
        private static GUIStyle _boton;

        /// <summary>Real pixels per virtual unit.</summary>
        public static float Escala => Screen.height / AltoReferencia;

        /// <summary>Screen width in virtual units.</summary>
        public static float AnchoVirtual => Screen.width / Escala;

        /// <summary>Panel background.</summary>
        public static GUIStyle Caja => Asegurar(ref _caja, () => new GUIStyle(GUI.skin.box) { padding = new RectOffset(10, 10, 8, 8) });

        /// <summary>Body text.</summary>
        public static GUIStyle Texto => Asegurar(ref _texto, () => new GUIStyle(GUI.skin.label) { fontSize = TamanoTexto, wordWrap = true });

        /// <summary>Smaller text for dense panels such as the legend.</summary>
        public static GUIStyle TextoChico => Asegurar(ref _textoChico, () => new GUIStyle(GUI.skin.label) { fontSize = TamanoTextoChico, wordWrap = true });

        /// <summary>Bold title.</summary>
        public static GUIStyle Titulo => Asegurar(ref _titulo, () => new GUIStyle(GUI.skin.label) { fontSize = TamanoTitulo, fontStyle = FontStyle.Bold });

        /// <summary>Button text.</summary>
        public static GUIStyle Boton => Asegurar(ref _boton, () => new GUIStyle(GUI.skin.button) { fontSize = TamanoTexto });

        /// <summary>Call at the start of OnGUI.</summary>
        public static void AplicarEscala() => GUI.matrix = Matrix4x4.Scale(new Vector3(Escala, Escala, 1f));

        /// <summary>Virtual rectangle to real GUI pixels (origin top-left).</summary>
        public static Rect APantalla(Rect rectVirtual) =>
            new Rect(rectVirtual.x * Escala, rectVirtual.y * Escala, rectVirtual.width * Escala, rectVirtual.height * Escala);

        /// <summary>World point to virtual GUI coordinates (origin top-left).</summary>
        public static Vector2 MundoAVirtual(Camera camara, Vector3 mundo)
        {
            var pantalla = camara.WorldToScreenPoint(mundo);
            return new Vector2(pantalla.x, Screen.height - pantalla.y) / Escala;
        }

        private static GUIStyle Asegurar(ref GUIStyle estilo, System.Func<GUIStyle> crear) => estilo ??= crear();
    }
}
