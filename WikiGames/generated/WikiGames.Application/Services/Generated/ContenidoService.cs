using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

namespace WikiGames.Application.Services;

public sealed partial class ContenidoService : ServiceEntity<Contenido>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IContenidoRepository _repository;

    public ContenidoService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public ContenidoService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private ContenidoService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.ContenidoRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.ContenidoRepository;
    }

    public Task<Contenido> AddContenidoDefaultAsync(Contenido entity) =>
        AddDefaultAsync(entity);

    public Task DeleteContenidoDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Contenido?> ReadOIDContenidoDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Contenido>> ReadAllContenidoDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Contenido> ModifierContenidoDefaultAsync(Contenido entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Contenido> NewAsync(string p_texto, DateTime p_fechaCreacion, int p_numeroMeGusta, int p_usuarioCreador_oid)
    {
        Usuario usuarioCreadorEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_usuarioCreador_oid)
            ?? throw new ModelException("Usuario not found for Contenido.UsuarioCreador.");

        Contenido entity = new Contenido
        {
            Texto = p_texto,
            FechaCreacion = p_fechaCreacion,
            NumeroMeGusta = p_numeroMeGusta,
            UsuarioCreador = usuarioCreadorEntity,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task DestroyAsync(int p_oid)
    {
        Contenido entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Contenido not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModifyAsync(int p_oid, string p_texto, DateTime p_fechaCreacion, int p_numeroMeGusta, int p_usuarioCreador_oid)
    {
        Contenido entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Contenido not found.");
        entity.Texto = p_texto;
        entity.FechaCreacion = p_fechaCreacion;
        entity.NumeroMeGusta = p_numeroMeGusta;
        await _repository.SaveAsync(entity);
    }

    public async Task<Contenido?> ReadOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Contenido>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task AddDenunciaAsync(int p_oid, IReadOnlyList<int> p_denuncia_oids)
    {
        Contenido entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Contenido not found.");
        entity.Denuncias ??= new List<Denuncia>();
        foreach (var relatedId in p_denuncia_oids)
        {
            Denuncia relatedEntity = await _unitOfWork.DenunciaRepository.ReadAsync(relatedId)
                ?? throw new ModelException("Denuncia not found for Contenido.Denuncias.");
            if (!entity.Denuncias.Contains(relatedEntity))
            {
                entity.Denuncias.Add(relatedEntity);
            }
            relatedEntity.ContenidoDenunciado = entity;
            await _unitOfWork.DenunciaRepository.SaveAsync(relatedEntity);
        }
        await _repository.SaveAsync(entity);
    }

}
