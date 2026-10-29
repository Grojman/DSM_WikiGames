using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

namespace WikiGames.Application.Services;

public sealed partial class CategoriaService : ServiceEntity<Categoria>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoriaRepository _repository;

    public CategoriaService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public CategoriaService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private CategoriaService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.CategoriaRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.CategoriaRepository;
    }

    public Task<Categoria> AddCategoriaDefaultAsync(Categoria entity) =>
        AddDefaultAsync(entity);

    public Task DeleteCategoriaDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Categoria?> ReadOIDCategoriaDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Categoria>> ReadAllCategoriaDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Categoria> ModifierCategoriaDefaultAsync(Categoria entity) =>
        ModifierDefaultAsync(entity);

    public async Task DestroyAsync(int p_oid)
    {
        Categoria entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Categoria not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModifyAsync(int p_oid, string p_name, int p_videojuego_oid)
    {
        Categoria entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Categoria not found.");
        entity.Name = p_name;
        await _repository.SaveAsync(entity);
    }

    public async Task<Categoria?> ReadOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Categoria>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task AddHijoAsync(int p_oid, IReadOnlyList<int> p_categoria2_oids)
    {
        Categoria entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Categoria not found.");
        entity.Hijos ??= new List<Categoria>();
        foreach (var relatedId in p_categoria2_oids)
        {
            Categoria relatedEntity = await _unitOfWork.CategoriaRepository.ReadAsync(relatedId)
                ?? throw new ModelException("Categoria not found for Categoria.Hijos.");
            if (!entity.Hijos.Contains(relatedEntity))
            {
                entity.Hijos.Add(relatedEntity);
            }
            relatedEntity.Padre = entity;
            await _unitOfWork.CategoriaRepository.SaveAsync(relatedEntity);
        }
        await _repository.SaveAsync(entity);
    }

    public async Task AddPadreAsync(int p_oid, int p_categoria2_oid)
    {
        Categoria entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Categoria not found.");
        Categoria relatedEntity = await _unitOfWork.CategoriaRepository.ReadAsync(p_categoria2_oid)
            ?? throw new ModelException("Categoria not found for Categoria.Padre.");
        entity.Padre = relatedEntity;
        relatedEntity.Hijos ??= new List<Categoria>();
        if (!relatedEntity.Hijos.Contains(entity))
        {
            relatedEntity.Hijos.Add(entity);
        }
        await _unitOfWork.CategoriaRepository.SaveAsync(relatedEntity);
        await _repository.SaveAsync(entity);
    }

}
