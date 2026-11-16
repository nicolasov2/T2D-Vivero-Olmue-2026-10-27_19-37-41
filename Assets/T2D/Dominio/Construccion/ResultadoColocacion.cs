#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>UT 1678. Why a station can or cannot be placed on a cell.</summary>
    public enum ResultadoColocacion
    {
        /// <summary>The station can be placed.</summary>
        Valida,
        /// <summary>The cell is outside the map.</summary>
        FueraDelMapa,
        /// <summary>The cell is part of a route.</summary>
        EsRuta,
        /// <summary>Another station already stands on the cell.</summary>
        Ocupada,
        /// <summary>Seedlings, blocked cell or silo.</summary>
        NoEdificable,
        /// <summary>Water or energy credits do not cover the price.</summary>
        CreditosInsuficientes,
        /// <summary>Confirm was pressed without a previewed station (1787).</summary>
        SinEstacionElegida,
    }

    /// <summary>UT 1678. Player-facing text for each placement result.</summary>
    public static class TextosDeColocacion
    {
        /// <summary>Message shown under the preview or in the rejection notice.</summary>
        public static string Mensaje(this ResultadoColocacion resultado) => resultado switch
        {
            ResultadoColocacion.Valida => "Casilla válida",
            ResultadoColocacion.FueraDelMapa => "Fuera del mapa",
            ResultadoColocacion.EsRuta => "No se puede construir sobre una ruta",
            ResultadoColocacion.Ocupada => "La casilla ya está ocupada",
            ResultadoColocacion.NoEdificable => "Solo se construye sobre suelo edificable",
            ResultadoColocacion.CreditosInsuficientes => Creditos.MensajeInsuficientes,
            ResultadoColocacion.SinEstacionElegida => "Elige una estación en el panel",
            _ => throw new ArgumentOutOfRangeException(nameof(resultado), resultado, "Resultado de colocación sin texto."),
        };
    }
}
