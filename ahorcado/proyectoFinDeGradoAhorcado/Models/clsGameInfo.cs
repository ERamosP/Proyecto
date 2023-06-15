namespace proyectoFinDeGradoAhorcado.Models
{
    public class clsGameInfo
    {
        #region Atributos
        private static int numJugadoresEnPartida;
        private static int numJugadoresListos;
        #endregion

        #region Propiedades
        public static int NumJugadoresEnPartida
        {
            get { return numJugadoresEnPartida; }
            set { numJugadoresEnPartida = value; }
        }
        public static int NumJugadoresListos
        {
            get { return numJugadoresListos; }
            set { numJugadoresListos = value; }
        }
        #endregion

        #region Contructores
        public clsGameInfo()
        {
            numJugadoresEnPartida = 0;
            numJugadoresListos= 0;
        }

        #endregion
    }
}
