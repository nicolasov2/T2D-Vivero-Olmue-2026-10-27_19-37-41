#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>
    /// Player balance of water and energy credits. Minimal version of UT 1748 so that
    /// building (1687) and dismantling (1766) can charge and refund.
    /// </summary>
    public sealed class Creditos
    {
        /// <summary>Message shown when a price cannot be paid.</summary>
        public const string MensajeInsuficientes = "Créditos insuficientes";

        /// <summary>Starts with the given balance.</summary>
        public Creditos(Costo saldoInicial)
        {
            Agua = saldoInicial.Agua;
            Energia = saldoInicial.Energia;
        }

        /// <summary>Raised after every change of balance.</summary>
        public event Action? Cambiaron;

        /// <summary>Water credits available.</summary>
        public int Agua { get; private set; }

        /// <summary>Energy credits available.</summary>
        public int Energia { get; private set; }

        /// <summary>True when both water and energy cover the price.</summary>
        public bool Alcanza(Costo costo) => Agua >= costo.Agua && Energia >= costo.Energia;

        /// <summary>Charges the price, or fails without touching the balance.</summary>
        public Resultado<Costo, string> Pagar(Costo costo)
        {
            if (!Alcanza(costo))
            {
                return Resultado<Costo, string>.Fallo(MensajeInsuficientes);
            }

            Agua -= costo.Agua;
            Energia -= costo.Energia;
            Cambiaron?.Invoke();
            return Resultado<Costo, string>.Exito(costo);
        }

        /// <summary>Adds credits (refunds and, later, passive income).</summary>
        public void Abonar(Costo monto)
        {
            Agua += monto.Agua;
            Energia += monto.Energia;
            Cambiaron?.Invoke();
        }
    }
}
