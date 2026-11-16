#nullable enable
using System;
using System.Collections.Generic;

namespace T2D.Dominio
{
    /// <summary>
    /// Raw level configuration, as it comes from the <c>NivelDefinicion</c> asset. It is not validated:
    /// <see cref="ConstructorDeNivel"/> checks it and builds a playable <see cref="Nivel"/>.
    /// </summary>
    public sealed class DatosDeNivel
    {
        /// <summary>Level name.</summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>Columns of the map.</summary>
        public int Ancho { get; set; }

        /// <summary>Rows of the map.</summary>
        public int Alto { get; set; }

        /// <summary>Cell of the silo; every route ends here.</summary>
        public Posicion PosicionSilo { get; set; }

        /// <summary>Water the silo starts with.</summary>
        public int CapacidadSilo { get; set; }

        /// <summary>Credits the player starts with.</summary>
        public Costo CreditosIniciales { get; set; }

        /// <summary>UT 1680. Points of each route, from the entrance to the silo.</summary>
        public IReadOnlyList<IReadOnlyList<Posicion>> Rutas { get; set; } = Array.Empty<IReadOnlyList<Posicion>>();

        /// <summary>UT 1686. Seedling rows.</summary>
        public IReadOnlyList<DatosDePasillo> Pasillos { get; set; } = Array.Empty<DatosDePasillo>();

        /// <summary>Cells where nothing can be built or walked (shed, fence, trees).</summary>
        public IReadOnlyList<Posicion> Bloqueadas { get; set; } = Array.Empty<Posicion>();
    }
}
