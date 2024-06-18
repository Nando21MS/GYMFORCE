using Microsoft.Data.SqlClient;
using System;
using System.Data;
using GYMFORCE_API.Models;
using GYMFORCE_API.Repositorio.Interfaces;


namespace GYMFORCE_API.Repositorio.DAO
{
    public class CategoriaDAO : ICategoria
    {
        //Definir la cadena de conexion
        private readonly string? cadena;

        public CategoriaDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }

        public IEnumerable<Categoria> listadoCategoria()
        {
            List<Categoria> aCategoria = new List<Categoria>();
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_LISTADOCATEGORIAS", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aCategoria.Add(new Categoria
                {
                    id_categoria = int.Parse(dr[0].ToString()),
                    nom_cat = dr[1].ToString()
                });
            }
            cn.Close();
            return aCategoria;
        }
    }
}

