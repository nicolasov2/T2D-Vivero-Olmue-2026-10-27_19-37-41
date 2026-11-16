using NUnit.Framework;
using T2D.Dominio;

namespace T2D.Tests
{
    /// <summary>UT 1766 Desmantelar o reciclar estación.</summary>
    public class DesmantelarTests
    {
        private Nivel _nivel;
        private Estacion _estacion;

        [SetUp]
        public void Preparar()
        {
            _nivel = NivelDePrueba.Construir();
            _estacion = _nivel.Construccion.IntentarColocar(NivelDePrueba.Camara(), NivelDePrueba.SueloLibre).Valor;
        }

        [Test]
        public void PA1_ElPanelMuestraLosCreditosQueSeRecuperan()
        {
            Assert.AreEqual(new Costo(30, 22), CalculadoraDeReembolso.Reembolso(_estacion.Definicion.Costo));
        }

        [Test]
        public void PA2_AlDesmantelar_LaEstacionDesaparece()
        {
            var desmanteladas = 0;
            _nivel.Construccion.EstacionDesmantelada += _ => desmanteladas++;

            _nivel.Construccion.Desmantelar(_estacion);

            Assert.AreEqual(0, _nivel.Construccion.Estaciones.Count);
            Assert.IsNull(_nivel.Construccion.EstacionEn(NivelDePrueba.SueloLibre));
            Assert.AreEqual(1, desmanteladas);
        }

        [Test]
        public void PA3_SeSumaLaMitadDelCostoRedondeadaHaciaAbajo()
        {
            var resultado = _nivel.Construccion.Desmantelar(_estacion);

            Assert.AreEqual(new Costo(30, 22), resultado.Valor);
            Assert.AreEqual(70, _nivel.Creditos.Agua);
            Assert.AreEqual(77, _nivel.Creditos.Energia);
        }

        [Test]
        public void PA4_LaCasillaQuedaLibreParaVolverAConstruir()
        {
            _nivel.Construccion.Desmantelar(_estacion);

            Assert.IsFalse(_nivel.Cuadricula.Obtener(NivelDePrueba.SueloLibre).EstaOcupada);
            Assert.AreEqual(ResultadoColocacion.Valida, _nivel.Construccion.Validar(NivelDePrueba.Camara(), NivelDePrueba.SueloLibre));
        }

        [Test]
        public void DesmantelarDosVeces_LaSegundaFalla()
        {
            _nivel.Construccion.Desmantelar(_estacion);

            var segunda = _nivel.Construccion.Desmantelar(_estacion);

            Assert.IsFalse(segunda.EsExito);
            Assert.AreEqual(70, _nivel.Creditos.Agua);
        }

        [TestCase(60, 45, 30, 22)]
        [TestCase(1, 1, 0, 0)]
        [TestCase(0, 0, 0, 0)]
        [TestCase(99, 100, 49, 50)]
        public void Reembolso_EsLaMitadRedondeadaHaciaAbajo(int agua, int energia, int aguaEsperada, int energiaEsperada)
        {
            Assert.AreEqual(new Costo(aguaEsperada, energiaEsperada), CalculadoraDeReembolso.Reembolso(new Costo(agua, energia)));
        }
    }
}
