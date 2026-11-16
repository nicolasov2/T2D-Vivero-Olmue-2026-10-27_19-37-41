using System;
using System.Collections.Generic;
using T2D.Dominio;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace T2D.Juego
{
    /// <summary>UT 1690. Tints each seedling row from green to dry brown as its hydration drops.</summary>
    [RequireComponent(typeof(Partida))]
    public sealed class VistaPasillos : MonoBehaviour, IParteDePartida
    {
        [SerializeField] private Color colorSeco = new Color(0.78f, 0.62f, 0.36f);

        private readonly List<(Hidratacion hidratacion, Action alCambiar)> _suscripciones = new List<(Hidratacion, Action)>();
        private Partida _partida;

        /// <inheritdoc />
        public void Inicializar(Partida partida)
        {
            _partida = partida;
            foreach (var pasillo in partida.Nivel.Pasillos.Pasillos)
            {
                var actual = pasillo;
                Action alCambiar = () => Colorear(actual);
                pasillo.Hidratacion.Cambio += alCambiar;
                _suscripciones.Add((pasillo.Hidratacion, alCambiar));
            }
        }

        private void Colorear(PasilloDePlantines pasillo)
        {
            var color = Color.Lerp(colorSeco, ColorDeEspecie(pasillo.Especie), pasillo.Hidratacion.Fraccion);
            var mapa = GetComponent<VistaMapa>();
            foreach (var casilla in pasillo.Casillas)
            {
                var celda = mapa.ACelda(casilla);
                _partida.Tilemap.SetTileFlags(celda, TileFlags.None);
                _partida.Tilemap.SetColor(celda, color);
            }
        }

        // Tilemap.SetColor replaces the tile tint instead of multiplying it, so the healthy end of
        // the gradient is the species tile color, not white.
        private Color ColorDeEspecie(Especie especie) =>
            _partida.Paleta.TileDe(TipoCasilla.Plantines, especie) is Tile tile ? tile.color : Color.white;

        private void OnDestroy()
        {
            foreach (var (hidratacion, alCambiar) in _suscripciones)
            {
                hidratacion.Cambio -= alCambiar;
            }
        }
    }
}
