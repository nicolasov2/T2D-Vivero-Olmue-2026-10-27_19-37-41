#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>UT 1688. Says how many events to spawn as time passes (temporary until the waves UT, 1790).</summary>
    public sealed class TemporizadorDeAparicion
    {
        private float _acumulado;

        /// <summary>One event every <paramref name="intervalo"/> seconds.</summary>
        public TemporizadorDeAparicion(float intervalo)
        {
            if (intervalo <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(intervalo), "El intervalo debe ser mayor que 0.");
            }

            Intervalo = intervalo;
        }

        /// <summary>Seconds between events.</summary>
        public float Intervalo { get; }

        /// <summary>Seconds left until the next event is due.</summary>
        public float SegundosParaElSiguiente => Intervalo - _acumulado;

        /// <summary>Adds the elapsed time and returns how many events are due.</summary>
        public int Avanzar(float segundos)
        {
            if (segundos < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(segundos), "El tiempo no puede ser negativo.");
            }

            _acumulado += segundos;
            var debidos = (int)(_acumulado / Intervalo);
            _acumulado -= debidos * Intervalo;
            return debidos;
        }
    }
}
