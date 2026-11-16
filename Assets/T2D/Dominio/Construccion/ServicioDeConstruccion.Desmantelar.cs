#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>UT 1766. Dismantling: the reverse of placing a station.</summary>
    public sealed partial class ServicioDeConstruccion
    {
        /// <summary>Raised after a station is dismantled.</summary>
        public event Action<Estacion>? EstacionDesmantelada;

        /// <summary>Removes the station, frees its cell and refunds half its price (rounded down).</summary>
        public Resultado<Costo, string> Desmantelar(Estacion estacion)
        {
            if (!_estaciones.Remove(estacion))
            {
                return Resultado<Costo, string>.Fallo($"No hay una estación construida en {estacion.Posicion}.");
            }

            _cuadricula.Obtener(estacion.Posicion).Liberar();
            var reembolso = CalculadoraDeReembolso.Reembolso(estacion.Definicion.Costo);
            _creditos.Abonar(reembolso);
            EstacionDesmantelada?.Invoke(estacion);
            return Resultado<Costo, string>.Exito(reembolso);
        }
    }
}
