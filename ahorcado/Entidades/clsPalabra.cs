using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class clsPalabra
    {
        #region Propiedades

        public int id { get; set; }

        public string nombre { get; set; }


        #endregion

        #region Constructores
        public clsPalabra()
        {

        }
        public clsPalabra(int id,string nombre)
        {
            this.id = id;
            this.nombre = nombre;
        }

        #endregion
       
    }
}
