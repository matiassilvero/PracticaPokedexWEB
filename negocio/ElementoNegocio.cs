using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
	public class ElementoNegocio
	{
		public List<Elemento> listar()
		{//todo esto ahora va ser mas facil q cuando lo hice en PokemonNegocio
			List<Elemento> lista = new List<Elemento>();
			AccesoADatos datos = new AccesoADatos();//cuando hago esto nace un objeto que tiene un lector-
													//-un comando y una conexion, el comando y la conexion tienen instancia-
													//-y una cadena de conexion configurada 

			try
			{
				datos.setearConsulta("Select Id,Descripcion From ELEMENTOS");
				datos.ejecutarLectura();

				while (datos.Lector.Read())
				{//para ir leyendo los registros
					Elemento aux = new Elemento();
					aux.id = (int)datos.Lector["id"];
					aux.Descripcion = (string)datos.Lector["Descripcion"];

					lista.Add(aux);
				}

				return lista;
			}

			catch (Exception ex)
			{
				throw ex;
			}

			finally
			{
				datos.cerrarConexion();
			}
		}
	} 
}
