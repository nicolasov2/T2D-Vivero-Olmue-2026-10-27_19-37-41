using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1689. Strong wind: damages stations within reach of its route.</summary>
    [CreateAssetMenu(menuName = "T2D/Eventos/Viento fuerte", fileName = "Viento_fuerte")]
    public sealed class VientoDefinicion : EventoDefinicionSO
    {
        [SerializeField, Min(0)] private int alcance = 1;
        [SerializeField, Min(0)] private int danioAEstaciones = 1;

        /// <inheritdoc />
        protected override IEfectoDeEvento CrearEfecto() => new EfectoViento(alcance, danioAEstaciones);
    }
}
