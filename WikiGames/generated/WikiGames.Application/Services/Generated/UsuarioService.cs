using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

namespace WikiGames.Application.Services;

public sealed partial class UsuarioService : ServiceEntity<Usuario>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsuarioRepository _repository;
    private readonly IHariaPasswordHasher _passwordHasher;

    public UsuarioService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public UsuarioService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private UsuarioService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.UsuarioRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.UsuarioRepository;
        _passwordHasher = unitOfWork.PasswordHasher;
    }

    public Task<Usuario> AddUsuarioDefaultAsync(Usuario entity)
    {
        HashPasswordProperties(entity);
        return AddDefaultAsync(entity);
    }

    public Task DeleteUsuarioDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Usuario?> ReadOIDUsuarioDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Usuario>> ReadAllUsuarioDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Usuario> ModifierUsuarioDefaultAsync(Usuario entity)
    {
        HashPasswordProperties(entity);
        return ModifierDefaultAsync(entity);
    }

    private void HashPasswordProperties(Usuario entity)
    {
        if (entity.Password is not null)
        {
            entity.Password = _passwordHasher.HashPasswordIfNeeded(entity.Password);
        }
    }

    public async Task<Usuario> NewAsync(string p_name, string p_password, Rol p_rolUsuario)
    {
        Usuario entity = new Usuario
        {
            Name = p_name,
            Password = _passwordHasher.HashPasswordIfNeeded(p_password),
            RolUsuario = p_rolUsuario,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task DestroyAsync(int p_oid)
    {
        Usuario entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Usuario not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModifyAsync(int p_oid, string p_name, string p_password, Rol p_rolUsuario)
    {
        Usuario entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Usuario not found.");
        entity.Name = p_name;
        entity.Password = _passwordHasher.HashPasswordIfNeeded(p_password);
        entity.RolUsuario = p_rolUsuario;
        await _repository.SaveAsync(entity);
    }

    public async Task<Usuario?> ReadOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Usuario>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task AddDenunciasRecibidaAsync(int p_oid, IReadOnlyList<int> p_denuncia_oids)
    {
        Usuario entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Usuario not found.");
        entity.DenunciasRecibidas ??= new List<Denuncia>();
        foreach (var relatedId in p_denuncia_oids)
        {
            Denuncia relatedEntity = await _unitOfWork.DenunciaRepository.ReadAsync(relatedId)
                ?? throw new ModelException("Denuncia not found for Usuario.DenunciasRecibidas.");
            if (!entity.DenunciasRecibidas.Contains(relatedEntity))
            {
                entity.DenunciasRecibidas.Add(relatedEntity);
            }
            relatedEntity.Denunciado = entity;
            await _unitOfWork.DenunciaRepository.SaveAsync(relatedEntity);
        }
        await _repository.SaveAsync(entity);
    }

    public async Task<IReadOnlyList<Usuario>> FindByNameLinQAsync(string p_name)
    {
        return await _repository.FindByNameLinQAsync(p_name);
    }

    public async Task<IReadOnlyList<Usuario>> FindByTipoUsuarioAsync(Rol p_rol)
    {
        return await _repository.FindByTipoUsuarioAsync(p_rol);
    }

}
