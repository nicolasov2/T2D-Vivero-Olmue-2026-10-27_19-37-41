#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>
    /// UT 1787. Selection before building. Validity comes from <see cref="ValidadorDeColocacion"/> (1678)
    /// and building from <see cref="ServicioDeConstruccion"/> (1687), only on Confirm.
    /// </summary>
    public sealed class SelectorDeCasilla
    {
        /// <summary>Current state; starts with nothing selected.</summary>
        public EstadoSeleccion Estado { get; private set; } = SinSeleccion.Instancia;

        /// <summary>Touching a cell selects it, or moves the preview there when a station is chosen.</summary>
        public void Seleccionar(Posicion posicion)
        {
            Estado = Estado switch
            {
                SinSeleccion _ => new CasillaSeleccionada(posicion),
                CasillaSeleccionada _ => new CasillaSeleccionada(posicion),
                EstacionElegida elegida => new Previsualizando(posicion, elegida.Definicion),
                Previsualizando previa => new Previsualizando(posicion, previa.Definicion),
                _ => throw EstadoDesconocido(),
            };
        }

        /// <summary>Choosing a station previews it on the selected cell, or waits for a cell.</summary>
        public void ElegirEstacion(DefinicionEstacion definicion)
        {
            Estado = Estado switch
            {
                SinSeleccion _ => new EstacionElegida(definicion),
                CasillaSeleccionada seleccionada => new Previsualizando(seleccionada.Posicion, definicion),
                EstacionElegida _ => new EstacionElegida(definicion),
                Previsualizando previa => new Previsualizando(previa.Posicion, definicion),
                _ => throw EstadoDesconocido(),
            };
        }

        /// <summary>Clears everything without building or charging.</summary>
        public void Cancelar() => Estado = SinSeleccion.Instancia;

        /// <summary>Builds the previewed station. On success the selection resets; on failure it stays so the player can move it.</summary>
        public Resultado<Estacion, ResultadoColocacion> Confirmar(ServicioDeConstruccion construccion)
        {
            if (Estado is not Previsualizando previa)
            {
                return Resultado<Estacion, ResultadoColocacion>.Fallo(ResultadoColocacion.SinEstacionElegida);
            }

            var resultado = construccion.IntentarColocar(previa.Definicion, previa.Posicion);
            if (resultado.EsExito)
            {
                Estado = SinSeleccion.Instancia;
            }

            return resultado;
        }

        private InvalidOperationException EstadoDesconocido() =>
            new InvalidOperationException($"Estado de selección no contemplado: {Estado.GetType().Name}.");
    }
}
