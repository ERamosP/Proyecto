using Entidades;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace proyectoFinDeGradoAhorcado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class apiPalabras : ControllerBase
    {
        // GET: api/<apiPalabras>
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                clsPalabra palabra = DAL.Manejadora.ManejadoraPalabras_DAL.getPalabraRandom();
                return Ok(palabra);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

    }
}
