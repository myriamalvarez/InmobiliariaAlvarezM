using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Models
{
    public class RepositorioContrato : RepositorioBase
    {
        public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Contrato c)
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
                    command.Parameters.AddWithValue("@importe", c.Importe);
                    command.Parameters.AddWithValue("@fechaInicio", c.FechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", c.FechaFin);
                    command.Parameters.AddWithValue("@idInquilino", c.IdInquilino);
                    command.Parameters.AddWithValue("@idInmueble", c.IdInmueble);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    c.IdContrato = res;
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
        public int Modificacion(Contrato c)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Contrato SET Importe=@importe, FechaInicio=@fechainicio, FechaFin=@fechafin " +
                    $"WHERE IdContrato = {c.IdContrato}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@importe", SqlDbType.Decimal).Value = c.Importe;
                    command.Parameters.Add("@fechainicio", SqlDbType.VarChar).Value = c.FechaInicio;
                    command.Parameters.Add("@fechafin", SqlDbType.VarChar).Value = c.FechaFin;
                    //command.Parameters.Add("@idinquilino", SqlDbType.Int).Value = a.IdInquilino;
                    //command.Parameters.Add("@idinmueble", SqlDbType.Int).Value = a.IdInmueble;
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
                        Contrato c = new Contrato
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
                        res.Add(c);
                    }
                    connection.Close();
                }
            }
            return res;
        }
        public Contrato ObtenerPorId(int id)
        {
            Contrato c = null;
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
                    while (reader.Read())
                    {
                        c = new Contrato
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
                        return c;
                    }
                    connection.Close();
                }
            }
            return c;
        }
    }
}
