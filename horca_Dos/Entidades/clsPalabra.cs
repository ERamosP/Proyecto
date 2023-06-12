using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class clsPalabra
    {
        #region atributos privados

        private int _id;
        private string _nombre;

        #endregion

        #region Propiedades publicos

        public int id 
        {
            get { return _id; }
            set { _id = value; }
        }

        public string nombre 
        {
            get { return _nombre; }
            set { _nombre = value; }
        }


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

        ///Hola
       
    }
}
