#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace T2D.Dominio
{
    /// <summary>
    /// UT 1687. Places stations: validates (1678), charges the credits and takes the cell.
    /// Dismantling (1766) lives in <c>ServicioDeConstruccion.Desmantelar.cs</c>.
    /// </summary>
    public sealed partial class ServicioDeConstruccion
    {
        private readonly Cuadricula _cuadricula;
        private readonly Creditos _creditos;
        private readonly ValidadorDeColocacion _validador;
        private readonly List<Estacion> _estaciones = new List<Estacion>();

        /// <summary>Builds on this map, paying with this balance.</summary>
        public ServicioDeConstruccion(Cuadricula cuadricula, Creditos creditos)
        {
            _cuadricula = cuadricula;
            _creditos = creditos;
            _validador = new ValidadorDeColocacion(cuadricula, creditos);
        }

        /// <summary>Raised after a station is placed.</summary>
        public event Action<Estacion>? EstacionColocada;

        /// <summary>Stations standing on the map.</summary>
        public IReadOnlyList<Estacion> Estaciones => _estaciones;

        /// <summary>UT 1678. What would happen if the station were placed there, without changing anything.</summary>
        public ResultadoColocacion Validar(DefinicionEstacion definicion, Posicion posicion) =>
            _validador.Validar(definicion.Costo, posicion);

        /// <summary>Validates, charges and takes the cell; a rejected attempt changes nothing.</summary>
        public Resultado<Estacion, ResultadoColocacion> IntentarColocar(DefinicionEstacion definicion, Posicion posicion)
        {
            var validacion = Validar(definicion, posicion);
            if (validacion != ResultadoColocacion.Valida)
            {
                return Resultado<Estacion, ResultadoColocacion>.Fallo(validacion);
            }

            var pago = _creditos.Pagar(definicion.Costo);
            if (!pago.EsExito)
            {
                return Resultado<Estacion, ResultadoColocacion>.Fallo(ResultadoColocacion.CreditosInsuficientes);
            }

            _cuadricula.Obtener(posicion).Ocupar();
            var estacion = new Estacion(definicion, posicion);
            _estaciones.Add(estacion);
            EstacionColocada?.Invoke(estacion);
            return Resultado<Estacion, ResultadoColocacion>.Exito(estacion);
        }

        /// <summary>Station on the cell; <c>null</c> when the cell has none.</summary>
        public Estacion? EstacionEn(Posicion posicion) =>
            _estaciones.FirstOrDefault(estacion => estacion.Posicion == posicion);
    }
}
