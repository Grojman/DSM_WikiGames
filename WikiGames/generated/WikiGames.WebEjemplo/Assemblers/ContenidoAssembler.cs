// HARIA-WEB MVC managed assembler
using WikiGames.Domain.Entities;

namespace WikiGames.WebEjemplo.Assemblers;

public static class ContenidoAssembler
{
    /*PROTECTED REGION ID(ContenidoAssembler_method_ToViewModel) START*/
    public static WikiGames.WebEjemplo.ViewModels.ContenidoViewModel ToViewModel(Contenido entity) =>
        new()
        {
            Id = entity.Id,
            Texto = entity.Texto,
            FechaCreacion = entity.FechaCreacion,
            NumeroMeGusta = entity.NumeroMeGusta,
            UsuarioCreadorId = entity.UsuarioCreador?.Id,
            UsuarioCreadorLabel = entity.UsuarioCreador is null ? null : Convert.ToString(entity.UsuarioCreador.Name, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ContenidoAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(WikiGames.WebEjemplo.ViewModels.ContenidoViewModel model, Contenido entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Texto = model.Texto;
        entity.FechaCreacion = model.FechaCreacion;
        entity.NumeroMeGusta = model.NumeroMeGusta;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ContenidoAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
