// HARIA-WEB MVC managed assembler
using WikiGames.Domain.Entities;

namespace WikiGames.WebEjemplo.Assemblers;

public static class CategoriaAssembler
{
    /*PROTECTED REGION ID(CategoriaAssembler_method_ToViewModel) START*/
    public static WikiGames.WebEjemplo.ViewModels.CategoriaViewModel ToViewModel(Categoria entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            PadreId = entity.Padre?.Id,
            PadreLabel = entity.Padre is null ? null : Convert.ToString(entity.Padre.Name, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(CategoriaAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(WikiGames.WebEjemplo.ViewModels.CategoriaViewModel model, Categoria entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Name = model.Name;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(CategoriaAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
