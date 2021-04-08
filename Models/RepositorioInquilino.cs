using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Models
{
    public class RepositorioInquilino : RepositorioBase 
    {
		public RepositorioInquilino(IConfiguration configuration) : base(configuration)
		{
		}
		public int Alta(Inquilino e)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Inquilino (Nombre, Apellido, Dni, Direccion, Telefono, DireccionLaboral, NombreGarante, ApellidoGarante, DniGarante) " +
					$"VALUES (@nombre, @apellido, @dni, @direccion, @telefono, @direccionLaboral, @nombreGarante, @apellidoGarante, @dniGarante);" +
					"SELECT SCOPE_IDENTITY();";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.Nombre);
					command.Parameters.AddWithValue("@apellido", e.Apellido);
					command.Parameters.AddWithValue("@dni", e.Dni);
					command.Parameters.AddWithValue("@direccion", e.Direccion);
					command.Parameters.AddWithValue("@telefono", e.Telefono);
					command.Parameters.AddWithValue("@direccionLaboral", e.DireccionLaboral);
					command.Parameters.AddWithValue("@nombreGarante", e.NombreGarante);
					command.Parameters.AddWithValue("@apellidoGarante", e.ApellidoGarante);
					command.Parameters.AddWithValue("@dniGarante", e.DniGarante);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					e.IdInquilino = res;
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
				string sql = $"DELETE FROM Inquilino WHERE IdInquilino = @id";
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
		public int Modificacion(Inquilino e)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Inquilino SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Direccion=@direccion, Telefono=@telefono, DireccionLaboral=@direccionLaboral, NombreGarante=@nombreGarante, ApellidoGarante=@apellidoGarante, DniGarante=@dniGarante" +
					$" " +
					$"WHERE IdInquilino = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.Nombre);
					command.Parameters.AddWithValue("@apellido", e.Apellido);
					command.Parameters.AddWithValue("@dni", e.Dni);
					command.Parameters.AddWithValue("@direccion", e.Direccion);
					command.Parameters.AddWithValue("@telefono", e.Telefono);
					command.Parameters.AddWithValue("@direccionLaboral", e.DireccionLaboral);
					command.Parameters.AddWithValue("@nombreGarante", e.NombreGarante);
					command.Parameters.AddWithValue("@apellidoGarante", e.ApellidoGarante);
					command.Parameters.AddWithValue("@dniGarante", e.DniGarante);
					command.Parameters.AddWithValue("@id", e.IdInquilino);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Inquilino> ObtenerTodos()
		{
			IList<Inquilino> res = new List<Inquilino>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdInquilino, Nombre, Apellido, Telefono, ApellidoGarante" +
					$" FROM Inquilino";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inquilino e = new Inquilino
						{
							IdInquilino = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Telefono = reader.GetString(3),
							ApellidoGarante = reader.GetString(4),
						};
						res.Add(e);
					}
					connection.Close();
				}
			}
			return res;
		}

		virtual public Inquilino ObtenerPorId(int id)
		{
			Inquilino e = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdInquilino, Nombre, Apellido, Dni, Direccion, Telefono, DireccionLaboral, NombreGarante, ApellidoGarante, DniGarante FROM Inquilino" +
					$" WHERE IdInquilino=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Inquilino
						{
							IdInquilino = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							Direccion = reader.GetString(4),
							Telefono = reader.GetString(5),
							DireccionLaboral = reader.GetString(6),
							NombreGarante = reader.GetString(7),
							ApellidoGarante = reader.GetString(8),
							DniGarante = reader.GetString(9),
						};
					}
					connection.Close();
				}
			}
			return e;
		}
	}
}

