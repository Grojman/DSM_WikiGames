namespace WikiGames.Application.Abstractions;

public interface IUnitOfWork : IAsyncDisposable
{
    IUsuarioRepository UsuarioRepository { get; }
    IVideojuegoRepository VideojuegoRepository { get; }
    ICategoriaRepository CategoriaRepository { get; }
    ITemaConversacionRepository TemaConversacionRepository { get; }
    IComentarioRepository ComentarioRepository { get; }
    IDenunciaRepository DenunciaRepository { get; }
    IValoracionRepository ValoracionRepository { get; }
    IAccionSistemaRepository AccionSistemaRepository { get; }
    IRechazoRepository RechazoRepository { get; }
    IContenidoRepository ContenidoRepository { get; }
    INotificacionRepository NotificacionRepository { get; }

    IHariaPasswordHasher PasswordHasher { get; }
    IHariaTokenGenerator TokenGenerator { get; }

    Task BeginTransactionAsync(string? businessOperation = null);
    Task CommitAsync();
    Task RollbackAsync();
}
