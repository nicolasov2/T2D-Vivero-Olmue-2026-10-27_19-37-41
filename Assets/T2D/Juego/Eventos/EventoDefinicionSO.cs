using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>
    /// UT 1688. Editable data shared by every kind of stress event. Each kind (heat wave, wind,
    /// dehydration) is a subclass that adds its own effect, so each UT owns its own asset type.
    /// </summary>
    public abstract class EventoDefinicionSO : ScriptableObject
    {
        [SerializeField] private string nombre = "Evento de estrés";
        [SerializeField, Min(0.1f)] private float velocidad = 2f;
        [SerializeField, Min(0)] private int danioAlSilo = 10;
        [SerializeField] private Color color = Color.white;

        /// <summary>Name shown to the player.</summary>
        public string Nombre => nombre;

        /// <summary>Tint of the event on the map.</summary>
        public Color Color => color;

        /// <summary>Builds a fresh domain definition, so edits made during Play apply to the next event.</summary>
        public DefinicionEvento ADefinicion() => new DefinicionEvento(nombre, velocidad, danioAlSilo, CrearEfecto());

        /// <summary>What this kind of event does on every cell it enters.</summary>
        protected abstract IEfectoDeEvento CrearEfecto();
    }
}
