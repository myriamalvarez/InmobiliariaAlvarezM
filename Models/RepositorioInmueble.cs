using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Models
{
    public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
    {
		public RepositorioInmueble(IConfiguration configuration) : base(configuration)
		{
		}
		public int Alta(Inmueble entidad)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Inmueble (Direccion, Uso, Tipo, Ambientes, Precio, Estado, IdPropietario) " +
					$"VALUES (@direccion, @uso, @tipo, @ambientes, @precio, @estado, @idPropietario);" +
					"SELECT SCOPE_IDENTITY();";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@direccion", entidad.Direccion);
					command.Parameters.AddWithValue("@uso", entidad.Uso);
					command.Parameters.AddWithValue("@tipo", entidad.Tipo);
					command.Parameters.AddWithValue("@ambientes", entidad.Ambientes);
					command.Parameters.AddWithValue("@precio", entidad.Precio);
					command.Parameters.AddWithValue("@estado", entidad.Estado);
					command.Parameters.AddWithValue("@idPropietario", entidad.IdPropietario);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					entidad.IdInmueble = res;
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
				string sql = $"DELETE FROM Inmueble WHERE IdInmueble = @id";
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
		public int Modificacion(Inmueble entidad)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Inmueble SET Direccion=@direccion, Uso=@uso, Tipo=@tipo, Ambientes=@ambientes, Precio=@precio, Estado=@estado, IdPropietario=@idPropietario " +
					$"WHERE IdInmueble= @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@direccion", entidad.Direccion);
					command.Parameters.AddWithValue("@uso", entidad.Uso);
					command.Parameters.AddWithValue("@tipo", entidad.Tipo);
					command.Parameters.AddWithValue("@ambientes", entidad.Ambientes);
					command.Parameters.AddWithValue("@precio", entidad.Precio);
					command.Parameters.AddWithValue("@estado", entidad.Estado);
					command.Parameters.AddWithValue("@idPropietario", entidad.IdPropietario);
					command.Parameters.AddWithValue("@id", entidad.IdInmueble);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Inmueble> ObtenerTodos()
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdInmueble, Direccion, Uso, Tipo, Ambientes, Precio, Estado, Inmueble.IdPropietario, Propietario.Nombre, Propietario.Apellido FROM Inmueble INNER JOIN Propietario ON Propietario.IdPropietario=Inmueble.IdPropietario"; 
					
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble entidad = new Inmueble
						{
							IdInmueble = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Uso = reader.GetString(2),
							Tipo = reader.GetString(3),
							Ambientes = reader.GetInt32(4),
							Precio = reader.GetInt32(5),
							Estado = reader.GetInt32(6),
							IdPropietario = reader.GetInt32(7),
							Propietario = new Propietario
							{
								IdPropietario = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9)
                            }

						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Inmueble ObtenerPorId(int id)
		{
			Inmueble entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT i.IdInmueble, i.Direccion, i.Uso, i.Tipo, i.Ambientes, i.Precio, i.Estado, i.IdPropietario, p.Nombre, p.Apellido" +
				   $" FROM Inmueble i INNER JOIN Propietario p ON i.IdPropietario = p.IdPropietario" +
				   $" WHERE IdInmueble=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						entidad = new Inmueble
						{
							IdInmueble = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Uso = reader.GetString(2),
							Tipo = reader.GetString(3),
							Ambientes = reader.GetInt32(4),
							Precio = reader.GetInt32(5),
							Estado = reader.GetInt32(6),
							IdPropietario = reader.GetInt32(7),
							Propietario = new Propietario
							{
								IdPropietario = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							}
						};
					}
					connection.Close();
				}
			}
			return entidad;
		}

		public IList<Inmueble> BuscarPorPropietario(int id)
		{
			List<Inmueble> res = new List<Inmueble>();
			Inmueble entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdInmueble, Direccion, Uso, Tipo, Ambientes, Precio, Estado, i.IdPropietario, p.Nombre, p.Apellido" +
							 $" FROM Inmueble i INNER JOIN Propietario p ON i.IdPropietario = p.IdPropietario" +
							 $" WHERE p.IdPropietario=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						entidad = new Inmueble
						{
							IdInmueble = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Uso = reader.GetString(2),
							Tipo = reader.GetString(3),
							Ambientes = reader.GetInt32(4),
							Precio = reader.GetInt32(5),
							Estado = reader.GetInt32(6),
							IdPropietario = reader.GetInt32(7),
							Propietario = new Propietario
							{
								IdPropietario = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							}
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}
	}
}
