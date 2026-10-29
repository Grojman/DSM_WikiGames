using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

namespace WikiGames.Application.Services;

public sealed partial class NotificacionService : ServiceEntity<Notificacion>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificacionRepository _repository;

    public NotificacionService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public NotificacionService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private NotificacionService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.NotificacionRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.NotificacionRepository;
    }

    public Task<Notificacion> AddNotificacionDefaultAsync(Notificacion entity) =>
        AddDefaultAsync(entity);

    public Task DeleteNotificacionDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Notificacion?> ReadOIDNotificacionDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Notificacion>> ReadAllNotificacionDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Notificacion> ModifierNotificacionDefaultAsync(Notificacion entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Notificacion> NewAsync(string p_texto, DateTime p_fecha)
    {
        Notificacion entity = new Notificacion
        {
            Texto = p_texto,
            Fecha = p_fecha,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task DestroyAsync(int p_oid)
    {
        Notificacion entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Notificacion not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModifyAsync(int p_oid, string p_texto, DateTime p_fecha)
    {
        Notificacion entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Notificacion not found.");
        entity.Texto = p_texto;
        entity.Fecha = p_fecha;
        await _repository.SaveAsync(entity);
    }

    public async Task<Notificacion?> ReadOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Notificacion>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<IReadOnlyList<Notificacion>> FindByFechaAsync(string p_texto)
    {
        return await _repository.FindByFechaAsync(p_texto);
    }

}
