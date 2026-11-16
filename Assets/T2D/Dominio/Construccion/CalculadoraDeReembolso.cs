#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>UT 1766. Credits returned when a station is dismantled.</summary>
    public static class CalculadoraDeReembolso
    {
        /// <summary>Share of the price returned, fixed by "T2D Requisitos".</summary>
        public const double PorcentajeReembolso = 0.5;

        /// <summary>Half the water and half the energy of the price, each rounded down.</summary>
        public static Costo Reembolso(Costo costo) => new Costo(Redondear(costo.Agua), Redondear(costo.Energia));

        private static int Redondear(int monto) => (int)Math.Floor(monto * PorcentajeReembolso);
    }
}
