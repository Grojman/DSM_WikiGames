using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Application.UseCases.Notificacion;
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
        NotificacionUseCase notificacionUseCase = null!;
        UsuarioService usuarioService = null!;
        

        try
        {
            await _unitOfWork.BeginTransactionAsync("ContenidoUseCase.DarMeGustaAsync");
            contenidoService = new ContenidoService(_unitOfWork);
            notificacionUseCase = new NotificacionUseCase(_unitOfWork);
            usuarioService = new UsuarioService(_unitOfWork);

            var contenido = await contenidoService.ReadOIDAsync(p_oid) ?? throw new ModelException($"Contenido with OID {p_oid} not found.");
            var usuario = await _unitOfWork.UsuarioRepository.ReadAsync(p_user) ?? throw 
            new ModelException($"Usuario with OID {p_user} not found.");


            // añadir el relationer
            contenidoService.AddUsuarioFavoritoAsync(p_oid, );
            usuarioService.AddContenidoFavoritoAsync(p_user, );
            

            contenido.UsuarioFavorito.Add(usuario);
            usuario.ContenidoFavorito.Add(contenido);


            var notificacion = await notificacionUseCase.NewAsync($"El usuario {usuario.Name} ha dado me gusta a tu contenido: {contenido.Texto}", DateTime.Now);
            

            await _unitOfWork.NotificacionRepository.SaveAsync(notificacion);
            await _unitOfWork.ContenidoRepository.SaveAsync(contenido);

            // Write here your custom transaction code...
            // Use contenidoService, _unitOfWork.ContenidoRepository or _utils!.GetRequiredService<TService>() as the generated CP equivalent.

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
