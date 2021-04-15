﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Models
{
    public class RepositorioUsuario : RepositorioBase
    {
        public RepositorioUsuario(IConfiguration configuration) : base(configuration)
        {
        }

    public int Alta(Usuario u)
    {
        int res = -1;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = $"INSERT INTO Usuario (Nombre, Apellido, Avatar, Email, Clave, Rol) " +
                $"VALUES (@nombre, @apellido, @avatar, @email, @clave, @rol);" +
                $"SELECT SCOPE_IDENTITY();";//devuelve el id insertado
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", u.Nombre);
                command.Parameters.AddWithValue("@apellido", u.Apellido);
                    if (String.IsNullOrEmpty(u.Avatar))
                        command.Parameters.AddWithValue("@avatar", u.Avatar);
                    else
                        command.Parameters.AddWithValue("@avatar", u.Avatar);
                command.Parameters.AddWithValue("@email", u.Email);
                command.Parameters.AddWithValue("@clave", u.Clave);
                command.Parameters.AddWithValue("@rol", u.Rol);
                
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                u.IdUsuario = res;
                connection.Close();
            }
        }
        return res;
    }
    public int Baja(int id)
    {
        int res = -1;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = $"DELETE FROM Usuario WHERE IdUsuario = @id";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }
    public int Modificacion(Usuario u)
    {
        int res = -1;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = $"UPDATE Usuario SET Nombre=@nombre, Apellido=@apellido, Avatar=@avatar, Email=@email, Clave=@clave, Rol=@rol" +
                $"WHERE IdUsuario = @id";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", u.Nombre);
                command.Parameters.AddWithValue("@apellido", u.Apellido);
                command.Parameters.AddWithValue("@avatar", u.Avatar);
                command.Parameters.AddWithValue("@email", u.Email);
                command.Parameters.AddWithValue("@clave", u.Clave);
                command.Parameters.AddWithValue("@rol", u.Rol);
                command.Parameters.AddWithValue("@id", u.IdUsuario);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }


    public IList<Usuario> ObtenerTodos()
    {
        IList<Usuario> res = new List<Usuario>();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = $"SELECT IdUsuario, Nombre, Apellido, Avatar, Email, Clave, Rol" +
                $" FROM Usuario";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Usuario u = new Usuario
                    {
                        IdUsuario = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Apellido = reader.GetString(2),
                        Avatar = reader["Avatar"].ToString(),
                        Email = reader.GetString(4),
                        Clave = reader.GetString(5),
                        Rol = reader.GetInt32(6),
                        
                    };
                    res.Add(u);
                }
                connection.Close();
            }
        }
        return res;
    }

    public Usuario ObtenerPorId(int id)
    {
        Usuario u = null;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = $"SELECT IdUsuario, Nombre, Apellido, Avatar, Email, Clave, Rol FROM Usuario" +
                $" WHERE IdUsuario=@id";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    u = new Usuario
                    {
                        IdUsuario = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Apellido = reader.GetString(2),
                        Avatar = reader["Avatar"].ToString(),
                        Email = reader.GetString(4),
                        Clave = reader.GetString(5),
                        Rol = reader.GetInt32(6),
                        
                    };
                    return u;
                }
                connection.Close();
            }
        }
        return u;
    }
    public Usuario ObtenerPorEmail(string email)
    {
        Usuario u = null;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = $"SELECT IdUsuario, Nombre, Apellido, Avatar, Email, Clave, Rol FROM Usuario" +
                $" WHERE Email=@email";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    u = new Usuario
                    {
                        IdUsuario = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Apellido = reader.GetString(2),
                        Avatar = reader["Avatar"].ToString(),
                        Email = reader.GetString(4),
                        Clave = reader.GetString(5),
                        Rol = reader.GetInt32(6),
                       
                    };
                    return u;
                }
                connection.Close();
            }
        }
        return u;
    }
}
}