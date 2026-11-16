using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using T2D.Dominio;

namespace T2D.Tests
{
    /// <summary>UT 1680 Rutas: PA 1209 to 1212.</summary>
    public class RutasTests
    {
        [Test]
        public void PA1209_CadaNivelTieneUnaRutaDesdeLaEntradaHastaElSilo()
        {
            var nivel = NivelDePrueba.Construir();

            Assert.AreEqual(1, nivel.Rutas.Count);
            Assert.AreEqual(new Posicion(0, 2), nivel.Rutas[0].Entrada);
            Assert.AreEqual(nivel.Silo.Posicion, nivel.Rutas[0].Final);
        }

        [Test]
        public void PA1210_LasCasillasDeRutaSonDeOtroTipoQueElSuelo()
        {
            var nivel = NivelDePrueba.Construir();
            var tramo = nivel.Rutas[0].Puntos.Take(nivel.Rutas[0].Puntos.Count - 1);

            foreach (var punto in tramo)
            {
                Assert.AreEqual(TipoCasilla.Ruta, nivel.Cuadricula.Obtener(punto).Tipo);
            }
        }

        [Test]
        public void PA1211_ConVelocidad2_RecorreCuatroCasillasEnDosSegundos()
        {
            var recorrido = new RecorridoDeRuta(NivelDePrueba.Construir().Rutas[0], 2f);

            var nuevas = recorrido.Avanzar(2f);

            CollectionAssert.AreEqual(NivelDePrueba.Fila(2, 1, 4), nuevas);
            Assert.AreEqual(4, recorrido.IndiceActual);
        }

        [Test]
        public void PA1211_PasaPorTodasLasCasillasEnOrdenSinSalirseHastaElSilo()
        {
            var ruta = NivelDePrueba.Construir().Rutas[0];
            var recorrido = new RecorridoDeRuta(ruta, 1.5f);
            var visitadas = new List<Posicion>();

            for (var paso = 0; paso < 200 && !recorrido.HaLlegado; paso++)
            {
                visitadas.AddRange(recorrido.Avanzar(0.05f));
            }

            Assert.IsTrue(recorrido.HaLlegado);
            CollectionAssert.AreEqual(ruta.Puntos.Skip(1).ToList(), visitadas);
        }

        [Test]
        public void PA1211_EntreDosCasillas_LaPosicionSeInterpola()
        {
            var recorrido = new RecorridoDeRuta(NivelDePrueba.Construir().Rutas[0], 1f);

            recorrido.Avanzar(0.5f);

            Assert.AreEqual(0.5f, recorrido.PosicionInterpolada.X, 0.0001f);
            Assert.AreEqual(2f, recorrido.PosicionInterpolada.Y, 0.0001f);
        }

        [Test]
        public void PA1212_CambiarLaRutaEnLaConfiguracion_CambiaElRecorrido()
        {
            var datos = NivelDePrueba.Datos();
            var porFila3 = NivelDePrueba.Fila(3, 0, 7).Concat(new[] { NivelDePrueba.Silo }).ToArray();
            datos.Rutas = new[] { porFila3 };

            var nivel = NivelDePrueba.Construir(datos);

            Assert.AreEqual(new Posicion(1, 3), nivel.Rutas[0].Puntos[1]);
            Assert.AreEqual(TipoCasilla.Ruta, nivel.Cuadricula.Obtener(new Posicion(3, 3)).Tipo);
            Assert.AreEqual(TipoCasilla.SueloEdificable, nivel.Cuadricula.Obtener(new Posicion(3, 2)).Tipo);
        }

        [Test]
        public void Ruta_QuePasaDosVecesPorUnaCasilla_Falla()
        {
            var puntos = new[] { new Posicion(0, 0), new Posicion(1, 0), new Posicion(0, 0) };

            var resultado = Ruta.Crear(puntos, new LimitesDelMapa(3, 3));

            Assert.IsFalse(resultado.EsExito);
            StringAssert.Contains("dos veces", resultado.Error);
        }

        [Test]
        public void Recorrido_VelocidadCeroOTiempoNegativo_Lanza()
        {
            var ruta = NivelDePrueba.Construir().Rutas[0];

            Assert.Throws<ArgumentOutOfRangeException>(() => new RecorridoDeRuta(ruta, 0f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RecorridoDeRuta(ruta, 1f).Avanzar(-1f));
        }
    }
}
