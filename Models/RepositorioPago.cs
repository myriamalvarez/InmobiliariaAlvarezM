using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Models
{
	public class RepositorioPago : RepositorioBase, IRepositorioPago
	{
		public RepositorioPago(IConfiguration configuration) : base(configuration)
		{

		}

		public int Alta(Pago pago)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Pago (Importe, FechaDePago, IdContrato) " +
					$"VALUES (@Importe, @FechaDePago, @IdContrato);" +
					"SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@Importe", pago.Importe);
					command.Parameters.AddWithValue("@FechaDePago", pago.FechaDePago);
					command.Parameters.AddWithValue("@IdContrato", pago.IdContrato);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					pago.IdPago = res;
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
				string sql = $"DELETE FROM Pago WHERE IdPago = @id";
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
		public int Modificacion(Pago p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "UPDATE Pago SET " +
					"Importe=@importe, FechaDePago=@fechaDePago, IdContrato=@idContrato " +
					"WHERE IdPago = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@importe", p.Importe);
					command.Parameters.AddWithValue("@fechaDePago", p.FechaDePago);
					command.Parameters.AddWithValue("@idContrato", p.IdContrato);
					command.Parameters.AddWithValue("@id", p.IdPago);
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Pago> ObtenerTodos()
		{
			IList<Pago> res = new List<Pago>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdPago, p.Importe, FechaDePago, p.IdContrato, " +
					$"c.IdInmueble, c.IdInquilino," +
					$"Inm.Direccion," +
					$"Inq.Nombre, Inq.Apellido FROM Pago p INNER JOIN Contrato c ON p.IdContrato = c.IdContrato INNER JOIN Inmueble Inm ON Inm.IdInmueble = c.IdInmueble INNER JOIN Inquilino Inq ON Inq.IdInquilino = c.IdInquilino";

				using (SqlCommand command = new SqlCommand(sql, connection))
				{

					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago p = new Pago
						{
							IdPago = reader.GetInt32(0),
							Importe = reader.GetInt32(1),
							FechaDePago = reader.GetDateTime(2),
							IdContrato = reader.GetInt32(3),
							Contrato = new Contrato
							{
								IdInmueble = reader.GetInt32(4),
								IdInquilino = reader.GetInt32(5),
								Inmueble = new Inmueble
								{
									Direccion = reader.GetString(6),
								},
								Inquilino = new Inquilino
								{
									Nombre = reader.GetString(7),
									Apellido = reader.GetString(8),
								}
							}

						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Pago ObtenerPorId(int id)
		{
			Pago p = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdPago, p.Importe, FechaDePago, p.IdContrato, " +
					 $"c.IdInquilino, c.IdInmueble," +
					$"Inm.Direccion," +
					$"Inq.Nombre, Inq.Apellido FROM Pago p INNER JOIN Contrato c ON c.IdContrato=p.IdContrato " +
					$"INNER JOIN Inmueble Inm ON Inm.IdInmueble = c.IdInmueble " +
					$"INNER JOIN Inquilino Inq ON Inq.IdInquilino = c.IdInquilino " +
					$"WHERE p.IdPago=@id";

				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						p = new Pago
						{
							IdPago = reader.GetInt32(0),
							Importe = reader.GetInt32(1),
							FechaDePago = reader.GetDateTime(2),
							IdContrato = reader.GetInt32(3),
							
							
							Contrato = new Contrato
							{
								IdInmueble = reader.GetInt32(4),
								IdInquilino = reader.GetInt32(5),
								Inmueble = new Inmueble
								{
									Direccion = reader.GetString(6),
								},
								Inquilino = new Inquilino
								{
									Nombre = reader.GetString(7),
									Apellido = reader.GetString(8)
								}
							}
						};
					}
					connection.Close();
				}
			}
			return p;
		}


		/*public IList<Pago> ObtenerPorContrato(int id)
		{
			IList<Pago> res = new List<Pago>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdPago, p.Importe, FechaDePago, p.IdContrato, " +
					 $"c.IdInmueble, c.IdInquilino," +
					$"Inm.Direccion," +
					$"Inq.Nombre, Inq.Apellido FROM Pago p INNER JOIN Contrato c ON c.IdContrato=p.IdContrato " +
					$"INNER JOIN Inmueble Inm ON Inm.IdInmueble = c.IdInmueble " +
					$"INNER JOIN Inquilino Inq ON Inq.IdInquilino = c.IdInquilino " +
					$"WHERE p.IdContrato=@id";

				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago p = new Pago
						{
							IdPago = reader.GetInt32(0),
							Importe = reader.GetInt32(1),
							FechaDePago = reader.GetDateTime(2),
							IdContrato = reader.GetInt32(3),
							Contrato = new Contrato
							{
								IdInmueble = reader.GetInt32(4),
								IdInquilino = reader.GetInt32(5),
								Inmueble = new Inmueble
								{
									Direccion = reader.GetString(6),
									Tipo = reader.GetString(7),
								},
								Inquilino = new Inquilino
								{
									Nombre = reader.GetString(8),
									Apellido = reader.GetString(9)
								}
							}
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;

		}*/
		public IList<Pago> BuscarPorContrato(int id)
		{
			IList<Pago> res = new List<Pago>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{

				//sql funcional
				string consultasql = "SELECT c.IdContrato, FechaInicio, FechaFin, c.IdInquilino, c.IdInmueble," +
					" i.Nombre AS InquilinoNombre, i.Apellido AS InquilinoApellido," +
					"inm.Direccion, inm.IdPropietario, p.Nombre AS PropietarioNombre," +
					 "p.Apellido AS PropietarioApellido," +
					 "pa.IdPago AS PagoId, pa.Importe AS importe," +
					 "pa.FechaDePago AS fechaPago, pa.IdContrato" +
					 " FROM Contrato c " +
					 " INNER JOIN Inquilino i ON i.IdInquilino = c.IdInquilino" +
					 " INNER JOIN Inmueble inm ON inm.IdInmueble = c.IdInmueble" +
					 " INNER JOIN Propietario p ON inm.IdPropietario = p.IdPropietario" +
					 " INNER JOIN Pago pa ON pa.IdContrato = c.IdContrato " +
					 "WHERE c.IdContrato=@id";


				using (SqlCommand command = new SqlCommand(consultasql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago pago = new Pago
						{
							IdPago = reader.GetInt32(11),
							Importe = reader.GetInt32(12),
							FechaDePago = reader.GetDateTime(13),
							IdContrato = reader.GetInt32(0),

							Contrato = new Contrato
							{
								IdContrato = reader.GetInt32(0),
								FechaInicio = reader.GetDateTime(1),
								FechaFin = reader.GetDateTime(2),
								IdInquilino = reader.GetInt32(3),
								IdInmueble = reader.GetInt32(4),

								Inquilino = new Inquilino
								{
									IdInquilino = reader.GetInt32(3),
									Nombre = reader.GetString(5),
									Apellido = reader.GetString(6),
								},

								Inmueble = new Inmueble
								{
									IdInmueble = reader.GetInt32(4),
									Direccion = reader.GetString(7),
									IdPropietario = reader.GetInt32(8),

									Propietario = new Propietario
									{
										IdPropietario = reader.GetInt32(8),
										Nombre = reader.GetString(9),
										Apellido = reader.GetString(10),
									}
								}
							},




						};
						res.Add(pago);
					}
					connection.Close();
				}
			}
			return res;
		}
	}
}
