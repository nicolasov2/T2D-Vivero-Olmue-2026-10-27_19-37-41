#nullable enable
using System;

namespace T2D.Dominio
{
    /// <summary>
    /// Outcome of an operation that can fail for an expected domain reason (validation, balance,
    /// invalid transition). Exceptions are reserved for programming errors.
    /// </summary>
    /// <typeparam name="T">Value produced on success.</typeparam>
    /// <typeparam name="TError">Reason produced on failure.</typeparam>
    public abstract class Resultado<T, TError>
    {
        private protected Resultado()
        {
        }

        /// <summary>True when the operation succeeded and <see cref="Valor"/> is available.</summary>
        public abstract bool EsExito { get; }

        /// <summary>Value of a successful result. Reading it on a failure is a programming error.</summary>
        public abstract T Valor { get; }

        /// <summary>Reason of a failed result. Reading it on a success is a programming error.</summary>
        public abstract TError Error { get; }

        /// <summary>Builds a successful result.</summary>
        public static Resultado<T, TError> Exito(T valor) => new ResultadoExitoso(valor);

        /// <summary>Builds a failed result.</summary>
        public static Resultado<T, TError> Fallo(TError error) => new ResultadoFallido(error);

        private sealed class ResultadoExitoso : Resultado<T, TError>
        {
            private readonly T _valor;

            public ResultadoExitoso(T valor) => _valor = valor;

            public override bool EsExito => true;

            public override T Valor => _valor;

            public override TError Error => throw new InvalidOperationException("Un resultado exitoso no tiene error.");
        }

        private sealed class ResultadoFallido : Resultado<T, TError>
        {
            private readonly TError _error;

            public ResultadoFallido(TError error) => _error = error;

            public override bool EsExito => false;

            public override T Valor => throw new InvalidOperationException($"El resultado falló: {_error}");

            public override TError Error => _error;
        }
    }
}
