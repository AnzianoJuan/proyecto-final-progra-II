using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Text.Json;

namespace Proyecto_Programacion_II
{
    public class Pronostico : IMostrable
    {
        [JsonPropertyName("coord")]
        public Coordenada Coordenadas { get; set; }

        [JsonPropertyName("weather")]
        public List<Clima> Clima { get; set; }

        [JsonPropertyName("main")]
        public DatosClimaticos DatosDelClima { get; set; }

        [JsonPropertyName("wind")]
        public Viento Viento { get; set; }

        [JsonPropertyName("sys")]
        public Nacion Nacion { get; set; }

        [JsonPropertyName("visibility")]
        public float Visibilidad { get; set; } //Tiene su propio codigo para decir la distancia de vision en Km

        [JsonPropertyName("clouds")]
        public Nube Nubes { get; set; } //Representa el porcentaje del cielo cubierto por nubes

        [JsonPropertyName("name")]
        public string NombreCiudad { get; set; }


        // Propiedad para indicar si los datos son válidos o si representan un error.
        public Pronostico() { }

        public Pronostico(Coordenada coordenadas, List<Clima> clima, DatosClimaticos datosdelclima, Viento viento, Nacion nacion, float visibilidad, Nube nubes, string nombreCiudad)
        {
            this.Coordenadas = coordenadas;
            this.Clima = clima;
            this.DatosDelClima = datosdelclima;
            this.Viento = viento;
            this.Nacion = nacion;
            this.Visibilidad = visibilidad;
            this.Nubes = nubes;
            this.NombreCiudad = nombreCiudad;
        }

        public void MostrarDatos()
        {
            //Console.WriteLine($"Coordenada , Longitud : {this.Coordenadas.Longitud} - Latitud : {this.Coordenadas.Latitud}");
            foreach (Clima clima in this.Clima)
            {
                clima.MostrarDatos();
            }
            this.DatosDelClima.MostrarDatos();
            //Console.WriteLine($"Viento , Direccion :{this.Viento.Direccion} - Velocidad : {this.Viento.Velocidad}");
            Console.WriteLine($"Nombre ciudad : {this.NombreCiudad}");
            Console.WriteLine($"Nacion : {this.Nacion.NombreNacion}");
            Console.WriteLine($"Visibilidad: {this.Visibilidad}");
            Console.WriteLine($"Nubes: {this.Nubes.Nubosidad}%");



        }



        // Ahora recibe dos parámetros
        public static async Task<Pronostico> BuscarPronostico(string ciudad, string pais)
        {
            string uri = "";
            Pronostico pronostico = null;

            try
            {
                HttpClient client = new HttpClient();
                //  Usamos la ciudad y el  país separados por coma en la URL
                // string busqueda = $"{ciudad.Trim()},{pais.Trim()}";

                uri = $"https://api.openweathermap.org/data/2.5/weather?q={ciudad},{pais}&appid=68fdfc51189ff83605164cee70337d8c&units=metric&lang=es";

                string cadena = await client.GetStringAsync(uri); // Aquí puede lanzar HttpRequestException (404)

                pronostico = JsonSerializer.Deserialize<Pronostico>(cadena);

                if (pronostico != null)
                {
                    return pronostico;
                }
                else
                {
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                // Esto captura el error 404
                //Console.WriteLine($"Error al buscar: La ciudad '{ciudad},{paisCodigo}' no fue encontrada. (404)");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return null;
            }
        }

       

    }
}
