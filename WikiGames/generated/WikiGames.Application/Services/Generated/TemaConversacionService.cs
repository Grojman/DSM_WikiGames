using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

namespace WikiGames.Application.Services;

public sealed partial class TemaConversacionService : ServiceEntity<TemaConversacion>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITemaConversacionRepository _repository;

    public TemaConversacionService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public TemaConversacionService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private TemaConversacionService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.TemaConversacionRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.TemaConversacionRepository;
    }

    public Task<TemaConversacion> AddTemaConversacionDefaultAsync(TemaConversacion entity) =>
        AddDefaultAsync(entity);

    public Task DeleteTemaConversacionDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<TemaConversacion?> ReadOIDTemaConversacionDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<TemaConversacion>> ReadAllTemaConversacionDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<TemaConversacion> ModifierTemaConversacionDefaultAsync(TemaConversacion entity) =>
        ModifierDefaultAsync(entity);

    public async Task<TemaConversacion> NewAsync(string p_texto, DateTime p_fechaCreacion, int p_numeroMeGusta, string p_titulo, int p_videojuego_oid, int p_usuarioCreador_oid)
    {
        Usuario usuarioCreadorEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_usuarioCreador_oid)
            ?? throw new ModelException("Usuario not found for TemaConversacion.UsuarioCreador.");

        Videojuego videojuegoEntity = await _unitOfWork.VideojuegoRepository.ReadAsync(p_videojuego_oid)
            ?? throw new ModelException("Videojuego not found for TemaConversacion.Videojuego.");

        TemaConversacion entity = new TemaConversacion
        {
            Texto = p_texto,
            FechaCreacion = p_fechaCreacion,
            NumeroMeGusta = p_numeroMeGusta,
            Titulo = p_titulo,
            UsuarioCreador = usuarioCreadorEntity,
            Videojuego = videojuegoEntity,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task DestroyAsync(int p_oid)
    {
        TemaConversacion entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("TemaConversacion not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModifyAsync(int p_oid, string p_texto, DateTime p_fechaCreacion, int p_numeroMeGusta, string p_titulo, int p_videojuego_oid, int p_usuarioCreador_oid)
    {
        TemaConversacion entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("TemaConversacion not found.");
        entity.Texto = p_texto;
        entity.FechaCreacion = p_fechaCreacion;
        entity.NumeroMeGusta = p_numeroMeGusta;
        entity.Titulo = p_titulo;
        await _repository.SaveAsync(entity);
    }

    public async Task<TemaConversacion?> ReadOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<TemaConversacion>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<IReadOnlyList<TemaConversacion>> FindByTituloAsync(string p_texto)
    {
        return await _repository.FindByTituloAsync(p_texto);
    }

    public async Task<IReadOnlyList<TemaConversacion>> FindByRecientesAsync(string p_texto)
    {
        return await _repository.FindByRecientesAsync(p_texto);
    }

}
