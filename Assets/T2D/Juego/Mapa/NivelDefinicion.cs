using System.Collections.Generic;
using System.Linq;
using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>
    /// Single source of truth of a level: size, routes, seedling rows, blocked cells, silo and starting
    /// credits. Coordinates use x = column and y = row counted from the top, like the mockups.
    /// A new asset starts with the Olmué nursery of the mockups.
    /// </summary>
    [CreateAssetMenu(menuName = "T2D/Nivel", fileName = "Nivel")]
    public sealed class NivelDefinicion : ScriptableObject
    {
        [SerializeField] private string nombre = "Vivero de Olmué";
        [SerializeField, Min(1)] private int ancho = 16;
        [SerializeField, Min(1)] private int alto = 9;
        [SerializeField] private Vector2Int silo = new Vector2Int(15, 4);
        [SerializeField, Min(1)] private int capacidadSilo = 200;
        [SerializeField, Min(0)] private int aguaInicial = 300;
        [SerializeField, Min(0)] private int energiaInicial = 150;
        [SerializeField] private List<RutaSerializada> rutas = NivelDeEjemplo.Rutas();
        [SerializeField] private List<PasilloSerializado> pasillos = NivelDeEjemplo.Pasillos();
        [SerializeField] private List<Vector2Int> bloqueadas = NivelDeEjemplo.Bloqueadas();

        /// <summary>Rows of the map (needed to draw the routes in edit mode).</summary>
        public int Alto => alto;

        /// <summary>Routes as edited.</summary>
        public IReadOnlyList<RutaSerializada> Rutas => rutas;

        /// <summary>Unvalidated domain data; <see cref="ConstructorDeNivel"/> checks it.</summary>
        public DatosDeNivel ADatos() => new DatosDeNivel
        {
            Nombre = nombre,
            Ancho = ancho,
            Alto = alto,
            PosicionSilo = APosicion(silo),
            CapacidadSilo = capacidadSilo,
            CreditosIniciales = new Costo(aguaInicial, energiaInicial),
            Rutas = rutas.Select(ruta => (IReadOnlyList<Posicion>)ruta.Puntos.Select(APosicion).ToList()).ToList(),
            Pasillos = pasillos.Select(pasillo => pasillo.ADatos()).ToList(),
            Bloqueadas = bloqueadas.Select(APosicion).ToList(),
        };

        private static Posicion APosicion(Vector2Int celda) => new Posicion(celda.x, celda.y);
    }
}
