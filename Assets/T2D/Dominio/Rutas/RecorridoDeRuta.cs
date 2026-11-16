#nullable enable
using System;
using System.Collections.Generic;

namespace T2D.Dominio
{
    /// <summary>
    /// UT 1680. Moves something along a route at a constant speed, cell by cell, without leaving it.
    /// Pure C# so the movement is tested in EditMode; <c>SeguidorDeRuta</c> only draws it.
    /// </summary>
    public sealed class RecorridoDeRuta
    {
        /// <summary>Starts at the route entrance.</summary>
        public RecorridoDeRuta(Ruta ruta, float velocidad)
        {
            if (velocidad <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(velocidad), "La velocidad debe ser mayor que 0.");
            }

            Ruta = ruta;
            Velocidad = velocidad;
        }

        /// <summary>Route being followed.</summary>
        public Ruta Ruta { get; }

        /// <summary>Cells per second.</summary>
        public float Velocidad { get; }

        /// <summary>Index of the last route point reached.</summary>
        public int IndiceActual { get; private set; }

        /// <summary>Progress from the current point to the next one, from 0 to 1.</summary>
        public float ProgresoEnTramo { get; private set; }

        /// <summary>True once the last point (the silo) has been reached.</summary>
        public bool HaLlegado => IndiceActual == Ruta.Puntos.Count - 1;

        /// <summary>Last route cell reached.</summary>
        public Posicion CasillaActual => Ruta.Puntos[IndiceActual];

        /// <summary>Position between two cells, in cell units, for smooth drawing.</summary>
        public (float X, float Y) PosicionInterpolada
        {
            get
            {
                var actual = CasillaActual;
                if (HaLlegado)
                {
                    return (actual.X, actual.Y);
                }

                var siguiente = Ruta.Puntos[IndiceActual + 1];
                return (actual.X + ((siguiente.X - actual.X) * ProgresoEnTramo),
                        actual.Y + ((siguiente.Y - actual.Y) * ProgresoEnTramo));
            }
        }

        /// <summary>Advances the given time and returns the cells entered during it, in order.</summary>
        public IReadOnlyList<Posicion> Avanzar(float segundos)
        {
            if (segundos < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(segundos), "El tiempo no puede ser negativo.");
            }

            var nuevas = new List<Posicion>();
            var restante = segundos * Velocidad;
            while (restante > 0f && !HaLlegado)
            {
                var faltaEnTramo = 1f - ProgresoEnTramo;
                if (restante < faltaEnTramo)
                {
                    ProgresoEnTramo += restante;
                    break;
                }

                restante -= faltaEnTramo;
                IndiceActual++;
                ProgresoEnTramo = 0f;
                nuevas.Add(CasillaActual);
            }

            return nuevas;
        }
    }
}
