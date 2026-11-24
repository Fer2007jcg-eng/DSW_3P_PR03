using DSW_3P_PR03.DTOs;
using DSW_3P_PR03.Models;
using DSW_3P_PR03.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DSW_3P_PR03.Controllers;

[ApiController]
[Route("api/personajes")]
//[Authorize]
public class PersonajesController : ControllerBase
{
    private readonly IPersonajeService _service;

    public PersonajesController(IPersonajeService service)
    {
        _service = service;
    }

    // GET: api/personajes
    [HttpGet]
    [ProducesResponseType(
        typeof(List<Personaje>),
        StatusCodes.Status200OK
    )]
    public IActionResult ObtenerTodos()
    {
        return Ok(_service.ObtenerTodos());
    }

    // GET: api/personajes/1
    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(Personaje),
        StatusCodes.Status200OK
    )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ObtenerPorId(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                mensaje = "El ID debe ser un entero positivo."
            });
        }

        Personaje? personaje = _service.ObtenerPorId(id);

        if (personaje is null)
        {
            return NotFound(new
            {
                mensaje = $"No existe un personaje con el ID {id}."
            });
        }

        return Ok(personaje);
    }

    // GET: api/personajes/videojuego/Minecraft
    [HttpGet("videojuego/{nombre}")]
    [ProducesResponseType(
        typeof(List<Personaje>),
        StatusCodes.Status200OK
    )]
    public IActionResult ObtenerPorVideojuego(string nombre)
    {
        try
        {
            return Ok(_service.ObtenerPorVideojuego(nombre));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // GET: api/personajes/nivel/50
    [HttpGet("nivel/{nivel:int}")]
    [ProducesResponseType(
        typeof(List<Personaje>),
        StatusCodes.Status200OK
    )]
    public IActionResult ObtenerPorNivel(int nivel)
    {
        try
        {
            return Ok(_service.ObtenerPorNivel(nivel));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // POST: api/personajes
    [HttpPost]
    [ProducesResponseType(
        typeof(Personaje),
        StatusCodes.Status201Created
    )]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Agregar(
        [FromBody] CrearPersonajeDto nuevoPersonaje
    )
    {
        try
        {
            Personaje creado = _service.Agregar(nuevoPersonaje);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = creado.Id },
                creado
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // PUT: api/personajes/1
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Actualizar(
        int id,
        [FromBody] Personaje personajeActualizado
    )
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    mensaje = "El ID debe ser un entero positivo."
                });
            }

            bool actualizado = _service.Actualizar(
                id,
                personajeActualizado
            );

            if (!actualizado)
            {
                return NotFound(new
                {
                    mensaje = $"No existe un personaje con el ID {id}."
                });
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // DELETE: api/personajes/1
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Eliminar(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                mensaje = "El ID debe ser un entero positivo."
            });
        }

        bool eliminado = _service.Eliminar(id);

        if (!eliminado)
        {
            return NotFound(new
            {
                mensaje = $"No existe un personaje con el ID {id}."
            });
        }

        return NoContent();
    }
}