using System.Collections.Generic;
using System.Runtime.CompilerServices;
using WikiGames.Domain.Enums;

namespace WikiGames.Domain.Entities;

public partial class Denuncia
{
    public virtual int Id { get; set; }
    public virtual DateTime Fecha { get; set; }
    public virtual EstadoContenido Estado { get; set; }
    public virtual Notificacion? Notificacion { get; set; }
    public virtual Usuario Denunciante { get; set; } = null!;
    public virtual Usuario? Denunciado { get; set; }
    public virtual Contenido? ContenidoDenunciado { get; set; }

    private bool HasDefaultIdentity() => EqualityComparer<int>.Default.Equals(Id, default);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Denuncia other && !HasDefaultIdentity() && !other.HasDefaultIdentity() && EqualityComparer<int>.Default.Equals(Id, other.Id);
    public override int GetHashCode() => HasDefaultIdentity() ? RuntimeHelpers.GetHashCode(this) : EqualityComparer<int>.Default.GetHashCode(Id);
}
