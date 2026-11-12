using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WikiGames.Domain.Entities;

public partial class AccionSistema
{
    public virtual int Id { get; set; }
    public virtual DateTime Fecha { get; set; }
    public virtual string Descripcion { get; set; } = string.Empty;
    public virtual Usuario Causante { get; set; } = null!;
    public virtual Contenido ContenidoAfectado { get; set; } = null!;

    private bool HasDefaultIdentity() => EqualityComparer<int>.Default.Equals(Id, default);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is AccionSistema other && !HasDefaultIdentity() && !other.HasDefaultIdentity() && EqualityComparer<int>.Default.Equals(Id, other.Id);
    public override int GetHashCode() => HasDefaultIdentity() ? RuntimeHelpers.GetHashCode(this) : EqualityComparer<int>.Default.GetHashCode(Id);
}
