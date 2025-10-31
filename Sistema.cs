using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using Proyecto_Programacion_II;
using System.Linq;
using System;

public class Sistema
{
    // Mantiene la lista de todos los usuarios (clientes y administrador)
    public List<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public Sistema() { }

    public Sistema(List<Usuario> usuarios)
    {
        this.Usuarios = usuarios;
    }

    public void Registrarse(string rutaJson)
    {
        Console.Clear();
        Console.WriteLine("--- REGISTRO DE NUEVO USUARIO ---");
        Console.WriteLine("Ingrese '0' en cualquier momento para cancelar.\n");

        string nombre;

        while (true)
        {
            Console.Write("Ingrese nombre de usuario: ");
            nombre = Console.ReadLine();

            // Permitir salir
            if (nombre == "0")
            {
                Console.WriteLine("Registro cancelado.");
                return;
            }

            // Verificar si el nombre ya existe
            bool existe = Usuarios.Any(u => u.NombrePersona.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            if (existe)
            {
                Console.WriteLine(" Ese nombre de usuario ya está en uso. Intente con otro.\n");
                continue;
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("El nombre no puede estar vacío.");
                continue;
            }


            break;
        }

        string contraseña;

        while (true)
        {
            Console.Write("Ingrese contraseña: ");
            contraseña = Console.ReadLine();

            if (contraseña == "0")
            {
                Console.WriteLine("Registro cancelado.");
                return;
            }

            if (string.IsNullOrWhiteSpace(contraseña))
            {
                Console.WriteLine("La contraseña no puede estar vacía.\n");
                continue; //  vuelve a pedirla
            }

            break; // ✅ contraseña válida, sale del bucle
        }

        long dniLong;

        while (true)
        {
            Console.Write("Ingrese DNI (8 dígitos): ");
            string dni = Console.ReadLine();

            if (dni == "0")
            {
                Console.WriteLine("Registro cancelado.");
                return;
            }

            // Validar formato
            if (!long.TryParse(dni, out dniLong) || dni.Length != 8)
            {
                Console.WriteLine(" DNI inválido. Debe tener exactamente 8 dígitos numéricos.\n");
                continue;
            }

            // Verificar duplicado
            if (Usuarios.Any(u => u.Dni == dniLong))
            {
                Console.WriteLine(" Ese DNI ya está registrado. Ingrese otro.\n");
                continue;
            }

            // Si llega hasta acá, el DNI es válido y único
            Console.WriteLine("DNI válido y único registrado correctamente.\n");
            break;
        }




        // Crear y agregar el nuevo usuario
        Usuario nuevoUsuario = new Usuario
        {
            NombrePersona = nombre,
            Password = contraseña,
            EstadoUsuario = Tipo.Cliente, // por ejemplo, 1 = cliente
            CiudadesFavoritas = new List<string>(),
            Historial = new List<Pronostico>(),
            Dni = dniLong
        };

        this.Usuarios.Add(nuevoUsuario);

        // Guardar en el JSON
        string jsonNuevo = JsonSerializer.Serialize(Usuarios, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(rutaJson, jsonNuevo);

        Console.WriteLine(" Usuario creado y guardado correctamente.");
        Console.ReadKey();
    }

    public Usuario IniciarSesion()
    {
        Console.Clear();
        Console.WriteLine("=== INICIO DE SESIÓN ===\n");

        while (true)
        {
            Console.Write("Ingrese nombre de usuario (0 para salir): ");
            string nombre = Console.ReadLine();

            if (nombre == "0")
            {
                Console.WriteLine("Inicio de sesión cancelado.");
                return null;
            }

            Console.Write("Ingrese contraseña: ");
            string contrasenia = Console.ReadLine();

            if (contrasenia == "0")
            {
                Console.WriteLine("Inicio de sesión cancelado.");
                return null;
            }

            Console.Write("Ingrese DNI: ");
            string dniTexto = Console.ReadLine();
            long dni;

            if (dniTexto == "0")
            {
                Console.WriteLine("Inicio de sesión cancelado.");
                return null;
            }

            // Validar que el DNI tenga 8 dígitos
            while (!long.TryParse(dniTexto, out dni) || dniTexto.Length != 8)
            {
                Console.Write("DNI inválido. Ingrese nuevamente (8 dígitos): ");
                dniTexto = Console.ReadLine();

                if (dniTexto == "0")
                {
                    Console.WriteLine("Inicio de sesión cancelado.");
                    return null;
                }
            }

            // Buscar usuario que coincida con nombre, contraseña y DNI
            var usuario = Usuarios.FirstOrDefault(u =>
                u.NombrePersona.Equals(nombre, StringComparison.OrdinalIgnoreCase) &&
                u.Password == contrasenia &&
                u.Dni == dni
            );

            if (usuario != null)
            {
                Console.WriteLine($"\nBienvenido, {usuario.NombrePersona}!");
                // Podés continuar con la lógica del menú principal o lo que necesites
                return usuario;
            }
            else
            {
                Console.WriteLine("\nDatos incorrectos. Verifique usuario, contraseña o DNI.\n");
                Console.WriteLine("¿Desea intentar nuevamente? (S/N)");
                string respuesta = Console.ReadLine();

                if (respuesta.Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Inicio de sesión cancelado.");
                    return null;
                }
                Console.Clear();
            }
        }
    }

    public void MostrarUsuarios()
    {
        Console.Clear();
        Console.WriteLine("usuarios registrados");
        int i = 0;
        foreach (Usuario item in this.Usuarios)
        {
            if (item.EstadoUsuario == Tipo.Cliente)
            {
                Console.WriteLine($"{i} - nombre : {item.NombrePersona} - password : {item.Password} - dni : {item.Dni}");
                i++;
            }
        }
    }

    public void ModificarUsuarioAdmin(string rutaJson)
    {
        Console.Clear();
        Console.WriteLine("=== MODIFICAR USUARIO ===\n");

        Console.Write("Ingrese el nombre del usuario a modificar: ");
        string nombre = Console.ReadLine();

        Usuario usuario = Usuarios.FirstOrDefault(u =>
            u.NombrePersona.Equals(nombre, StringComparison.OrdinalIgnoreCase));

        if (usuario == null)
        {
            Console.WriteLine(" Usuario no encontrado.");
            return;
        }

        Console.WriteLine($"\nUsuario encontrado:");
        Console.WriteLine($"Nombre: {usuario.NombrePersona}");
        Console.WriteLine($"Contraseña: {usuario.Password}");
        Console.WriteLine($"DNI: {usuario.Dni}\n");

        Console.WriteLine("¿Qué desea modificar?");
        Console.WriteLine("1. Nombre");
        Console.WriteLine("2. Contraseña");
        Console.WriteLine("3. DNI");
        Console.WriteLine("4. Cancelar");

        Console.Write("\nOpción: ");
        string opcion = Console.ReadLine();

        switch (opcion)
        {
            // ====== CAMBIO DE NOMBRE ======
            case "1":
                while (true)
                {
                    Console.Write("Nuevo nombre: ");
                    string nuevoNombre = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(nuevoNombre))
                    {
                        Console.WriteLine("⚠ El nombre no puede estar vacío.");
                        continue;
                    }

                    if (Usuarios.Any(u => u.NombrePersona.Equals(nuevoNombre, StringComparison.OrdinalIgnoreCase) && u != usuario))
                    {
                        Console.WriteLine("⚠️ Ese nombre ya está en uso. Intente con otro.");
                        continue;
                    }

                    usuario.NombrePersona = nuevoNombre;
                    break;
                }
                break;

            // ====== CAMBIO DE CONTRASEÑA ======
            case "2":
                while (true)
                {
                    Console.Write("Nueva contraseña: ");
                    string nuevaPassword = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(nuevaPassword))
                    {
                        Console.WriteLine("⚠ La contraseña no puede estar vacía ni contener solo espacios.");
                        continue;
                    }

                    if (nuevaPassword.Contains(" "))
                    {
                        Console.WriteLine(" La contraseña no puede contener espacios en blanco.");
                        continue;
                    }

                    usuario.Password = nuevaPassword;
                    break;
                }
                break;

            // ====== CAMBIO DE DNI ======
            case "3":
                while (true)
                {
                    Console.Write("Nuevo DNI (8 dígitos): ");
                    string nuevoDniStr = Console.ReadLine();

                    if (!long.TryParse(nuevoDniStr, out long nuevoDni) || nuevoDniStr.Length != 8)
                    {
                        Console.WriteLine(" El DNI debe tener exactamente 8 dígitos numéricos.");
                        continue;
                    }

                    if (Usuarios.Any(u => u.Dni == nuevoDni && u != usuario))
                    {
                        Console.WriteLine(" Ese DNI ya está registrado. Ingrese otro.");
                        continue;
                    }

                    usuario.Dni = nuevoDni;
                    break;
                }
                break;

            // ====== CANCELAR ======
            case "4":
                Console.WriteLine("Operación cancelada.");
                return;

            // ====== OPCIÓN INVÁLIDA ======
            default:
                Console.WriteLine(" Opción no válida.");
                return;
        }

        // ====== GUARDAR CAMBIOS ======
        string jsonNuevo = JsonSerializer.Serialize(Usuarios, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(rutaJson, jsonNuevo);

        Console.WriteLine("\n Usuario modificado correctamente.");
        Console.ReadKey();
    }


    public void EliminarUsuarioAdmin(string rutaJson)
    {
        Console.WriteLine("Ingrese el nombre del usuario que desea eliminar:");
        string nombre = Console.ReadLine();

        Usuario usuario = Usuarios.FirstOrDefault(u =>
            u.NombrePersona.Equals(nombre, StringComparison.OrdinalIgnoreCase));

        if (usuario == null)
        {
            Console.WriteLine("Usuario no encontrado.");
            return;
        }

        Console.WriteLine($"Usuario encontrado: {usuario.NombrePersona} (DNI: {usuario.Dni})");
        Console.Write("¿Está seguro que desea eliminar este usuario? (S/N): ");
        string confirmacion = Console.ReadLine();

        if (!confirmacion.Equals("S", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Operación cancelada.");
            return;
        }

        Usuarios.Remove(usuario);

        // Guardar la lista actualizada en el JSON
        string jsonNuevo = JsonSerializer.Serialize(Usuarios, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(rutaJson, jsonNuevo);

        Console.WriteLine("Usuario eliminado correctamente.");
    }

}