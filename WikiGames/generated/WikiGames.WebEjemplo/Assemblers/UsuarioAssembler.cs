// HARIA-WEB MVC managed assembler
using WikiGames.Domain.Entities;

namespace WikiGames.WebEjemplo.Assemblers;

public static class UsuarioAssembler
{
    /*PROTECTED REGION ID(UsuarioAssembler_method_ToViewModel) START*/
    public static WikiGames.WebEjemplo.ViewModels.UsuarioViewModel ToViewModel(Usuario entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            RolUsuario = entity.RolUsuario,

        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(UsuarioAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(WikiGames.WebEjemplo.ViewModels.UsuarioViewModel model, Usuario entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Name = model.Name;
        entity.Email = model.Email;
        entity.RolUsuario = model.RolUsuario;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(UsuarioAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
