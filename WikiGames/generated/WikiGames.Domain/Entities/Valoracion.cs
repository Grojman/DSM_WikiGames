using System.Collections.Generic;

namespace WikiGames.Domain.Entities;

public partial class Valoracion : Contenido
{
    public virtual float Puntuacion { get; set; }
    public virtual Videojuego Videojuego { get; set; } = null!;
}
