using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AhorcadoMAUI.Services;
using AhorcadoMAUI.ViewModels.Utilidades;
using AhorcadoMAUI.Views;
using CommunityToolkit.Maui.Views;
using Entidades;
using Plugin.Maui.Audio;

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
        private readonly IAudioManager audio;
        private readonly IAudioManager audio2;
        private readonly IAudioManager audio3;
        private readonly IAudioManager audio4;
        private readonly IAudioManager audio5;

        IAudioPlayer aciertoAudio;
        IAudioPlayer falloAudio;
        IAudioPlayer victoriaAudio;
        IAudioPlayer derrotaAudio;

        //private bool juegoTerminado; multiplayer

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
        public clsSinglePlayerVM(IAudioManager audioManager, IAudioManager audioManager2, IAudioManager audioManager3, IAudioManager audioManager4, IAudioManager audioManager5)
        {

            this.audio = audioManager;

            this.audio2 = audioManager2;

            this.audio3 = audioManager3;

            this.audio4 = audioManager4;

            this.audio5 = audioManager5;

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
            crearAudios();
            //juegoTerminado = false;
            intentosRestantes = 5;
            palabraAleatoria();
            actualizarImagen();
            letrasSeleccionadas = "";
            lblAvisos = "";
            inputJugador =  "";
            enviarInputCommand = new DelegateCommand(enviarInputCommand_Executed, enviarInputCommand_CanExecute);


            NotifyPropertyChanged("IntentosRestantes");
            NotifyPropertyChanged("LetrasSeleccionadas");
            NotifyPropertyChanged("LblAvisos");
            NotifyPropertyChanged("InputJugador");
            
        }

        /// <summary>
        /// Método que muestra al usuario un popup personalizado con su victoria o derrota.
        /// Le da la opción de jugar de nuevo o salir del juego.
        /// </summary>
        private async Task mostrarPopUpFin()
        {
            int alturaPopup = 0;


            if (intentosRestantes == 0)
            {
                
                derrotaAudio.Play();
                alturaPopup = 350;
                imagen = "muerto.png";
            }
            else if (letrasRestantes == 0)
            {
                victoriaAudio.Play();
                alturaPopup = 375;
                imagen = "salvado.png";
            }

            var popup = new FinalPopUp(imagen, alturaPopup, palabraParaAdivinar.nombre);

            var result = await App.Current.MainPage.ShowPopupAsync(popup);

            if (result is bool boolResult)
            {
                if (boolResult)
                {

                    crearPartida();
                }
                else
                {

                    await Application.Current.MainPage.Navigation.PopToRootAsync();
                }
            }


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
                lblAvisos = "¡Cuidado con las letras repetidas!";
            }
            else
            {
                lblAvisos = "";

                if (palabraParaAdivinar.nombre.Contains(inputJugador))
                {

                    aciertoAudio.Play();

                    // cambiamos cada _ por la letra en adivinado
                    for (int i = 0; i < adivinado.Length; i++)
                    {
                        if (palabraParaAdivinar.nombre[i].Equals(inputJugador[0]))
                        {
                            adivinado[i] = inputJugador[0];
                            letrasRestantes--;
                        }
                    }
                    NotifyPropertyChanged("Adivinado"); 
                }
                else
                {

                    if (falloAudio.IsPlaying)
                    {

                        falloAudio.Stop();

                        falloAudio.Play();

                    }
                    else {


                        falloAudio.Play();

                    }

                    
                    intentosRestantes--;
                    actualizarImagen();
                    NotifyPropertyChanged("IntentosRestantes");
                    

                }

                letrasSeleccionadas += " " + inputJugador[0];

                

                NotifyPropertyChanged("LetrasSeleccionadas");


                if (intentosRestantes == 0 || letrasRestantes == 0)
                {
                    mostrarPopUpFin();
                }
                
            }

            NotifyPropertyChanged("LblAvisos");

        }

        /// <summary>
        /// Método que realiza la llamada a la API y recibe una palabra aleatoria que hay que adivinar.
        /// Prepara la palabra a adivinar a base de guiones bajos
        /// </summary>
        private async void palabraAleatoria()
        {

            // TODO: cambiar la app para que la llamada a la API se haga en el menú inicial y pase la palabra al VM
            // así mostramos el error en el menú si no conecta con la API.
            try
            {
                palabraParaAdivinar = await palabraService.getPalabraAleatoria();
            }
            catch (Exception ex)
            {
                lblAvisos = "Error de conexión";
            }
            

            adivinado = new StringBuilder();

            letrasRestantes = palabraParaAdivinar.nombre.Length;

            for (int i = 0; i < palabraParaAdivinar.nombre.Length; i++)
            {
                adivinado.Append("*");
            }

            NotifyPropertyChanged("Adivinado");
            NotifyPropertyChanged("LblAvisos");
        }

        /// <summary>
        /// Método para actualizar la imagen en caso de error , la imagen será actualizada por la correspondiente en caso de que el jugador cometa un error.
        /// </summary>
        private void actualizarImagen() {

            imagen = $"a{intentosRestantes}a.png";

            NotifyPropertyChanged("Imagen");
        
        }



        /// <summary>
        /// Método para crear e inicializar los audios correspondientes
        /// </summary>
        public async void crearAudios()
        {

            aciertoAudio = audio2.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("right.wav"));

            falloAudio = audio3.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("miss.wav"));

            victoriaAudio = audio4.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("win.wav"));

            derrotaAudio = audio5.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("lose.wav"));

        }


        #endregion
    }
}
