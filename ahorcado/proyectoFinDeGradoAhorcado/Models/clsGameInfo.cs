namespace proyectoFinDeGradoAhorcado.Models
{
    public class clsGameInfo
    {
        #region Atributos
        private static int numJugadores;
        #endregion

        #region Propiedades
        public static int NumJugadores
        {
            get { return numJugadores; }
            set { numJugadores = value; }
        }
        #endregion

        #region Contructores
        public clsGameInfo()
        {
            numJugadores = 0;
        }

        #endregion
    }
}
