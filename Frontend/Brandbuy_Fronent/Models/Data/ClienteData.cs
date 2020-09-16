using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brandbuy_Fronent.Models.Data
{
    public class ClienteData
    {
        public IConfiguration Configuration { get; }

        public ClienteData(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string ResgistraCliente(Cliente c)
        {
            var salida = "";
            NpgsqlConnection conexion = new NpgsqlConnection();
            var cadenaDeConexion = Configuration["ConnectionStrings:DefaultConnection"];

            using (var connection = new NpgsqlConnection(cadenaDeConexion))
            {
                connection.Open();

                string sql = $"Select registrarClientes ('{c.NombreC}','{c.Correo}','{c.PasswordC}')";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            salida = dataReader[0].ToString();
                            System.Diagnostics.Debug.WriteLine("repuesta*************" + dataReader[0].ToString());
                        }

                    }
                }
                connection.Close();
            }

            return salida;
        }

        public string IniciaSesionCliente(Cliente c)
        {
            var salida = "";
            NpgsqlConnection conexion = new NpgsqlConnection();
            var cadenaDeConexion = Configuration["ConnectionStrings:DefaultConnection"];
            System.Diagnostics.Debug.WriteLine("repuestaaaaaaaaaaaa" + c.NombreC+c.PasswordC);
            using (var connection = new NpgsqlConnection(cadenaDeConexion))
            {
                connection.Open();

                string sql = $"Select iniciarSesion('{c.NombreC}','{c.PasswordC}')";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            salida = dataReader[0].ToString();
                            System.Diagnostics.Debug.WriteLine("repuesta*************" + dataReader[0].ToString());
                        }

                    }
                }
                connection.Close();
            }

            return salida;
        }

    }
}
