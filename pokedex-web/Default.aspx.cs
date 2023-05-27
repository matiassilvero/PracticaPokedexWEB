using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using negocio;
using dominio;

namespace pokedex_web
{
    public partial class Default : System.Web.UI.Page
    {
        //creo una propiedad tipo LISTA POKEMONS
        public List<Pokemon> ListaPokemon { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {//voy a usar esto para mi lista de Pokemons y mostrar las tarjetas
            PokemonNegocio negocio = new PokemonNegocio();
            //voy a cargar la lista yendo a la base de datos. Una vez q tenga cargada la lista de
            //Pokemons(una vez q carga la pantalla, xq esto se ejecuta cuando carga la pantalla), voy
            //a estar en condiciones de usar la lista en el front de esta pantalla(Default.aspx), y en esa pantalla puedo usar un ForEach
            ListaPokemon = negocio.listarConSP();

            //voy a cargar de datos el repetidor, lo pongo dentro de un if xq queremos q se cargue solo si es la primera vez q se ejecuta
            if(!IsPostBack)
            {   
             repRepetidor.DataSource = ListaPokemon;
             repRepetidor.DataBind();
            }

        }

        protected void btnEjemplo_Click(object sender, EventArgs e)//el sender es el elemento q envió el evento, el sender es un object pero yo se q es un boton
        {//cuando haga click, el boton me trae a este evento con un argumento
            string valor = ((Button)sender).CommandArgument;//casteo explicito xq yo se q es un boton
        }

    }
}