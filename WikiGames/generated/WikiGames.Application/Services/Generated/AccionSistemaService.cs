using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

namespace WikiGames.Application.Services;

public sealed partial class AccionSistemaService : ServiceEntity<AccionSistema>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccionSistemaRepository _repository;

    public AccionSistemaService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public AccionSistemaService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private AccionSistemaService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.AccionSistemaRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.AccionSistemaRepository;
    }

    public Task<AccionSistema> AddAccionSistemaDefaultAsync(AccionSistema entity) =>
        AddDefaultAsync(entity);

    public Task DeleteAccionSistemaDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<AccionSistema?> ReadOIDAccionSistemaDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<AccionSistema>> ReadAllAccionSistemaDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<AccionSistema> ModifierAccionSistemaDefaultAsync(AccionSistema entity) =>
        ModifierDefaultAsync(entity);

    public async Task<AccionSistema> NewAsync(DateTime p_fecha, string p_descripcion, int p_causante_oid, int p_contenidoAfectado_oid)
    {
        Usuario causanteEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_causante_oid)
            ?? throw new ModelException("Usuario not found for AccionSistema.Causante.");

        Contenido contenidoAfectadoEntity = await _unitOfWork.ContenidoRepository.ReadAsync(p_contenidoAfectado_oid)
            ?? throw new ModelException("Contenido not found for AccionSistema.ContenidoAfectado.");

        AccionSistema entity = new AccionSistema
        {
            Fecha = p_fecha,
            Descripcion = p_descripcion,
            Causante = causanteEntity,
            ContenidoAfectado = contenidoAfectadoEntity,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task DestroyAsync(int p_oid)
    {
        AccionSistema entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("AccionSistema not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModifyAsync(int p_oid, DateTime p_fecha, string p_descripcion, int p_causante_oid, int p_contenidoAfectado_oid)
    {
        AccionSistema entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("AccionSistema not found.");
        entity.Fecha = p_fecha;
        entity.Descripcion = p_descripcion;
        await _repository.SaveAsync(entity);
    }

    public async Task<AccionSistema?> ReadOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<AccionSistema>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<IReadOnlyList<AccionSistema>> FindByFechaLinQAsync(DateTime p_fecha)
    {
        return await _repository.FindByFechaLinQAsync(p_fecha);
    }

}
