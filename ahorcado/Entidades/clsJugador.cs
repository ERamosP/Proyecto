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
        private bool listo;

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

        public bool Listo {

            get { return listo; }
            set { listo = value; }
        }
        #endregion

        #region Constructores
        public clsJugador()
        {

        }

        public clsJugador(int idJugador, string nomreJugador, bool listo)
        {
            this.idJugador = idJugador;
            this.nomreJugador = nomreJugador;
            this.listo = listo;
        }


        #endregion
    }
}
