using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Proyecto_Programacion_II
{
    public class Nacion
    {
        [JsonPropertyName("country")]
        public string NombreNacion { get; set; }

        public Nacion() { }

        public Nacion(string nombreNacion)
        {
            this.NombreNacion = nombreNacion;
        }
    }
}
