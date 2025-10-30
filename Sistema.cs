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

            break;
        }

        Console.Write("Ingrese contraseña: ");
        string contraseña = Console.ReadLine();

        if (contraseña == "0")
        {
            Console.WriteLine("Registro cancelado.");
            return;
        }

        // Crear y agregar el nuevo usuario
        Usuario nuevoUsuario = new Usuario
        {
            NombrePersona = nombre,
            Password = contraseña,
            EstadoUsuario = Tipo.Cliente, // por ejemplo, 1 = cliente
            CiudadesFavoritas = new List<string>(),
            Historial = new List<Pronostico>()
        };

        this.Usuarios.Add(nuevoUsuario);

        // Guardar en el JSON
        string jsonNuevo = JsonSerializer.Serialize(Usuarios, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(rutaJson, jsonNuevo);

        Console.WriteLine(" Usuario creado y guardado correctamente.");
        Console.ReadKey();
    }

}
