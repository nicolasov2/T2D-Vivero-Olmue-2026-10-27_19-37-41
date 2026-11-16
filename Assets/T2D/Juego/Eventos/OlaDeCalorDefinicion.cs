using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1688. Heat wave: in Sprint 1 it only drains the silo on arrival.</summary>
    [CreateAssetMenu(menuName = "T2D/Eventos/Ola de calor", fileName = "Ola_de_calor")]
    public sealed class OlaDeCalorDefinicion : EventoDefinicionSO
    {
        /// <inheritdoc />
        protected override IEfectoDeEvento CrearEfecto() => new EfectoOlaDeCalor();
    }
}
