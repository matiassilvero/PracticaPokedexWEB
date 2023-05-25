using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Elemento//Agrego esta clase, asi podemos joinear Tabla Pokemons y Tabla Elementos
                           //en la clase Pokemon, va haber un atributo que es una clase Elemento
    {
        public int id { get; set; }
        public string Descripcion { get; set; }

        //hago esto porque mi dgv gralmente mapea los atributos de la clase Pokemon, y cuando yo lo-
        //- joineo con Elementos, tiene q mostrar un atributo de la clase Elemento(la cual es un atributo)-
        //- de mi clase Pokemon, entonces no sabe q mostrar(me muestra la definicion y el nombre de la clase, -
        //- xq por defecto muestra el ToString, y ahi dice eso), entonces ahora yo sobreescribo el ToString-
        //- para q me muestre la Descripcion de mi Elemento
        public override string ToString()
        {
            return Descripcion;//q me devuelva la descripcion de mi Elemento
        }

    }
}
