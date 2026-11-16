using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>Paints the tilemap from the level and converts between screen, world and cells.</summary>
    [DisallowMultipleComponent]
    public sealed class VistaMapa : MonoBehaviour, IParteDePartida
    {
        private Partida _partida;

        private int Alto => _partida.Nivel.Cuadricula.Alto;

        /// <inheritdoc />
        public void Inicializar(Partida partida)
        {
            _partida = partida;
            Pintar();
        }

        /// <summary>Tilemap cell of a domain cell.</summary>
        public Vector3Int ACelda(Posicion posicion) => Coordenadas.ACelda(posicion, Alto);

        /// <summary>World center of a cell.</summary>
        public Vector3 CentroDe(Posicion posicion) => _partida.Grid.GetCellCenterWorld(ACelda(posicion));

        /// <summary>World point of a position between cells, in cell units (used to move events smoothly).</summary>
        public Vector3 CentroDe(float x, float y)
        {
            var local = new Vector3(x + 0.5f, Alto - 1 - y + 0.5f, 0f);
            return _partida.Grid.LocalToWorld(Vector3.Scale(local, _partida.Grid.cellSize));
        }

        /// <summary>Cell under a screen point; false when the point is outside the map.</summary>
        public bool IntentarObtenerPosicion(Vector2 posicionEnPantalla, out Posicion posicion)
        {
            var camara = _partida.Camara;
            var mundo = camara.ScreenToWorldPoint(new Vector3(posicionEnPantalla.x, posicionEnPantalla.y, -camara.transform.position.z));
            posicion = Coordenadas.APosicion(_partida.Grid.WorldToCell(mundo), Alto);
            return _partida.Nivel.Cuadricula.Contiene(posicion);
        }

        private void Pintar()
        {
            var nivel = _partida.Nivel;
            _partida.Tilemap.ClearAllTiles();
            foreach (var casilla in nivel.Cuadricula.Casillas())
            {
                var pasillo = nivel.Pasillos.PasilloEn(casilla.Posicion);
                var especie = pasillo == null ? Especie.Palto : pasillo.Especie;
                _partida.Tilemap.SetTile(ACelda(casilla.Posicion), _partida.Paleta.TileDe(casilla.Tipo, especie));
            }
        }
    }
}
