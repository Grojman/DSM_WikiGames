using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

namespace WikiGames.Application.Services;

public sealed partial class DenunciaService : ServiceEntity<Denuncia>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDenunciaRepository _repository;

    public DenunciaService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public DenunciaService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private DenunciaService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.DenunciaRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.DenunciaRepository;
    }

    public Task<Denuncia> AddDenunciaDefaultAsync(Denuncia entity) =>
        AddDefaultAsync(entity);

    public Task DeleteDenunciaDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Denuncia?> ReadOIDDenunciaDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Denuncia>> ReadAllDenunciaDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Denuncia> ModifierDenunciaDefaultAsync(Denuncia entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Denuncia> NewAsync(DateTime p_fecha, EstadoContenido p_estado, int p_denunciante_oid)
    {
        Usuario denuncianteEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_denunciante_oid)
            ?? throw new ModelException("Usuario not found for Denuncia.Denunciante.");

        Denuncia entity = new Denuncia
        {
            Fecha = p_fecha,
            Estado = p_estado,
            Denunciante = denuncianteEntity,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task DestroyAsync(int p_oid)
    {
        Denuncia entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Denuncia not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModifyAsync(int p_oid, DateTime p_fecha, EstadoContenido p_estado, int p_denunciante_oid)
    {
        Denuncia entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Denuncia not found.");
        entity.Fecha = p_fecha;
        entity.Estado = p_estado;
        await _repository.SaveAsync(entity);
    }

    public async Task<Denuncia?> ReadOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Denuncia>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task AddDenunciadoAsync(int p_oid, int p_usuario_oid)
    {
        Denuncia entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Denuncia not found.");
        Usuario relatedEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_usuario_oid)
            ?? throw new ModelException("Usuario not found for Denuncia.Denunciado.");
        entity.Denunciado = relatedEntity;
        relatedEntity.DenunciasRecibidas ??= new List<Denuncia>();
        if (!relatedEntity.DenunciasRecibidas.Contains(entity))
        {
            relatedEntity.DenunciasRecibidas.Add(entity);
        }
        await _unitOfWork.UsuarioRepository.SaveAsync(relatedEntity);
        await _repository.SaveAsync(entity);
    }

    public async Task AddContenidoDenunciadoAsync(int p_oid, int p_contenido_oid)
    {
        Denuncia entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Denuncia not found.");
        Contenido relatedEntity = await _unitOfWork.ContenidoRepository.ReadAsync(p_contenido_oid)
            ?? throw new ModelException("Contenido not found for Denuncia.ContenidoDenunciado.");
        entity.ContenidoDenunciado = relatedEntity;
        relatedEntity.Denuncias ??= new List<Denuncia>();
        if (!relatedEntity.Denuncias.Contains(entity))
        {
            relatedEntity.Denuncias.Add(entity);
        }
        await _unitOfWork.ContenidoRepository.SaveAsync(relatedEntity);
        await _repository.SaveAsync(entity);
    }

}
