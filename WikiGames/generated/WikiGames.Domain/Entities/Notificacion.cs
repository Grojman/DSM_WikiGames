using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WikiGames.Domain.Entities;

public partial class Notificacion
{
    public virtual int Id { get; set; }
    public virtual string Texto { get; set; } = string.Empty;
    public virtual DateTime Fecha { get; set; }
    public virtual Denuncia? Denuncia { get; set; }
    public virtual IList<Usuario> Notificados { get; set; } = new List<Usuario>();

    private bool HasDefaultIdentity() => EqualityComparer<int>.Default.Equals(Id, default);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Notificacion other && !HasDefaultIdentity() && !other.HasDefaultIdentity() && EqualityComparer<int>.Default.Equals(Id, other.Id);
    public override int GetHashCode() => HasDefaultIdentity() ? RuntimeHelpers.GetHashCode(this) : EqualityComparer<int>.Default.GetHashCode(Id);
}
