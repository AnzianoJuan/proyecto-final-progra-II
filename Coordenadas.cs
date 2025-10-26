using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Proyecto_Programacion_II
{
    public class Coordenada
    {
        [JsonPropertyName("lon")]
        public float Longitud { get; set; }
        
        [JsonPropertyName("lat")]
        public float Latitud { get; set; }

        public Coordenada() { }

        public Coordenada(float longitud, float latitud)
        {
            this.Longitud = longitud;
            this.Latitud = latitud;
        }
    }
}
