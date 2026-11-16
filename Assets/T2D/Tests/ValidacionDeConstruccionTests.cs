using System;
using NUnit.Framework;
using T2D.Dominio;

namespace T2D.Tests
{
    /// <summary>UT 1678 Validación de construcción.</summary>
    public class ValidacionDeConstruccionTests
    {
        private Nivel _nivel;
        private DefinicionEstacion _camara;

        [SetUp]
        public void Preparar()
        {
            _nivel = NivelDePrueba.Construir();
            _camara = NivelDePrueba.Camara();
        }

        [Test]
        public void PA1_NoSePuedeColocarSobreUnaRuta()
        {
            Assert.AreEqual(ResultadoColocacion.EsRuta, _nivel.Construccion.Validar(_camara, new Posicion(3, 2)));
        }

        [Test]
        public void PA2_NoSePuedeColocarSobreUnaCasillaOcupada()
        {
            _nivel.Construccion.IntentarColocar(_camara, NivelDePrueba.SueloLibre);

            Assert.AreEqual(ResultadoColocacion.Ocupada, _nivel.Construccion.Validar(_camara, NivelDePrueba.SueloLibre));
        }

        [Test]
        public void PA3_NoSePuedeColocarSobrePlantinesBloqueadaNiSilo()
        {
            Assert.AreEqual(ResultadoColocacion.NoEdificable, _nivel.Construccion.Validar(_camara, new Posicion(2, 0)));
            Assert.AreEqual(ResultadoColocacion.NoEdificable, _nivel.Construccion.Validar(_camara, new Posicion(0, 0)));
            Assert.AreEqual(ResultadoColocacion.NoEdificable, _nivel.Construccion.Validar(_camara, NivelDePrueba.Silo));
        }

        [Test]
        public void PA4_SinCreditosSuficientes_NoSeInstalaYAvisa()
        {
            var cara = new DefinicionEstacion("Cara", new Costo(200, 0), 1, 1);

            var resultado = _nivel.Construccion.IntentarColocar(cara, NivelDePrueba.SueloLibre);

            Assert.IsFalse(resultado.EsExito);
            Assert.AreEqual(ResultadoColocacion.CreditosInsuficientes, resultado.Error);
            Assert.AreEqual("Créditos insuficientes", resultado.Error.Mensaje());
        }

        [Test]
        public void PA5_UnaCasillaDeSueloLibreConSaldoEsValida()
        {
            Assert.AreEqual(ResultadoColocacion.Valida, _nivel.Construccion.Validar(_camara, NivelDePrueba.SueloLibre));
            Assert.AreEqual(ResultadoColocacion.FueraDelMapa, _nivel.Construccion.Validar(_camara, new Posicion(-1, 0)));
        }

        [Test]
        public void PA6_UnIntentoRechazadoNoDescuentaCreditos()
        {
            _nivel.Construccion.IntentarColocar(_camara, new Posicion(3, 2));

            Assert.AreEqual(100, _nivel.Creditos.Agua);
            Assert.AreEqual(100, _nivel.Creditos.Energia);
            Assert.AreEqual(0, _nivel.Construccion.Estaciones.Count);
        }

        [Test]
        public void CadaResultadoTieneUnMensajeParaElJugador()
        {
            foreach (ResultadoColocacion resultado in Enum.GetValues(typeof(ResultadoColocacion)))
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(resultado.Mensaje()), resultado.ToString());
            }
        }
    }
}
