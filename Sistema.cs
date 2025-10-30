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
}
