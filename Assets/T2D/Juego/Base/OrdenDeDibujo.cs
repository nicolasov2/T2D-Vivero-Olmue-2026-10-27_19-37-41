namespace T2D.Juego
{
    /// <summary>Sorting orders shared by every sprite, from back to front.</summary>
    public static class OrdenDeDibujo
    {
        /// <summary>Highlight of the selected cell.</summary>
        public const int Marco = 5;

        /// <summary>Coverage radius of the selected station.</summary>
        public const int Radio = 6;

        /// <summary>Stations on the map.</summary>
        public const int Estacion = 10;

        /// <summary>Preview of the station being placed.</summary>
        public const int Previsualizacion = 12;

        /// <summary>Stress events walking the routes.</summary>
        public const int Evento = 20;
    }
}
