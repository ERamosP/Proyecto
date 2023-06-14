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

            if (clsGameInfo.NumJugadores == 0)
            {
                await Clients.Caller.SendAsync("Esperar", jugador);
                clsGameInfo.NumJugadores++;
            }
            else if (clsGameInfo.NumJugadores == 1)
            {
                await Clients.Caller.SendAsync("Empezar", jugador);
                clsGameInfo.NumJugadores++;
            }
            else
            {
                await Clients.Caller.SendAsync("Espectador", jugador);
                clsGameInfo.NumJugadores++;
            }

        }

        // TODO: decidir cómo va a ser la vista. Ver si le pasamos la imagen o los intentos
        // restantes del jugador contrincante 
        public async Task ActualizarImagenAhorcado(string imagenAhorcadoSource)
        {
            await Clients.All.SendAsync("Actualizar", imagenAhorcadoSource);
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
            clsGameInfo.NumJugadores = 0;
        }
    }
}
