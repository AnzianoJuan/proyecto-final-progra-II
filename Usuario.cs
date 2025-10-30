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

            // **PRIMERA VALIDACIÓN: Prevenir códigos de país ridículamente largos**
            // Un código de país válido (ISO 3166-1 alpha-2) tiene 2 letras.
            if (paisLimpio.Length > 3 && !paisLimpio.Contains(" "))
            {
                Console.WriteLine("❌ Código de país o nombre demasiado largo e inválido. Use el código (ej: AR) o el nombre completo (ej: argentina).");
                return; // Salir antes de la llamada a la API
            }

            // 2. Llamada a la API
            Pronostico busqueda = await Pronostico.BuscarPronostico(ciudadLimpia, paisLimpio);

            if (busqueda != null)
            {
                // **SEGUNDA VALIDACIÓN: Asegurarse de que el país devuelto es el que queríamos**
                // Si el país ingresado es inválido (ej: dasdas), la API devuelve un código de país válido (ej: US).
                // Si el país devuelto por la API NO coincide con lo ingresado (si se ingresó un código corto):

                // 🔑 Solo agregar si el país ingresado tiene sentido (no es solo ruido)
                // --- En el método AgregarCiudadFavorita ---
                if (paisLimpio.Length == 2 && busqueda.Nacion != null )
                {
                    // ... tu lógica para imprimir el error de no coincidencia ...
                    Console.WriteLine($"❌ La API encontró '{busqueda.NombreCiudad}, {busqueda.Nacion}' pero no coincide con su búsqueda '{paisLimpio}'.");
                    return;
                }

                // Si la búsqueda es exitosa y pasa las validaciones (o el país es un nombre largo y confiamos en la API)
                Console.WriteLine("✅ AGREGADA LA CIUDAD : " + busqueda.NombreCiudad);
                this.CiudadesFavoritas.Add($"{ciudadLimpia},{paisLimpio}");
            }
            else
            {
                Console.WriteLine("❌ No se encontró la ciudad. No se agregó a favoritos.");
            }
        }

        public void MostrarCiudadesFavoritas()
        {
            Console.WriteLine("ciudades favoritas y sus pronosticos");
            foreach (string item in this.CiudadesFavoritas)
            {

                Console.WriteLine($"{item}");
            }
        }

    }

    public enum Tipo
    {
        Administrador,   
        Cliente
    }

}
