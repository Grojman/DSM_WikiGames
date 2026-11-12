// HARIA-WEB MVC managed assembler
using WikiGames.Domain.Entities;

namespace WikiGames.WebEjemplo.Assemblers;

public static class RechazoAssembler
{
    /*PROTECTED REGION ID(RechazoAssembler_method_ToViewModel) START*/
    public static WikiGames.WebEjemplo.ViewModels.RechazoViewModel ToViewModel(Rechazo entity) =>
        new()
        {
            Id = entity.Id,
            Fecha = entity.Fecha,
            Motivo = entity.Motivo,
            VideojuegoId = entity.Videojuego?.Id,
            VideojuegoLabel = entity.Videojuego is null ? null : Convert.ToString(entity.Videojuego.Titulo, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
            GestorId = entity.Gestor?.Id,
            GestorLabel = entity.Gestor is null ? null : Convert.ToString(entity.Gestor.Name, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(RechazoAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(WikiGames.WebEjemplo.ViewModels.RechazoViewModel model, Rechazo entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Fecha = model.Fecha;
        entity.Motivo = model.Motivo;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(RechazoAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
