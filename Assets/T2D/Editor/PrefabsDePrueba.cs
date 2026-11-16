using T2D.Juego;
using UnityEditor;
using UnityEngine;

namespace T2D.Herramientas
{
    /// <summary>Placeholder prefabs of the station and the stress event.</summary>
    internal static class PrefabsDePrueba
    {
        private const float EscalaEstacion = 0.8f;
        private const float EscalaLente = 0.45f;
        private const float EscalaEvento = 0.6f;

        /// <summary>Blue square with a light lens: the fixed SBC camera of the mockups.</summary>
        public static GameObject Estacion(string carpeta, Recursos recursos)
        {
            var objeto = new GameObject("Estacion");
            objeto.transform.localScale = Vector3.one * EscalaEstacion;
            Dibujar(objeto, recursos.Cuadrado, "#3B5F8A", OrdenDeDibujo.Estacion, recursos.Material);
            var lente = new GameObject("Lente");
            lente.transform.SetParent(objeto.transform, false);
            lente.transform.localScale = Vector3.one * EscalaLente;
            Dibujar(lente, recursos.Circulo, "#9CC8F2", OrdenDeDibujo.Estacion + 1, recursos.Material);
            objeto.AddComponent<VistaEstacion>();
            objeto.AddComponent<VistaEstadoEstacion>();
            return Guardar(objeto, $"{carpeta}/Estacion.prefab");
        }

        /// <summary>Disk tinted at runtime with the color of each event kind.</summary>
        public static GameObject Evento(string carpeta, Recursos recursos)
        {
            var objeto = new GameObject("Evento");
            objeto.transform.localScale = Vector3.one * EscalaEvento;
            Dibujar(objeto, recursos.Circulo, "#FFFFFF", OrdenDeDibujo.Evento, recursos.Material);
            objeto.AddComponent<SeguidorDeRuta>();
            objeto.AddComponent<VistaEventoEstres>();
            return Guardar(objeto, $"{carpeta}/Evento.prefab");
        }

        private static void Dibujar(GameObject objeto, Sprite sprite, string colorHex, int orden, Material material)
        {
            var renderer = objeto.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.color = Colores.Hex(colorHex);
            renderer.sortingOrder = orden;
            if (material != null)
            {
                renderer.sharedMaterial = material;
            }
        }

        private static GameObject Guardar(GameObject objeto, string ruta)
        {
            var prefab = PrefabUtility.SaveAsPrefabAsset(objeto, ruta);
            Object.DestroyImmediate(objeto);
            return prefab;
        }
    }
}
