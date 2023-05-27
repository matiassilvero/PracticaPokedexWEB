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
    public partial class PokemonsLista : System.Web.UI.Page
    {
        public bool FiltroAvanzado { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
             FiltroAvanzado = chkAvanzado.Checked;//le pongo lo q tiene el checkbox
             PokemonNegocio negocio = new PokemonNegocio();
             Session.Add("listaPokemons", negocio.listarConSP());//capturo con Session para dps usar la Session en el filtro
             dgvPokemons.DataSource = Session["listaPokemons"];
             dgvPokemons.DataBind();
            }
        }

        protected void dgvPokemons_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = dgvPokemons.SelectedDataKey.Value.ToString();
            Response.Redirect("FormularioPokemon.aspx?id=" + id);
        }

        protected void dgvPokemons_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvPokemons.PageIndex = e.NewPageIndex;
            dgvPokemons.DataBind();
        }

        protected void filtro_TextChanged(object sender, EventArgs e)
        {
            List<Pokemon> lista = (List<Pokemon>)Session["listaPokemons"];
            List<Pokemon> listaFiltrada = lista.FindAll(x => x.Nombre.ToUpper().Contains(txtfiltro.Text.ToUpper()));
            dgvPokemons.DataSource = listaFiltrada;
            dgvPokemons.DataBind();
        }

        protected void chkAvanzado_CheckedChanged(object sender, EventArgs e)
        {//cada vez q cambie el checked de filtro avanzado(checkee o descheckee), entra aca
         //este fragmento de codigo maneja q se active o inactive el campo de filtro 
            FiltroAvanzado = chkAvanzado.Checked;//depende lo q tenga lo guarda en la variable
            txtfiltro.Enabled = !FiltroAvanzado;//a la caja de texto del filtro comun, le agrego lo contrario de lo q tenga
            //el filtro avanzado. Si el filtro avanzado esta activo, lo niego y le digo q el Enable es false(para q el filtro
            //rapido este deshabilitado) Y cuando el FiltroAvanzado este false, lo voy a negar y habilito el filtro rapido
            }

        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        {//esto tiene el autopostback en true xq yo necesito q cada vez q cambie, vaya al servidor y q me llene
         //el otro desplegable. Xq cada vez q me cambie la seleccion del cambio necesito q cambie el de criterio
            ddlCriterio.Items.Clear();//esto es para q no me acumule las cosas cargadas
            if(ddlCampo.SelectedItem.ToString() == "Número")
            {
                ddlCriterio.Items.Add("Igual a");
                ddlCriterio.Items.Add("Mayor a");
                ddlCriterio.Items.Add("Menor a");
            }
            else
            {
                ddlCriterio.Items.Add("Contiene");
                ddlCriterio.Items.Add("Comienza con");
                ddlCriterio.Items.Add("Termina con");
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                PokemonNegocio negocio = new PokemonNegocio();
                dgvPokemons.DataSource = negocio.filtrar(
                    ddlCampo.SelectedItem.ToString(),
                    ddlCriterio.SelectedItem.ToString(), 
                    txtFiltroAvanzado.Text, 
                    ddlEstado.SelectedItem.ToString());
                dgvPokemons.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex);
                throw;
            }
        }
    }
}