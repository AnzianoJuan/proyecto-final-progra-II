using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace Proyecto_Programacion_II
{
    internal class Program
    {

        static string RutaJson = "../../Sistema.json";

        static async Task Main(string[] args)
        {
            int opcionMenuUsuario = 0;
            Usuario cliente = CargarCliente(); // intenta cargar el usuario guardado

            while (opcionMenuUsuario != 4)
            {
                Console.Clear(); // Limpia antes de mostrar el menú principal
                opcionMenuUsuario = MenuUsuario();

                if (opcionMenuUsuario == 1)
                {
                    Console.Clear();
                    if (cliente != null)
                    {
                        Console.WriteLine("Ya existe un usuario. No puede crear otro.");
                        Console.ReadKey(); // Esperar para ver el mensaje
                    }
                    else
                    {
                        Console.Write("Ingrese nombre: ");
                        string nombre = Console.ReadLine();

                        Console.Write("Ingrese contraseña: ");
                        string contraseña = Console.ReadLine();
                        cliente = new Usuario(nombre, contraseña);
                        GuardarCliente(cliente);

                        Console.WriteLine("Usuario creado y guardado correctamente.");
                        Console.ReadKey(); // Esperar para ver el mensaje
                    }
                }
                else if (opcionMenuUsuario == 2)
                {
                    Console.Clear();
                    if (cliente == null)
                    {
                        Console.WriteLine("Primero cree un usuario.");
                        Console.ReadKey();
                    }
                    else
                    {
                        bool sesion = IniciarSesion(ref cliente);

                        if (sesion)
                        {
                               

                            int opcionMenuPronostico = 0;
                            while (opcionMenuPronostico != 8)
                            {
                                Console.Clear(); // Limpia antes de mostrar el menú de pronóstico
                                                 // Limpia antes de mostrar el menú de pronóstico
                                opcionMenuPronostico = MenuPronostico();

                                if (opcionMenuPronostico == 1)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Ingrese ciudad:");
                                    string ciudad = Console.ReadLine();
                                    Console.WriteLine("Ingrese país:");
                                    string pais = Console.ReadLine();

                                    // NOTA: Asume que Pronostico.BuscarPronostico existe y es visible.
                                    Pronostico pronosticoBuscado = await Pronostico.BuscarPronostico(ciudad, pais);

                                    if (pronosticoBuscado != null)
                                    {
                                        cliente.Historial.Add(pronosticoBuscado);
                                        GuardarCliente(cliente);
                                        pronosticoBuscado.MostrarDatos();
                                        Console.WriteLine("Pronóstico agregado al historial.");

                                        //Console.WriteLine("Quieres agregar esta ciudad a tu lista de Favoritos?");
                                        //Console.WriteLine("1-YES\n2-NO");

                                    }
                                    else
                                    {
                                        //Console.WriteLine("No se pudo obtener el pronóstico o la deserialización fue nula.");
                                    }
                                    Console.ReadKey(); // Esperar para ver el resultado
                                }
                                else if (opcionMenuPronostico == 2)
                                {
                                    Console.Clear();
                                    cliente.MostrarDatos(); // Mostrar datos usuario
                                    Console.ReadKey();
                                }else if(opcionMenuPronostico == 3)
                                {
                                    Console.Clear(); // Limpia antes de mostrar el menú de pronóstico
                                    cliente.MostrarHistorialPronosticos();
                                    Console.ReadKey();
                                }
                                else if(opcionMenuPronostico == 4)
                                {
                                    Console.Clear();
                                    Console.WriteLine("borrando historial..");
                                    cliente.EliminarHistorial();
                                    GuardarCliente(cliente);
                                    Console.ReadKey();
                                }else if(opcionMenuPronostico == 5)
                                {
                                    string ciudad, pais;    
                                    Console.Clear();
                                    Console.Write("ciudad : ");
                                    ciudad = Console.ReadLine();
                                    Console.Write("pais: ");
                                    pais = Console.ReadLine();
                                    await cliente.AgregarCiudadFavorita(ciudad,pais);
                                    GuardarCliente(cliente);
                                    Console.ReadKey();
                                }else if(opcionMenuPronostico == 6)
                                {
                                    Console.Clear();
                                    cliente.MostrarCiudadesFavoritas();
                                    Console.ReadKey();
                                }
                            }
                            Console.WriteLine("Saliste del menú de pronóstico.");
                            Console.ReadKey();
                        }
                        else
                        {
                            // La sesión no pudo iniciar.
                            Console.ReadKey();
                        }
                    }
                }
                else if (opcionMenuUsuario == 3)
                {
                    Console.Clear();
                    if (cliente == null)
                    {
                        Console.WriteLine("Primero cree un usuario.");
                        Console.ReadKey();
                    }
                    else
                    {
                        ModificarUsuario(ref cliente);
                        Console.ReadKey();
                    }
                }
            }
            Console.Clear();
            Console.WriteLine("Saliste del sistema...");
        }

        static int MenuUsuario()
        {
            int opcion;
            string cadena;

            Console.WriteLine("--- MENU USUARIO ---");
            Console.WriteLine("1. Crear usuario");
            Console.WriteLine("2. Iniciar sesión");
            Console.WriteLine("3. Modificar usuario (olvido de credenciales)");
            Console.WriteLine("4. Salir");
            Console.Write("Seleccione una opción: ");
            cadena = Console.ReadLine();

            while (!int.TryParse(cadena, out opcion) || (opcion < 1 || opcion > 4))
            {
                Console.Clear(); // Limpia si la entrada es incorrecta
                Console.WriteLine($"INGRESA CORRECTAMENTE. USTED INGRESÓ: {cadena}");
                Console.WriteLine("--- MENU USUARIO ---");
                Console.WriteLine("1. Crear usuario");
                Console.WriteLine("2. Iniciar sesión");
                Console.WriteLine("3. Modificar usuario (olvido de credenciales)");
                Console.WriteLine("4. Salir");
                Console.Write("Seleccione una opción: ");
                cadena = Console.ReadLine();
            }

            return opcion;

        }

        static int MenuPronostico()
        {
            int opcion;
            string cadena;

            Console.WriteLine("--- MENÚ PRONÓSTICO ---");
            Console.WriteLine("1. Buscar pronóstico");
            Console.WriteLine("2. Mostrar datos del usuario");
            Console.WriteLine("3. Mostrar Historial de pronosticos");
            Console.WriteLine("4. Eliminar Historial");
            Console.WriteLine("5. Agregar ciudad favorita o pais");
            Console.WriteLine("6. Mostrar Ciudades favoritas y paises con sus pronosticos");
            Console.WriteLine("7. Eliminar ciudad favorita");
            Console.WriteLine("8. Salir");
            Console.Write("Seleccione una opción: ");
            cadena = Console.ReadLine();

            while (!int.TryParse(cadena, out opcion) || (opcion < 1 || opcion > 8))
            {
                Console.WriteLine("--- MENÚ PRONÓSTICO ---");
                Console.WriteLine("1. Buscar pronóstico");
                Console.WriteLine("2. Mostrar datos del usuario");
                Console.WriteLine("3. Mostrar Historial de pronosticos");
                Console.WriteLine("4. Eliminar Historial");
                Console.WriteLine("5. Agregar ciudad favorita o pais");
                Console.WriteLine("6. Mostrar Ciudades favoritas y paises con sus pronosticos");
                Console.WriteLine("7. Eliminar ciudad favorita");
                Console.WriteLine("8. Salir");
                Console.Write("Seleccione una opción: ");
                cadena = Console.ReadLine();
            }

            return opcion;
        }


        static void ModificarUsuario(ref Usuario cliente)
        {
            Console.Clear();
            Console.WriteLine("--- MODIFICAR PERFIL ---");
            Console.WriteLine("1. Nombre");
            Console.WriteLine("2. Contraseña");
            Console.WriteLine("3. Salir");
            Console.Write("¿Qué deseas modificar?: ");

            string opcion = Console.ReadLine();
            switch (opcion)
            {
                case "1":
                    Console.Write("Ingrese el nuevo nombre: ");
                    cliente.NombrePersona = Console.ReadLine();
                    break;

                case "2":
                    Console.Write("Ingrese la nueva contraseña: ");
                    cliente.Password = Console.ReadLine();
                    break;

                case "3":
                    Console.WriteLine("Saliste de la modificación.");
                    return;

                default:
                    Console.WriteLine("Opción no válida.");
                    return;
            }

            // Guardar el usuario actualizado en el archivo JSON
            GuardarCliente(cliente);
            Console.WriteLine("Datos actualizados correctamente.");
        }

        static bool IniciarSesion(ref Usuario cliente)
        {
            Console.WriteLine("--- INICIAR SESIÓN ---");
            Console.Write("Ingrese nombre de usuario: ");
            string nombre = Console.ReadLine();

            Console.Write("Ingrese contraseña: ");
            string contraseña = Console.ReadLine();

            if (cliente != null && cliente.NombrePersona == nombre && cliente.Password == contraseña)
            {
                Console.Clear();
                Console.WriteLine("¡Bienvenido de nuevo, " + cliente.NombrePersona + "!\n");
                return true;
            }
            else
            {
                Console.WriteLine("Nombre de usuario o contraseña incorrectos.\n");
                return false;
            }
        }

        // MÉTODOS DE PERSISTENCIA (A REMPLAZAR POR LA CLASE SISTEMA)

        static void GuardarCliente(Usuario usuario)
        {
            string json = JsonSerializer.Serialize(usuario, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(RutaJson, json);
        }

        static Usuario CargarCliente()
        {
            try
            {
                string json = System.IO.File.ReadAllText(RutaJson);

                if (string.IsNullOrWhiteSpace(json) || new FileInfo(RutaJson).Length == 0 || json.Replace(" ", "").Replace("\r", "").Replace("\n", "") == "{}")
                {
                    return null;
                }

                return JsonSerializer.Deserialize<Usuario>(json);
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