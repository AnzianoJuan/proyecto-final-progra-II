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

        public long Dni { get; set; }

        public Usuario(string nombre, string pass,long dni) : base(nombre, pass)
        {
            this.Historial = new List<Pronostico>();
            this.CiudadesFavoritas = new List<string>();
            this.EstadoUsuario = Tipo.Cliente;
        }

        public Usuario() { }

        public override void MostrarDatos()
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine($"Nombre : {this.NombrePersona}");
            Console.WriteLine($"Password : {this.Password}");
            Console.WriteLine($"DNI : {this.Dni}");
            Console.WriteLine("-----------------------------");

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

            // 2. Llamada a la API para corroborar si existe
            Pronostico busqueda = await Pronostico.BuscarPronostico(ciudadLimpia, paisLimpio);
            if (busqueda != null)
            {

                // Si la búsqueda es exitosa y pasa las validaciones (o el país es un nombre largo y confiamos en la API)
                Console.WriteLine(" AGREGADA LA CIUDAD : " + busqueda.NombreCiudad);
                this.CiudadesFavoritas.Add($"{busqueda.NombreCiudad},{busqueda.Nacion.NombreNacion}");
            }
            else
            {
                Console.WriteLine("No se encontró la ciudad. No se agregó a favoritos.");
            }
        }

        public async void MostrarCiudadesFavoritas()
        {
            int i = 1;
            Console.WriteLine("ciudades favoritas / paises y sus pronosticos");
            foreach (string item in this.CiudadesFavoritas)
            {
                Console.WriteLine($"{i} - {item}");
                Pronostico busqueda = await Pronostico.BuscarPronostico(item);
                i++;
                if (busqueda != null)
                {
                    busqueda.MostrarDatos();
                    Console.WriteLine("-------------------");
                }
                else
                {
                    Console.WriteLine("no encontrado");
                }
            }
        }

       
            public void EliminarUnFavorito()
            {
            if (this.CiudadesFavoritas.Count == 0)
            {
                Console.WriteLine("No hay ciudades favoritas para eliminar.");
                Console.ReadKey();
                return;
            }

            int i = 0;
            string cadena = "";
            int opcionEliminar;

            Console.WriteLine("--- CIUDADES FAVORITAS ---");
            foreach (string item in this.CiudadesFavoritas)
            {
                Console.WriteLine($"{i} - {item}");
                i++;
            }

            int opcionSalir = i;
            Console.WriteLine($"{opcionSalir} - NINGUNA OPCIÓN (salir)");
            Console.Write("\nElija qué ciudad desea eliminar: ");
            cadena = Console.ReadLine();

            while (!int.TryParse(cadena, out opcionEliminar) || opcionEliminar > i || opcionEliminar < 0)
            {
                Console.Write("Opción inválida. Elija una de las opciones mostradas: ");
                cadena = Console.ReadLine();
            }

            if (opcionEliminar == opcionSalir)
            {
                Console.WriteLine("Saliendo sin eliminar...");
                Console.ReadKey();
                return;
            }

            string ciudadEliminada = this.CiudadesFavoritas[opcionEliminar];
            this.CiudadesFavoritas.RemoveAt(opcionEliminar);

            Console.WriteLine($"\nCiudad '{ciudadEliminada}' eliminada correctamente.");
            Console.ReadKey();
        }




    }
}

public enum Tipo
{
    Administrador,
    Cliente
}

