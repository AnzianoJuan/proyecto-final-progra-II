using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace Proyecto_Programacion_II
{
    internal class Program
    {

        static string RutaJson = "../../Cliente.json";

        static async Task Main(string[] args)
        {
            int opcionMenuUsuario = 0;
            Cliente cliente = CargarCliente(); // intenta cargar el usuario guardado

            while (opcionMenuUsuario != 4)
            {
                opcionMenuUsuario = MenuUsuario();
                if (opcionMenuUsuario == 1)
                {
                    if (cliente != null)
                    {
                        Console.WriteLine("Ya existe un usuario. No puede crear otro.");
                    }
                    else
                    {
                        Console.Write("Ingrese nombre: ");
                        string nombre = Console.ReadLine();

                        Console.Write("Ingrese contraseña: ");
                        string contraseña = Console.ReadLine();

                        cliente = new Cliente(nombre, contraseña);
                        GuardarCliente(cliente);

                        Console.WriteLine("Usuario creado y guardado correctamente.\n");

                    }
                }else if(opcionMenuUsuario == 2)
                {
                    if (cliente == null)
                    {
                        Console.WriteLine("pimero cree un usuario");
                    }
                    else
                    {
                        bool sesion = IniciarSesion(ref cliente);

                        int opcionMenuPronostico = 0;
                        while(opcionMenuPronostico != 4)
                        if (sesion)
                        {
                            opcionMenuPronostico = MenuPronostico();
                            if(opcionMenuPronostico == 1)
                            {
                                    Console.WriteLine("ingrese ciudad");
                                    string ciudad = Console.ReadLine();
                                    Console.WriteLine("ingrese pais");
                                    string pais = Console.ReadLine();
                                    Pronostico pronosticoBuscado =  await Pronostico.BuscarPronostico(ciudad,pais);
                                    cliente.Historial.Add(pronosticoBuscado);
                                    GuardarCliente(cliente);
                            }else if(opcionMenuPronostico == 2)
                            {
                                    cliente.MostrarDatos();
                            }
                        }
                        Console.WriteLine("saliste del menu de pronostico");

                    }
                }else if(opcionMenuUsuario == 3)
                {
                    if(cliente == null)
                    {
                        Console.WriteLine("pimero cree un usuario");
                    }
                    else
                    {
                        ModificarUsuario(ref cliente);

                    }
                }
            }
            Console.WriteLine("saliste del sistema...");
        }

        static int MenuUsuario()
        {
            int opcion;
            string cadena;


            Console.WriteLine("--- MENU USUARIO ---");
            Console.WriteLine("1. crear usuario");
            Console.WriteLine("2. iniciar sesion");
            Console.WriteLine("3. modificar usuario , por gusto personal o por si se olvida las credenciales");
            Console.WriteLine("4. salir");
            cadena = Console.ReadLine();

            while (!int.TryParse(cadena, out opcion) || (opcion < 1 || opcion > 4))
            {
                Console.WriteLine($"INGRESA CORRECTAMENTE , USTED INGRESO : {cadena}");
                cadena = Console.ReadLine();
            }

            return opcion;

        }

        static int MenuPronostico()
        {
            int opcion;
            string cadena;

            Console.WriteLine("--- MENU PRONOSTICO ---");
            Console.WriteLine("1. Buscar pronóstico");
            Console.WriteLine("2. Mostrar datos del usuario e historial");
            Console.WriteLine("3. Agregar ciudad favorita");
            Console.WriteLine("4. Salir");

            cadena = Console.ReadLine();

            while (!int.TryParse(cadena, out opcion) || (opcion < 1 || opcion > 4))
            {
                Console.WriteLine($"¡Entrada no válida! Por favor ingrese un número entre 1 y 4.");
                cadena = Console.ReadLine();
            }

            return opcion;
        }


        static void ModificarUsuario(ref Cliente cliente)
        {
            Console.WriteLine("¿Qué deseas modificar?");
            Console.WriteLine("1. Nombre");
            Console.WriteLine("2. Contraseña");
            Console.WriteLine("3. Salir");

            string opcion = Console.ReadLine();
            switch (opcion)
            {
                case "1":
                    // Modificar nombre
                    Console.Write("Ingrese el nuevo nombre: ");
                    cliente.NombrePersona = Console.ReadLine();
                    break;

                case "2":
                    // Modificar contraseña
                    Console.Write("Ingrese la nueva contraseña: ");
                    cliente.Password = Console.ReadLine();
                    break;

                case "3":
                    Console.WriteLine("saliste de la modificacion");
                    return;

                default:
                    Console.WriteLine("Opción no válida.");
                    return;
            }

            // Guardar el usuario actualizado en el archivo JSON
            GuardarCliente(cliente);
            Console.WriteLine("Datos actualizados correctamente.");
        }

            static bool IniciarSesion(ref Cliente cliente)
             {

            Console.Write("Ingrese nombre de usuario: ");
            string nombre = Console.ReadLine();

            Console.Write("Ingrese contraseña: ");
            string contraseña = Console.ReadLine();

            if (cliente != null && cliente.NombrePersona == nombre && cliente.Password == contraseña)
            {
                Console.WriteLine("¡Bienvenido de nuevo, " + cliente.NombrePersona + "!\n");
                return true;
            }
            else
            {
                Console.WriteLine("Nombre de usuario o contraseña incorrectos.\n");
                return false;
            }
        }

        static void GuardarCliente(Cliente usuario)
        {
            string json = JsonSerializer.Serialize(usuario, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(RutaJson, json);
        }

        static Cliente CargarCliente()
        {
            try
            {
                string json = System.IO.File.ReadAllText(RutaJson);

                if (string.IsNullOrWhiteSpace(json) || new FileInfo(RutaJson).Length == 0 || json.Replace(" ", "").Replace("\r", "").Replace("\n", "") == "{}")
                {
                    return null;
                }

                return JsonSerializer.Deserialize<Cliente>(json);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error al leer el archivo: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error al deserializar el archivo JSON: {ex.Message}");
                return null;
            }


        }

    }
}
