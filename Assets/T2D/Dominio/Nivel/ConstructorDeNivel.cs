#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace T2D.Dominio
{
    /// <summary>Validates a level configuration and builds the playable <see cref="Nivel"/>.</summary>
    public static class ConstructorDeNivel
    {
        /// <summary>Returns the level, or the first configuration problem found in plain words.</summary>
        public static Resultado<Nivel, string> Construir(DatosDeNivel datos)
        {
            var error = ValidarDatosBasicos(datos);
            if (error.Length > 0)
            {
                return Resultado<Nivel, string>.Fallo(error);
            }

            var limites = new LimitesDelMapa(datos.Ancho, datos.Alto);
            var rutas = CrearRutas(datos, limites);
            if (!rutas.EsExito)
            {
                return Resultado<Nivel, string>.Fallo(rutas.Error);
            }

            var casillasDeRuta = new HashSet<Posicion>(rutas.Valor.SelectMany(ruta => ruta.Puntos));
            casillasDeRuta.Remove(datos.PosicionSilo);
            var reservadas = new HashSet<Posicion>(casillasDeRuta) { datos.PosicionSilo };
            reservadas.UnionWith(datos.Bloqueadas);
            var pasillos = RegistroDePasillos.Crear(datos.Pasillos, limites, reservadas);
            if (!pasillos.EsExito)
            {
                return Resultado<Nivel, string>.Fallo(pasillos.Error);
            }

            var tipos = TiposEspeciales(datos, casillasDeRuta, pasillos.Valor);
            var cuadricula = new Cuadricula(datos.Ancho, datos.Alto, posicion =>
                tipos.TryGetValue(posicion, out var tipo) ? tipo : TipoCasilla.SueloEdificable);
            var silo = new Silo(datos.PosicionSilo, datos.CapacidadSilo);
            var nivel = new Nivel(datos.Nombre, cuadricula, rutas.Valor, pasillos.Valor, silo, new Creditos(datos.CreditosIniciales));
            return Resultado<Nivel, string>.Exito(nivel);
        }

        private static string ValidarDatosBasicos(DatosDeNivel datos)
        {
            if (datos.Ancho <= 0 || datos.Alto <= 0)
            {
                return $"El mapa debe medir al menos 1x1 (está en {datos.Ancho}x{datos.Alto}).";
            }

            if (!Cuadricula.EstaDentro(datos.PosicionSilo, datos.Ancho, datos.Alto))
            {
                return $"El silo {datos.PosicionSilo} está fuera del mapa.";
            }

            if (datos.CapacidadSilo <= 0)
            {
                return "La capacidad del silo debe ser mayor que 0.";
            }

            if (datos.Rutas.Count == 0)
            {
                return "El nivel no tiene rutas: agrega al menos una en la NivelDefinicion.";
            }

            return BuscarBloqueadaInvalida(datos);
        }

        private static string BuscarBloqueadaInvalida(DatosDeNivel datos)
        {
            foreach (var bloqueada in datos.Bloqueadas)
            {
                if (!Cuadricula.EstaDentro(bloqueada, datos.Ancho, datos.Alto))
                {
                    return $"La casilla bloqueada {bloqueada} está fuera del mapa.";
                }

                if (bloqueada == datos.PosicionSilo)
                {
                    return $"El silo {bloqueada} no puede estar en una casilla bloqueada.";
                }
            }

            return string.Empty;
        }

        private static Resultado<IReadOnlyList<Ruta>, string> CrearRutas(DatosDeNivel datos, LimitesDelMapa limites)
        {
            var bloqueadas = new HashSet<Posicion>(datos.Bloqueadas);
            var rutas = new List<Ruta>(datos.Rutas.Count);
            for (var i = 0; i < datos.Rutas.Count; i++)
            {
                var creada = Ruta.Crear(datos.Rutas[i], limites);
                var error = creada.EsExito ? ErrorDeRutaEnNivel(creada.Valor, datos.PosicionSilo, bloqueadas) : creada.Error;
                if (error.Length > 0)
                {
                    return Resultado<IReadOnlyList<Ruta>, string>.Fallo($"Ruta {i + 1}: {error}");
                }

                rutas.Add(creada.Valor);
            }

            return Resultado<IReadOnlyList<Ruta>, string>.Exito(rutas);
        }

        private static string ErrorDeRutaEnNivel(Ruta ruta, Posicion silo, HashSet<Posicion> bloqueadas)
        {
            if (ruta.Final != silo)
            {
                return $"debe terminar en el silo {silo} y termina en {ruta.Final}.";
            }

            foreach (var punto in ruta.Puntos)
            {
                if (bloqueadas.Contains(punto))
                {
                    return $"pasa por la casilla bloqueada {punto}.";
                }
            }

            return string.Empty;
        }

        private static Dictionary<Posicion, TipoCasilla> TiposEspeciales(DatosDeNivel datos, HashSet<Posicion> casillasDeRuta, RegistroDePasillos pasillos)
        {
            var tipos = new Dictionary<Posicion, TipoCasilla>();
            foreach (var bloqueada in datos.Bloqueadas)
            {
                tipos[bloqueada] = TipoCasilla.Bloqueada;
            }

            foreach (var casilla in pasillos.Pasillos.SelectMany(pasillo => pasillo.Casillas))
            {
                tipos[casilla] = TipoCasilla.Plantines;
            }

            foreach (var casilla in casillasDeRuta)
            {
                tipos[casilla] = TipoCasilla.Ruta;
            }

            tipos[datos.PosicionSilo] = TipoCasilla.Silo;
            return tipos;
        }
    }
}
