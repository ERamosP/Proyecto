using Entidades;
using Microsoft.AspNetCore.SignalR;
using proyectoFinDeGradoAhorcado.Models;

namespace proyectoFinDeGradoAhorcado.Hubs
{
    public class ahorcadoHub : Hub
    {
        /// <summary>
        /// Comprobar el número de jugador que entra y dependiendo de ello, mandar la función correspondiente
        /// Primer jugador: espera al segundo.
        /// Segundo jugador: empieza la partida y da turno al jugador 1.
        /// Tres en adelante: solo puede observar la partida.
        /// </summary>
        /// <param name="jugador"></param>
        /// <returns></returns>
        public async Task UnirseAlJuego(clsJugador jugador)
        {

            if (clsGameInfo.NumJugadoresEnPartida == 0)
            {
                await Clients.Caller.SendAsync("Esperar", jugador);
                clsGameInfo.NumJugadoresEnPartida++;
            }
            else if (clsGameInfo.NumJugadoresEnPartida == 1)
            {
                await Clients.All.SendAsync("ElegirPalabra", jugador);
                clsGameInfo.NumJugadoresEnPartida++;
            }
            else
            {
                await Clients.Caller.SendAsync("Espectador", jugador);
                clsGameInfo.NumJugadoresEnPartida++;
            }

        }

        /// <summary>
        /// Envía la palabra para adivinar al contrincante
        /// </summary>
        /// <param name="palabra"></param>
        /// <returns></returns>
        public async Task PalabraAleatoria(string palabra)
        {
            if (clsGameInfo.NumJugadoresListos == 0)
            {
                await Clients.All.SendAsync("EsperarPalabra", palabra);
                clsGameInfo.NumJugadoresListos++;
            }
            else if (clsGameInfo.NumJugadoresListos == 1)
            {
                await Clients.All.SendAsync("EmpezarJuego", palabra);
                clsGameInfo.NumJugadoresListos++;
            }
        }


        // TODO: decidir cómo va a ser la vista. Ver si le pasamos la imagen o los intentos
        // restantes del jugador contrincante 
        public async Task ActualizarImagenAhorcado(string imagenAhorcadoSource)
        {
            await Clients.Others.SendAsync("Actualizar", imagenAhorcadoSource);
        }

        /// <summary>
        /// Avisa a todos los jugadores del fin de la partida y quién ganó.
        /// Reinicia el gameInfo.
        /// </summary>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        public async Task FinDePartida(string mensaje)
        {
            await Clients.All.SendAsync("AvisarFinPartida", mensaje);
            clsGameInfo.NumJugadoresEnPartida = 0;
            clsGameInfo.NumJugadoresListos = 0;
        }
    }
}
