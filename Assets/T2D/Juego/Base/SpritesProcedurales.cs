using System;
using System.Collections.Generic;
using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>
    /// Sprites drawn in code when the match starts: a seedling per species (1686) and the route
    /// arrow (1680). No image assets are added to the repository. Each sprite is drawn once.
    /// </summary>
    public static class SpritesProcedurales
    {
        private const int Lado = 64;
        private const float PixelesPorUnidad = 64f;
        private const float Suavidad = 0.08f;
        private const float GradosHojaLateral = 35f;

        private static readonly Color Bolsa = new Color(0.23f, 0.18f, 0.16f);
        private static readonly Color Tallo = new Color(0.36f, 0.5f, 0.18f);
        private static readonly Color HojaPalto = new Color(0.18f, 0.42f, 0.18f);
        private static readonly Color HojaCitrico = new Color(0.3f, 0.6f, 0.23f);
        private static readonly Color Fruto = new Color(0.95f, 0.6f, 0.18f);
        private static readonly Color Transparente = new Color(0f, 0f, 0f, 0f);

        private static readonly Dictionary<Especie, Sprite> Plantines = new Dictionary<Especie, Sprite>();
        private static Sprite _flecha;

        /// <summary>Seedling in its nursery bag: wide dark leaves for avocado, round leaves and fruit for citrus.</summary>
        public static Sprite Plantin(Especie especie)
        {
            if (Plantines.TryGetValue(especie, out var guardado) && guardado != null)
            {
                return guardado;
            }

            var sprite = Crear($"Plantin_{especie}", especie == Especie.Palto ? (Func<float, float, Color>)PixelDePalto : PixelDeCitrico);
            Plantines[especie] = sprite;
            return sprite;
        }

        /// <summary>White arrow pointing right, tinted and rotated by the route view.</summary>
        public static Sprite Flecha()
        {
            if (_flecha == null)
            {
                _flecha = Crear("Flecha", PixelDeFlecha);
            }

            return _flecha;
        }

        private static Sprite Crear(string nombre, Func<float, float, Color> pixel)
        {
            var textura = new Texture2D(Lado, Lado, TextureFormat.RGBA32, false)
            {
                name = nombre,
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
            };
            var pixeles = new Color[Lado * Lado];
            for (var i = 0; i < pixeles.Length; i++)
            {
                pixeles[i] = pixel(((i % Lado) + 0.5f) / Lado, ((i / Lado) + 0.5f) / Lado);
            }

            textura.SetPixels(pixeles);
            textura.Apply();
            var sprite = Sprite.Create(textura, new Rect(0f, 0f, Lado, Lado), new Vector2(0.5f, 0.5f), PixelesPorUnidad);
            sprite.name = nombre;
            return sprite;
        }

        private static Color PixelDePalto(float u, float v)
        {
            var color = BolsaYTallo(u, v);
            color = Sobre(color, HojaPalto, Elipse(u, v, 0.35f, 0.6f, 0.17f, 0.08f, GradosHojaLateral));
            color = Sobre(color, HojaPalto, Elipse(u, v, 0.65f, 0.64f, 0.17f, 0.08f, -GradosHojaLateral));
            return Sobre(color, HojaPalto, Elipse(u, v, 0.5f, 0.8f, 0.08f, 0.15f, 0f));
        }

        private static Color PixelDeCitrico(float u, float v)
        {
            var color = BolsaYTallo(u, v);
            color = Sobre(color, HojaCitrico, Elipse(u, v, 0.4f, 0.62f, 0.12f, 0.12f, 0f));
            color = Sobre(color, HojaCitrico, Elipse(u, v, 0.6f, 0.62f, 0.12f, 0.12f, 0f));
            color = Sobre(color, HojaCitrico, Elipse(u, v, 0.5f, 0.75f, 0.13f, 0.12f, 0f));
            color = Sobre(color, Fruto, Elipse(u, v, 0.42f, 0.66f, 0.05f, 0.05f, 0f));
            return Sobre(color, Fruto, Elipse(u, v, 0.6f, 0.74f, 0.05f, 0.05f, 0f));
        }

        private static Color BolsaYTallo(float u, float v)
        {
            var color = Sobre(Transparente, Bolsa, Rectangulo(u, v, 0.36f, 0.08f, 0.64f, 0.3f));
            return Sobre(color, Tallo, Rectangulo(u, v, 0.48f, 0.28f, 0.52f, 0.64f));
        }

        private static Color PixelDeFlecha(float u, float v)
        {
            // Triangle (0.3, 0.25) - (0.3, 0.75) - (0.78, 0.5): inside when right of the back edge
            // and under both slanted edges.
            var mitad = Mathf.Abs(v - 0.5f);
            var limite = 0.25f * (0.78f - u) / 0.48f;
            var dentro = u >= 0.3f && u <= 0.78f && mitad <= limite;
            return dentro ? Color.white : Transparente;
        }

        // Coverage from 0 to 1 of an ellipse centred on (cx, cy), radii (rx, ry), turned by grados.
        private static float Elipse(float u, float v, float cx, float cy, float rx, float ry, float grados)
        {
            var radianes = grados * Mathf.Deg2Rad;
            var dx = u - cx;
            var dy = v - cy;
            var lx = (dx * Mathf.Cos(radianes)) + (dy * Mathf.Sin(radianes));
            var ly = (-dx * Mathf.Sin(radianes)) + (dy * Mathf.Cos(radianes));
            var distancia = Mathf.Sqrt(((lx / rx) * (lx / rx)) + ((ly / ry) * (ly / ry)));
            return Mathf.Clamp01((1f - distancia) / Suavidad);
        }

        private static float Rectangulo(float u, float v, float x0, float y0, float x1, float y1) =>
            u >= x0 && u <= x1 && v >= y0 && v <= y1 ? 1f : 0f;

        private static Color Sobre(Color fondo, Color encima, float cobertura)
        {
            if (cobertura <= 0f)
            {
                return fondo;
            }

            // Over a transparent pixel keep the full colour, so soft edges do not darken.
            var resultado = fondo.a <= 0f ? encima : Color.Lerp(fondo, encima, cobertura);
            resultado.a = Mathf.Max(fondo.a, cobertura);
            return resultado;
        }
    }
}
