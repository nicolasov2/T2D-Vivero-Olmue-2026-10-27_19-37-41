using System;
using NUnit.Framework;
using T2D.Dominio;

namespace T2D.Tests
{
    public class NivelTests
    {
        [Test]
        public void Construir_NivelValido_AsignaElTipoDeCadaCasilla()
        {
            var cuadricula = NivelDePrueba.Construir().Cuadricula;

            Assert.AreEqual(TipoCasilla.Ruta, cuadricula.Obtener(new Posicion(3, 2)).Tipo);
            Assert.AreEqual(TipoCasilla.Silo, cuadricula.Obtener(NivelDePrueba.Silo).Tipo);
            Assert.AreEqual(TipoCasilla.Plantines, cuadricula.Obtener(new Posicion(2, 0)).Tipo);
            Assert.AreEqual(TipoCasilla.Bloqueada, cuadricula.Obtener(new Posicion(0, 0)).Tipo);
            Assert.AreEqual(TipoCasilla.SueloEdificable, cuadricula.Obtener(new Posicion(6, 4)).Tipo);
        }

        [Test]
        public void Construir_SinRutas_Falla()
        {
            var datos = NivelDePrueba.Datos();
            datos.Rutas = Array.Empty<System.Collections.Generic.IReadOnlyList<Posicion>>();

            AssertFallaCon(datos, "no tiene rutas");
        }

        [Test]
        public void Construir_RutaQueNoTerminaEnElSilo_Falla()
        {
            var datos = NivelDePrueba.Datos();
            datos.Rutas = new[] { NivelDePrueba.Fila(2, 0, 6) };

            AssertFallaCon(datos, "debe terminar en el silo");
        }

        [Test]
        public void Construir_RutaConPuntosNoAdyacentes_Falla()
        {
            var datos = NivelDePrueba.Datos();
            datos.Rutas = new[] { new[] { new Posicion(0, 2), new Posicion(2, 2), new Posicion(3, 2) } };

            AssertFallaCon(datos, "no son adyacentes");
        }

        [Test]
        public void Construir_RutaPorCasillaBloqueada_Falla()
        {
            var datos = NivelDePrueba.Datos();
            datos.Bloqueadas = new[] { new Posicion(4, 2) };

            AssertFallaCon(datos, "bloqueada (4, 2)");
        }

        [Test]
        public void Construir_PasillosSuperpuestos_Falla()
        {
            var datos = NivelDePrueba.Datos();
            datos.Pasillos = new[]
            {
                new DatosDePasillo(Especie.Palto, new Posicion(2, 0), 2, 1),
                new DatosDePasillo(Especie.Citrico, new Posicion(3, 0), 2, 1),
            };

            AssertFallaCon(datos, "El pasillo 2");
        }

        [Test]
        public void Construir_SiloFueraDelMapa_Falla()
        {
            var datos = NivelDePrueba.Datos();
            datos.PosicionSilo = new Posicion(9, 2);

            AssertFallaCon(datos, "fuera del mapa");
        }

        private static void AssertFallaCon(DatosDeNivel datos, string fragmento)
        {
            var resultado = ConstructorDeNivel.Construir(datos);

            Assert.IsFalse(resultado.EsExito);
            StringAssert.Contains(fragmento, resultado.Error);
        }
    }
}
