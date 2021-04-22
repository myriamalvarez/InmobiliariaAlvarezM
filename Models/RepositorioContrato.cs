using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Models
{
    public class RepositorioContrato : RepositorioBase, IRepositorioContrato
    {
        public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Contrato entidad)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Contrato (Importe, FechaInicio, FechaFin, IdInquilino, IdInmueble) " +
                    $"VALUES (@importe, @fechaInicio, @fechaFin, @idInquilino, @idInmueble);" +
                    "SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@importe", entidad.Importe);
                    command.Parameters.AddWithValue("@fechaInicio", entidad.FechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", entidad.FechaFin);
                    command.Parameters.AddWithValue("@idInquilino", entidad.IdInquilino);
                    command.Parameters.AddWithValue("@idInmueble", entidad.IdInmueble);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    entidad.IdContrato = res;
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
                string sql = $"DELETE FROM Contrato WHERE IdContrato = {id}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        public int Modificacion(Contrato entidad)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Contrato SET Importe=@importe, FechaInicio=@fechainicio, FechaFin=@fechafin " +
                    $"WHERE IdContrato = {entidad.IdContrato}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@importe", SqlDbType.Decimal).Value = entidad.Importe;
                    command.Parameters.Add("@fechainicio", SqlDbType.VarChar).Value = entidad.FechaInicio;
                    command.Parameters.Add("@fechafin", SqlDbType.VarChar).Value = entidad.FechaFin;
                    command.Parameters.Add("@idinquilino", SqlDbType.Int).Value = entidad.IdInquilino;
                    command.Parameters.Add("@idinmueble", SqlDbType.Int).Value = entidad.IdInmueble;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        public IList<Contrato> ObtenerTodos()
        {
            IList<Contrato> res = new List<Contrato>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdContrato, Importe, FechaInicio, FechaFin, Contrato.IdInquilino, Contrato.IdInmueble, Inquilino.Nombre, Inquilino.Apellido, Inmueble.Direccion FROM Contrato INNER JOIN Inquilino ON Inquilino.IdInquilino=Contrato.IdInquilino INNER JOIN Inmueble ON Inmueble.IdInmueble=Contrato.IdInmueble";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Contrato entidad = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            Importe = reader.GetInt32(1),
                            FechaInicio = reader.GetDateTime(2),
                            FechaFin = reader.GetDateTime(3),
                            Inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(4),
                                Nombre = reader.GetString(6),
                                Apellido = reader.GetString(7),
                            },
                            Inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32(5),
                                Direccion = reader.GetString(8)
                            }
                        };
                        res.Add(entidad);
                    }
                    connection.Close();
                }
            }
            return res;
        }
        public Contrato ObtenerPorId(int id)
        {
            Contrato entidad = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdContrato, Importe, FechaInicio, FechaFin, Contrato.IdInquilino, Contrato.IdInmueble, Inquilino.Nombre, Inquilino.Apellido, Inmueble.Direccion FROM Contrato JOIN Inquilino ON(Inquilino.IdInquilino=Contrato.IdInquilino) JOIN Inmueble ON(Inmueble.IdInmueble=Contrato.IdInmueble)" +
                    $" WHERE IdContrato=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        entidad = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            Importe = reader.GetInt32(1),
                            FechaInicio = reader.GetDateTime(2),
                            FechaFin = reader.GetDateTime(3),
                            Inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(4),
                                Nombre = reader.GetString(6),
                                Apellido = reader.GetString(7),
                            },
                            Inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32(5),
                                Direccion = reader.GetString(8)
                            }
                        };
                        return entidad;
                    }
                    connection.Close();
                }
            }
            return entidad;
        }

        public IList<Contrato> ObtenerTodosDonde(int IdInmueble, string fechaInicio, string fechaFin)
        {
            IList<Contrato> res = new List<Contrato>();
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string where = "";

                if (IdInmueble !=0)
                {
                    where = "WHERE n.IdInmueble" + IdInmueble;
                    where = (fechaInicio != "0001-01-01") && (fechaFin != "0001-01-01") ? where + " AND FechaInicio<= '" + fechaInicio + "' AND FechaFin>= '" + fechaFin + "'" : where;
                }
                else if((fechaInicio != "0001-01-01") && (fechaFin != "0001-01-01"))
                {
                    where = "WHERE FechaDeInicio <= '" + fechaInicio + "' AND FechaDeFinalizacion>= '" + fechaFin + "'";
                }

                string sql = "SELECT IdContrato, Importe, FechaInicio, FechaFin, c.IdInquilino, c.IdInmueble, i.Nombre, i.Apellido, n.Direccion" +
                    $" FROM Contrato c INNER JOIN Inquilino i ON i.IdInquilino = c.IdInquilino INNER JOIN Inmueble n ON n.IdInmueble = c.IdInmueble " + where;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Contrato entidad = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            Importe = reader.GetInt32(1),
                            FechaInicio = reader.GetDateTime(2),
                            FechaFin = reader.GetDateTime(3),
                            IdInquilino = reader.GetInt32(4),
                            IdInmueble = reader.GetInt32(5),
                            Inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(4),
                                Nombre = reader.GetString(6),
                                Apellido = reader.GetString(7),
                            },
                            Inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32(5),
                                Direccion = reader.GetString(8),
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
