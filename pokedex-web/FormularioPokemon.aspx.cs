using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dominio;
using negocio;

namespace pokedex_web
{
    public partial class FormularioPokemon : System.Web.UI.Page
    {
        public bool ConfirmaEliminacion { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfirmaEliminacion = false;//por defecto arranca en false el confirmar eliminacion

            txtId.Enabled = false;//hace q esta parte(la caja de Id) no este disponible, va estar deshabilitada
            try
            {//configuracion inicial de la pantalla
                if (!IsPostBack)//para los desplegables
                {//traigo de la BD la info de los elementos
                    ElementoNegocio negocio = new ElementoNegocio();
                    //ahora debo seleccionar el tipo de pokemon y la debilidad
                    //en este caso, ambos datos pertenecen a Elemento, entonces creo una lista
                    List<Elemento> lista = negocio.listar();//me traigo la lista de la BD una vez, la guardo y cargo los desplegables

                    ddlTipo.DataSource = lista;
                    //debemos configurar q dato muestra en pantalla y cual va tener escondido
                    //este es el valor q nos va a devolver por id
                    ddlTipo.DataValueField = "Id";//este va ser mi value
                                                  //este es lo q queremos que muestre:
                    ddlTipo.DataTextField = "Descripcion";//es el nombre de la propiedad de la clase | este va ser mi text
                    ddlTipo.DataBind();

                    ddlDebilidad.DataSource = lista;
                    ddlDebilidad.DataValueField = "Id";
                    ddlDebilidad.DataTextField = "Descripcion";
                    ddlDebilidad.DataBind();
                }

                //configuracion si estamos modificando
                //primero pregunto si tengo un ID, si trajo un id voy a la BD a traer el pokemon. Previo a usar esto agregue este parametro: List<Pokemon> listar(string id="")
                //aca hice un operador ternario: si el Request.QueryString["id"] es distinto de nulo, le guardo lo q contiene, sino le guardo vacio("")
                string id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";
                if (id!="" && !IsPostBack )//si esta viniendo informado un id. Si es vacio quiere decir q no esta recibiendo nada
                {           //Tambien pregunto si no es postback, xq antes de clickear aceptar(cuando estoy modificando) pase por el evento de carga,
                            //y ahi volvi a cargar todos los elementos originales. Entonces, quiero cargar esto si: traigo un ID y si se esta cargando la pantalla por primera vez
                    PokemonNegocio negocio =  new PokemonNegocio();
                    //List<Pokemon> lista = negocio.listar(id);//le paso el id q traje
                    //Pokemon seleccionado = lista[0];//devuelve el objeto pokemon, el unico q tienen esta lista y lo guarda
                    Pokemon seleccionado = (negocio.listar(id))[0];//hice lo mismo q en las 2 lineas de arriba, solo q simple

                    //guardo el pokemon seleccioando en la session
                    Session.Add("pokeSeleccionado", seleccionado);

                    //Ahora cargo los campos
                    txtId.Text = id;
                    txtNombre.Text = seleccionado.Nombre;
                    txtDescripcion.Text = seleccionado.Descripcion;
                    txtImagenUrl.Text = seleccionado.UrlImagen;
                    txtNumero.Text = seleccionado.Numero.ToString();

                    ddlTipo.SelectedValue = seleccionado.Tipo.id.ToString();
                    ddlDebilidad.SelectedValue = seleccionado.Debilidad.id.ToString();

                    //forzamos este metodo asi muestra la imagen
                    txtImagenUrl_TextChanged(sender, e);

                    //configurar acciones
                    if (!seleccionado.Activo)//si el pokemon no esta activo el boton nos va a decir Reactivar(por defecto el boton esta en Inactivar)
                    {
                        btnInactivar.Text = "Reactivar";
                    }
                }
            }
            catch (Exception ex)
            {

                Session.Add("error", ex);
                //agregamos una redireccion a otra pantalla de error
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {//aca creamos un nuevo pokemon
                Pokemon nuevo = new Pokemon();
                PokemonNegocio negocio = new PokemonNegocio();//para agregar el pokemon nuevo

                nuevo.Numero = int.Parse(txtNumero.Text);
                nuevo.Nombre = txtNombre.Text;
                nuevo.Descripcion = txtDescripcion.Text;
                nuevo.UrlImagen = txtImagenUrl.Text;

                //tipo es un objeto, por lo tanto voy a tener q agregar un nuevo tipo antes de cargar el dato
                //y como estoy en el ambito web, cuando yo seleccione/capture la seleccion del desplegable no voy 
                //a tener un objeto como con los comboBox en las app de escritorio, xq esto cuando se renderea/dibuja
                //en la web es texto plano, como mucho un entero. Por lo tanto debo generar un new Elemento para q tenga un objeto
                nuevo.Tipo = new Elemento();
                nuevo.Tipo.id = int.Parse(ddlTipo.SelectedValue);
                //lo mismo q hice arriba
                nuevo.Debilidad = new Elemento();
                nuevo.Debilidad.id = int.Parse(ddlDebilidad.SelectedValue);

                if (Request.QueryString["id"] !=  null)//esto tambien puede ser: if (txtId.text !=  null)
                {//si recibo un id por parametro es xq estoy modificando
                    nuevo.id = int.Parse(Request.QueryString["id"]);//FUNDAMENTAL, le paso el id que recibo a mi pokemon modificado
                    //tambien puede ser asi la linea de arriba: nuevo.id = int.Parse(txtId.text);
                    negocio.modificarPokemonConSP(nuevo);
                }
                else
                {
                    negocio.agregarConSP(nuevo);
                }
                Response.Redirect("PokemonsLista.aspx", false);




            }
            catch (Exception ex)
            {
                Session.Add("error", ex);
                throw;
            }
        }

        protected void txtImagenUrl_TextChanged(object sender, EventArgs e)
        {
            imgPokemon.ImageUrl = txtImagenUrl.Text;    
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {//este evento confirma la eliminacion
            ConfirmaEliminacion = true;
        }

        protected void btnConfirmaEliminar_Click(object sender, EventArgs e)
        {//vamos a eliminar el Pokemon
            try
            {
                if(chkConfirmaEliminacion.Checked)//si el Check de Confirma eliminacion esta checkeado entra aca
                {
                 PokemonNegocio negocio = new PokemonNegocio();
                 negocio.eliminar(int.Parse(txtId.Text));//el id lo tenemos en la caja de texto
                    Response.Redirect("PokemonsLista.aspx");
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex);
            }
        }

        protected void btnInactivar_Click(object sender, EventArgs e)
        {//Aca inactivo mi poquemon, es una baja logica, distinto a eliminar q hace una baja fisica(desde la BD)
            try
            {
                PokemonNegocio negocio = new PokemonNegocio();
                Pokemon seleccionado = (Pokemon)Session["pokeSeleccionado"];

                //le envío el id de mi pokemon seleccionado(el q guarde en mi session) y el estado opuesto a q si esta Activo o no
                negocio.eliminarLogico(seleccionado.id, !seleccionado.Activo);
                Response.Redirect("PokemonsLista.aspx");
            }
            catch (Exception ex)
            {
                Session.Add("error", ex);
            }
        }
    }
}