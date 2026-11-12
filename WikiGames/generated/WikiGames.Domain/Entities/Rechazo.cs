using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WikiGames.Domain.Entities;

public partial class Rechazo
{
    public virtual int Id { get; set; }
    public virtual DateTime Fecha { get; set; }
    public virtual string Motivo { get; set; } = string.Empty;
    public virtual Videojuego Videojuego { get; set; } = null!;
    public virtual Usuario Gestor { get; set; } = null!;

    private bool HasDefaultIdentity() => EqualityComparer<int>.Default.Equals(Id, default);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Rechazo other && !HasDefaultIdentity() && !other.HasDefaultIdentity() && EqualityComparer<int>.Default.Equals(Id, other.Id);
    public override int GetHashCode() => HasDefaultIdentity() ? RuntimeHelpers.GetHashCode(this) : EqualityComparer<int>.Default.GetHashCode(Id);
}
