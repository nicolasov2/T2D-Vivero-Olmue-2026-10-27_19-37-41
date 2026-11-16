#nullable enable
namespace T2D.Dominio
{
    /// <summary>UT 1688. Outcome of one step of a stress event.</summary>
    public enum AvanceEvento
    {
        /// <summary>Still walking the route.</summary>
        EnCamino,
        /// <summary>Reached the silo and drained it.</summary>
        LlegoAlSilo,
    }

    /// <summary>
    /// UT 1688. A stress event walking a route: applies its effect on each new cell and drains the
    /// silo when it arrives. After arriving it does nothing more.
    /// </summary>
    public sealed class EventoEstres
    {
        /// <summary>Places the event at the route entrance.</summary>
        public EventoEstres(DefinicionEvento definicion, Ruta ruta)
        {
            Definicion = definicion;
            Recorrido = new RecorridoDeRuta(ruta, definicion.Velocidad);
        }

        /// <summary>Kind of event.</summary>
        public DefinicionEvento Definicion { get; }

        /// <summary>Movement along the route (1680).</summary>
        public RecorridoDeRuta Recorrido { get; }

        /// <summary>True once it has reached the silo.</summary>
        public bool Termino { get; private set; }

        /// <summary>Moves the event and applies its effects on the level.</summary>
        public AvanceEvento Avanzar(float segundos, Nivel nivel)
        {
            if (Termino)
            {
                return AvanceEvento.LlegoAlSilo;
            }

            foreach (var casilla in Recorrido.Avanzar(segundos))
            {
                Definicion.Efecto.AlEntrarEnCasilla(casilla, nivel);
            }

            if (!Recorrido.HaLlegado)
            {
                return AvanceEvento.EnCamino;
            }

            nivel.Silo.RecibirDanio(Definicion.DanioAlSilo);
            Termino = true;
            return AvanceEvento.LlegoAlSilo;
        }
    }
}
