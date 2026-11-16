using NUnit.Framework;
using T2D.Dominio;

namespace T2D.Tests
{
    /// <summary>UT 1687 Colocar cámara fija SBC.</summary>
    public class ColocarCamaraTests
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
        public void PA1_ElCostoSeMuestraEnAguaYEnergia()
        {
            Assert.AreEqual("60 agua · 45 energía", _camara.Costo.ToString());
        }

        [Test]
        public void PA2_EnSueloLibreConSaldo_LaCamaraApareceEnEsaCasilla()
        {
            var colocadas = 0;
            _nivel.Construccion.EstacionColocada += _ => colocadas++;

            var resultado = _nivel.Construccion.IntentarColocar(_camara, NivelDePrueba.SueloLibre);

            Assert.IsTrue(resultado.EsExito);
            Assert.AreEqual(NivelDePrueba.SueloLibre, resultado.Valor.Posicion);
            Assert.AreEqual(1, colocadas);
        }

        [Test]
        public void PA3_AlInstalarSeDescuentaSuCosto()
        {
            _nivel.Construccion.IntentarColocar(_camara, NivelDePrueba.SueloLibre);

            Assert.AreEqual(40, _nivel.Creditos.Agua);
            Assert.AreEqual(55, _nivel.Creditos.Energia);
        }

        [Test]
        public void PA4_LaCasillaQuedaOcupada()
        {
            _nivel.Construccion.IntentarColocar(_camara, NivelDePrueba.SueloLibre);

            Assert.IsTrue(_nivel.Cuadricula.Obtener(NivelDePrueba.SueloLibre).EstaOcupada);
        }

        [Test]
        public void PA5_AlSeleccionarLaCamaraSeConoceSuRadio()
        {
            _nivel.Construccion.IntentarColocar(_camara, NivelDePrueba.SueloLibre);

            var seleccionada = _nivel.Construccion.EstacionEn(NivelDePrueba.SueloLibre);

            Assert.IsNotNull(seleccionada);
            Assert.AreEqual(2, seleccionada.Definicion.RadioCobertura);
        }

        [Test]
        public void EnUnaCasillaSinEstacionNoHayNadaQueSeleccionar()
        {
            Assert.IsNull(_nivel.Construccion.EstacionEn(NivelDePrueba.SueloLibre));
        }
    }
}
