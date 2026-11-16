using T2D.Juego;
using UnityEngine;

namespace T2D.Herramientas
{
    /// <summary>Generated sprites, tiles and material shared by the prefabs and the scene.</summary>
    internal sealed class Recursos
    {
        public Recursos(Sprite cuadrado, Sprite circulo, Sprite marco, Material material, PaletaDeTiles paleta)
        {
            Cuadrado = cuadrado;
            Circulo = circulo;
            Marco = marco;
            Material = material;
            Paleta = paleta;
        }

        /// <summary>White square.</summary>
        public Sprite Cuadrado { get; }

        /// <summary>White disk.</summary>
        public Sprite Circulo { get; }

        /// <summary>White outline.</summary>
        public Sprite Marco { get; }

        /// <summary>Unlit URP material; null in the built-in pipeline.</summary>
        public Material Material { get; }

        /// <summary>Tiles by cell type.</summary>
        public PaletaDeTiles Paleta { get; }
    }
}
