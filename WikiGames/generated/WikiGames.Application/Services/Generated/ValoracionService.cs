using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

namespace WikiGames.Application.Services;

public sealed partial class ValoracionService : ServiceEntity<Valoracion>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValoracionRepository _repository;

    public ValoracionService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public ValoracionService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private ValoracionService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.ValoracionRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.ValoracionRepository;
    }

    public Task<Valoracion> AddValoracionDefaultAsync(Valoracion entity) =>
        AddDefaultAsync(entity);

    public Task DeleteValoracionDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Valoracion?> ReadOIDValoracionDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Valoracion>> ReadAllValoracionDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Valoracion> ModifierValoracionDefaultAsync(Valoracion entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Valoracion> NewAsync(string p_texto, DateTime p_fechaCreacion, int p_numeroMeGusta, float p_puntuacion, int p_usuarioCreador_oid, int p_videojuego_oid)
    {
        Usuario usuarioCreadorEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_usuarioCreador_oid)
            ?? throw new ModelException("Usuario not found for Valoracion.UsuarioCreador.");

        Videojuego videojuegoEntity = await _unitOfWork.VideojuegoRepository.ReadAsync(p_videojuego_oid)
            ?? throw new ModelException("Videojuego not found for Valoracion.Videojuego.");

        Valoracion entity = new Valoracion
        {
            Texto = p_texto,
            FechaCreacion = p_fechaCreacion,
            NumeroMeGusta = p_numeroMeGusta,
            Puntuacion = p_puntuacion,
            UsuarioCreador = usuarioCreadorEntity,
            Videojuego = videojuegoEntity,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task DestroyAsync(int p_oid)
    {
        Valoracion entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Valoracion not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModifyAsync(int p_oid, string p_texto, DateTime p_fechaCreacion, int p_numeroMeGusta, float p_puntuacion, int p_usuarioCreador_oid, int p_videojuego_oid)
    {
        Valoracion entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Valoracion not found.");
        entity.Texto = p_texto;
        entity.FechaCreacion = p_fechaCreacion;
        entity.NumeroMeGusta = p_numeroMeGusta;
        entity.Puntuacion = p_puntuacion;
        await _repository.SaveAsync(entity);
    }

    public async Task<Valoracion?> ReadOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Valoracion>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

}
