using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programacion_II
{
    public class Usuario : Persona
    {
        public List<Pronostico> Historial { get; set; }

        public List<string> CiudadesFavoritas { get; set; }
        public Tipo EstadoUsuario { get; set; }

        public Usuario(string nombre,string pass) : base(nombre, pass)
        {
            this.NombrePersona = nombre;
            this.Password = pass;
            this.Historial = new List<Pronostico>();
            this.CiudadesFavoritas = new List<string>();
            this.EstadoUsuario = Tipo.Cliente;
        }

        public Usuario() { }

        public override void MostrarDatos()
        {
            Console.WriteLine("-------------");
            Console.WriteLine($"Nombre : {this.NombrePersona}");
            Console.WriteLine($"Password : {this.Password}");
            Console.WriteLine("-------------");
           
        }

        public void MostrarHistorialPronosticos()
        {
            Console.WriteLine($"Mostrando historial");
            Console.WriteLine("-------------");
            foreach (Pronostico item in this.Historial)
            {

                item.MostrarDatos();
                Console.WriteLine("-------------");
            }
        }

        public void EliminarHistorial()
        {
            this.Historial.Clear();
        }

        public async Task AgregarCiudadFavorita(string ciudad, string pais)
        {
            // 1. Limpieza preliminar
            string ciudadLimpia = ciudad.Trim();
            string paisLimpio = pais.Trim();

            // 2. Llamada a la API
            Pronostico busqueda = await Pronostico.BuscarPronostico(ciudadLimpia, paisLimpio);

            if (busqueda != null)
            {
                
                // Si la búsqueda es exitosa y pasa las validaciones (o el país es un nombre largo y confiamos en la API)
                Console.WriteLine("✅ AGREGADA LA CIUDAD : " + busqueda.NombreCiudad);
                this.CiudadesFavoritas.Add($"{busqueda.NombreCiudad},{busqueda.Nacion.NombreNacion}");
            }
            else
            {
                Console.WriteLine("❌ No se encontró la ciudad. No se agregó a favoritos.");
            }
        }

        public async void MostrarCiudadesFavoritas()
        {
            Console.WriteLine("ciudades favoritas / paises y sus pronosticos");
            foreach (string item in this.CiudadesFavoritas)
            {
                Console.WriteLine($"{item}");
                Pronostico busqueda = await Pronostico.BuscarPronostico(item);
            }
        }

    }

    public enum Tipo
    {
        Administrador,   
        Cliente
    }

}
