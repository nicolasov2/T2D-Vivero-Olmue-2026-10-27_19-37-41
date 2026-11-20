using System;
using System.Collections.Generic;
using T2D.Dominio;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace T2D.Juego
{
    /// <summary>
    /// UT 1686 and 1690. Draws a seedling on every cell of each row and tints the row and its
    /// seedlings from healthy to dry brown as its hydration drops.
    /// </summary>
    [RequireComponent(typeof(Partida))]
    public sealed class VistaPasillos : MonoBehaviour, IParteDePartida
    {
        private const float EscalaPlantin = 0.9f;

        [SerializeField] private Color colorSeco = new Color(0.78f, 0.62f, 0.36f);
        [SerializeField] private Color colorPlantinSeco = new Color(0.55f, 0.4f, 0.22f);

        private readonly List<(Hidratacion hidratacion, Action alCambiar)> _suscripciones = new List<(Hidratacion, Action)>();
        private readonly Dictionary<PasilloDePlantines, List<SpriteRenderer>> _plantines = new Dictionary<PasilloDePlantines, List<SpriteRenderer>>();
        private Partida _partida;

        /// <inheritdoc />
        public void Inicializar(Partida partida)
        {
            _partida = partida;
            foreach (var pasillo in partida.Nivel.Pasillos.Pasillos)
            {
                _plantines[pasillo] = CrearPlantines(pasillo);
                var actual = pasillo;
                Action alCambiar = () => Colorear(actual);
                pasillo.Hidratacion.Cambio += alCambiar;
                _suscripciones.Add((pasillo.Hidratacion, alCambiar));
            }
        }

        private List<SpriteRenderer> CrearPlantines(PasilloDePlantines pasillo)
        {
            // Built from the grid directly: VistaMapa may start after this component.
            var alto = _partida.Nivel.Cuadricula.Alto;
            var sprite = SpritesProcedurales.Plantin(pasillo.Especie);
            var plantines = new List<SpriteRenderer>(pasillo.CantidadDePlantines);
            foreach (var casilla in pasillo.Casillas)
            {
                var plantin = FabricaDeSprites.Crear(_partida, $"Plantín {casilla}", sprite, OrdenDeDibujo.Plantin);
                plantin.transform.position = _partida.Grid.GetCellCenterWorld(Coordenadas.ACelda(casilla, alto));
                plantin.transform.localScale = new Vector3(EscalaPlantin, EscalaPlantin, 1f);
                plantin.gameObject.SetActive(true);
                plantines.Add(plantin);
            }

            return plantines;
        }

        private void Colorear(PasilloDePlantines pasillo)
        {
            var fraccion = pasillo.Hidratacion.Fraccion;
            var color = Color.Lerp(colorSeco, ColorDeEspecie(pasillo.Especie), fraccion);
            var mapa = GetComponent<VistaMapa>();
            foreach (var casilla in pasillo.Casillas)
            {
                var celda = mapa.ACelda(casilla);
                _partida.Tilemap.SetTileFlags(celda, TileFlags.None);
                _partida.Tilemap.SetColor(celda, color);
            }

            var colorPlantin = Color.Lerp(colorPlantinSeco, Color.white, fraccion);
            foreach (var plantin in _plantines[pasillo])
            {
                plantin.color = colorPlantin;
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
