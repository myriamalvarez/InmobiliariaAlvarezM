﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Models
{
    public enum enRoles
    {
        SuperAdministrador = 1,
        Administrador = 2,
        Empleado = 3,
    }
    public class Usuario
    {
        [Key]
        [Display(Name = "Codigo")]
        public int IdUsuario { get; set; }
       // [Required]
        public string Nombre { get; set; }
       // [Required]
        public string Apellido { get; set; }
        public string Avatar { get; set; }
        public IFormFile AvatarFile { get; set; }
        //[Required, EmailAddress]
        public string Email { get; set; }
        //[Required,DataType(DataType.Password)]
        public string Clave { get; set; }
        public int Rol { get; set; }

        public string RolNombre => Rol > 0 ? ((enRoles)Rol).ToString() : "";

        public static IDictionary<int, string> ObtenerRoles()
        {
            SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
            Type tipoEnumRol = typeof(enRoles);
            foreach (var valor in Enum.GetValues(tipoEnumRol))
            {
                roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
            }
            return roles;
        }
    }
}
