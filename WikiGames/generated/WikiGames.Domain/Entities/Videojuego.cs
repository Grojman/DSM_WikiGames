using System.Collections.Generic;
using WikiGames.Domain.Enums;

namespace WikiGames.Domain.Entities;

public partial class Videojuego : Contenido
{
    public virtual string Titulo { get; set; } = string.Empty;
    public virtual string Enlace { get; set; } = string.Empty;
    public virtual int NumeroLikes { get; set; }
    public virtual string Imagen { get; set; } = string.Empty;
    public virtual EstadoContenido Estado { get; set; }
    public virtual IList<TemaConversacion> Temas { get; set; } = new List<TemaConversacion>();
    public virtual IList<Valoracion> Valoraciones { get; set; } = new List<Valoracion>();
    public virtual Rechazo? Rechazo { get; set; }
    public virtual IList<Categoria> Categorias { get; set; } = new List<Categoria>();
}
