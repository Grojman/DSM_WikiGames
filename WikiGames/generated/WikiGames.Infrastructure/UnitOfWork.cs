using NHibernate;
using Microsoft.Extensions.Logging;
using WikiGames.Application.Abstractions;
using WikiGames.Infrastructure.Exceptions;
using WikiGames.Infrastructure.Logging;
using WikiGames.Infrastructure.Repositories;
using WikiGames.Infrastructure.Security;

namespace WikiGames.Infrastructure;

public sealed class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly ISession _session;
    private readonly ILogger<UnitOfWork>? _logger;
    private readonly Microsoft.Extensions.Logging.ILoggerFactory? _loggerFactory;
    private ITransaction? _transaction;
    private SqlStatementCaptureScope? _sqlCaptureScope;
    private bool _disposed;

    public UnitOfWork(UnitOfWorkFactory unitOfWorkFactory)
        : this(
            unitOfWorkFactory.OpenSession(),
            unitOfWorkFactory.LoggerFactory,
            unitOfWorkFactory.PasswordHasher,
            unitOfWorkFactory.TokenGenerator)
    {
    }

    public UnitOfWork(ISession session, Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = null, IHariaPasswordHasher? passwordHasher = null, IHariaTokenGenerator? tokenGenerator = null)
    {
        _session = session;
        _loggerFactory = loggerFactory;
        _logger = loggerFactory?.CreateLogger<UnitOfWork>();
        PasswordHasher = passwordHasher ?? new HariaPasswordHasher();
        TokenGenerator = tokenGenerator ?? new HariaTokenGenerator();
        UsuarioRepository = new UsuarioRepository(session, _loggerFactory?.CreateLogger<UsuarioRepository>());
        VideojuegoRepository = new VideojuegoRepository(session, _loggerFactory?.CreateLogger<VideojuegoRepository>());
        CategoriaRepository = new CategoriaRepository(session, _loggerFactory?.CreateLogger<CategoriaRepository>());
        TemaConversacionRepository = new TemaConversacionRepository(session, _loggerFactory?.CreateLogger<TemaConversacionRepository>());
        ComentarioRepository = new ComentarioRepository(session, _loggerFactory?.CreateLogger<ComentarioRepository>());
        DenunciaRepository = new DenunciaRepository(session, _loggerFactory?.CreateLogger<DenunciaRepository>());
        ValoracionRepository = new ValoracionRepository(session, _loggerFactory?.CreateLogger<ValoracionRepository>());
        AccionSistemaRepository = new AccionSistemaRepository(session, _loggerFactory?.CreateLogger<AccionSistemaRepository>());
        RechazoRepository = new RechazoRepository(session, _loggerFactory?.CreateLogger<RechazoRepository>());
        ContenidoRepository = new ContenidoRepository(session, _loggerFactory?.CreateLogger<ContenidoRepository>());
        NotificacionRepository = new NotificacionRepository(session, _loggerFactory?.CreateLogger<NotificacionRepository>());
    }

    public IUsuarioRepository UsuarioRepository { get; }
    public IVideojuegoRepository VideojuegoRepository { get; }
    public ICategoriaRepository CategoriaRepository { get; }
    public ITemaConversacionRepository TemaConversacionRepository { get; }
    public IComentarioRepository ComentarioRepository { get; }
    public IDenunciaRepository DenunciaRepository { get; }
    public IValoracionRepository ValoracionRepository { get; }
    public IAccionSistemaRepository AccionSistemaRepository { get; }
    public IRechazoRepository RechazoRepository { get; }
    public IContenidoRepository ContenidoRepository { get; }
    public INotificacionRepository NotificacionRepository { get; }

    public IHariaPasswordHasher PasswordHasher { get; }

    public IHariaTokenGenerator TokenGenerator { get; }

    public Task BeginTransactionAsync(string? businessOperation = null)
    {
        try
        {
            if (_transaction is null)
            {
                var transactionId = Guid.NewGuid();
                var effectiveBusinessOperation = ResolveBusinessOperation(businessOperation);
                _transaction = _session.BeginTransaction();
                _sqlCaptureScope = SqlStatementCapture.Begin(transactionId, effectiveBusinessOperation);

                _logger?.LogDebug(
                    "NHibernate transaction started. BusinessOperation={BusinessOperation} TransactionId={TransactionId}",
                    effectiveBusinessOperation,
                    transactionId);
            }

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error beginning NHibernate transaction.");
            DisposeSqlCaptureScope();
            throw new DataException("Error beginning NHibernate transaction.", ex);
        }
    }

    public async Task CommitAsync()
    {
        try
        {
            if (_transaction is not null)
            {
                await _transaction.CommitAsync();

                var sqlStatements = GetCapturedSqlStatements();
                var businessOperations = GetCapturedBusinessOperations();
                _logger?.LogInformation(
                    "NHibernate transaction committed successfully. BusinessOperation={BusinessOperation} BusinessOperations={BusinessOperations} TransactionId={TransactionId} SqlStatementCount={SqlStatementCount}",
                    _sqlCaptureScope?.BusinessOperation,
                    businessOperations,
                    _sqlCaptureScope?.TransactionId,
                    sqlStatements.Count);

                if (sqlStatements.Count > 0)
                {
                    _logger?.LogDebug(
                        "NHibernate SQL statements committed. BusinessOperation={BusinessOperation} BusinessOperations={BusinessOperations} TransactionId={TransactionId} SqlStatements={SqlStatements}",
                        _sqlCaptureScope?.BusinessOperation,
                        businessOperations,
                        _sqlCaptureScope?.TransactionId,
                        sqlStatements);
                }

                _transaction.Dispose();
                _transaction = null;
                DisposeSqlCaptureScope();
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                ex,
                "Error committing NHibernate transaction. TransactionId={TransactionId}",
                _sqlCaptureScope?.TransactionId);
            throw new DataException("Error committing NHibernate transaction.", ex);
        }
    }

    public async Task RollbackAsync()
    {
        try
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync();

                var sqlStatements = GetCapturedSqlStatements();
                var businessOperations = GetCapturedBusinessOperations();
                _logger?.LogInformation(
                    "NHibernate transaction rolled back. BusinessOperation={BusinessOperation} BusinessOperations={BusinessOperations} TransactionId={TransactionId} SqlStatementCount={SqlStatementCount}",
                    _sqlCaptureScope?.BusinessOperation,
                    businessOperations,
                    _sqlCaptureScope?.TransactionId,
                    sqlStatements.Count);

                if (sqlStatements.Count > 0)
                {
                    _logger?.LogDebug(
                        "NHibernate SQL statements rolled back. BusinessOperation={BusinessOperation} BusinessOperations={BusinessOperations} TransactionId={TransactionId} SqlStatements={SqlStatements}",
                        _sqlCaptureScope?.BusinessOperation,
                        businessOperations,
                        _sqlCaptureScope?.TransactionId,
                        sqlStatements);
                }

                _transaction.Dispose();
                _transaction = null;
                DisposeSqlCaptureScope();
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                ex,
                "Error rolling back NHibernate transaction. TransactionId={TransactionId}",
                _sqlCaptureScope?.TransactionId);
            throw new DataException("Error rolling back NHibernate transaction.", ex);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        if (_transaction is not null)
        {
            await RollbackAsync();
        }

        _session.Dispose();
        _disposed = true;
    }

    private IReadOnlyList<string> GetCapturedSqlStatements() =>
        _sqlCaptureScope?.GetStatements() ?? Array.Empty<string>();

    private IReadOnlyList<string> GetCapturedBusinessOperations() =>
        _sqlCaptureScope?.GetBusinessOperations() ?? Array.Empty<string>();

    private static string ResolveBusinessOperation(string? businessOperation)
    {
        if (!string.IsNullOrWhiteSpace(businessOperation))
        {
            return businessOperation;
        }

        return "UnitOfWork transaction";
    }

    private void DisposeSqlCaptureScope()
    {
        _sqlCaptureScope?.Dispose();
        _sqlCaptureScope = null;
    }
}
