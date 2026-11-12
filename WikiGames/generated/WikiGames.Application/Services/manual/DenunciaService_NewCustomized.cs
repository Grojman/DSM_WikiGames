using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.Services_Denuncia_newCustomized) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.Services;

public sealed partial class DenunciaService
{
    public async Task<Denuncia> NewCustomizedAsync(int p_denunciante_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.Services_Denuncia_newCustomized) ENABLED START*/
        Usuario denuncianteEntity = await _unitOfWork.UsuarioRepository.ReadAsync(p_denunciante_oid)
            ?? throw new ModelException("Usuario not found for Denuncia.Denunciante.");
        
        DateTime p_fecha = DateTime.Now;
        
        Denuncia entity = new Denuncia
        {
            Fecha = p_fecha,
            Denunciante = denuncianteEntity,
        };

        await _repository.SaveAsync(entity);

        Notificacion notificacion = new Notificacion
    {
        Texto = "Tu denuncia se ha registrado correctamente.",
        Fecha = p_fecha
    };

    entity.Notificacion = notificacion;

    notificacion.Notificados.Add(denuncianteEntity);

    await _unitOfWork.NotificacionRepository.SaveAsync(notificacion);

        return entity;
        /*PROTECTED REGION END*/
    }
}
