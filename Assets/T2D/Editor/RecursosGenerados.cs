using System.IO;
using UnityEditor;
using UnityEngine;

namespace T2D.Herramientas
{
    /// <summary>Creates the placeholder sprites and the unlit material used by the test scene.</summary>
    internal static class RecursosGenerados
    {
        private const int Lado = 64;
        private const float GrosorBorde = 1f;
        private const float GrosorMarco = 5f;
        private static readonly Color Transparente = new Color(1f, 1f, 1f, 0f);
        private static readonly Color Borde = new Color(0.85f, 0.85f, 0.85f, 1f);

        /// <summary>White square with a thin grey border (tiles and stations).</summary>
        public static Sprite Cuadrado(string carpeta) => CrearSprite(carpeta, "Cuadrado", (x, y) =>
            EsBorde(x, y, GrosorBorde) ? Borde : Color.white);

        /// <summary>White disk (events, lenses and coverage radius).</summary>
        public static Sprite Circulo(string carpeta) => CrearSprite(carpeta, "Circulo", (x, y) =>
        {
            var centro = (Lado - 1) / 2f;
            var distancia = Vector2.Distance(new Vector2(x, y), new Vector2(centro, centro));
            return new Color(1f, 1f, 1f, Mathf.Clamp01(centro - distancia));
        });

        /// <summary>White outline (selected cell).</summary>
        public static Sprite Marco(string carpeta) => CrearSprite(carpeta, "Marco", (x, y) =>
            EsBorde(x, y, GrosorMarco) ? Color.white : Transparente);

        /// <summary>
        /// Unlit sprite material when the project uses URP, so sprites show without 2D lights.
        /// Returns null in the built-in pipeline, whose default sprite material is already unlit.
        /// </summary>
        public static Material MaterialSinLuz(string carpeta)
        {
            var shader = Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default");
            if (shader == null)
            {
                return null;
            }

            var material = new Material(shader) { name = "Sprite_SinLuz" };
            AssetDatabase.CreateAsset(material, $"{carpeta}/Sprite_SinLuz.mat");
            return material;
        }

        private static bool EsBorde(int x, int y, float grosor) =>
            x < grosor || y < grosor || x >= Lado - grosor || y >= Lado - grosor;

        private static Sprite CrearSprite(string carpeta, string nombre, System.Func<int, int, Color> pixel)
        {
            var ruta = $"{carpeta}/{nombre}.png";
            var textura = new Texture2D(Lado, Lado, TextureFormat.RGBA32, false);
            for (var x = 0; x < Lado; x++)
            {
                for (var y = 0; y < Lado; y++)
                {
                    textura.SetPixel(x, y, pixel(x, y));
                }
            }

            textura.Apply();
            File.WriteAllBytes(ruta, textura.EncodeToPNG());
            Object.DestroyImmediate(textura);
            AssetDatabase.ImportAsset(ruta, ImportAssetOptions.ForceSynchronousImport);
            var importador = (TextureImporter)AssetImporter.GetAtPath(ruta);
            importador.textureType = TextureImporterType.Sprite;
            importador.spriteImportMode = SpriteImportMode.Single;
            importador.spritePixelsPerUnit = Lado;
            importador.alphaIsTransparency = true;
            importador.mipmapEnabled = false;
            importador.SaveAndReimport();
            return AssetDatabase.LoadAssetAtPath<Sprite>(ruta);
        }
    }
}
