// HARIA-WEB MVC managed assembler
using WikiGames.Domain.Entities;

namespace WikiGames.Web.Assemblers;

public static class AccionSistemaAssembler
{
    /*PROTECTED REGION ID(AccionSistemaAssembler_method_ToViewModel) START*/
    public static WikiGames.Web.ViewModels.AccionSistemaViewModel ToViewModel(AccionSistema entity) =>
        new()
        {
            Id = entity.Id,
            Fecha = entity.Fecha,
            Descripcion = entity.Descripcion,
            CausanteId = entity.Causante?.Id,
            CausanteLabel = entity.Causante is null ? null : Convert.ToString(entity.Causante.Name, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
            ContenidoAfectadoId = entity.ContenidoAfectado?.Id,
            ContenidoAfectadoLabel = entity.ContenidoAfectado is null ? null : Convert.ToString(entity.ContenidoAfectado.Texto, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(AccionSistemaAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(WikiGames.Web.ViewModels.AccionSistemaViewModel model, AccionSistema entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Fecha = model.Fecha;
        entity.Descripcion = model.Descripcion;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(AccionSistemaAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
