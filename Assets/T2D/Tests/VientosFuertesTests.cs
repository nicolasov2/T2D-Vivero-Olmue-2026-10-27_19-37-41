using System;
using NUnit.Framework;
using T2D.Dominio;

namespace T2D.Tests
{
    /// <summary>UT 1689 Vientos fuertes. The camera stands at (3,1), next to the route on row 2.</summary>
    public class VientosFuertesTests
    {
        private Nivel _nivel;
        private Estacion _camara;

        [SetUp]
        public void Preparar()
        {
            _nivel = NivelDePrueba.Construir();
            _camara = _nivel.Construccion.IntentarColocar(NivelDePrueba.Camara(), NivelDePrueba.SueloLibre).Valor;
        }

        [Test]
        public void PA1_ElVientoApareceEnLaEntradaYAvanzaASuVelocidad()
        {
            var viento = NuevoViento(3f, new EfectoViento(1, 1));

            Assert.AreEqual(_nivel.Rutas[0].Entrada, viento.Recorrido.CasillaActual);
            viento.Avanzar(1f, _nivel);
            Assert.AreEqual(3, viento.Recorrido.IndiceActual);
        }

        [Test]
        public void PA2_AlPasarAUnaCasillaOMenos_LeRestaIntegridad()
        {
            var viento = NuevoViento(1f, new EfectoViento(1, 1));

            viento.Avanzar(2f, _nivel);

            Assert.AreEqual(2, _camara.Integridad.Actual);
        }

        [Test]
        public void PA3_ConIntegridadCero_LaEstacionQuedaFueraDeServicio()
        {
            NuevoViento(1f, new EfectoViento(1, 1)).Avanzar(20f, _nivel);

            Assert.AreEqual(0, _camara.Integridad.Actual);
            Assert.IsFalse(_camara.EstaOperativa);
        }

        [Test]
        public void PA4_AlLlegarAlSilo_DescuentaSuDanio()
        {
            NuevoViento(1f, new EfectoViento(1, 1)).Avanzar(20f, _nivel);

            Assert.AreEqual(45, _nivel.Silo.Agua);
        }

        [Test]
        public void PA5_ConAlcanceCeroNoAlcanzaLaEstacionVecina()
        {
            NuevoViento(1f, new EfectoViento(0, 2)).Avanzar(20f, _nivel);

            Assert.AreEqual(3, _camara.Integridad.Actual);
        }

        [Test]
        public void EstacionesAlAlcance_CuentaDiagonales()
        {
            var alcanzadas = DanioPorViento.EstacionesAlAlcance(new Posicion(2, 2), 1, _nivel.Construccion.Estaciones);

            Assert.AreEqual(1, alcanzadas.Count);
        }

        [Test]
        public void Integridad_DanioNegativoOMaximaCero_Lanza()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Integridad(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Integridad(3).RecibirDanio(-1));
        }

        private EventoEstres NuevoViento(float velocidad, EfectoViento efecto) =>
            new EventoEstres(new DefinicionEvento("Viento fuerte", velocidad, 5, efecto), _nivel.Rutas[0]);
    }
}
