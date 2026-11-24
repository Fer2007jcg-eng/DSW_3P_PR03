using System.Text.Json;
using DSW_3P_PR03.Models;

namespace DSW_3P_PR03.Repositories;

public class PersonajeJsonRepository : IPersonajeRepository
{
    private readonly string _ruta;

    private readonly JsonSerializerOptions _opciones = new()
    {
        WriteIndented = true
    };

    public PersonajeJsonRepository(IWebHostEnvironment environment)
    {
        _ruta = Path.Combine(
            environment.ContentRootPath,
            "Data",
            "personajes.json"
        );

        string? directorio = Path.GetDirectoryName(_ruta);

        if (directorio is not null)
        {
            Directory.CreateDirectory(directorio);
        }

        if (!File.Exists(_ruta))
        {
            File.WriteAllText(_ruta, "[]");
        }
    }

    private List<Personaje> Leer()
    {
        string json = File.ReadAllText(_ruta);

        return JsonSerializer.Deserialize<List<Personaje>>(json)
               ?? new List<Personaje>();
    }

    private void Guardar(List<Personaje> personajes)
    {
        string json = JsonSerializer.Serialize(personajes, _opciones);

        File.WriteAllText(_ruta, json);
    }

    public List<Personaje> ObtenerTodos()
    {
        return Leer();
    }

    public Personaje? ObtenerPorId(int id)
    {
        return Leer().FirstOrDefault(p => p.Id == id);
    }

    public List<Personaje> ObtenerPorVideojuego(string nombre)
    {
        return Leer()
            .Where(p => p.Videojuego.Equals(
                nombre,
                StringComparison.OrdinalIgnoreCase
            ))
            .ToList();
    }

    public List<Personaje> ObtenerPorNivel(int nivel)
    {
        return Leer()
            .Where(p => p.Nivel >= nivel)
            .ToList();
    }

    public Personaje Agregar(Personaje nuevoPersonaje)
    {
        List<Personaje> personajes = Leer();

        nuevoPersonaje.Id = personajes.Count == 0
            ? 1
            : personajes.Max(p => p.Id) + 1;

        personajes.Add(nuevoPersonaje);

        Guardar(personajes);

        return nuevoPersonaje;
    }

    public bool Actualizar(int id, Personaje personajeActualizado)
    {
        List<Personaje> personajes = Leer();

        Personaje? existente = personajes.FirstOrDefault(p => p.Id == id);

        if (existente is null)
        {
            return false;
        }

        existente.Nombre = personajeActualizado.Nombre;
        existente.Videojuego = personajeActualizado.Videojuego;
        existente.Clase = personajeActualizado.Clase;
        existente.Nivel = personajeActualizado.Nivel;
        existente.Poder = personajeActualizado.Poder;
        existente.Disponible = personajeActualizado.Disponible;
        existente.ImagenUrl = personajeActualizado.ImagenUrl;

        Guardar(personajes);

        return true;
    }

    public bool Eliminar(int id)
    {
        List<Personaje> personajes = Leer();

        Personaje? existente = personajes.FirstOrDefault(p => p.Id == id);

        if (existente is null)
        {
            return false;
        }

        personajes.Remove(existente);

        Guardar(personajes);

        return true;
    }
}