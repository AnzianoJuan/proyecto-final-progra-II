using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programacion_II
{
    public abstract class Persona: IMostrable 
    {
        public string NombrePersona { get; set; }

        public string Password { get; set; }

        public Persona(string nombrePersona, string password)
        {
            this.NombrePersona = nombrePersona;
            this.Password = password;
        }

        public Persona() { }

        public abstract void MostrarDatos();
    }
}
