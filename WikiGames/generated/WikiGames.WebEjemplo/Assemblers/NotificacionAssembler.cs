// HARIA-WEB MVC managed assembler
using WikiGames.Domain.Entities;

namespace WikiGames.WebEjemplo.Assemblers;

public static class NotificacionAssembler
{
    /*PROTECTED REGION ID(NotificacionAssembler_method_ToViewModel) START*/
    public static WikiGames.WebEjemplo.ViewModels.NotificacionViewModel ToViewModel(Notificacion entity) =>
        new()
        {
            Id = entity.Id,
            Texto = entity.Texto,
            Fecha = entity.Fecha,
            DenunciaId = entity.Denuncia?.Id,
            DenunciaLabel = entity.Denuncia is null ? null : Convert.ToString(entity.Denuncia.Id, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(NotificacionAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(WikiGames.WebEjemplo.ViewModels.NotificacionViewModel model, Notificacion entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Texto = model.Texto;
        entity.Fecha = model.Fecha;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(NotificacionAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
