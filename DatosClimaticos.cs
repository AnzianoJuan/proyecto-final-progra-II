using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Proyecto_Programacion_II
{
    public class DatosClimaticos
    {
        [JsonPropertyName("temp")]
        public float Temperatura { get; set; }

        [JsonPropertyName("feels_like")]
        public float SensacionTermica { get; set; }

        [JsonPropertyName("temp_max")]
        public float Temp_Max { get; set; }

        [JsonPropertyName("temp_min")]
        public float Temp_Min { get; set; }

        [JsonPropertyName("pressure")]
        public float Presion { get; set; }

        [JsonPropertyName("humidity")]
        public float Humedad { get; set; }

        [JsonPropertyName("sea_level")]
        public float NivelDelMar { get; set; }

        public DateTime FechaHoraBusqueda { get; set; }

        public DatosClimaticos()
        {
            this.FechaHoraBusqueda = DateTime.Now;
        }


        public DatosClimaticos(float temp, float sensacionT, float max, float min, float presion, float humedad, float lvlmar)
        {
            this.Temperatura = temp;
            this.SensacionTermica = sensacionT;
            this.Temp_Max = max;
            this.Temp_Min = min;
            this.Presion = presion;
            this.Humedad = humedad;
            this.NivelDelMar = lvlmar;
            this.FechaHoraBusqueda = DateTime.Now;
        }

        public void MostrarDatos()
        {
            Console.Write($"Fecha y Hora de Busqueda: {this.FechaHoraBusqueda.Day}/{this.FechaHoraBusqueda.Month}/{this.FechaHoraBusqueda.Year} "); //agregado fecha/hora
            if (DateTime.Now.Hour < 10)
            {
                Console.Write($"- 0{this.FechaHoraBusqueda.Hour}");
            }
            else
            {
                Console.Write($"- {this.FechaHoraBusqueda.Hour}");
            }
            if (DateTime.Now.Minute < 10)
            {
                Console.WriteLine($":0{this.FechaHoraBusqueda.Minute}");
            }
            else
            {
                Console.WriteLine($":{this.FechaHoraBusqueda.Minute}");
            }
            Console.WriteLine($"Temperatura: {this.Temperatura}");
            Console.WriteLine($"Sensacion Termica: {this.SensacionTermica}");
            Console.WriteLine($"Temperatura Maxima: {this.Temp_Max}");
            Console.WriteLine($"Temperatura Minima: {this.Temp_Min}");
            Console.WriteLine($"Presion: {this.Presion}");
            Console.WriteLine($"Humedad: {this.Humedad}");
            Console.WriteLine($"Nivel del Mar: {this.NivelDelMar}");

        }
    }
}