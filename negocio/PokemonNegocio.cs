using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;//para declarar objetos para conectarme incluyo esta libreria
using dominio;
using System.Runtime.CompilerServices;

namespace negocio
{
  public class PokemonNegocio//en esta clase creo los metodos de acceso a datos para los Pokemon
  {
    public List<Pokemon> listar()//esta funcion lee registros de la BD y devuelve varios
    {
     List<Pokemon> lista = new List<Pokemon>();
     SqlConnection conexion = new SqlConnection();//para establecer la conexion debo declarar algunos objetos y configurarlos
     SqlCommand comando = new SqlCommand();//<-una vez conectado, necesito realizar acciones
     SqlDataReader lector;//como resultado de la lectura de la BD voy a obtener un set de datos, eso lo alojo en un lector llamado 'lector'
                          //no le generamos instancia xq cuando hago la lectura ya obtengo un objeto tipo SqlDataReader
                          //'lector' es una variable, todo lo demas de arriba son objetos
    
      try  //en try config todo para la conexion con la BD
      {   
        //lo primero q config es la cadena de conexion, lo cual es un atributo de la conexion en si
        //aca le aclaro: 1- a donde me quiero conectar, a q servidor(el nombre de donde me conecto a mi DB)-
        //-la direccion de mi motor de DB local,tambien puede ser: "server = .\\sqlexpress" o "server = (local)\\sqlexpress"
        //2- a q base de datos me voy a conectar, el nombre de mi DB
        //3- como me voy a conectar(Autenticación de Windows(si es local) o Autenticación de SQL Server)-
        //-en este caso me conecto a autenticacion de windows, si era a otro tenia q poner  integrated security = false; y el user y contraseña
        conexion.ConnectionString = "server = desktop-nfbo3q3\\sqlexpress; database=POKEDEX_DB; integrated security=true";
 
        //ahora configuro el comando, sirve para realizar la accion, en este caso voy a realizar una lectura
        //la lectura la voy a hacer mandando desde aca la sentencia sql q quiero ejecutar
        //hay diferentes tipos de comando, texto: le mando una sentencia sql
        //tipo procedimiento almacenado: aca le pido q ejecute una funcion q esta almacenada en la BD
        //en este caso usamos tipo texto
        comando.CommandType = System.Data.CommandType.Text;

        //y aca le paso el texto q quiero obtener, esta es la consulta q le mando desde mi app a la BD:
        //Primero tenia solamente: comando.CommandText = "Select Numero, Nombre, Descripcion, UrlImagen From POKEMONS";
        //Ahora mas cosas porque lo joinee con la Tabla ELEMENTOS de la DB, tambien agregue q solo muestre a los activos
        comando.CommandText = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad,P.id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad And P.activo = 1";

        //ese comando q le mande lo va ejecutar en esta conexion q estableci mas arriba:
        comando.Connection = conexion;
        
        //ahora abro la conexion:
        conexion.Open();
        
        //realizo la lectura:
        lector = comando.ExecuteReader();//esto da como resultado un SqlDataReader(lo q no hizo falta q instancie mas arriba)
        
        //hasta aca tengo lo datos en mi objeto -lector- q es tipo SqlDataReader
        //esto me genero como una tabla virtual con un puntero q vamos a ir posicionando en memoria-
        //-a eso lo voy a transformar en una coleccion de objetos:
        
        while (lector.Read())//si hay un registro devuelve true y posiciona el puntero en la sgte pos
        {
            Pokemon aux = new Pokemon();//a esto lo cargamos con los datos del registro q me devolvio
            aux.id = (int)lector["id"];
            aux.Numero = lector.GetInt32(0);//getint32 porque Numero es de tipo int, tiene indice 0 porque lo puse primero en: comando.CommandText = "Select Numero, Nombre, Descripcion From POKEMONS";
            aux.Nombre = (string)lector["Nombre"];//con este formato puede leer cualquier cosa, es mas practico q la manera de arriba, aparte no tenes q poner el indice
            aux.Descripcion = (string)lector["Descripcion"];
            
            //voy a preguntar si lo q esta dentro del lector no es NULL,
            //si no esta NULL, lo leo y lo guardo en la propiedad
            //sino el programa se rompe cuando encuentra una imagen nula
            //if(!(lector.IsDBNull(lector.GetOrdinal("UrlImagen"))))//TENGO ESTA FORMA PARA HACERLO
                //aux.UrlImagen = (string)lector["UrlImagen"];
            if (!(lector["UrlImagen"] is DBNull))//Y ESTA FORMA, USAMOS ESTA XQ ES MAS PRACTICA
                aux.UrlImagen = (string)lector["UrlImagen"];

            //Añadimos estas lineas, aca joinea las tablas Pokemons y Elementos
            aux.Tipo = new Elemento();//si no pongo esto, cuando quiera hacer Tipo.Descripcion me va dar NULL porque no existe un objeto del tipo Elemento cargado aca
            aux.Tipo.Descripcion = (string)lector["Tipo"];
            aux.Tipo.id = (int)lector["idTipo"];            
            aux.Debilidad = new Elemento();
            aux.Debilidad.Descripcion = (string)lector["Debilidad"];
            aux.Debilidad.id = (int)lector["idDebilidad"];

            lista.Add(aux);//en cada vuelta de este ciclo va reutilizar la variable aux pero va crear una nueva instancia-
                           //-le guarda los datos y lo guarda en la lista
        }
        //despues devuelve la lista
        conexion.Close();
        return lista;
      }
      catch (Exception ex)//devuelve el error, en vez q crashee la app
      {  
              throw ex;
      }
    }   
  
    public void agregarPokemon(Pokemon nuevo)
    {
      AccesoADatos datos = new AccesoADatos();
      
      try
      {
          //ATENCION ACA con como pusimos y concatenemos lo q esta entre comillas
          //aca las columnas deben coincidir con los values(los valores)
          datos.setearConsulta("Insert into POKEMONS (Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad, UrlImagen)values(" + nuevo.Numero + ", '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "', 1, @IdTipo, @IdDebilidad, @UrlImagen)");
          //lo q le tengo q mandar a la DB es un numero, en cambio Tipo y Debilidad son Objetos
          //lo q creo con los @ de IdTipo y IdDebilidad son como variables, las voy a pasar como parametros a Comando
          //voy a AccesoADatos y lo hago ahi, porque esta encapsulado
          //cuando estas 2 lineas de abajo se ejecuten, van a reemplazar a IdTipo e IdDebilidad q puse en el Insert
          datos.setearParametro("@IdTipo", nuevo.Tipo.id);
          datos.setearParametro("@IdDebilidad", nuevo.Debilidad.id);
          datos.setearParametro("@UrlImagen", nuevo.UrlImagen);
          
          //esta sentencia la creo en AccesoADatos
          datos.ejecutarAccion();
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

    public void modificarPokemon(Pokemon poke)
        {
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.setearConsulta("update POKEMONS set Numero = @numero, Nombre = @nombre, Descripcion = @desc, UrlImagen = @img, IdTipo = @idTipo, IdDebilidad = @idDebilidad Where Id = @id");
                datos.setearParametro("@numero", poke.Numero);
                datos.setearParametro("@nombre", poke.Nombre);
                datos.setearParametro("@desc", poke.Descripcion);
                datos.setearParametro("@img", poke.UrlImagen);
                datos.setearParametro("@idTipo", poke.Tipo.id);
                datos.setearParametro("@idDebilidad", poke.Debilidad.id);
                datos.setearParametro("@id", poke.id);

                datos.ejecutarAccion();
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

    public void eliminar(int id)//recibe por parametro un ID del pokemon q quiere eliminar 
    {
      try
      {
          AccesoADatos datos = new AccesoADatos();
          //la consulta(igual q en SSMS) en este caso va a ser ELIMINAR(delete)
          datos.setearConsulta("delete From pokemons where id = @id");
          datos.setearParametro("@id",id);
          datos.ejecutarAccion();
      
      
      }
      catch (Exception)
      {

          throw;
      }      
    }

        public void eliminarLogico(int id)
        {
            try
            {
                AccesoADatos datos = new AccesoADatos();
                datos.setearConsulta("update POKEMONS set Activo = 0 Where id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public List<Pokemon> filtrar(string campo, string criterio, string filtro)
        {
            List<Pokemon> lista = new List<Pokemon>();
            AccesoADatos datos = new AccesoADatos();
            try
            {
                //a esta consulta(q usamos mas arriba tmb) la usamos como base, le vamos a agregar posibles filtros al final
                string consulta = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad,P.id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad And P.activo = 1 And ";
                //ahora a nuestra consulta le agregamos algo al final
                if (campo == "Número")//el campo q recibo por parametro
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Numero > " + filtro;//a la consulta le concateno: Numero > filtro (filtro es lo q escribio el user)
                            break;
                        case "Menor a":
                            consulta += "Numero < " + filtro;
                            break;
                        default:
                            consulta += "Numero = " + filtro;
                            break;
                    }
                }
                else if(campo == "Nombre")
                {
                    switch(criterio)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + filtro + "%'";//FIJARSE BIEN, xq en SQL lee lo q va entre las comillas simples ''
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch(criterio)
                    {
                        case "Comienza con":
                            consulta += "P.Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "P.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "P.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.id = (int)datos.Lector["id"];
                    aux.Numero = datos.Lector.GetInt32(0);
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["UrlImagen"];
                    aux.Tipo = new Elemento();
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Tipo.id = (int)datos.Lector["idTipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];
                    aux.Debilidad.id = (int)datos.Lector["idDebilidad"];

                    lista.Add(aux);
                }

                return lista;
            }

            catch (Exception ex)
            {

                throw ex;
            }
        }
    }   
}   