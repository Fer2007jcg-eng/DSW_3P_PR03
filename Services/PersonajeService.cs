using DSW_3P_PR03.DTOs;
using DSW_3P_PR03.Models;
using DSW_3P_PR03.Repositories;

namespace DSW_3P_PR03.Services;

public class PersonajeService : IPersonajeService
{
    private readonly IPersonajeRepository _repository;

    public PersonajeService(IPersonajeRepository repository)
    {
        _repository = repository;
    }

    private static void ValidarNombre(string? nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException(
                "El nombre del personaje es obligatorio."
            );
        }
    }

    private static void ValidarVideojuego(string? videojuego)
    {
        if (string.IsNullOrWhiteSpace(videojuego))
        {
            throw new ArgumentException(
                "El videojuego es obligatorio."
            );
        }
    }

    private static void ValidarClase(string? clase)
    {
        if (string.IsNullOrWhiteSpace(clase))
        {
            throw new ArgumentException(
                "La clase del personaje es obligatoria."
            );
        }
    }

    private static void ValidarNivel(int nivel)
    {
        if (nivel < 1 || nivel > 100)
        {
            throw new ArgumentException(
                "El nivel debe estar entre 1 y 100."
            );
        }
    }

    private static void ValidarPoder(int poder)
    {
        if (poder <= 0)
        {
            throw new ArgumentException(
                "El poder debe ser mayor que cero."
            );
        }
    }

    private static void ValidarImagenUrl(string? imagenUrl)
    {
        if (string.IsNullOrWhiteSpace(imagenUrl))
        {
            throw new ArgumentException(
                "La URL de la imagen es obligatoria."
            );
        }

        bool urlValida = Uri.TryCreate(
            imagenUrl,
            UriKind.Absolute,
            out Uri? resultado
        );

        if (!urlValida ||
            resultado is null ||
            (resultado.Scheme != Uri.UriSchemeHttp &&
             resultado.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException(
                "La imagen debe contener una URL absoluta válida mediante HTTP o HTTPS."
            );
        }
    }

    private static void ValidarPersonaje(Personaje personaje)
    {
        ValidarNombre(personaje.Nombre);
        ValidarVideojuego(personaje.Videojuego);
        ValidarClase(personaje.Clase);
        ValidarNivel(personaje.Nivel);
        ValidarPoder(personaje.Poder);
        ValidarImagenUrl(personaje.ImagenUrl);

        personaje.Nombre = personaje.Nombre.Trim();
        personaje.Videojuego = personaje.Videojuego.Trim();
        personaje.Clase = personaje.Clase.Trim();
        personaje.ImagenUrl = personaje.ImagenUrl.Trim();
    }

    public List<Personaje> ObtenerTodos()
    {
        return _repository.ObtenerTodos();
    }

    public Personaje? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public List<Personaje> ObtenerPorVideojuego(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException(
                "El nombre del videojuego es obligatorio."
            );
        }

        return _repository.ObtenerPorVideojuego(nombre.Trim());
    }

    public List<Personaje> ObtenerPorNivel(int nivel)
    {
        if (nivel < 1 || nivel > 100)
        {
            throw new ArgumentException(
                "El nivel debe estar entre 1 y 100."
            );
        }

        return _repository.ObtenerPorNivel(nivel);
    }

    public Personaje Agregar(CrearPersonajeDto nuevoPersonaje)
    {
        ArgumentNullException.ThrowIfNull(nuevoPersonaje);

        Personaje personaje = new()
        {
            Nombre = nuevoPersonaje.Nombre,
            Videojuego = nuevoPersonaje.Videojuego,
            Clase = nuevoPersonaje.Clase,
            Nivel = nuevoPersonaje.Nivel,
            Poder = nuevoPersonaje.Poder,
            Disponible = true,
            ImagenUrl = nuevoPersonaje.ImagenUrl
        };

        ValidarPersonaje(personaje);

        return _repository.Agregar(personaje);
    }

    public bool Actualizar(int id, Personaje personajeActualizado)
    {
        ArgumentNullException.ThrowIfNull(personajeActualizado);

        ValidarPersonaje(personajeActualizado);

        return _repository.Actualizar(id, personajeActualizado);
    }

    public bool Eliminar(int id)
    {
        return _repository.Eliminar(id);
    }
}
