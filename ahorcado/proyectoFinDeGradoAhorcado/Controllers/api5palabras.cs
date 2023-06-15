using Entidades;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace proyectoFinDeGradoAhorcado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class api5palabras : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                List<clsPalabra> ListadoPalabras = DAL.Manejadora.ManejadoraPalabras_DAL.get5PalabrasRandom();
                return Ok(ListadoPalabras);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
