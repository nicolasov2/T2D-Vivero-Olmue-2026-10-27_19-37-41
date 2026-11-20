using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using T2D.Dominio;

namespace T2D.Tests
{
    /// <summary>UT 1688 Olas de calor.</summary>
    public class OlasDeCalorTests
    {
        private Nivel _nivel;

        [SetUp]
        public void Preparar() => _nivel = NivelDePrueba.Construir();

        [Test]
        public void PA1_LaOlaApareceEnLaEntradaDeLaRuta()
        {
            var ola = NuevaOla(2f, 10);

            Assert.AreEqual(_nivel.Rutas[0].Entrada, ola.Recorrido.CasillaActual);
        }

        [Test]
        public void PA2_LaOlaAvanzaALaVelocidadDeSuConfiguracion()
        {
            var ola = NuevaOla(2f, 10);

            ola.Avanzar(1f, _nivel);

            Assert.AreEqual(2, ola.Recorrido.IndiceActual);
        }

        [Test]
        public void PA3_AlLlegarAlSilo_DescuentaSuDanioYTermina()
        {
            var ola = NuevaOla(2f, 10);

            var avance = ola.Avanzar(10f, _nivel);
            ola.Avanzar(10f, _nivel);

            Assert.AreEqual(AvanceEvento.LlegoAlSilo, avance);
            Assert.IsTrue(ola.Termino);
            Assert.AreEqual(40, _nivel.Silo.Agua);
        }

        [Test]
        public void PA4_VelocidadYDanioSeCambianEnLaConfiguracion()
        {
            var lenta = NuevaOla(1f, 25);

            lenta.Avanzar(1f, _nivel);
            lenta.Avanzar(20f, _nivel);

            Assert.AreEqual(25, _nivel.Silo.Agua);
        }

        [Test]
        public void LaOlaSoloEntraEnCasillasDeSuRuta()
        {
            var ola = NuevaOla(2f, 10);
            var casillasDeRuta = new HashSet<Posicion>(ola.Recorrido.Ruta.Puntos);

            var visitadas = ola.Recorrido.Avanzar(20f);

            Assert.IsTrue(ola.Recorrido.HaLlegado);
            Assert.IsTrue(visitadas.All(casilla => casillasDeRuta.Contains(casilla)));
        }

        [Test]
        public void EnElSprint1LaOlaNoDaniaEstaciones()
        {
            var estacion = _nivel.Construccion.IntentarColocar(NivelDePrueba.Camara(), NivelDePrueba.SueloLibre).Valor;

            NuevaOla(2f, 10).Avanzar(10f, _nivel);

            Assert.AreEqual(estacion.Integridad.Maxima, estacion.Integridad.Actual);
            Assert.AreSame(estacion, _nivel.Construccion.EstacionEn(NivelDePrueba.SueloLibre));
            Assert.IsTrue(_nivel.Cuadricula.Obtener(NivelDePrueba.SueloLibre).EstaOcupada);
        }

        [Test]
        public void Temporizador_DiceCuantasOlasTocanSegunElTiempo()
        {
            var temporizador = new TemporizadorDeAparicion(2f);

            Assert.AreEqual(0, temporizador.Avanzar(1f));
            Assert.AreEqual(1, temporizador.Avanzar(1f));
            Assert.AreEqual(2, temporizador.Avanzar(5f));
            Assert.AreEqual(1, temporizador.Avanzar(1f));
        }

        [Test]
        public void Temporizador_DiceCuantoFaltaParaElSiguiente()
        {
            var temporizador = new TemporizadorDeAparicion(4f);

            temporizador.Avanzar(1.5f);
            Assert.AreEqual(2.5f, temporizador.SegundosParaElSiguiente, 0.0001f);

            temporizador.Avanzar(3f);
            Assert.AreEqual(3.5f, temporizador.SegundosParaElSiguiente, 0.0001f);
        }

        [Test]
        public void DefinicionConVelocidadCero_Lanza()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DefinicionEvento("Ola", 0f, 1, new EfectoOlaDeCalor()));
        }

        private EventoEstres NuevaOla(float velocidad, int danio) =>
            new EventoEstres(new DefinicionEvento("Ola de calor", velocidad, danio, new EfectoOlaDeCalor()), _nivel.Rutas[0]);
    }
}
