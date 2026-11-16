using System.Collections.Generic;
using T2D.Juego;
using UnityEditor;
using UnityEngine;

namespace T2D.Herramientas
{
    /// <summary>Level, station and event assets of the test scene. Existing assets are reused, never overwritten.</summary>
    internal sealed class ContenidoDePrueba
    {
        private ContenidoDePrueba(NivelDefinicion nivel, EstacionDefinicionSO camara, IReadOnlyList<EventoDefinicionSO> eventos)
        {
            Nivel = nivel;
            Camara = camara;
            Eventos = eventos;
        }

        public NivelDefinicion Nivel { get; }

        public EstacionDefinicionSO Camara { get; }

        public IReadOnlyList<EventoDefinicionSO> Eventos { get; }

        /// <summary>Loads each asset from <paramref name="carpeta"/> or creates it with the mockup values.</summary>
        public static ContenidoDePrueba CargarOCrear(string carpeta)
        {
            var nivel = CargarOCrear<NivelDefinicion>($"{carpeta}/Nivel_Olmue.asset", _ => { });
            var camara = CargarOCrear<EstacionDefinicionSO>($"{carpeta}/Camara_SBC.asset", _ => { });
            var ola = CargarOCrear<OlaDeCalorDefinicion>($"{carpeta}/Ola_de_calor.asset", s => Evento(s, "Ola de calor", 2f, 10, "#E8590C"));
            var viento = CargarOCrear<VientoDefinicion>($"{carpeta}/Viento_fuerte.asset", s =>
            {
                Evento(s, "Viento fuerte", 3f, 5, "#3E6E9E");
                PropiedadesSerializadas.Campo(s, "alcance").intValue = 1;
                PropiedadesSerializadas.Campo(s, "danioAEstaciones").intValue = 1;
            });
            var vector = CargarOCrear<DeshidratacionDefinicion>($"{carpeta}/Vector_de_deshidratacion.asset", s =>
            {
                Evento(s, "Vector de deshidratación", 1.5f, 5, "#C9A227");
                PropiedadesSerializadas.Campo(s, "alcance").intValue = 1;
                PropiedadesSerializadas.Campo(s, "danioHidratacion").intValue = 10;
            });
            return new ContenidoDePrueba(nivel, camara, new EventoDefinicionSO[] { ola, viento, vector });
        }

        private static T CargarOCrear<T>(string ruta, System.Action<SerializedObject> valoresIniciales) where T : ScriptableObject
        {
            var existente = AssetDatabase.LoadAssetAtPath<T>(ruta);
            if (existente != null)
            {
                return existente;
            }

            var nuevo = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(nuevo, ruta);
            PropiedadesSerializadas.Editar(nuevo, valoresIniciales);
            return nuevo;
        }

        private static void Evento(SerializedObject serializado, string nombre, float velocidad, int danioAlSilo, string colorHex)
        {
            PropiedadesSerializadas.Campo(serializado, "nombre").stringValue = nombre;
            PropiedadesSerializadas.Campo(serializado, "velocidad").floatValue = velocidad;
            PropiedadesSerializadas.Campo(serializado, "danioAlSilo").intValue = danioAlSilo;
            PropiedadesSerializadas.Campo(serializado, "color").colorValue = Colores.Hex(colorHex);
        }
    }
}
