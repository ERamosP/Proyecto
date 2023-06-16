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
using AhorcadoMAUI.Services;

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
        private string palabraEnviar;
        private List<clsPalabra> listaPalabras;
        /************************************/
        private clsPalabra palabraParaAdivinar = new clsPalabra();
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
        public clsMultiplayerVM(IAudioManager audioManager, IAudioManager audioManager2, IAudioManager audioManager3, IAudioManager audioManager4, IAudioManager audioManager5)
        {
            this.audio = audioManager;

            this.audio2 = audioManager2;

            this.audio3 = audioManager3;

            this.audio4 = audioManager4;

            this.audio5 = audioManager5;

            crearPartida();

            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5083/ahorcado")
            .Build();

            _connection.On<clsJugador>("Esperar", esperarJugadores); // jugador 1
            _connection.On<clsJugador>("ElegirPalabra", partidaCompletada); // jugador 2 + le avisa de que empieza la partida
            //_connection.On<clsJugador>("Espectador", eresEspectador); // jugador 3 o mayor a 3 = espectador 
            _connection.On<string>("EsperarPalabra", esperarPalabra); // jugador 3 o mayor a 3 = espectador 

            _connection.On<string>("EmpezarJuego", recibirPalabra); // actualiza el tablero de ambos jugadores + cambia los turnos
            _connection.On<string>("Actualizar", actualizarInfoContrincante); // avisa del fin de la partida a todos los jugadores 
            _connection.On<string>("AvisarFinPartida", finPartida); // avisa del fin de la partida a todos los jugadores 

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
            jugador.Listo = false;
            lblAvisos = $"¡Eres el {jugador.NombreJugador}! Esperando a otro jugador";
            jugadorEnPartida = jugador;


            NotifyPropertyChanged(nameof(LblAvisos));

        }
        /// <summary>
        /// Método que completa la partida y muestra a los jugadores el listado de palabras. Selecciona una y la envía al otro jugador 
        /// </summary>
        public async Task partidaCompletada(clsJugador jugador)
        {
            await palabraAleatoria();

            if (jugador.IdJugador == 1)
            {

                var popup = new SeleccionPopUp(listaPalabras);

                var result = await App.Current.MainPage.ShowPopupAsync(popup);
                palabraEnviar = result.ToString();
            }
            else
            {
                jugador.IdJugador = 2;
                jugador.NombreJugador = "Jugador 2";
                jugador.Listo = false;
                lblAvisos = $"¡Eres el {jugador.NombreJugador}!";
                jugadorEnPartida = jugador;


                var popup = new SeleccionPopUp(listaPalabras);

                await MainThread.InvokeOnMainThreadAsync(async () =>
                {

                    var result = await App.Current.MainPage.ShowPopupAsync(popup);
                    palabraEnviar = result.ToString();
                }
                );


            }

            jugadorEnPartida.Listo = true;

            // le pasamos la palabra, cuando cree lo del popup lo cambio
            await _connection.InvokeCoreAsync("PalabraAleatoria", args: new[] { palabraEnviar });
            lblAvisos = $"El otro tiene que adivinar {palabraEnviar}.";
            NotifyPropertyChanged(nameof(LblAvisos));
        }

        /// <summary>
        /// Método que le pasa al contrincante la palabra elegida para que adivine y te hace esperar a que el otro te mande la suya. 
        /// </summary>
        /// <param name="palabra"></param>
        public void esperarPalabra(string palabra)
        {
            /*if (jugadorEnPartida.Listo)
            {
                lblAvisos = $"El otro tiene que adivinar {palabra}.";
            }
            else if(!jugadorEnPartida.Listo)
            {*/
            palabraParaAdivinar.nombre = palabra;

            ponerAsteriscos();
            //}

            NotifyPropertyChanged(nameof(LblAvisos));
        }

        /// <summary>
        /// Método que genera migrañas
        /// </summary>
        /// <param name="palabra"></param>
        public void recibirPalabra(string palabra)
        {
            if (string.IsNullOrEmpty(palabraParaAdivinar.nombre))
            {
                palabraParaAdivinar.nombre = palabra;

                ponerAsteriscos();
            }

            empiezaJuego = true;
            lblAvisos = "Comienza la partida";

            NotifyPropertyChanged(nameof(LblAvisos));
        }

        /// <summary>
        /// Método que recibe del Hub la info del contrincante y la actualiza en pantalla.
        /// </summary>
        /// <param name="info"></param>
        public void actualizarInfoContrincante(string info)
        {
            infoContrincante = info;
            NotifyPropertyChanged(nameof(InfoContrincante));
        }

        /// <summary>
        /// Método que muestra a ambos jugadores quién ha sido el ganador de la partida.
        /// </summary>
        /// <param name="mensajeFin"></param>
        private async Task finPartida(string mensaje)
        {
            /*int alturaPopup = 0;
            

            if (mensaje.StartsWith("Ha perdido"))
            {

                derrotaAudio.Play();
                alturaPopup = 350;
                imagen = "muerto.png";
            }
            else 
            {
                victoriaAudio.Play();
                alturaPopup = 375;
                imagen = "salvado.png";
            }

            //popup fin del juego con un único botón para salir de la app 

            var popup = new FinalPopUp(imagen, alturaPopup, mensaje);

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
            }*/

            lblAvisos = mensaje;
            NotifyPropertyChanged("LblAvisos");


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

            if (!string.IsNullOrEmpty(inputJugador) && inputJugador.Length == 1 && Regex.IsMatch(inputJugador, @"^[A-z]") && empiezaJuego == true)
            {
                sePuedeEnviar = true;
            }

            return sePuedeEnviar;
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Método comprueba si la letra se encuentra en la palabra. 
        /// Si se encuentra, cambia cada posición de _ donde se encuentre la letra en la palabra adivinar por la letra introducida por el usuario. 
        /// </summary>
        private async void comprobarInput()
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
                    //le mandamos nuestros intentos restantes al otro jugador
                    await _connection.InvokeCoreAsync("ActualizarInfoContrincante", args: new[] { intentosRestantes.ToString() });


                }

                letrasSeleccionadas += " " + inputJugador[0];



                NotifyPropertyChanged("LetrasSeleccionadas");



                if (intentosRestantes == 0)
                {
                    juegoTerminado = true;

                    await _connection.InvokeCoreAsync("FinDePartida", args: new[] {$"Ha perdido : {jugadorEnPartida.NombreJugador}" });
                }
                else if (letrasRestantes == 0)
                {
                    juegoTerminado = true;

                    await _connection.InvokeCoreAsync("FinDePartida", args: new[] { $"Ha ganado : {jugadorEnPartida.NombreJugador}" });

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
            crearAudios();
            intentosRestantes = 5;
            actualizarImagen();
            letrasSeleccionadas = "";
            lblAvisos = "";
            inputJugador = "";
            infoContrincante = "5";
            enviarInputCommand = new DelegateCommand(enviarInputCommand_Executed, enviarInputCommand_CanExecute);


            NotifyPropertyChanged("IntentosRestantes");
            NotifyPropertyChanged("LetrasSeleccionadas");
            NotifyPropertyChanged("LblAvisos");
            NotifyPropertyChanged("InputJugador");
            NotifyPropertyChanged("InfoContrincante");

        }

        /// <summary>
        /// Método que realiza la llamada a la API y recibe un listado de palabras para que el jugador elija una que enviar al contrincante
        /// </summary>
        private async Task palabraAleatoria()
        {

            try
            {
                listaPalabras = await palabraService.get3PalabraAleatoria();
            }
            catch (Exception ex)
            {
                lblAvisos = "Error de conexión";
            }

        }

        private async void ponerAsteriscos()
        {
            adivinado = new StringBuilder();

            letrasRestantes = palabraParaAdivinar.nombre.Length;

            for (int i = 0; i < palabraParaAdivinar.nombre.Length; i++)
            {
                adivinado.Append("*"); //
            }

            NotifyPropertyChanged("Adivinado");
            NotifyPropertyChanged("LblAvisos");
        }

        /// <summary>
        /// Método para actualizar la imagen en caso de error , la imagen será actualizada por la correspondiente en caso de que el jugador cometa un error.
        /// </summary>
        private void actualizarImagen()
        {

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
