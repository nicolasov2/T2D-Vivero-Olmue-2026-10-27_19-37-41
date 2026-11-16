using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1690. Dehydration vector: lowers the hydration of seedling rows within reach of its route.</summary>
    [CreateAssetMenu(menuName = "T2D/Eventos/Deshidratación", fileName = "Vector_de_deshidratacion")]
    public sealed class DeshidratacionDefinicion : EventoDefinicionSO
    {
        [SerializeField, Min(0)] private int alcance = 1;
        [SerializeField, Min(0)] private int danioHidratacion = 10;

        /// <inheritdoc />
        protected override IEfectoDeEvento CrearEfecto() => new EfectoDeshidratacion(alcance, danioHidratacion);
    }
}
