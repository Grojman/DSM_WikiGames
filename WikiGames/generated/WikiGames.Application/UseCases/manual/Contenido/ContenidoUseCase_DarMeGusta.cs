using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Contenido_darMeGusta) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Contenido;

public sealed partial class ContenidoUseCase
{
    public async Task DarMeGustaAsync(int p_oid, int p_user)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Contenido_darMeGusta) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        ContenidoService contenidoService = null!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("ContenidoUseCase.DarMeGustaAsync");
            contenidoService = new ContenidoService(_unitOfWork);

            global::WikiGames.Domain.Entities.Contenido contenido = await _unitOfWork.ContenidoRepository.ReadAsync(p_oid)
                ?? throw new ModelException($"No existe el contenido {p_oid}.");
            global::WikiGames.Domain.Entities.Usuario usuario = await _unitOfWork.UsuarioRepository.ReadAsync(p_user)
                ?? throw new ModelException($"No existe el usuario {p_user}.");

            if (contenido.UsuariosMeGusta.Any(u => u.Id == usuario.Id))
            {
                throw new ModelException("El usuario ya había dado me gusta a este contenido.");
            }

            // Relationer Contenido.addUsuarioMeGusta.
            await contenidoService.AddUsuarioMeGustaAsync(p_oid, new[] { usuario.Id });

            contenido.NumeroMeGusta++;
            await _unitOfWork.ContenidoRepository.SaveAsync(contenido);

            var notificacionUseCase = _utils is null
                ? new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork)
                : new global::WikiGames.Application.UseCases.Notificacion.NotificacionUseCase(_unitOfWork, _utils);
            await notificacionUseCase.NewAsync(
                $"A {usuario.Name} le ha gustado tu contenido: \"{contenido.Texto}\"",
                DateTime.Now,
                new[] { contenido.UsuarioCreador.Id });

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Contenido.darMeGusta.", ex);
        }
        /*PROTECTED REGION END*/
    }
}
