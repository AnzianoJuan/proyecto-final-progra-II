using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Data;

namespace Proyecto_Programacion_II
{
    internal class Program
    {
        const string RutaJson = "../../Sistema.json";

        static async Task Main(string[] args)
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                // pregunta si existe el archivo para que no tire la excepcion
                if (File.Exists(RutaJson))
                {
                    //asi se evita que exista el archivo , pero que este nulo
                    string json = File.ReadAllText(RutaJson);
                    if (!string.IsNullOrWhiteSpace(json))
                        usuarios = JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
                    else
                        usuarios = new List<Usuario>();
                }
                else
                {
                    Console.WriteLine("Archivo no encontrado, se creará uno nuevo con el admin por defecto.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al deserializar el archivo JSON: {ex.Message}");
            }

            // Si no había usuarios, agregamos el admin por defecto
            if (usuarios.Count == 0)
            {
                usuarios.Add(new Usuario
                {
                    NombrePersona = "admin",
                    Password = "admin",
                    EstadoUsuario = 0,
                    CiudadesFavoritas = new List<string>(),
                    Historial = new List<Pronostico>(),
                    Dni = 10000000
                });

                string jsonNuevo = JsonSerializer.Serialize(usuarios, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(RutaJson, jsonNuevo);
            }

            // Creamos el sistema con los usuarios cargados y deserializados para manipularlos
            Sistema sistema = new Sistema(usuarios);

            int opcionMenuUsuario = 0;

            while (opcionMenuUsuario != 4)
            {
                Console.Clear(); // Limpia antes de mostrar el menú principal
                opcionMenuUsuario = MenuUsuario();

                if (opcionMenuUsuario == 1)
                {
                    //pasamos por parametros el rachivo json para que lo agregue a la lista de usuarios y tambien lo use para corroborar si existe
                    sistema.Registrarse(RutaJson);
                }
                else if (opcionMenuUsuario == 2)
                {
                    Usuario usuarioLogeado = sistema.IniciarSesion();
                    // si el usuario se logeo correctamente la funcion devuelve un usuario
                    if (usuarioLogeado != null) // si es distinto de nulo es que se logeo correctamente
                    {
                        if (usuarioLogeado.EstadoUsuario == Tipo.Administrador) // si es administrador se le abre otro menu
                        {
                            // Menú para administradores
                            int opcionMenuAdmin = 0;
                            while (opcionMenuAdmin != 5)
                            {
                                Console.Clear();
                                opcionMenuAdmin = MenuAdmin(); // método que muestra opciones de admin
                                if(opcionMenuAdmin == 1)
                                {
                                    sistema.MostrarUsuarios(); // muestra los usuarios logeados
                                }
                                else if(opcionMenuAdmin == 2)
                                {
                                    sistema.Registrarse(RutaJson);// registra un usuario de forma manual
                                }else if(opcionMenuAdmin == 3)
                                {
                                    Console.Clear();
                                    sistema.ModificarUsuarioAdmin(RutaJson);//modifica usuario de forma manual 
                                }else if(opcionMenuAdmin == 4)
                                {
                                    Console.Clear();
                                    sistema.EliminarUsuarioAdmin(RutaJson);// elimina usuario de forma manual
                                }
                                //Console.Clear();
                                Console.ReadKey();
                            }
                            Console.WriteLine("saliste del menu de admin....");
                        }
                        else if (usuarioLogeado.EstadoUsuario == Tipo.Cliente)
                        {
                            int opcionMenuPronostico = 0;
                            while (opcionMenuPronostico != 9)
                            {
                                Console.Clear(); // Limpia antes de mostrar el menú de pronóstico
                                opcionMenuPronostico = MenuPronostico();

                                // menu pronostico
                                if (opcionMenuPronostico == 1)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Ingrese ciudad:");
                                    string ciudad = Console.ReadLine();
                                    Console.WriteLine("Ingrese país:");
                                    string pais = Console.ReadLine();

                                    // busca el pronostico de la ciudad dada
                                    Pronostico pronosticoBuscado = await Pronostico.BuscarPronostico(ciudad, pais);

                                    if (pronosticoBuscado != null)
                                    {// si lo encuentra lo agrega al historial de buscados
                                        usuarioLogeado.Historial.Add(pronosticoBuscado);
                                        GuardarUsuarios(usuarios);// guarda en el json para la serializacion
                                        pronosticoBuscado.MostrarDatos();
                                        Console.WriteLine("Pronóstico agregado al historial.");

                                        Console.WriteLine("Quieres agregar esta ciudad/pais a tu lista de Favoritos?");
                                        Console.WriteLine("1-Si\n2-No");
                                        string cadena = Console.ReadLine();
                                        int agregarCiudadInt;
                                        // validacion
                                        while (!int.TryParse(cadena, out agregarCiudadInt) || agregarCiudadInt < 1 || agregarCiudadInt > 2)
                                        {
                                            Console.Write("Elija una de las opciones: ");
                                            cadena = Console.ReadLine();
                                        }

                                        if (agregarCiudadInt == 1)
                                        {
                                            //agrega a la lista de ciudades favoritas del usuario
                                            cadena = $"{pronosticoBuscado.NombreCiudad},{pronosticoBuscado.Nacion.NombreNacion}";
                                            usuarioLogeado.CiudadesFavoritas.Add(cadena);
                                            GuardarUsuarios(usuarios);//serializacion
                                            Console.WriteLine($"agregado a favoritos : {cadena}");
                                        }
                                        else
                                        {
                                            Console.WriteLine("no se guardo en favoritos");
                                        }
                                    }
                                    else
                                    {
                                         Console.WriteLine("No se pudo obtener el pronóstico o la deserialización fue nula.");
                                    }
                                    Console.ReadKey(); // Esperar para ver el resultado
                                }
                                else if (opcionMenuPronostico == 2)
                                {
                                    Console.Clear();
                                    usuarioLogeado.MostrarDatos(); // Mostrar datos usuario
                                    Console.ReadKey();
                                }
                                else if (opcionMenuPronostico == 3)
                                {
                                    bool salirModificacion = false;

                                    while (!salirModificacion)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("¿Qué desea modificar?");
                                        Console.WriteLine("1. Nombre");
                                        Console.WriteLine("2. Contraseña");
                                        Console.WriteLine("3. DNI");
                                        Console.WriteLine("4. Cancelar");

                                        string opcion = Console.ReadLine();
                                        bool modifico = false; // <-- Bandera para saber si hubo cambios

                                        switch (opcion)
                                        {
                                            case "1":
                                                string nuevoNombre;
                                                do
                                                {
                                                    Console.Write("Nuevo nombre (o 0 para cancelar): ");
                                                    nuevoNombre = Console.ReadLine();

                                                    if (nuevoNombre == "0")
                                                    {
                                                        Console.WriteLine("Modificación cancelada.");
                                                        salirModificacion = true;
                                                        break;
                                                    }
                                                    //validacion
                                                    if (string.IsNullOrWhiteSpace(nuevoNombre) || !nuevoNombre.All(char.IsLetter) || nuevoNombre.Contains(' '))
                                                    {
                                                        Console.WriteLine("El nombre no puede estar vacío.");
                                                        continue;
                                                    }
                                                    //validacion de que no exista ese nombre en la lista
                                                    if (usuarios.Any(u => u.NombrePersona.Equals(nuevoNombre, StringComparison.OrdinalIgnoreCase) && u != usuarioLogeado))
                                                    {
                                                        Console.WriteLine("Ese nombre ya existe, ingrese otro.");
                                                        nuevoNombre = null; // fuerza el bucle
                                                    }

                                                } while (string.IsNullOrWhiteSpace(nuevoNombre) && !salirModificacion);

                                                if (!salirModificacion && nuevoNombre != null)
                                                {//le asigna al usuario el nuevo nombre si paso la validaciones
                                                    usuarioLogeado.NombrePersona = nuevoNombre;
                                                    modifico = true;
                                                }
                                                break;

                                            case "2":
                                                Console.Write("Nueva contraseña (o 0 para cancelar): ");
                                                string nuevaPassword = Console.ReadLine();

                                                if (nuevaPassword == "0")
                                                {
                                                    Console.WriteLine("Modificación cancelada.");
                                                    salirModificacion = true;
                                                    break;
                                                }
                                                // validacion de la contraseña
                                                if (string.IsNullOrWhiteSpace(nuevaPassword) || nuevaPassword.Contains(' '))
                                                {
                                                    Console.WriteLine("La contraseña no puede estar vacía.");
                                                    break;
                                                }
                                                // asignacion de la nueva contraseña
                                                usuarioLogeado.Password = nuevaPassword;
                                                modifico = true;
                                                break;

                                            case "3":
                                                long nuevoDni = 0;
                                                bool dniValido = false;
                                                while (!dniValido && !salirModificacion)
                                                {
                                                    Console.Write("Nuevo DNI (8 dígitos) o 0 para cancelar: ");
                                                    string dniStr = Console.ReadLine();

                                                    if (dniStr == "0")
                                                    {
                                                        Console.WriteLine("Modificación cancelada.");
                                                        salirModificacion = true;
                                                        break;
                                                    }

                                                    if (dniStr.Length != 8)
                                                    {
                                                        Console.WriteLine("El DNI debe tener exactamente 8 dígitos.");
                                                        continue;
                                                    }
                                                    // validacion
                                                    if (!long.TryParse(dniStr, out nuevoDni) || dniStr.Length != 8 || !dniStr.All(char.IsDigit))
                                                    {
                                                        Console.WriteLine("El DNI debe ser numérico.");
                                                        continue;
                                                    }
                                                    //validacion si ese dni ya existe en la lista
                                                    if (usuarios.Any(u => u.Dni == nuevoDni && u != usuarioLogeado))
                                                    {
                                                        Console.WriteLine("Ese DNI ya existe, ingrese otro.");
                                                        continue;
                                                    }

                                                    usuarioLogeado.Dni = nuevoDni;
                                                    dniValido = true;
                                                    modifico = true;
                                                }
                                                break;

                                            case "4":
                                                Console.WriteLine("Operación cancelada.");
                                                salirModificacion = true;
                                                break;

                                            default:
                                                Console.WriteLine("Opción no válida.");
                                                break;
                                        }

                                        // Solo guarda y muestra el mensaje si realmente se modificó algo
                                        if (modifico)
                                        {
                                            GuardarUsuarios(usuarios);
                                            Console.WriteLine("Usuario modificado correctamente.");
                                        }

                                        Console.WriteLine("\nPresione una tecla para continuar...");
                                        Console.ReadKey();
                                    }
                                }
                                else if (opcionMenuPronostico == 4)
                                {
                                    Console.Clear(); // Limpia antes de mostrar el menú de pronóstico
                                    usuarioLogeado.MostrarHistorialPronosticos();
                                    Console.ReadKey();
                                }
                                else if (opcionMenuPronostico == 5)
                                {
                                    Console.Clear();
                                    Console.WriteLine("borrando historial..");
                                    usuarioLogeado.EliminarHistorial();// borra historial de usuario
                                    GuardarUsuarios(usuarios);//serializa
                                    Console.ReadKey();
                                }
                                else if (opcionMenuPronostico == 6)
                                {
                                    Console.Clear();
                                    usuarioLogeado.MostrarCiudadesFavoritas();//muestra ciudades favoritas junto con sus pronosticos actuales
                                    Console.ReadKey();
                                }
                                else if (opcionMenuPronostico == 7)
                                {
                                    Console.Clear();
                                    usuarioLogeado.EliminarUnFavorito();//elimina ciudad favorita
                                    GuardarUsuarios(usuarios);
                                    Console.ReadKey();
                                }else if (opcionMenuPronostico == 8)
                                {
                                    int eleccion;
                                    Console.Clear();
                                    Console.WriteLine("!!ESTA SEGURO EN DARSE DE BAJA?!!");
                                    Console.WriteLine("1. si");
                                    Console.WriteLine("2. no");
                                    // se da de baja el mismo usuario de forma manual
                                    string cadena = Console.ReadLine();
                                    while(!int.TryParse(cadena , out eleccion) || eleccion < 1 || eleccion > 2 )
                                    {
                                        Console.WriteLine("ingrese correctamente 1 - si ,2 - no");
                                        cadena = Console.ReadLine();
                                    }

                                    if(eleccion == 1)
                                    {
                                        Console.WriteLine("dado de baja correctamente...");
                                        Console.WriteLine("saliendo del programa");
                                        usuarios.Remove(usuarioLogeado);//se quita de la lista
                                        GuardarUsuarios(usuarios);// se serializa
                                        return;
                                    }
                                    else
                                    {
                                        Console.WriteLine("se cancelo la opcion..");
                                    }

                                    Console.ReadKey();
                                }
                            }
                            Console.WriteLine("Saliste del menú de pronóstico.");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Inicio de sesión cancelado o fallido.");
                        Console.ReadKey();
                    }
                }else if(opcionMenuUsuario == 3)
                {
                    Console.Clear();
                    Console.WriteLine("contacto de administradores");
                    Console.WriteLine("juan.anziano@frsn.utn.edu.ar");
                    Console.WriteLine("julian.riolfo@frsn.utn.edu.ar");
                    Console.ReadKey();
                }

            }

            Console.Clear();
            Console.WriteLine("Saliste del sistema...");
        }
    
        static int MenuAdmin()
        {
            int opcion;
            string cadena;

            Console.WriteLine("--- MENÚ ADMINISTRADOR ---");
            Console.WriteLine("1. mostrar usuarios");
            Console.WriteLine("2. agregar usuario");
            Console.WriteLine("3. modificar usuario");
            Console.WriteLine("4. eliminar usuario");
            Console.WriteLine("5. Salir");
            Console.Write("Seleccione una opción: ");
            cadena = Console.ReadLine();
            //validacion
            while (!int.TryParse(cadena, out opcion) || (opcion < 1 || opcion > 5))
            {
                Console.WriteLine("--- MENÚ ADMINISTRADOR ---");
                Console.WriteLine("1. mostrar usuarios");
                Console.WriteLine("2. agregar usuario");
                Console.WriteLine("3. modificar usuario");
                Console.WriteLine("4. eliminar usuario");
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione una opción: ");
                cadena = Console.ReadLine();
            }

            return opcion;

        }


        static int MenuUsuario()
        {
            int opcion;
            string cadena;

            Console.WriteLine("--- MENU USUARIO ---");
            Console.WriteLine("1. Registrarse");
            Console.WriteLine("2. Iniciar sesión");
            Console.WriteLine("3. perdi mi cuenta");
            Console.WriteLine("4. Salir");
            Console.Write("Seleccione una opción: ");
            cadena = Console.ReadLine();

            while (!int.TryParse(cadena, out opcion) || (opcion < 1 || opcion > 4))
            {
                Console.Clear(); // Limpia si la entrada es incorrecta
                Console.WriteLine($"OPCION NO VALIDA. USTED INGRESÓ: {cadena}");
                Console.WriteLine("--- MENU USUARIO ---");
                Console.WriteLine("1. Registrarse");
                Console.WriteLine("2. Iniciar sesión");
                Console.WriteLine("3. perdi mi cuenta");
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
            Console.WriteLine("3. Modificar usuario");
            Console.WriteLine("4. Mostrar Historial de pronosticos");
            Console.WriteLine("5. Eliminar Historial");
            Console.WriteLine("6. Mostrar Ciudades favoritas y paises con sus pronosticos");
            Console.WriteLine("7. Eliminar ciudad favorita");
            Console.WriteLine("8. Darme de baja");
            Console.WriteLine("9. Salir");
            Console.Write("Seleccione una opción: ");
            cadena = Console.ReadLine();

            while (!int.TryParse(cadena, out opcion) || (opcion < 1 || opcion > 9))
            {
                Console.Clear(); // Limpia si la entrada es incorrecta
                Console.WriteLine($"OPCION NO VALIDA. USTED INGRESÓ: {cadena}");
                Console.WriteLine("--- MENÚ PRONÓSTICO ---");
                Console.WriteLine("1. Buscar pronóstico");
                Console.WriteLine("2. Mostrar datos del usuario");
                Console.WriteLine("3. Modificar usuario");
                Console.WriteLine("4. Mostrar Historial de pronosticos");
                Console.WriteLine("5. Eliminar Historial");
                Console.WriteLine("6. Mostrar Ciudades favoritas y paises con sus pronosticos");
                Console.WriteLine("7. Eliminar ciudad favorita");
                Console.WriteLine("8. Darme de baja");
                Console.WriteLine("9. Salir");
                Console.Write("Seleccione una opción: ");
                cadena = Console.ReadLine();
            }

            return opcion;
        }

        public static void GuardarUsuarios(List<Usuario> listaUsuarios)
        {
            try
            {
                string json = JsonSerializer.Serialize(listaUsuarios, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(RutaJson, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar los usuarios: {ex.Message}");
            }
        }


    }
}
