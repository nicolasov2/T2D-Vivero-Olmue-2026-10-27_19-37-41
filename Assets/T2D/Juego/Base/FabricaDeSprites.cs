using UnityEngine;

namespace T2D.Juego
{
    /// <summary>Creates the helper sprites (preview, highlight, radius) that the views place at runtime.</summary>
    public static class FabricaDeSprites
    {
        /// <summary>Hidden child sprite of <paramref name="padre"/> using the match material.</summary>
        public static SpriteRenderer Crear(Partida padre, string nombre, Sprite sprite, int orden)
        {
            var objeto = new GameObject(nombre);
            objeto.transform.SetParent(padre.transform, false);
            var renderer = objeto.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = orden;
            if (padre.MaterialSprites != null)
            {
                renderer.sharedMaterial = padre.MaterialSprites;
            }

            objeto.SetActive(false);
            return renderer;
        }
    }
}
