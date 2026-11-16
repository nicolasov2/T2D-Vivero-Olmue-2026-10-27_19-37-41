using UnityEngine;

namespace T2D.Herramientas
{
    /// <summary>Colors of the mockups, written as hex like in the design documents.</summary>
    internal static class Colores
    {
        /// <summary>Parses "#RRGGBB"; a malformed constant is a bug in this tool.</summary>
        public static Color Hex(string hex) =>
            ColorUtility.TryParseHtmlString(hex, out var color) ? color : throw new System.ArgumentException($"Color inválido: {hex}", nameof(hex));
    }
}
