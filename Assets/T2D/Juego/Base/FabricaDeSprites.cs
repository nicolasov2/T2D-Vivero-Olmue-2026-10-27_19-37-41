using UnityEngine;

namespace T2D.Juego
{
    /// <summary>Creates the helper sprites (preview, highlight, radius) that the views place at runtime.</summary>
    public static class FabricaDeSprites
    {
        /// <summary>Hidden child sprite of <paramref name="padre"/> using the match material.</summary>
        public static SpriteRenderer Crear(Partida padre, string nombre, Sprite sprite, int orden) =>
            Crear(padre.transform, padre.MaterialSprites, nombre, sprite, orden);

        /// <summary>Hidden child sprite of any transform (a station or an event), using <paramref name="material"/> when given.</summary>
        public static SpriteRenderer Crear(Transform padre, Material material, string nombre, Sprite sprite, int orden)
        {
            var objeto = new GameObject(nombre);
            objeto.transform.SetParent(padre, false);
            var renderer = objeto.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = orden;
            if (material != null)
            {
                renderer.sharedMaterial = material;
            }

            objeto.SetActive(false);
            return renderer;
        }
    }
}
