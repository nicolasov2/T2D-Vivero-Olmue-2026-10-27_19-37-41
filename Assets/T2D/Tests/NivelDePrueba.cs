using System.Collections.Generic;
using System.Linq;
using T2D.Dominio;

namespace T2D.Tests
{
    /// <summary>
    /// Small 8x5 level shared by the tests:
    /// route along row 2 from (0,2) to the silo at (7,2); palto row at (2,0)-(3,0);
    /// citrus row at (4,4)-(5,4); blocked cell (0,0); 100 water and 100 energy; silo with 50 water.
    /// </summary>
    internal static class NivelDePrueba
    {
        public static readonly Posicion Silo = new Posicion(7, 2);
        public static readonly Posicion SueloLibre = new Posicion(3, 1);

        public static DatosDeNivel Datos() => new DatosDeNivel
        {
            Nombre = "Prueba",
            Ancho = 8,
            Alto = 5,
            PosicionSilo = Silo,
            CapacidadSilo = 50,
            CreditosIniciales = new Costo(100, 100),
            Rutas = new[] { Fila(2, 0, 7) },
            Pasillos = new[]
            {
                new DatosDePasillo(Especie.Palto, new Posicion(2, 0), 2, 1),
                new DatosDePasillo(Especie.Citrico, new Posicion(4, 4), 2, 1),
            },
            Bloqueadas = new[] { new Posicion(0, 0) },
        };

        public static Nivel Construir() => Construir(Datos());

        public static Nivel Construir(DatosDeNivel datos) => ConstructorDeNivel.Construir(datos).Valor;

        public static DefinicionEstacion Camara() => new DefinicionEstacion("Cámara SBC", new Costo(60, 40), 2, 3);

        public static IReadOnlyList<Posicion> Fila(int y, int desdeX, int hastaX) =>
            Enumerable.Range(desdeX, hastaX - desdeX + 1).Select(x => new Posicion(x, y)).ToArray();
    }
}
