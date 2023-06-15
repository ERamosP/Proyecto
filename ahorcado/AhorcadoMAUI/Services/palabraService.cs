using System;
using Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AhorcadoMAUI.Services
{
    public class palabraService
    {
        /// <summary>
        /// Función que llama a la api, pide una palabra aleatoria y la devuelve
        /// Precondiciones: Ninguna
        /// Postcondiciones: Si no encuentra ninguna palabra devuelve null.
        /// Si hay un error, lanza la axcepción correspondiente
        /// </summary>
        /// <returns>La palabra devuelta por la api</returns>
        public static async Task<clsPalabra> getPalabraAleatoria()
        {
            HttpClient httpClient;
            string json;
            string uri = $"{clsUriBase.getUriBase()}apipalabras";
            Uri miUri = new Uri(uri);
            HttpResponseMessage httpResponse;
            clsPalabra palabraEncontrada = new clsPalabra();

            httpClient = new HttpClient();
            try
            {
                httpResponse = await httpClient.GetAsync(miUri);

                if (httpResponse.IsSuccessStatusCode)
                {
                    json = await httpClient.GetStringAsync(miUri);
                    httpClient.Dispose();
                    palabraEncontrada = JsonConvert.DeserializeObject<clsPalabra>(json);
                    palabraEncontrada.nombre = palabraEncontrada.nombre.ToLower();

                }
                else if (httpResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    palabraEncontrada = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return palabraEncontrada;
        }


        /// <summary>
        /// Función que llama a la api, pide 3 palabras aleatorias y las devuelve
        /// Precondiciones: Ninguna
        /// Postcondiciones: Si no encuentra ninguna palabra devuelve null. 
        /// Si hay un error, lanza la axcepción correspondiente
        /// </summary>
        /// <returns>La palabra devuelta por la api</returns>
        public static async Task<List<clsPalabra>> get3PalabraAleatoria()
        {
            HttpClient httpClient;
            string json;
            string uri = $"{clsUriBase.getUriBase()}api3palabras";
            Uri miUri = new Uri(uri);
            HttpResponseMessage httpResponse;
            List<clsPalabra> palabrasEncontradas = new List<clsPalabra>();

            httpClient = new HttpClient();
            try
            {
                httpResponse = await httpClient.GetAsync(miUri);

                if (httpResponse.IsSuccessStatusCode)
                {
                    json = await httpClient.GetStringAsync(miUri);
                    httpClient.Dispose();
                    palabrasEncontradas = JsonConvert.DeserializeObject<List<clsPalabra>>(json);
                    for (int i = 0; i < palabrasEncontradas.Count; i++)
                    {
                        palabrasEncontradas[i].nombre = palabrasEncontradas[i].nombre.ToLower();
                    }

                }
                else if (httpResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    palabrasEncontradas = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return palabrasEncontradas;
        }

    }
}
