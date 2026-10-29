using System.Collections.Generic;

namespace WikiGames.Domain.Entities;

public partial class TemaConversacion : Contenido
{
    public virtual string Titulo { get; set; } = string.Empty;
    public virtual Videojuego Videojuego { get; set; } = null!;
    public virtual IList<Comentario> Comentarios { get; set; } = new List<Comentario>();
}
