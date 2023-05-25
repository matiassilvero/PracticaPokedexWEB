using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;//agregamos esta para poner bien los nombres de las columnas(con tildes) en mi DGV          
                            //con esto me sirve el [DisplayName]

namespace dominio
{
    //esta clase define el modelo de mi clase, el objeto q voy a usar
    //cada Clase va necesitar su clase "negocio"(es un ej) para acceder a la DB
    public class Pokemon
    {
        public int id { get; set; }

        [DisplayName("Número")]//debemos ponerlo arriba de lo q queremos arreglar
        public int Numero { get; set; }
        public string Nombre { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        public string UrlImagen { get; set; }
        public Elemento Tipo { get; set; }
        public Elemento Debilidad { get; set; }
    }
}
