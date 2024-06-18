using Microsoft.Data.SqlClient;
using System;
using System.Data;
using GYMFORCE_API.Models;
using GYMFORCE_API.Repositorio.Interfaces;

namespace GYMFORCE_API.Repositorio.DAO
{
    public class ProductoDAO : IProducto
    {
        //Definir la cadena de conexion
        private readonly string? cadena;

        public ProductoDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }
        public ProductoO buscarProducto(int id)
        {
            return listadoProductoO().FirstOrDefault(v => v.id_producto == id);
        }

        public IEnumerable<Producto> listadoProductoPorProveedor(int proveedorId)
        {
            List<Producto> productosPorProveedor = new List<Producto>();
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_LISTADOPRODUCTOS_POR_PROVEEDOR", cn); // Nombre del procedimiento almacenado
            cmd.CommandType = CommandType.StoredProcedure;

            // Agregar parámetro @ID_PROVEEDOR
            cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.Int).Value = proveedorId;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                productosPorProveedor.Add(new Producto
                {
                    id_producto = int.Parse(dr["ID_PRODUCTO"].ToString()),
                    nom_prod = dr["NOM_PROD"].ToString(),
                    des_prod = dr["DES_PROD"].ToString(),
                    nom_cat = dr["NOM_CAT"].ToString(),
                    pre_prod = Convert.ToDouble(dr["PRE_PROD"]),
                    stock = int.Parse(dr["STOCK"].ToString()),
                    raz_soc = dr["RAZ_SOC"].ToString(),
                    foto_prod = dr["FOTO_PROD"].ToString()
                });
            }
            cn.Close();
            return productosPorProveedor;
        }


        public IEnumerable<Producto> listadoProducto()
        {
            List<Producto> aProducto = new List<Producto>();
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_LISTADOPRODUCTOS", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aProducto.Add(new Producto
                {
                    id_producto = int.Parse(dr[0].ToString()),
                    nom_prod = dr[1].ToString(),
                    des_prod = dr[2].ToString(),
                    nom_cat = dr[3].ToString(),
                    pre_prod = Double.Parse(dr[4].ToString()),
                    stock = int.Parse(dr[5].ToString()),
                    raz_soc = dr[6].ToString(),
                    foto_prod = dr[7].ToString()
                });
            }
            cn.Close();
            return aProducto;
        }

        public IEnumerable<ProductoO> listadoProductoO()
        {
            List<ProductoO> aProducto = new List<ProductoO>();
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_LISTADOPRODUCTOS_O", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aProducto.Add(new ProductoO
                {
                    id_producto = int.Parse(dr[0].ToString()),
                    nom_prod = dr[1].ToString(),
                    des_prod  = dr[2].ToString(),
                    id_categoria = int.Parse(dr[3].ToString()),
                    pre_prod = Double.Parse(dr[4].ToString()),
                    stock = int.Parse(dr[5].ToString()),
                    id_proveedor = int.Parse(dr[6].ToString()),
                    foto_prod = dr[7].ToString(),
                });
            }
            cn.Close();
            return aProducto;
        }

        public string modificaProducto(ProductoO objP)
        {
            string mensaje = "";
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MERGE_PRODUCTO", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ide", objP.id_producto);
                cmd.Parameters.AddWithValue("@nom", objP.nom_prod);
                cmd.Parameters.AddWithValue("@des", objP.des_prod);
                cmd.Parameters.AddWithValue("@cat", objP.id_categoria);
                cmd.Parameters.AddWithValue("@pre", objP.pre_prod);
                cmd.Parameters.AddWithValue("@stock", objP.stock);
                cmd.Parameters.AddWithValue("@prov", objP.id_proveedor);
                cmd.Parameters.AddWithValue("@foto_prod", objP.foto_prod);
                int n = cmd.ExecuteNonQuery();
                mensaje = n.ToString() + " Producto actualizado correctamente..!!";
            }
            catch (Exception ex)
            {
                mensaje = "Error al actualizar..!!" + ex.Message;
            }
            cn.Close();
            return mensaje;
        }

        public string nuevoProducto(ProductoO objP)
        {
            string mensaje = "";
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MERGE_PRODUCTO", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ide", objP.id_producto);
                cmd.Parameters.AddWithValue("@nom", objP.nom_prod);
                cmd.Parameters.AddWithValue("@des", objP.des_prod);
                cmd.Parameters.AddWithValue("@cat", objP.id_categoria);
                cmd.Parameters.AddWithValue("@pre", objP.pre_prod);
                cmd.Parameters.AddWithValue("@stock", objP.stock);
                cmd.Parameters.AddWithValue("@prov", objP.id_proveedor);
                cmd.Parameters.AddWithValue("@foto_prod", objP.foto_prod);
                int n = cmd.ExecuteNonQuery();
                mensaje = n.ToString() + " Producto registrado correctamente..!!";
            }
            catch (Exception ex)
            {
                mensaje = "Error al registrar..!!" + ex.Message;
            }
            cn.Close();
            return mensaje;
        }

        // Método para obtener el reporte de productos
        public IEnumerable<Producto> reporteProducto(string nombre = null, int? categoria = null, int? stock = null, int? proveedor = null)
        {
            List<Producto> aProducto = new List<Producto>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_REPORTEPRODUCTOS", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Añadir parámetros con valores nulos si no se proporcionan
                    cmd.Parameters.AddWithValue("@NOMBRE", (object)nombre ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CATEGORIA", (object)categoria ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@STOCK", (object)stock ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PROVEEDOR", (object)proveedor ?? DBNull.Value);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            aProducto.Add(new Producto
                            {
                                id_producto = dr.GetInt32(dr.GetOrdinal("ID_PRODUCTO")),
                                nom_prod = dr.GetString(dr.GetOrdinal("NOM_PROD")),
                                des_prod = dr.GetString(dr.GetOrdinal("DES_PROD")),
                                nom_cat = dr.GetString(dr.GetOrdinal("NOM_CAT")),
                                pre_prod = Convert.ToDouble(dr.GetDecimal(dr.GetOrdinal("PRE_PROD"))),
                                stock = dr.GetInt32(dr.GetOrdinal("STOCK")),
                                raz_soc = dr.GetString(dr.GetOrdinal("RAZ_SOC")),
                                foto_prod = dr.GetString(dr.GetOrdinal("FOTO_PROD"))
                            });
                        }
                    }
                }
            }
            return aProducto;
        }
    }
}
