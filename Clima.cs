using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Proyecto_Programacion_II
{
    public class Clima : IMostrable
    {
        [JsonPropertyName("id")]
        public int ID { get; set; } //  identificador numérico del tipo de clima.

        [JsonPropertyName("main")]
        public string EstadoClima { get; set; }

        [JsonPropertyName("description")]
        public string Descripcion { get; set; }

        [JsonPropertyName("icon")]
        public string Icono { get; set; } // es el código del ícono gráfico que representa visualmente el clima.

        public Clima() { }
        
        public Clima(int id, string estadoclima, string descripcion, string icono)
        {
            this.ID = id;
            this.EstadoClima = estadoclima;
            this.Descripcion = descripcion;
            this.Icono = icono;
        }

        public void MostrarDatos()
        {
            Console.WriteLine($"Estado del Clima: {this.EstadoClima} - {this.Descripcion}");
            //Console.WriteLine($"Identificador Numerico(ID): {this.ID}");
            //Console.WriteLine($"Icono: {this.Icono}");
        }
    }
}
