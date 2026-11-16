namespace T2D.Juego
{
    /// <summary>A component on the Partida object that starts once the level is built.</summary>
    public interface IParteDePartida
    {
        /// <summary>Called by <see cref="Partida"/> in Start, after the level is valid.</summary>
        void Inicializar(Partida partida);
    }
}
