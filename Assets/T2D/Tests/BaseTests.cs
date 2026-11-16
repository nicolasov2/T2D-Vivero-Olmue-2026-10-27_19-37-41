using System;
using NUnit.Framework;
using T2D.Dominio;

namespace T2D.Tests
{
    public class BaseTests
    {
        [Test]
        public void Posicion_EsAdyacente_SoloConLadoComun()
        {
            var centro = new Posicion(1, 1);

            Assert.IsTrue(centro.EsAdyacenteA(new Posicion(1, 2)));
            Assert.IsFalse(centro.EsAdyacenteA(new Posicion(2, 2)));
            Assert.IsFalse(centro.EsAdyacenteA(centro));
        }

        [Test]
        public void Posicion_Distancia_CuentaDiagonalesComoUno()
        {
            Assert.AreEqual(2, new Posicion(0, 0).DistanciaA(new Posicion(2, 1)));
            Assert.AreEqual(1, new Posicion(3, 3).DistanciaA(new Posicion(4, 4)));
        }

        [Test]
        public void Cuadricula_ObtenerFueraDelMapa_Lanza()
        {
            var cuadricula = new Cuadricula(2, 2, _ => TipoCasilla.SueloEdificable);

            Assert.IsFalse(cuadricula.Contiene(new Posicion(2, 0)));
            Assert.Throws<ArgumentOutOfRangeException>(() => cuadricula.Obtener(new Posicion(2, 0)));
        }

        [Test]
        public void Cuadricula_TamanoInvalido_Lanza()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Cuadricula(0, 3, _ => TipoCasilla.SueloEdificable));
        }

        [Test]
        public void Casilla_OcuparDosVeces_Lanza()
        {
            var casilla = new Casilla(new Posicion(0, 0), TipoCasilla.SueloEdificable);
            casilla.Ocupar();

            Assert.Throws<InvalidOperationException>(() => casilla.Ocupar());
        }

        [Test]
        public void Costo_Negativo_Lanza()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Costo(-1, 0));
        }

        [Test]
        public void Creditos_PagarSinSaldo_FallaYNoCambiaNada()
        {
            var creditos = new Creditos(new Costo(10, 10));

            var pago = creditos.Pagar(new Costo(20, 5));

            Assert.IsFalse(pago.EsExito);
            Assert.AreEqual(Creditos.MensajeInsuficientes, pago.Error);
            Assert.AreEqual(10, creditos.Agua);
            Assert.AreEqual(10, creditos.Energia);
        }

        [Test]
        public void Creditos_PagarYAbonar_ActualizanElSaldo()
        {
            var creditos = new Creditos(new Costo(100, 100));
            var cambios = 0;
            creditos.Cambiaron += () => cambios++;

            creditos.Pagar(new Costo(60, 45));
            creditos.Abonar(new Costo(5, 5));

            Assert.AreEqual(45, creditos.Agua);
            Assert.AreEqual(60, creditos.Energia);
            Assert.AreEqual(2, cambios);
        }

        [Test]
        public void Silo_DanioMayorQueElAgua_QuedaEnCero()
        {
            var silo = new Silo(new Posicion(0, 0), 50);

            var descontada = silo.RecibirDanio(80);

            Assert.AreEqual(50, descontada);
            Assert.AreEqual(0, silo.Agua);
            Assert.IsTrue(silo.EstaAgotado);
        }

        [Test]
        public void Resultado_LeerLoQueNoTiene_Lanza()
        {
            var exito = Resultado<int, string>.Exito(1);
            var fallo = Resultado<int, string>.Fallo("motivo");

            Assert.AreEqual(1, exito.Valor);
            Assert.AreEqual("motivo", fallo.Error);
            Assert.Throws<InvalidOperationException>(() => _ = exito.Error);
            Assert.Throws<InvalidOperationException>(() => _ = fallo.Valor);
        }
    }
}
