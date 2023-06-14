using Entidades;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Manejadora
{
    public class ManejadoraPalabras_DAL
    {
        /// <summary>
        /// Función que devuelve una palabra, ésta está aleatoriamente seleccionada de la BBDD a través de su QUERY
        /// Precondicion: 
        /// Postcondicion: Devuelve una palabra de la BBDD;
        /// </summary>
        /// <returns></returns>
        public static clsPalabra getPalabraRandom()
        {

            clsConexion miConexion;

            SqlConnection conexion;

            SqlCommand miComando;
            clsPalabra palabra = null;
            SqlDataReader miLector;

            miConexion = new clsConexion();
            miComando = new SqlCommand();

            try
            {

                conexion = miConexion.getConnection();
                miComando.CommandText = "SELECT TOP 1 id, nombre FROM palabra ORDER BY NEWID()";
                miComando.Connection = conexion;
                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        palabra = new clsPalabra();

                        palabra.id = (int)miLector["id"];
                        palabra.nombre = (string)miLector["nombre"];
                    }
                }
                miLector.Close();
                miConexion.closeConnection(ref conexion);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return palabra;

        }
    }



}
