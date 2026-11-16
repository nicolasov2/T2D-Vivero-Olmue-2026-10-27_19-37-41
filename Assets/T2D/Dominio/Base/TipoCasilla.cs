#nullable enable
namespace T2D.Dominio
{
    /// <summary>What a nursery cell is made of. Only <see cref="SueloEdificable"/> accepts stations.</summary>
    public enum TipoCasilla
    {
        /// <summary>Free ground where stations can be built.</summary>
        SueloEdificable,
        /// <summary>Part of a route; stress events walk here (1680).</summary>
        Ruta,
        /// <summary>Seedling row (1686).</summary>
        Plantines,
        /// <summary>Shed, fence or trees: nothing can be built or walked.</summary>
        Bloqueada,
        /// <summary>The central water tank at the end of every route.</summary>
        Silo,
    }
}
