using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

namespace WikiGames.Application.Services;

public sealed partial class RechazoService : ServiceEntity<Rechazo>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRechazoRepository _repository;

    public RechazoService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public RechazoService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private RechazoService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.RechazoRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.RechazoRepository;
    }

    public Task<Rechazo> AddRechazoDefaultAsync(Rechazo entity) =>
        AddDefaultAsync(entity);

    public Task DeleteRechazoDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Rechazo?> ReadOIDRechazoDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Rechazo>> ReadAllRechazoDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Rechazo> ModifierRechazoDefaultAsync(Rechazo entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Rechazo> NewAsync(DateTime p_fecha, string p_motivo, int p_videojuego_oid, int p_gestor_oid)
    {
        Videojuego videojuegoEntity = await _unitOfWork.VideojuegoRepository.ReadAsync(p_videojuego_oid)
            ?? throw new ModelException("Videojuego not found for Rechazo.Videojuego.");

        Usuario gestorEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_gestor_oid)
            ?? throw new ModelException("Usuario not found for Rechazo.Gestor.");

        Rechazo entity = new Rechazo
        {
            Fecha = p_fecha,
            Motivo = p_motivo,
            Videojuego = videojuegoEntity,
            Gestor = gestorEntity,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task DestroyAsync(int p_oid)
    {
        Rechazo entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Rechazo not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModifyAsync(int p_oid, DateTime p_fecha, string p_motivo, int p_videojuego_oid, int p_gestor_oid)
    {
        Rechazo entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Rechazo not found.");
        entity.Fecha = p_fecha;
        entity.Motivo = p_motivo;
        await _repository.SaveAsync(entity);
    }

    public async Task<Rechazo?> ReadOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Rechazo>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

}
