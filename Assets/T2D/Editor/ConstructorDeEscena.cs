using System.IO;
using System.Linq;
using T2D.Juego;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace T2D.Herramientas
{
    /// <summary>
    /// Menu T2D → Crear escena de prueba. Generates sprites, tiles, prefabs and the Nivel_Olmue scene.
    /// Assets in Contenido/ (level, station and events) are created only once so tester edits survive.
    /// </summary>
    public static class ConstructorDeEscena
    {
        private const string Raiz = "Assets/T2D";
        private const string Contenido = Raiz + "/Contenido";
        private const string Generado = Raiz + "/Generado";
        private const string Escenas = Raiz + "/Escenas";
        private const string RutaEscena = Escenas + "/Nivel_Olmue.unity";

        /// <summary>Builds (or rebuilds) the test scene and opens it.</summary>
        [MenuItem("T2D/Crear escena de prueba")]
        public static void CrearEscenaDePrueba()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() || !ConfirmarReemplazo())
            {
                return;
            }

            // Opening a single scene unloads assets nothing references yet, which would null the
            // references to freshly created level and station assets. Open it before touching assets.
            var escena = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            AssetDatabase.DeleteAsset(Generado);
            CrearCarpeta("Contenido");
            CrearCarpeta("Generado");
            CrearCarpeta("Escenas");
            var cuadrado = RecursosGenerados.Cuadrado(Generado);
            var recursos = new Recursos(cuadrado, RecursosGenerados.Circulo(Generado), RecursosGenerados.Marco(Generado), RecursosGenerados.MaterialSinLuz(Generado), CrearPaleta(cuadrado));
            CrearEscena(escena, recursos, ContenidoDePrueba.CargarOCrear(Contenido));
            AgregarABuild();
            AssetDatabase.SaveAssets();
            Debug.Log($"[T2D] Escena de prueba lista en {RutaEscena}. Presiona Play.");
        }

        private static bool ConfirmarReemplazo() =>
            !File.Exists(RutaEscena) || EditorUtility.DisplayDialog(
                "T2D", "La escena Nivel_Olmue ya existe. ¿Reemplazarla?\nNo se tocan las definiciones de la carpeta Contenido.", "Reemplazar", "Cancelar");

        private static void CrearCarpeta(string nombre)
        {
            if (!AssetDatabase.IsValidFolder($"{Raiz}/{nombre}"))
            {
                AssetDatabase.CreateFolder(Raiz, nombre);
            }
        }

        private static PaletaDeTiles CrearPaleta(Sprite cuadrado)
        {
            var paleta = ScriptableObject.CreateInstance<PaletaDeTiles>();
            AssetDatabase.CreateAsset(paleta, $"{Generado}/PaletaDeTiles.asset");
            PropiedadesSerializadas.Editar(paleta, serializado =>
            {
                Asignar(serializado, "suelo", CrearTile("Suelo", cuadrado, "#E8DDC3"));
                Asignar(serializado, "ruta", CrearTile("Ruta", cuadrado, "#B9925E"));
                Asignar(serializado, "palto", CrearTile("Palto", cuadrado, "#86B979"));
                Asignar(serializado, "citrico", CrearTile("Citrico", cuadrado, "#D9B35A"));
                Asignar(serializado, "bloqueada", CrearTile("Bloqueada", cuadrado, "#9A988F"));
                Asignar(serializado, "silo", CrearTile("Silo", cuadrado, "#2F6FB0"));
            });
            return paleta;
        }

        private static void Asignar(SerializedObject serializado, string campo, Object valor) =>
            PropiedadesSerializadas.Campo(serializado, campo).objectReferenceValue = valor;

        private static Tile CrearTile(string nombre, Sprite sprite, string colorHex)
        {
            var tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprite;
            tile.color = Colores.Hex(colorHex);
            tile.flags = TileFlags.None;
            tile.colliderType = Tile.ColliderType.None;
            AssetDatabase.CreateAsset(tile, $"{Generado}/Tile_{nombre}.asset");
            return tile;
        }

        private static void CrearEscena(Scene escena, Recursos recursos, ContenidoDePrueba contenido)
        {
            var prefabEstacion = PrefabsDePrueba.Estacion(Generado, recursos);
            var prefabEvento = PrefabsDePrueba.Evento(Generado, recursos);
            CrearCamara();
            var grid = new GameObject("Grid").AddComponent<Grid>();
            var mapa = new GameObject("Mapa");
            mapa.transform.SetParent(grid.transform, false);
            var tilemap = mapa.AddComponent<Tilemap>();
            var renderer = mapa.AddComponent<TilemapRenderer>();
            if (recursos.Material != null)
            {
                renderer.sharedMaterial = recursos.Material;
            }

            var partida = ArmarPartida(grid, tilemap, recursos);
            PropiedadesSerializadas.Asignar(partida, "nivel", contenido.Nivel);
            PropiedadesSerializadas.AsignarLista(partida.GetComponent<PanelDeConstruccion>(), "estaciones", contenido.Camara);
            PropiedadesSerializadas.Asignar(partida.GetComponent<ControladorDeConstruccion>(), "prefabEstacion", prefabEstacion.GetComponent<VistaEstacion>());
            var generador = partida.GetComponent<GeneradorDeEventos>();
            PropiedadesSerializadas.AsignarLista(generador, "eventos", contenido.Eventos.Cast<Object>().ToArray());
            PropiedadesSerializadas.Asignar(generador, "prefabEvento", prefabEvento.GetComponent<VistaEventoEstres>());
            EditorSceneManager.SaveScene(escena, RutaEscena);
        }

        private static void CrearCamara()
        {
            var objeto = new GameObject("Main Camera") { tag = "MainCamera" };
            var camara = objeto.AddComponent<Camera>();
            camara.orthographic = true;
            camara.clearFlags = CameraClearFlags.SolidColor;
            camara.backgroundColor = Colores.Hex("#DCD3BC");
            objeto.transform.position = new Vector3(0f, 0f, -10f);
            objeto.AddComponent<AudioListener>();
        }

        private static Partida ArmarPartida(Grid grid, Tilemap tilemap, Recursos recursos)
        {
            var objeto = new GameObject("Partida");
            var partida = objeto.AddComponent<Partida>();
            objeto.AddComponent<HudDePartida>();
            objeto.AddComponent<MensajeDeRechazo>();
            objeto.AddComponent<VistaPasillos>();
            objeto.AddComponent<RotuloDePasillo>();
            objeto.AddComponent<ControladorDeConstruccion>();
            objeto.AddComponent<PanelDeConstruccion>();
            objeto.AddComponent<VistaSeleccion>();
            objeto.AddComponent<VistaPrevisualizacion>();
            objeto.AddComponent<BotonesConfirmacion>();
            objeto.AddComponent<VistaRadioCobertura>();
            objeto.AddComponent<PanelDeEstacion>();
            objeto.AddComponent<GeneradorDeEventos>();
            objeto.AddComponent<DibujoDeRuta>();
            PropiedadesSerializadas.Editar(partida, serializado =>
            {
                Asignar(serializado, "paleta", recursos.Paleta);
                Asignar(serializado, "grid", grid);
                Asignar(serializado, "tilemap", tilemap);
                Asignar(serializado, "spriteCuadrado", recursos.Cuadrado);
                Asignar(serializado, "spriteCirculo", recursos.Circulo);
                Asignar(serializado, "spriteMarco", recursos.Marco);
                Asignar(serializado, "materialSprites", recursos.Material);
            });
            return partida;
        }

        private static void AgregarABuild()
        {
            var escenas = EditorBuildSettings.scenes.ToList();
            if (escenas.Any(escena => escena.path == RutaEscena))
            {
                return;
            }

            escenas.Insert(0, new EditorBuildSettingsScene(RutaEscena, true));
            EditorBuildSettings.scenes = escenas.ToArray();
        }
    }
}
