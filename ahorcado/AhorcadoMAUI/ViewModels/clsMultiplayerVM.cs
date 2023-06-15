using AhorcadoMAUI.ViewModels.Utilidades;
using AhorcadoMAUI.Views;
using CommunityToolkit.Maui.Views;
using Entidades;
using Plugin.Maui.Audio;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AhorcadoMAUI.ViewModels
{
    public class clsMultiplayerVM : clsVMBase
    {

        #region Atributos
        private clsJugador jugadorEnPartida;
        private bool empiezaJuego;
        private bool juegoTerminado;
        private string infoContrincante;
        private HubConnection _connection;
        /************************************/
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
        IAudioPlayer musicaFondo;
        IAudioPlayer aciertoAudio;
        IAudioPlayer falloAudio;
        IAudioPlayer victoriaAudio;
        IAudioPlayer derrotaAudio;
        #endregion

        #region Propiedades
        public clsJugador JugadorEnPartida
        {
            get { return jugadorEnPartida; }
        }
        public string InfoContrincante
        {
            get { return infoContrincante; }
        }
        #endregion

        #region Constructores
        public clsMultiplayerVM(IAudioManager audioManager, IAudioManager audioManager2, IAudioManager audioManager3, IAudioManager audioManager4, IAudioManager audioManager5)
        {
            crearPartida();

            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5083/ahorcado")
            .Build();

            _connection.On<clsJugador>("Esperar", esperarJugadores); // jugador 1
            _connection.On<clsJugador>("ElegirPalabra", partidaCompletada); // jugador 2 + le avisa de que empieza la partida
            //_connection.On<clsJugador>("Espectador", eresEspectador); // jugador 3 o mayor a 3 = espectador 
            _connection.On<string>("EsperarPalabra", esperarPalabra); // jugador 3 o mayor a 3 = espectador 

            _connection.On<string>("EmpezarJuego", recibirPalabra); // actualiza el tablero de ambos jugadores + cambia los turnos

            //_connection.On<string>("AvisarFinPartida", finPartida); // avisa del fin de la partida a todos los jugadores 

            try
            {
                _connection.StartAsync();
            }
            catch (Exception ex)
            {
                lblAvisos = "Error de conexión, inténtelo de nuevo más tarde.";
                NotifyPropertyChanged(nameof(lblAvisos));
            }

            unirseALaPartida();

        }
        #endregion

        #region SignalR
        /// <summary>
        /// Método que crea una nueva clase jugador e invoca el método del hub "UnirseAlJuego"
        /// </summary>
        private async void unirseALaPartida()
        {
            jugadorEnPartida = new clsJugador();
            await _connection.InvokeCoreAsync("UnirseAlJuego", args: new[] { jugadorEnPartida });

        }
        /// <summary>
        /// Método que avisa al jugador 1 la ficha que le toca y que tiene que esperar a otro jugador para poder iniciar la partida.
        /// </summary>
        /// <param name="jugador"></param>
        public void esperarJugadores(clsJugador jugador)
        {
            jugador.IdJugador = 1;
            jugador.NombreJugador = "Jugador 1";
            lblAvisos = $"¡Eres el {jugador.NombreJugador}! Esperando a otro jugador";
            jugadorEnPartida = jugador;

            NotifyPropertyChanged(nameof(lblAvisos));

        }
        /// <summary>
        /// Método que completa la partida y muestra a los jugadores el listado de palabras. Selecciona una y la envía al otro jugador 
        /// </summary>
        public async void partidaCompletada(clsJugador jugador)
        {
            if (jugador.IdJugador == 1)
            {
                // popup para elegir palabra
            }
            else
            {
                jugador.IdJugador = 2;
                jugador.NombreJugador = "Jugador 2";
                lblAvisos = $"¡Eres el {jugador.NombreJugador}!";
                jugadorEnPartida = jugador;

                // popup para elegir palabra
            }

            // le pasamos la palabra, cuando cree lo del popup lo cambio
            await _connection.InvokeCoreAsync("PalabraAleatoria", args: new[] { jugadorEnPartida });

            NotifyPropertyChanged(nameof(lblAvisos));
        }
        /// <summary>
        /// 
        /// </summary>
        public void esperarPalabra(string palabra)
        {
            // decirle al jugador listo que espere y al que está decidiendo aun le manda la palabra que debe adivinar 
        }

        public void recibirPalabra(string palabra)
        {
            // le manda la palabra al jugador que estaba esperando y comienza la partida de ambos
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
        /// Método que comprueba si se ha cumplido alguno de los requisitos necesarios para terminar la partida
        /// </summary>
        private async Task mostrarPopUpFin()
        {
            int alturaPopup = 0;

            musicaFondo.Stop();

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
                    derrotaAudio.Stop();

                    crearPartida();
                }
                else
                {
                    derrotaAudio.Stop();

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

            if (letrasSeleccionadas.Contains(inputJugador))
            {
                lblAvisos = "Deja de repetir letras";
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
                    else
                    {


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
        /// Método que inicializa lo necesario para comenzar la partida de un solo jugador.
        /// </summary>
        private void crearPartida()
        {
            empiezaJuego = false;
            juegoTerminado = false;
            infoContrincante = "";
            crearAudios();
            intentosRestantes = 5;
            actualizarImagen();
            letrasSeleccionadas = "";
            lblAvisos = "";
            inputJugador = "";
            enviarInputCommand = new DelegateCommand(enviarInputCommand_Executed, enviarInputCommand_CanExecute);


            NotifyPropertyChanged("IntentosRestantes");
            NotifyPropertyChanged("LetrasSeleccionadas");
            NotifyPropertyChanged("LblAvisos");
            NotifyPropertyChanged("InputJugador");

        }

        /// <summary>
        /// Método para actualizar la imagen en caso de error , la imagen será actualizada por la correspondiente en caso de que el jugador cometa un error.
        /// </summary>
        private void actualizarImagen()
        {

            imagen = $"a{intentosRestantes}a.png";

            NotifyPropertyChanged("Imagen");

        }

        public async void crearAudios()
        {

            musicaFondo = audio.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("background_music.wav"));

            aciertoAudio = audio2.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("right.wav"));

            falloAudio = audio3.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("miss.wav"));

            victoriaAudio = audio4.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("win.wav"));

            derrotaAudio = audio5.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("lose.wav"));

            musicaFondo.Play();
        }
        #endregion




    }
}
