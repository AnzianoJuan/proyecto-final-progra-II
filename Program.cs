using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Proyecto_Programacion_II
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string cadena = "";
            string uri = "";
            Pronostico pronostico1 = new Pronostico();

            try
            {
                HttpClient client = new HttpClient();
                uri = $"https://api.openweathermap.org/data/2.5/weather?q=Ramallo,AR&appid=68fdfc51189ff83605164cee70337d8c&units=metric&lang=es";
                cadena = await client.GetStringAsync(uri);
                pronostico1 = JsonSerializer.Deserialize<Pronostico>(cadena);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //68fdfc51189ff83605164cee70337d8c
            //68fdfc51189ff83605164cee70337d8c      //dos veces por las dudas

            pronostico1.MostrarDatos();
            Console.ReadLine();

        }
    }
}
