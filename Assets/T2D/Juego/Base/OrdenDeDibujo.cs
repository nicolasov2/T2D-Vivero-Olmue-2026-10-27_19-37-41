namespace T2D.Juego
{
    /// <summary>Sorting orders shared by every sprite, from back to front.</summary>
    public static class OrdenDeDibujo
    {
        /// <summary>Seedlings drawn on their row (1686).</summary>
        public const int Plantin = 2;

        /// <summary>Arrows that show where each route goes (1680).</summary>
        public const int Flecha = 3;

        /// <summary>Marker on the entrance of each route (1680).</summary>
        public const int Entrada = 4;

        /// <summary>Highlight of the selected cell.</summary>
        public const int Marco = 5;

        /// <summary>Coverage radius of the selected station.</summary>
        public const int Radio = 6;

        /// <summary>Stations on the map.</summary>
        public const int Estacion = 10;

        /// <summary>Preview of the station being placed.</summary>
        public const int Previsualizacion = 12;

        /// <summary>Background of the integrity bar above a station (1689).</summary>
        public const int BarraFondo = 15;

        /// <summary>Fill of the integrity bar above a station (1689).</summary>
        public const int BarraRelleno = 16;

        /// <summary>Reach of a stress event around it (1688, 1689, 1690).</summary>
        public const int Alcance = 19;

        /// <summary>Stress events walking the routes.</summary>
        public const int Evento = 20;
    }
}
