using System.Collections.Generic;
using System.Runtime.CompilerServices;
using WikiGames.Domain.Enums;

namespace WikiGames.Domain.Entities;

public partial class Usuario
{
    public virtual int Id { get; set; }
    public virtual string Name { get; set; } = string.Empty;
    public virtual string Password { get; set; } = string.Empty;
    public virtual Rol RolUsuario { get; set; }
    public virtual IList<Contenido> ContenidoGenerado { get; set; } = new List<Contenido>();
    public virtual IList<Rechazo> Rechazos { get; set; } = new List<Rechazo>();
    public virtual IList<AccionSistema> Acciones { get; set; } = new List<AccionSistema>();
    public virtual IList<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
    public virtual IList<Denuncia> DenunciasGeneradas { get; set; } = new List<Denuncia>();
    public virtual IList<Denuncia> DenunciasRecibidas { get; set; } = new List<Denuncia>();
    public virtual IList<Contenido> ContenidoFavorito { get; set; } = new List<Contenido>();

    private bool HasDefaultIdentity() => EqualityComparer<int>.Default.Equals(Id, default);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Usuario other && !HasDefaultIdentity() && !other.HasDefaultIdentity() && EqualityComparer<int>.Default.Equals(Id, other.Id);
    public override int GetHashCode() => HasDefaultIdentity() ? RuntimeHelpers.GetHashCode(this) : EqualityComparer<int>.Default.GetHashCode(Id);
}
