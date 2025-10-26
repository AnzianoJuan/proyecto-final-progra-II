using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Proyecto_Programacion_II
{
    public class Nube
    {
        [JsonPropertyName("all")]
        public float Nubosidad {  get; set; }

        public Nube() { }

        public Nube(float nubosidad)
        {
            this.Nubosidad = nubosidad;
        }
    }
}
