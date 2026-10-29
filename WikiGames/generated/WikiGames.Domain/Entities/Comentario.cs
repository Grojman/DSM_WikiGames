using System.Collections.Generic;

namespace WikiGames.Domain.Entities;

public partial class Comentario : Contenido
{
    public virtual TemaConversacion Tema { get; set; } = null!;
    public virtual IList<Comentario> Respuestas { get; set; } = new List<Comentario>();
    public virtual Comentario? ComentarioAsociado { get; set; }
}
