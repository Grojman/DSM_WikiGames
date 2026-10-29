using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WikiGames.Domain.Entities;

public partial class Categoria
{
    public virtual int Id { get; set; }
    public virtual string Name { get; set; } = string.Empty;
    public virtual IList<Categoria> Hijos { get; set; } = new List<Categoria>();
    public virtual Categoria? Padre { get; set; }
    public virtual Videojuego Videojuego { get; set; } = null!;

    private bool HasDefaultIdentity() => EqualityComparer<int>.Default.Equals(Id, default);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Categoria other && !HasDefaultIdentity() && !other.HasDefaultIdentity() && EqualityComparer<int>.Default.Equals(Id, other.Id);
    public override int GetHashCode() => HasDefaultIdentity() ? RuntimeHelpers.GetHashCode(this) : EqualityComparer<int>.Default.GetHashCode(Id);
}
