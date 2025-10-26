using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Proyecto_Programacion_II
{
    public class Viento
    {
        [JsonPropertyName("speed")]
        public float Velocidad { get; set; }

        [JsonPropertyName("deg")]
        public int Direccion { get; set; }

        public Viento() { }

        public Viento(float velocidad, int direccion)
        {
            this.Velocidad = velocidad;
            this.Direccion = direccion;
        }
    }
}
