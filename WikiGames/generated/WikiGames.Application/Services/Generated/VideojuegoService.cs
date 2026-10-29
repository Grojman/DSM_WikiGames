using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

namespace WikiGames.Application.Services;

public sealed partial class VideojuegoService : ServiceEntity<Videojuego>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVideojuegoRepository _repository;

    public VideojuegoService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public VideojuegoService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private VideojuegoService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.VideojuegoRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.VideojuegoRepository;
    }

    public Task<Videojuego> AddVideojuegoDefaultAsync(Videojuego entity) =>
        AddDefaultAsync(entity);

    public Task DeleteVideojuegoDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Videojuego?> ReadOIDVideojuegoDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Videojuego>> ReadAllVideojuegoDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Videojuego> ModifierVideojuegoDefaultAsync(Videojuego entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Videojuego> NewAsync(string p_texto, DateTime p_fechaCreacion, int p_numeroMeGusta, string p_titulo, string p_enlace, int p_numeroLikes, string p_imagen, EstadoContenido p_estado, int p_usuarioCreador_oid)
    {
        Usuario usuarioCreadorEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_usuarioCreador_oid)
            ?? throw new ModelException("Usuario not found for Videojuego.UsuarioCreador.");

        Videojuego entity = new Videojuego
        {
            Texto = p_texto,
            FechaCreacion = p_fechaCreacion,
            NumeroMeGusta = p_numeroMeGusta,
            Titulo = p_titulo,
            Enlace = p_enlace,
            NumeroLikes = p_numeroLikes,
            Imagen = p_imagen,
            Estado = p_estado,
            UsuarioCreador = usuarioCreadorEntity,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task DestroyAsync(int p_oid)
    {
        Videojuego entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Videojuego not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModifyAsync(int p_oid, string p_texto, DateTime p_fechaCreacion, int p_numeroMeGusta, string p_titulo, string p_enlace, int p_numeroLikes, string p_imagen, EstadoContenido p_estado, int p_usuarioCreador_oid)
    {
        Videojuego entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Videojuego not found.");
        entity.Texto = p_texto;
        entity.FechaCreacion = p_fechaCreacion;
        entity.NumeroMeGusta = p_numeroMeGusta;
        entity.Titulo = p_titulo;
        entity.Enlace = p_enlace;
        entity.NumeroLikes = p_numeroLikes;
        entity.Imagen = p_imagen;
        entity.Estado = p_estado;
        await _repository.SaveAsync(entity);
    }

    public async Task<Videojuego?> ReadOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Videojuego>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

}
