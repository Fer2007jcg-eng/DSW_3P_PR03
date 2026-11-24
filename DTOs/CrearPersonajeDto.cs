namespace DSW_3P_PR03.DTOs;

public class CrearPersonajeDto
{
    public string Nombre { get; set; } = "";
    public string Videojuego { get; set; } = "";
    public string Clase { get; set; } = "";
    public int Nivel { get; set; }
    public int Poder { get; set; }
    public string ImagenUrl { get; set; } = "";
}