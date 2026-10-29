using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WikiGames.Domain.Entities;

public partial class Contenido
{
    public virtual int Id { get; set; }
    public virtual string Texto { get; set; } = string.Empty;
    public virtual DateTime FechaCreacion { get; set; }
    public virtual int NumeroMeGusta { get; set; }
    public virtual Usuario UsuarioCreador { get; set; } = null!;
    public virtual IList<AccionSistema> Acciones { get; set; } = new List<AccionSistema>();
    public virtual IList<Denuncia> Denuncias { get; set; } = new List<Denuncia>();
    public virtual IList<Usuario> UsuarioFavorito { get; set; } = new List<Usuario>();

    private bool HasDefaultIdentity() => EqualityComparer<int>.Default.Equals(Id, default);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Contenido other && !HasDefaultIdentity() && !other.HasDefaultIdentity() && EqualityComparer<int>.Default.Equals(Id, other.Id);
    public override int GetHashCode() => HasDefaultIdentity() ? RuntimeHelpers.GetHashCode(this) : EqualityComparer<int>.Default.GetHashCode(Id);
}
