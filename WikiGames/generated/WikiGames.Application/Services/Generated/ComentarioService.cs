using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

namespace WikiGames.Application.Services;

public sealed partial class ComentarioService : ServiceEntity<Comentario>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IComentarioRepository _repository;

    public ComentarioService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public ComentarioService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private ComentarioService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.ComentarioRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.ComentarioRepository;
    }

    public Task<Comentario> AddComentarioDefaultAsync(Comentario entity) =>
        AddDefaultAsync(entity);

    public Task DeleteComentarioDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Comentario?> ReadOIDComentarioDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Comentario>> ReadAllComentarioDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Comentario> ModifierComentarioDefaultAsync(Comentario entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Comentario> NewAsync(string p_texto, DateTime p_fechaCreacion, int p_numeroMeGusta, int p_tema_oid, int p_usuarioCreador_oid)
    {
        Usuario usuarioCreadorEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_usuarioCreador_oid)
            ?? throw new ModelException("Usuario not found for Comentario.UsuarioCreador.");

        TemaConversacion temaEntity = await _unitOfWork.TemaConversacionRepository.ReadAsync(p_tema_oid)
            ?? throw new ModelException("TemaConversacion not found for Comentario.Tema.");

        Comentario entity = new Comentario
        {
            Texto = p_texto,
            FechaCreacion = p_fechaCreacion,
            NumeroMeGusta = p_numeroMeGusta,
            UsuarioCreador = usuarioCreadorEntity,
            Tema = temaEntity,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task DestroyAsync(int p_oid)
    {
        Comentario entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Comentario not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModifyAsync(int p_oid, string p_texto, DateTime p_fechaCreacion, int p_numeroMeGusta, int p_tema_oid, int p_usuarioCreador_oid)
    {
        Comentario entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Comentario not found.");
        entity.Texto = p_texto;
        entity.FechaCreacion = p_fechaCreacion;
        entity.NumeroMeGusta = p_numeroMeGusta;
        await _repository.SaveAsync(entity);
    }

    public async Task<Comentario?> ReadOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Comentario>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task AddComentarioAsociadoAsync(int p_oid, int p_comentario2_oid)
    {
        Comentario entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Comentario not found.");
        Comentario relatedEntity = await _unitOfWork.ComentarioRepository.ReadAsync(p_comentario2_oid)
            ?? throw new ModelException("Comentario not found for Comentario.ComentarioAsociado.");
        entity.ComentarioAsociado = relatedEntity;
        relatedEntity.Respuestas ??= new List<Comentario>();
        if (!relatedEntity.Respuestas.Contains(entity))
        {
            relatedEntity.Respuestas.Add(entity);
        }
        await _unitOfWork.ComentarioRepository.SaveAsync(relatedEntity);
        await _repository.SaveAsync(entity);
    }

    public async Task AddRespuestaAsync(int p_oid, IReadOnlyList<int> p_comentario2_oids)
    {
        Comentario entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Comentario not found.");
        entity.Respuestas ??= new List<Comentario>();
        foreach (var relatedId in p_comentario2_oids)
        {
            Comentario relatedEntity = await _unitOfWork.ComentarioRepository.ReadAsync(relatedId)
                ?? throw new ModelException("Comentario not found for Comentario.Respuestas.");
            if (!entity.Respuestas.Contains(relatedEntity))
            {
                entity.Respuestas.Add(relatedEntity);
            }
            relatedEntity.ComentarioAsociado = entity;
            await _unitOfWork.ComentarioRepository.SaveAsync(relatedEntity);
        }
        await _repository.SaveAsync(entity);
    }

}
