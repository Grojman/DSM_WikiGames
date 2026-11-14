// HARIA-WEB MVC managed assembler
using WikiGames.Domain.Entities;

namespace WikiGames.Web.Assemblers;

public static class ValoracionAssembler
{
    /*PROTECTED REGION ID(ValoracionAssembler_method_ToViewModel) START*/
    public static WikiGames.Web.ViewModels.ValoracionViewModel ToViewModel(Valoracion entity) =>
        new()
        {
            Puntuacion = entity.Puntuacion,
            Id = entity.Id,
            Texto = entity.Texto,
            FechaCreacion = entity.FechaCreacion,
            NumeroMeGusta = entity.NumeroMeGusta,
            VideojuegoId = entity.Videojuego?.Id,
            VideojuegoLabel = entity.Videojuego is null ? null : Convert.ToString(entity.Videojuego.Titulo, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
            UsuarioCreadorId = entity.UsuarioCreador?.Id,
            UsuarioCreadorLabel = entity.UsuarioCreador is null ? null : Convert.ToString(entity.UsuarioCreador.Name, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ValoracionAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(WikiGames.Web.ViewModels.ValoracionViewModel model, Valoracion entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Puntuacion = model.Puntuacion;
        entity.Texto = model.Texto;
        entity.FechaCreacion = model.FechaCreacion;
        entity.NumeroMeGusta = model.NumeroMeGusta;
    }
    /*PROTECTED REGION END*/
    /*PROTECTED REGION ID(ValoracionAssembler_method_Create) START*/
    public static Valoracion Create(WikiGames.Web.ViewModels.ValoracionViewModel model)
    {
        var entity = new Valoracion();
        ApplyToEntity(model, entity);
        return entity;
    }
    /*PROTECTED REGION END*/
    /*PROTECTED REGION ID(ValoracionAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
