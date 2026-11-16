using System;
using NUnit.Framework;
using T2D.Dominio;

namespace T2D.Tests
{
    /// <summary>UT 1686 Puestos de plantas.</summary>
    public class PuestosDePlantasTests
    {
        [Test]
        public void PA1_SeCreanTodosLosPasillosDeLaConfiguracion()
        {
            var pasillos = NivelDePrueba.Construir().Pasillos.Pasillos;

            Assert.AreEqual(2, pasillos.Count);
            Assert.AreEqual(Especie.Palto, pasillos[0].Especie);
            Assert.AreEqual(Especie.Citrico, pasillos[1].Especie);
        }

        [Test]
        public void PA2_PaltoYCitricoSonPlantinesDeDistintaEspecie()
        {
            var nivel = NivelDePrueba.Construir();
            var palto = nivel.Pasillos.PasilloEn(new Posicion(2, 0));
            var citrico = nivel.Pasillos.PasilloEn(new Posicion(4, 4));

            Assert.AreEqual(TipoCasilla.Plantines, nivel.Cuadricula.Obtener(new Posicion(2, 0)).Tipo);
            Assert.IsNotNull(palto);
            Assert.IsNotNull(citrico);
            Assert.AreNotEqual(palto.Especie, citrico.Especie);
        }

        [Test]
        public void PA3_ElRotuloTieneEspecieYCantidadDePlantines()
        {
            var pasillo = NivelDePrueba.Construir().Pasillos.PasilloEn(new Posicion(3, 0));

            Assert.IsNotNull(pasillo);
            Assert.AreEqual(Especie.Palto, pasillo.Especie);
            Assert.AreEqual(2, pasillo.CantidadDePlantines);
        }

        [Test]
        public void PA3_EnUnaCasillaSinPlantinesNoHayRotulo()
        {
            Assert.IsNull(NivelDePrueba.Construir().Pasillos.PasilloEn(new Posicion(6, 4)));
        }

        [Test]
        public void PA4_MoverUnPasilloEnLaConfiguracion_CambiaElMapa()
        {
            var datos = NivelDePrueba.Datos();
            datos.Pasillos = new[] { new DatosDePasillo(Especie.Palto, new Posicion(5, 0), 2, 1) };

            var cuadricula = NivelDePrueba.Construir(datos).Cuadricula;

            Assert.AreEqual(TipoCasilla.Plantines, cuadricula.Obtener(new Posicion(5, 0)).Tipo);
            Assert.AreEqual(TipoCasilla.SueloEdificable, cuadricula.Obtener(new Posicion(2, 0)).Tipo);
        }

        [Test]
        public void PasilloSobreUnaRuta_SeRechaza()
        {
            var datos = NivelDePrueba.Datos();
            datos.Pasillos = new[] { new DatosDePasillo(Especie.Citrico, new Posicion(3, 1), 1, 2) };

            var resultado = ConstructorDeNivel.Construir(datos);

            Assert.IsFalse(resultado.EsExito);
            StringAssert.Contains("(3, 2)", resultado.Error);
        }

        [Test]
        public void PasilloDeTamanoCero_Lanza()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DatosDePasillo(Especie.Palto, new Posicion(0, 0), 0, 1));
        }
    }
}
