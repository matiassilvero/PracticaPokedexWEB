<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="pokedex_web.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Hola!</h1>
    <p>Llegaste al Pokedex Web, tu lugar Pokemon...</p>

    <%--yo voy a querer mostrar las tarjetas dependiendo de cuantos Pokemons tenga, tengo q envolver el foreach con los piquitos y % para 
        poder USAR CODIGO C# aca. Atento con el ":" pegado al primer piquito% en el h5 cuando ponemos los nombres de las propiedades--%>
    <div class="row row-cols-1 row-cols-md-3 g-4">
        <%--        <%
            foreach (dominio.Pokemon poke in ListaPokemon)
            {
        %>
        <div class="col">
            <div class="card">
                <img src=" <%: poke.UrlImagen %>" class="card-img-top" alt="...">
                <div class="card-body">
                    <h5 class="card-title"> <%: poke.Nombre %> </h5> 
                    <p class="card-text"> <%: poke.Descripcion %> </p>
                    <a href="DetallePokemon.aspx?id=<%: poke.id %>">Ver detalle</a>
                </div>
            </div>
        </div>
        <%  } %>--%>


        <%--Ahora vamos a practicar lo mismo, solo q con REPEATER, es propia de ASP. es lo mismo q obtenemos con el ForEach
            la diferencia con el FOREACH es q aca dentro de los piquitos% pongo #Eval() y el nombre de la propiedad del objeto de 
            la lista q repito. Atento con poner #Eval... PEGADO al piquito% cuando pongo los nombres de las propiedades.
            Es mas corto y mas limpio q un foreach. Con repeater puedo mandar un dato al codebehind, cosa q con el foreach es mas complicado.--%>
        <asp:Repeater runat="server" ID="repRepetidor">
            <ItemTemplate>

                <div class="col">
                    <div class="card" >
                        <img src=" <%#Eval("UrlImagen") %>" class="card-img-top" alt="...">
                        <div class="card-body">
                            <h5 class="card-title"><%#Eval("Nombre") %> </h5>
                            <p class="card-text"><%#Eval("Descripcion") %> </p>
                            <a href="DetallePokemon.aspx?id=<%#Eval("id") %>">Ver detalle</a>
                            <asp:button Text="Ejemplo" CssClass="btn btn-primary" ID="btnEjemplo" runat="server" CommandArgument='<%#Eval("id")%>' CommandName="PokemonId" OnClick="btnEjemplo_Click"/>
                           <%-- cada vez q toque este boton va a ir al evento q creo con OnClick, para capturar el valor le pongo el 
                            ComandArgument, este boton ejecuta un comando q lleva un argumento. Con esto configuro el boton para q cuando lance una accion, se lleve un valor como argumento
                               Atento que va entre comillas simples el ComandArgument--%>
                        </div>
                    </div>
                </div>

            </ItemTemplate>
        </asp:Repeater>
    </div>

</asp:Content>
