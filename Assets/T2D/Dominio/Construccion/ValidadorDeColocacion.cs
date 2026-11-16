#nullable enable
namespace T2D.Dominio
{
    /// <summary>
    /// UT 1678. Placement rules as a pure check: it never changes the map or the balance, so the
    /// preview can ask it every frame and the construction service asks it again before charging.
    /// </summary>
    public sealed class ValidadorDeColocacion
    {
        private readonly Cuadricula _cuadricula;
        private readonly Creditos _creditos;

        /// <summary>Validates against this map and this balance.</summary>
        public ValidadorDeColocacion(Cuadricula cuadricula, Creditos creditos)
        {
            _cuadricula = cuadricula;
            _creditos = creditos;
        }

        /// <summary>Checks, in this order: inside the map, not a route, free, buildable ground, enough credits.</summary>
        public ResultadoColocacion Validar(Costo costo, Posicion posicion)
        {
            if (!_cuadricula.Contiene(posicion))
            {
                return ResultadoColocacion.FueraDelMapa;
            }

            var casilla = _cuadricula.Obtener(posicion);
            if (casilla.Tipo == TipoCasilla.Ruta)
            {
                return ResultadoColocacion.EsRuta;
            }

            if (casilla.EstaOcupada)
            {
                return ResultadoColocacion.Ocupada;
            }

            if (!casilla.EsEdificable)
            {
                return ResultadoColocacion.NoEdificable;
            }

            return _creditos.Alcanza(costo) ? ResultadoColocacion.Valida : ResultadoColocacion.CreditosInsuficientes;
        }
    }
}
