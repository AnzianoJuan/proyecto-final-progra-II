using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programacion_II
{
    public class Cliente : Persona
    {
        public List<Pronostico> Historial { get; set; }

        public List<string> CiudadesFavoritas { get; set; }

        public Estado EstadoUsuario { get; set; }

        public Cliente(string nombre,string pass):base(nombre,pass)
        {
            this.NombrePersona = nombre;
            this.Password = pass;
            this.Historial = new List<Pronostico>();
            this.CiudadesFavoritas = new List<string>();
            this.EstadoUsuario = Estado.Registrado;
        }

        public Cliente() { }

        public override void MostrarDatos()
        {
            Console.WriteLine("-------------");
            Console.WriteLine($"Nombre : {this.NombrePersona}");
            Console.WriteLine($"Password : {this.Password}");
            Console.WriteLine($"Estado : {this.EstadoUsuario}");
            Console.WriteLine("-------------");
            Console.WriteLine($"Mostrando historial");
            Console.WriteLine("-------------");
            foreach (Pronostico item in this.Historial)
            {

                item.MostrarDatos();
                Console.WriteLine("-------------");
            }
            Console.WriteLine("ciudades favoritas");
            Console.WriteLine("-------------");
            foreach (string item in this.CiudadesFavoritas)
            {
                Console.WriteLine($"Ciudad : {item}");
                Console.WriteLine("-------------");

            }


        }


    }

    public enum Estado
    {
        Registrado,
        NoRegistrado
    }

}
