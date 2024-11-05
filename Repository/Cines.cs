using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UltimoParcialDesarolloWebChristianB.Models;

namespace UltimoParcialDesarolloWebChristianB.Repository
{
    public class Cines
    {
        private readonly string connectionString = "Server=svr-sql-ctezo.southcentralus.cloudapp.azure.com;Database=db_banco;User Id=usr_DesaWeb;Password=GuasTa360#;TrustServerCertificate=true";

        public async Task<List<dynamic>> ListarCines()
        {
            List<dynamic> cines = new List<dynamic>();
            string query = "SELECT CineID, Nombre, Ubicacion FROM Cine";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                cines.Add(new
                                {
                                    CineID = reader["CineID"],
                                    Nombre = reader["Nombre"],
                                    Ubicacion = reader["Ubicacion"]
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return cines;
        }

        public async Task<string> GuardarCine(Cine cine)
        {
            string query = "INSERT INTO Cine (Nombre, Ubicacion) VALUES (@Nombre, @Ubicacion)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", cine.Nombre);
                        cmd.Parameters.AddWithValue("@Ubicacion", cine.Ubicacion);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> EliminarCine(int id)
        {
            string query = "DELETE FROM Cine WHERE CineID = @CineID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CineID", id);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<List<dynamic>> ListarPeliculas()
        {
            List<dynamic> peliculas = new List<dynamic>();
            string query = @"
        SELECT P.PeliculaID, P.Titulo, P.Sinopsis, P.FechaEstreno, 
               C.Nombre AS CineNombre, G.Nombre AS GeneroNombre
        FROM Pelicula P
        JOIN Cine C ON P.CineID = C.CineID
        JOIN GeneroPelicula G ON P.GeneroID = G.GeneroID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                peliculas.Add(new
                                {
                                    PeliculaID = reader["PeliculaID"],
                                    Titulo = reader["Titulo"].ToString(),  // Asegurarse de convertir a string
                                    CineNombre = reader["CineNombre"].ToString(),
                                    GeneroNombre = reader["GeneroNombre"].ToString(),
                                    Sinopsis = reader["Sinopsis"] != DBNull.Value ? reader["Sinopsis"].ToString() : "Sin sinopsis disponible",  // Manejar valores nulos
                                    FechaEstreno = reader["FechaEstreno"] != DBNull.Value
                                        ? ((DateTime)reader["FechaEstreno"]).ToString("yyyy-MM-dd")
                                        : "Fecha no disponible"  // Formato de fecha
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return peliculas;
        }



        public async Task<string> InsertarPelicula(Pelicula pelicula)
        {
            string query = @"
        INSERT INTO Pelicula (Titulo, CineID, GeneroID, Sinopsis, FechaEstreno) 
        VALUES (@Titulo, @CineID, @GeneroID, @Sinopsis, @FechaEstreno)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Titulo", pelicula.Titulo);
                        cmd.Parameters.AddWithValue("@CineID", pelicula.CineID);
                        cmd.Parameters.AddWithValue("@GeneroID", pelicula.GeneroID);
                        cmd.Parameters.AddWithValue("@Sinopsis", pelicula.Sinopsis);
                        cmd.Parameters.AddWithValue("@FechaEstreno", pelicula.FechaEstreno);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public async Task<string> EliminarPelicula(int id)
        {
            string query = "DELETE FROM Pelicula WHERE PeliculaID = @PeliculaID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PeliculaID", id);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}