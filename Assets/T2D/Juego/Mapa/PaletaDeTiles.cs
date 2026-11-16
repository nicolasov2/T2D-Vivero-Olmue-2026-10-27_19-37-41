using System;
using T2D.Dominio;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace T2D.Juego
{
    /// <summary>Tile used to paint each kind of cell.</summary>
    [CreateAssetMenu(menuName = "T2D/Paleta de tiles", fileName = "PaletaDeTiles")]
    public sealed class PaletaDeTiles : ScriptableObject
    {
        [SerializeField] private TileBase suelo = null;
        [SerializeField] private TileBase ruta = null;
        [SerializeField] private TileBase palto = null;
        [SerializeField] private TileBase citrico = null;
        [SerializeField] private TileBase bloqueada = null;
        [SerializeField] private TileBase silo = null;

        /// <summary>Tile for a cell; seedling cells need the species of their row.</summary>
        public TileBase TileDe(TipoCasilla tipo, Especie especie) => tipo switch
        {
            TipoCasilla.SueloEdificable => suelo,
            TipoCasilla.Ruta => ruta,
            TipoCasilla.Plantines => especie == Especie.Palto ? palto : citrico,
            TipoCasilla.Bloqueada => bloqueada,
            TipoCasilla.Silo => silo,
            _ => throw new ArgumentOutOfRangeException(nameof(tipo), tipo, "Tipo de casilla sin tile."),
        };
    }
}
