using NUnit.Framework;
using T2D.Dominio;

namespace T2D.Tests
{
    /// <summary>UT 1690 Deshidratación. Palto row at (2,0)-(3,0), citrus row at (4,4)-(5,4), route on row 2.</summary>
    public class DeshidratacionTests
    {
        private Nivel _nivel;
        private PasilloDePlantines _palto;
        private PasilloDePlantines _citrico;

        [SetUp]
        public void Preparar()
        {
            _nivel = NivelDePrueba.Construir();
            _palto = _nivel.Pasillos.Pasillos[0];
            _citrico = _nivel.Pasillos.Pasillos[1];
        }

        [Test]
        public void PA1_ElVectorApareceEnLaEntradaYAvanzaASuVelocidad()
        {
            var vector = NuevoVector(new EfectoDeshidratacion(2, 10));

            Assert.AreEqual(_nivel.Rutas[0].Entrada, vector.Recorrido.CasillaActual);
            vector.Avanzar(1f, _nivel);
            Assert.AreEqual(1, vector.Recorrido.IndiceActual);
        }

        [Test]
        public void PA2_SoloBajaLaHidratacionDeLosPasillosAlAlcance()
        {
            var vector = NuevoVector(new EfectoDeshidratacion(2, 10));

            vector.Avanzar(1f, _nivel);

            Assert.AreEqual(90, _palto.Hidratacion.Actual);
            Assert.AreEqual(Hidratacion.Maxima, _citrico.Hidratacion.Actual);
        }

        [Test]
        public void PA2_AlRecorrerTodaLaRuta_CadaCasillaAlAlcanceRestaUnaVez()
        {
            NuevoVector(new EfectoDeshidratacion(2, 10)).Avanzar(20f, _nivel);

            Assert.AreEqual(50, _palto.Hidratacion.Actual);
            Assert.AreEqual(40, _citrico.Hidratacion.Actual);
        }

        [Test]
        public void PA3_LaHidratacionNuncaBajaDeCero()
        {
            NuevoVector(new EfectoDeshidratacion(2, 60)).Avanzar(20f, _nivel);

            Assert.AreEqual(0, _palto.Hidratacion.Actual);
            Assert.AreEqual(0f, _palto.Hidratacion.Fraccion, 0.0001f);
        }

        [Test]
        public void PA4_AlLlegarAlSilo_DescuentaSuDanio()
        {
            NuevoVector(new EfectoDeshidratacion(2, 10)).Avanzar(20f, _nivel);

            Assert.AreEqual(45, _nivel.Silo.Agua);
        }

        [Test]
        public void PA5_ConAlcanceCeroNoAlcanzaNingunPasillo()
        {
            NuevoVector(new EfectoDeshidratacion(0, 10)).Avanzar(20f, _nivel);

            Assert.AreEqual(Hidratacion.Maxima, _palto.Hidratacion.Actual);
            Assert.AreEqual(Hidratacion.Maxima, _citrico.Hidratacion.Actual);
        }

        private EventoEstres NuevoVector(EfectoDeshidratacion efecto) =>
            new EventoEstres(new DefinicionEvento("Vector de deshidratación", 1f, 5, efecto), _nivel.Rutas[0]);

        [Test]
        public void ElPasilloQuedaSecoDesdeElUmbral()
        {
            var hidratacion = new Hidratacion();

            hidratacion.Deshidratar(Hidratacion.Maxima - Hidratacion.UmbralSeco - 1);
            Assert.IsFalse(hidratacion.EstaSeca);

            hidratacion.Deshidratar(1);
            Assert.IsTrue(hidratacion.EstaSeca);
        }

        [Test]
        public void CadaEfectoDiceSuAlcance()
        {
            Assert.AreEqual(0, new EfectoOlaDeCalor().Alcance);
            Assert.AreEqual(2, new EfectoViento(2, 5).Alcance);
            Assert.AreEqual(1, new EfectoDeshidratacion(1, 10).Alcance);
        }
    }
}
