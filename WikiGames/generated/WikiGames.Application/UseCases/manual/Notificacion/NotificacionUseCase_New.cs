using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Notificacion_new) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Notificacion;

public sealed partial class NotificacionUseCase
{
    public async Task<global::WikiGames.Domain.Entities.Notificacion> NewAsync(string p_texto, DateTime p_fecha, IReadOnlyList<int> p_notificados_oids)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Notificacion_new) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        NotificacionService notificacionService = null!;
        UsuarioService usuarioService = null!;
        global::WikiGames.Domain.Entities.Notificacion result = default(global::WikiGames.Domain.Entities.Notificacion)!;
        var destinatarios = new List<string>();

        try
        {
            await _unitOfWork.BeginTransactionAsync("NotificacionUseCase.NewAsync");
            notificacionService = new NotificacionService(_unitOfWork);
            usuarioService = new UsuarioService(_unitOfWork);

            if (string.IsNullOrWhiteSpace(p_texto))
            {
                throw new ModelException("La notificación debe tener un texto.");
            }

            if (p_notificados_oids is null || p_notificados_oids.Count == 0)
            {
                throw new ModelException("La notificación debe tener al menos un usuario notificado.");
            }

            global::WikiGames.Domain.Entities.Notificacion entity = new global::WikiGames.Domain.Entities.Notificacion
            {
                Texto = p_texto,
                Fecha = p_fecha,
            };

            await _unitOfWork.NotificacionRepository.SaveAsync(entity);

            foreach (int usuarioOid in p_notificados_oids)
            {
                global::WikiGames.Domain.Entities.Usuario usuario = await _unitOfWork.UsuarioRepository.ReadAsync(usuarioOid)
                    ?? throw new ModelException($"No existe el usuario {usuarioOid} a notificar.");

                // Relationer Usuario.addNotificacione: asocia la notificación al usuario (en ambos sentidos).
                await usuarioService.AddNotificacioneAsync(usuarioOid, new[] { entity.Id });
                destinatarios.Add(usuario.Email);
            }

            result = entity;

            await _unitOfWork.CommitAsync();

            IEmailService? emailService = _utils?.GetService<IEmailService>();
            if (emailService is not null)
            {
                const string asunto = "WikiGames: tienes una nueva notificación";
                string cuerpo = "Tienes una nueva notificación en WikiGames:" + Environment.NewLine + Environment.NewLine
                    + result.Texto + Environment.NewLine + Environment.NewLine
                    + $"Fecha: {result.Fecha:dd/MM/yyyy}";

                foreach (string email in destinatarios)
                {
                    await emailService.SendEmailAsync(email, asunto, cuerpo);
                }
            }
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Notificacion.new.", ex);
        }


        return result;
        /*PROTECTED REGION END*/
    }
}
