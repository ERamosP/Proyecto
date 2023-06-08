using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class clsJugador
    {

        #region Atributos

        private int idJugador;
        private string nomreJugador;

        #endregion

        #region Propiedades
        public int IdJugador
        {
            get { return idJugador; }
            set { idJugador = value; }
        }

        public string NombreJugador
        {
            get { return nomreJugador; }
            set { nomreJugador = value; }
        }
        #endregion

        #region Constructores
        public clsJugador()
        {

        }

        public clsJugador(int idJugador, string nomreJugador)
        {
            this.idJugador = idJugador;
            this.nomreJugador = nomreJugador;
        }


        #endregion
    }
}
