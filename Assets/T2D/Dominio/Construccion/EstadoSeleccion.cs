#nullable enable
namespace T2D.Dominio
{
    /// <summary>
    /// UT 1787. What the player has selected before building. The design lists three states; a
    /// fourth one (<see cref="EstacionElegida"/>) covers choosing the station in the panel before
    /// touching a cell, which is the flow of UT 1687.
    /// </summary>
    public abstract class EstadoSeleccion
    {
        private protected EstadoSeleccion()
        {
        }
    }

    /// <summary>Nothing selected.</summary>
    public sealed class SinSeleccion : EstadoSeleccion
    {
        private SinSeleccion()
        {
        }

        /// <summary>The only instance: the state carries no data.</summary>
        public static SinSeleccion Instancia { get; } = new SinSeleccion();
    }

    /// <summary>A cell is selected and highlighted; no station chosen yet.</summary>
    public sealed class CasillaSeleccionada : EstadoSeleccion
    {
        /// <summary>Selects the cell.</summary>
        public CasillaSeleccionada(Posicion posicion) => Posicion = posicion;

        /// <summary>Selected cell.</summary>
        public Posicion Posicion { get; }
    }

    /// <summary>A station is chosen in the panel; the preview follows the pointer.</summary>
    public sealed class EstacionElegida : EstadoSeleccion
    {
        /// <summary>Chooses the station.</summary>
        public EstacionElegida(DefinicionEstacion definicion) => Definicion = definicion;

        /// <summary>Chosen station.</summary>
        public DefinicionEstacion Definicion { get; }
    }

    /// <summary>A semi-transparent station waits on a cell for Confirm or Cancel.</summary>
    public sealed class Previsualizando : EstadoSeleccion
    {
        /// <summary>Previews the station on the cell.</summary>
        public Previsualizando(Posicion posicion, DefinicionEstacion definicion)
        {
            Posicion = posicion;
            Definicion = definicion;
        }

        /// <summary>Cell of the preview.</summary>
        public Posicion Posicion { get; }

        /// <summary>Previewed station.</summary>
        public DefinicionEstacion Definicion { get; }
    }
}
