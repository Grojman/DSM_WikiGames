// HARIA-WEB MVC managed assembler
using WikiGames.Domain.Entities;

namespace WikiGames.WebEjemplo.Assemblers;

public static class DenunciaAssembler
{
    /*PROTECTED REGION ID(DenunciaAssembler_method_ToViewModel) START*/
    public static WikiGames.WebEjemplo.ViewModels.DenunciaViewModel ToViewModel(Denuncia entity) =>
        new()
        {
            Id = entity.Id,
            Fecha = entity.Fecha,
            Estado = entity.Estado,
            NotificacionId = entity.Notificacion?.Id,
            NotificacionLabel = entity.Notificacion is null ? null : Convert.ToString(entity.Notificacion.Texto, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
            DenuncianteId = entity.Denunciante?.Id,
            DenuncianteLabel = entity.Denunciante is null ? null : Convert.ToString(entity.Denunciante.Name, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
            DenunciadoId = entity.Denunciado?.Id,
            DenunciadoLabel = entity.Denunciado is null ? null : Convert.ToString(entity.Denunciado.Name, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
            ContenidoDenunciadoId = entity.ContenidoDenunciado?.Id,
            ContenidoDenunciadoLabel = entity.ContenidoDenunciado is null ? null : Convert.ToString(entity.ContenidoDenunciado.Texto, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(DenunciaAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(WikiGames.WebEjemplo.ViewModels.DenunciaViewModel model, Denuncia entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Fecha = model.Fecha;
        entity.Estado = model.Estado;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(DenunciaAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
