using NUnit.Framework;
using T2D.Dominio;

namespace T2D.Tests
{
    /// <summary>UT 1787 Selección de casilla y despliegue de infraestructura.</summary>
    public class SeleccionDeCasillaTests
    {
        private Nivel _nivel;
        private DefinicionEstacion _camara;
        private SelectorDeCasilla _selector;

        [SetUp]
        public void Preparar()
        {
            _nivel = NivelDePrueba.Construir();
            _camara = NivelDePrueba.Camara();
            _selector = new SelectorDeCasilla();
        }

        [Test]
        public void PA1_TocarUnaCasilla_LaSelecciona()
        {
            _selector.Seleccionar(NivelDePrueba.SueloLibre);

            var estado = _selector.Estado as CasillaSeleccionada;
            Assert.IsNotNull(estado);
            Assert.AreEqual(NivelDePrueba.SueloLibre, estado.Posicion);
        }

        [Test]
        public void PA2_ElegirEstacionConCasillaSeleccionada_LaPrevisualiza()
        {
            _selector.Seleccionar(NivelDePrueba.SueloLibre);
            _selector.ElegirEstacion(_camara);

            var estado = _selector.Estado as Previsualizando;
            Assert.IsNotNull(estado);
            Assert.AreEqual(NivelDePrueba.SueloLibre, estado.Posicion);
            Assert.AreSame(_camara, estado.Definicion);
        }

        [Test]
        public void PA2_ElegirEstacionYLuegoTocarCasilla_TambienPrevisualiza()
        {
            _selector.ElegirEstacion(_camara);
            _selector.Seleccionar(NivelDePrueba.SueloLibre);

            Assert.IsInstanceOf<Previsualizando>(_selector.Estado);
        }

        [Test]
        public void PA3_Cancelar_QuitaLaPrevisualizacionSinConstruirNiCobrar()
        {
            _selector.Seleccionar(NivelDePrueba.SueloLibre);
            _selector.ElegirEstacion(_camara);

            _selector.Cancelar();

            Assert.AreSame(SinSeleccion.Instancia, _selector.Estado);
            Assert.AreEqual(0, _nivel.Construccion.Estaciones.Count);
            Assert.AreEqual(100, _nivel.Creditos.Agua);
        }

        [Test]
        public void PA4_Confirmar_ConstruyeUnaVezYReiniciaLaSeleccion()
        {
            _selector.Seleccionar(NivelDePrueba.SueloLibre);
            _selector.ElegirEstacion(_camara);

            var resultado = _selector.Confirmar(_nivel.Construccion);

            Assert.IsTrue(resultado.EsExito);
            Assert.AreEqual(1, _nivel.Construccion.Estaciones.Count);
            Assert.AreEqual(NivelDePrueba.SueloLibre, _nivel.Construccion.Estaciones[0].Posicion);
            Assert.AreSame(SinSeleccion.Instancia, _selector.Estado);
        }

        [Test]
        public void ConfirmarSinPrevisualizacion_FallaYNoCambiaElEstado()
        {
            _selector.Seleccionar(NivelDePrueba.SueloLibre);

            var resultado = _selector.Confirmar(_nivel.Construccion);

            Assert.AreEqual(ResultadoColocacion.SinEstacionElegida, resultado.Error);
            Assert.IsInstanceOf<CasillaSeleccionada>(_selector.Estado);
        }

        [Test]
        public void ConfirmarEnCasillaInvalida_FallaYMantieneLaPrevisualizacion()
        {
            _selector.Seleccionar(new Posicion(3, 2));
            _selector.ElegirEstacion(_camara);

            var resultado = _selector.Confirmar(_nivel.Construccion);

            Assert.AreEqual(ResultadoColocacion.EsRuta, resultado.Error);
            Assert.IsInstanceOf<Previsualizando>(_selector.Estado);
        }
    }
}
