using Microsoft.Data.SqlClient;
using System;
using System.Data;
using GYMFORCE_API.Models;
using GYMFORCE_API.Repositorio.Interfaces;

namespace GYMFORCE_API.Repositorio.DAO
{
    public class ProveedorDAO : IProveedor
    {
        //Definir la cadena de conexion
        private readonly string? cadena;

        public ProveedorDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }
        public Proveedor buscarProveedor(int id)
        {
            Proveedor proveedor = null;
            using (var connection = new SqlConnection(cadena))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM PROVEEDOR WHERE ID_PROVEEDOR = @ID_PROVEEDOR", connection))
                {
                    command.Parameters.AddWithValue("@ID_PROVEEDOR", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            proveedor = new Proveedor
                            {
                                id_proveedor = reader.GetInt32(0),
                                raz_soc = reader.GetString(1),
                                ruc = reader.GetString(2),
                                telefono = reader.GetString(3),
                                correo = reader.GetString(4),
                                direccion = reader.GetString(5),
                                foto_prov = reader.GetString(6)
                            };
                        }
                    }
                }
            }
            return proveedor;
        }

        public IEnumerable<Proveedor> listadoProveedor()
        {
            List<Proveedor> aProveedor = new List<Proveedor>();
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_LISTADOPROVEEDORES", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aProveedor.Add(new Proveedor
                {
                    id_proveedor = int.Parse(dr[0].ToString()),
                    raz_soc = dr[1].ToString(),
                    ruc = dr[2].ToString(),
                    telefono = dr[3].ToString(),
                    correo = dr[4].ToString(),
                    direccion = dr[5].ToString(),
                    foto_prov = dr[6].ToString()
                });
            }
            cn.Close();
            return aProveedor;
        }

        public string modificaProveedor(Proveedor objP)
        {
            string mensaje = "";
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MERGE_PROVEEDOR", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ide", objP.id_proveedor);
                cmd.Parameters.AddWithValue("@raz_soc", objP.raz_soc);
                cmd.Parameters.AddWithValue("@ruc", objP.ruc);
                cmd.Parameters.AddWithValue("@tel", objP.telefono);
                cmd.Parameters.AddWithValue("@cor", objP.correo);
                cmd.Parameters.AddWithValue("@dir", objP.direccion);
                cmd.Parameters.AddWithValue("@foto_prov", objP.foto_prov);
                int n = cmd.ExecuteNonQuery();
                mensaje = n.ToString() + " Proveedor actualizado correctamente..!!";
            }
            catch (Exception ex)
            {
                mensaje = "Error al actualizar..!!" + ex.Message;
            }
            cn.Close();
            return mensaje;
        }

        public string nuevoProveedor(Proveedor objP)
        {
            string mensaje = "";
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MERGE_PROVEEDOR", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ide", objP.id_proveedor);
                cmd.Parameters.AddWithValue("@raz_soc", objP.raz_soc);
                cmd.Parameters.AddWithValue("@ruc", objP.ruc);
                cmd.Parameters.AddWithValue("@tel", objP.telefono);
                cmd.Parameters.AddWithValue("@cor", objP.correo);
                cmd.Parameters.AddWithValue("@dir", objP.direccion);
                cmd.Parameters.AddWithValue("@foto_prov", objP.foto_prov);
                int n = cmd.ExecuteNonQuery();
                mensaje = n.ToString() + " Proveedor registrado correctamente..!!";
            }
            catch (Exception ex)
            {
                mensaje = "Error al registrar..!!" + ex.Message;
            }
            cn.Close();
            return mensaje;
        }
    }
}

