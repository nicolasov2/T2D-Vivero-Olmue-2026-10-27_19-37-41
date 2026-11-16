using System;
using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1686. A seedling row as edited in the inspector: species, top-left cell and size.</summary>
    [Serializable]
    public sealed class PasilloSerializado
    {
        [SerializeField] private Especie especie = Especie.Palto;
        [SerializeField] private Vector2Int desde;
        [SerializeField] private Vector2Int tamano = Vector2Int.one;

        /// <summary>Default row, required by the Unity serializer.</summary>
        public PasilloSerializado()
        {
        }

        /// <summary>Row of the given species covering a rectangle.</summary>
        public PasilloSerializado(Especie especie, Vector2Int desde, Vector2Int tamano)
        {
            this.especie = especie;
            this.desde = desde;
            this.tamano = tamano;
        }

        /// <summary>Domain data of the row.</summary>
        public DatosDePasillo ADatos() => new DatosDePasillo(especie, new Posicion(desde.x, desde.y), tamano.x, tamano.y);
    }
}
