using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UltimoParcialDesarolloWebChristianB.Models;
using UltimoParcialDesarolloWebChristianB.Repository;

namespace UltimoParcialDesarolloWebChristianB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CineController : ControllerBase
    {
        private readonly Cines _cineRepository;

        public CineController(Cines cineRepository)
        {
            _cineRepository = cineRepository;
        }

        // Endpoint para listar los cines
        [HttpGet("listarCines")]
        public async Task<IActionResult> ListarCines()
        {
            List<dynamic> cines = await _cineRepository.ListarCines();
            if (cines == null || cines.Count == 0)
            {
                return NotFound("No se encontraron cines.");
            }
            return Ok(cines);
        }

        // Endpoint para guardar un nuevo cine
        [HttpPost("guardar")]
        public async Task<IActionResult> GuardarCine([FromBody] Cine cine)
        {
            if (cine == null || string.IsNullOrEmpty(cine.Nombre) || string.IsNullOrEmpty(cine.Ubicacion))
            {
                return BadRequest("Datos incompletos.");
            }

            var result = await _cineRepository.GuardarCine(cine);
            if (string.IsNullOrEmpty(result))
            {
                return Ok(new { mensaje = "Cine guardado con éxito" });
            }

            return BadRequest(new { mensaje = "Error al guardar el cine", error = result });
        }

        // Endpoint para eliminar un cine por su ID
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarCine(int id)
        {
            var result = await _cineRepository.EliminarCine(id);
            if (string.IsNullOrEmpty(result))
            {
                return Ok(new { mensaje = "Cine eliminado con éxito" });
            }

            return BadRequest(new { mensaje = "Error al eliminar el cine", error = result });
        }

        // Endpoint para listar películas (opcional)
        [HttpGet("listarPeliculas")]
        public async Task<IActionResult> ListarPeliculas()
        {
            List<dynamic> peliculas = await _cineRepository.ListarPeliculas();
            if (peliculas == null || peliculas.Count == 0)
            {
                return NotFound("No se encontraron películas.");
            }
            return Ok(peliculas);
        }

        // Endpoint para insertar una película (opcional)
        [HttpPost("insertarPelicula")]
        public async Task<IActionResult> InsertarPelicula([FromBody] Pelicula nuevaPelicula)
        {
            if (nuevaPelicula == null)
            {
                return BadRequest("La película no puede ser nula.");
            }

            // Verificar que las propiedades no estén vacías o nulas
            if (string.IsNullOrEmpty(nuevaPelicula.Titulo) ||
                string.IsNullOrEmpty(nuevaPelicula.Sinopsis) ||
                nuevaPelicula.FechaEstreno == default(DateTime) ||
                nuevaPelicula.CineID <= 0 ||
                nuevaPelicula.GeneroID <= 0)
            {
                return BadRequest("Los datos de la película son inválidos.");
            }

            var resultado = await _cineRepository.InsertarPelicula(nuevaPelicula);
            if (string.IsNullOrEmpty(resultado))
            {
                return Ok(new { mensaje = "Película guardada con éxito" });
            }

            return BadRequest(new { mensaje = resultado });
        }


        [HttpDelete("eliminarPelicula/{id}")]
        public async Task<IActionResult> EliminarPelicula(int id)
        {
            var resultado = await _cineRepository.EliminarPelicula(id);

            if (string.IsNullOrEmpty(resultado))
            {
                return Ok(new { mensaje = "Película eliminada con éxito" });
            }

            return BadRequest(new { mensaje = resultado });
        }

    }
}
