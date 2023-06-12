﻿using System;
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
        private string adivinado; // ponemos un guión en cada posición
        private string lblAvisos;
        private ObservableCollection<char> letrasSeleccionadas;
        private string inputJugador;
        private DelegateCommand enviarInputCommand;
        private string imagen;
        private int intentosRestantes;
        private bool seHaAdivinadoLaPalabra;
        private bool juegoTerminado;

        #endregion

        #region Propiedades
        public string Adivinado
        {
            get { return adivinado; }
        }
        public string LblAvisos
        {
            get { return lblAvisos; }
        }
        public ObservableCollection<char> LetrasSeleccionadas
        {
            get { return letrasSeleccionadas; }
        }
        public string InputJugador
        {
            get { return inputJugador; }
            set { inputJugador = value; }
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
            Regex validCharacters = new Regex("^[a-z]$");

            if (!String.IsNullOrEmpty(inputJugador) && inputJugador.Length==1 && validCharacters.IsMatch(inputJugador))
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
            seHaAdivinadoLaPalabra = false; 
            //imagen = la que que tenga solo la horca
            palabraAleatoria();
            intentosRestantes = 5;
            letrasSeleccionadas = new ObservableCollection<char>();
        }

        /// <summary>
        /// Método que comprueba si se ha cumplido alguno de los requisitos necesarios para terminar la partida
        /// </summary>
        private void comprobarFin()
        {

        }

        /// <summary>
        /// Método comprueba si la letra se encuentra en la palabra. 
        /// Si se encuentra, cambia cada posición de _ donde se encuentre la letra en la palabra adivinar por la letra introducida por el usuario. 
        /// </summary>
        private void comprobarInput()
        {

        }

        /// <summary>
        /// Método que realiza la llamada a la API y recibe una palabra aleatoria que hay que adivinar.
        /// </summary>
        private void palabraAleatoria()
        {
            //palabraParaAdivinar = llamada API
        }
        #endregion
    }
}
