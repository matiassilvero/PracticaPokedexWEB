using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace negocio
{
    public class AccesoADatos//para tener una clase para acceder a la DB, sino vamos a tener
                             //q escribir todo esto de aca abajo a cada rato
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;
        public SqlDataReader Lector
        {//con esto puedo leer el lector desde afuera
            get { return lector; }
        }
        public AccesoADatos()//me creo un constructor, cuando nace se crea con una conexion
        {
            conexion = new SqlConnection("server = desktop-nfbo3q3\\sqlexpress; database=POKEDEX_DB; integrated security=true");
            comando = new SqlCommand();
        }

        public void setearConsulta(string consulta)
        {//de este modo yo encapsulo la accion de darle un tipo y de darle la consulta
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void ejecutarLectura()
        {//este metodo realiza la lectura y la guarda en el lector
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ejecutarAccion()
        {
            comando.Connection = conexion;

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex )
            {

                throw ex;
            }

        }

        public void setearParametro(string nombre, object valor)
        {
            //este AddWithValue permite q le cargue un nombre de nu parametro y el valor
            //esto resuelve lo de mis @ (IdTipo e IdDebilidad)
            comando.Parameters.AddWithValue(nombre, valor);
        }
        public void cerrarConexion() 
        {//cuando cierro la conexion, cierro el lector si es q esta utilizandose
            if (lector != null)
                lector.Close();
            conexion.Close();
        }
    }
}
