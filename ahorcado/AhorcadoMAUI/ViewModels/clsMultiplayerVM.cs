using AhorcadoMAUI.ViewModels.Utilidades;
using Entidades;
using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhorcadoMAUI.ViewModels
{
    public class clsMultiplayerVM : clsVMBase
    {

        #region Atributos
        private clsJugador jugador;
        private bool empiezaJuego;
        private bool juegoTerminado;
        private string infoContrincante;
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
        public clsJugador Jugador
        {
            get { return jugador; }
        }
        public string InfoContrincante
        {
            get { return infoContrincante; }
        }
        #endregion

        public clsMultiplayerVM(IAudioManager audioManager, IAudioManager audioManager2, IAudioManager audioManager3, IAudioManager audioManager4, IAudioManager audioManager5)
        {


        }



    }
}
