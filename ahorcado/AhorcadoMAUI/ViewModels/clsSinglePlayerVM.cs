using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AhorcadoMAUI.ViewModels.Utilidades;
using Entidades;

namespace AhorcadoMAUI.ViewModels
{
    public class clsSinglePlayerVM : clsVMBase
    {
        #region Atributos
        private clsPalabra palabraParaAdivinar;
        private StringBuilder adivinado; // ponemos un guión en cada posición
        private int letrasRestantes;// cuando llegue a 0 se ha adivinado la palabra y acaba el juego. El jugador no necesita verlo.
        private string lblAvisos;
        private string letrasSeleccionadas;
        private string inputJugador;
        private DelegateCommand enviarInputCommand;
        private string imagen;
        private int intentosRestantes;
        private bool juegoTerminado;

        #endregion

        #region Propiedades
        public StringBuilder Adivinado
        {
            get { return adivinado; }
        }
        public string LblAvisos
        {
            get { return lblAvisos; }
        }
        public string LetrasSeleccionadas
        {
            get { return letrasSeleccionadas; }
        }
        public string InputJugador
        {
            get { return inputJugador; }
            set
            {
                if (value != null)
                {
                    inputJugador = value;
                    NotifyPropertyChanged(nameof(InputJugador));
                    EnviarInputCommand.RaiseCanExecuteChanged();
                }
                
            }
        }
        public DelegateCommand EnviarInputCommand
        {
            get { return enviarInputCommand; }
        }
        public string Imagen
        {
            get { return imagen; }
        }
        public int IntentosRestantes
        {
            get { return intentosRestantes; }
        }
        #endregion

        #region Constructores
        public clsSinglePlayerVM()
        {
            crearPartida();
        }
        #endregion

        #region Commands
        /// <summary>
        /// Función del botón. Comprueba si el input del jugador se encuentra en la palabra a adivinar. 
        /// Si se encuentra, desbloquea esas letras en la string adivinado.
        /// </summary>
        private void enviarInputCommand_Executed()
        {
            comprobarInput();
        }

        /// <summary>
        /// Habilita o deshabilita el botón dependiendo de si el input del jugador es válido o no
        /// </summary>
        /// <returns></returns>
        private bool enviarInputCommand_CanExecute()
        {
            bool sePuedeEnviar = false;

            if (!string.IsNullOrEmpty(inputJugador) && inputJugador.Length == 1 && Regex.IsMatch(inputJugador, @"^[A-z]"))
            {
                sePuedeEnviar = true;
            }

            return sePuedeEnviar;
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método que inicializa lo necesario para comenzar la partida de un solo jugador.
        /// </summary>
        private void crearPartida()
        {
            juegoTerminado = false;
            imagen = "a5a.png"; //imagen = la que que tenga solo la horca
            palabraAleatoria();
            intentosRestantes = 5;
            letrasSeleccionadas = "";
            enviarInputCommand = new DelegateCommand(enviarInputCommand_Executed, enviarInputCommand_CanExecute);
        }

        /// <summary>
        /// Método que comprueba si se ha cumplido alguno de los requisitos necesarios para terminar la partida
        /// </summary>
        private void comprobarFin()
        {
            if (intentosRestantes == 0)
            {
                lblAvisos = "has perdio";
            }
            else if (letrasRestantes == 0)
            {
                lblAvisos = $"has ganao y la palabra era {palabraParaAdivinar.nombre}";
            }

            NotifyPropertyChanged(nameof(LblAvisos));
        }

        /// <summary>
        /// Método comprueba si la letra se encuentra en la palabra. 
        /// Si se encuentra, cambia cada posición de _ donde se encuentre la letra en la palabra adivinar por la letra introducida por el usuario. 
        /// </summary>
        private void comprobarInput()
        {

            inputJugador = inputJugador.ToLower();

            if(letrasSeleccionadas.Contains(inputJugador))
            {
                lblAvisos = "Deja de repetir letras";
            }
            else
            {
                if (palabraParaAdivinar.nombre.Contains(inputJugador))
                {
                    // cambiamos cada _ por la letra en adivinado
                    for (int i = 0; i < adivinado.Length; i++)
                    {
                        if (palabraParaAdivinar.nombre[i].Equals(inputJugador[0]))
                        {
                            adivinado[i] = inputJugador[0];
                        }
                    }
                    NotifyPropertyChanged("Adivinado");

                }
                else
                {
                    intentosRestantes--;
                    letrasSeleccionadas += " " + inputJugador[0];
                    actualizarImagen();
                    NotifyPropertyChanged("IntentosRestantes");
                    NotifyPropertyChanged("LetrasSeleccionadas");

                }

                comprobarFin();
            }

            NotifyPropertyChanged(nameof(LblAvisos));
           
            
        }

        /// <summary>
        /// Método que realiza la llamada a la API y recibe una palabra aleatoria que hay que adivinar.
        /// Prepara la palabra a adivinar a base de guiones bajos
        /// </summary>
        private void palabraAleatoria()
        {
            palabraParaAdivinar = new clsPalabra(1, "cacahuete");

            adivinado = new StringBuilder();

            letrasRestantes = palabraParaAdivinar.nombre.Length;

            for (int i = 0; i < palabraParaAdivinar.nombre.Length; i++)
            {
                adivinado.Append("*");
            }
        }

        /// <summary>
        /// Método para actualizar la imagen en caso de error , la imagen será actualizada por la correspondiente en caso de que el jugador cometa un error.
        /// </summary>
        private void actualizarImagen() {

            imagen = $"a{intentosRestantes}a.png";

            NotifyPropertyChanged("Imagen");
        
        }

        #endregion
    }
}
