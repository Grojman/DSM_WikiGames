// HARIA-WEB MVC managed assembler
using WikiGames.Domain.Entities;

namespace WikiGames.Web.Assemblers;

public static class VideojuegoAssembler
{
    /*PROTECTED REGION ID(VideojuegoAssembler_method_ToViewModel) START*/
    public static WikiGames.Web.ViewModels.VideojuegoViewModel ToViewModel(Videojuego entity) =>
        new()
        {
            Titulo = entity.Titulo,
            Enlace = entity.Enlace,
            Imagen = entity.Imagen,
            Estado = entity.Estado,
            PuntuacionMedia = entity.PuntuacionMedia,
            Id = entity.Id,
            Texto = entity.Texto,
            FechaCreacion = entity.FechaCreacion,
            NumeroMeGusta = entity.NumeroMeGusta,
            Valoraciones = entity.Valoraciones.Select(ValoracionAssembler.ToViewModel).ToList(),
            RechazoId = entity.Rechazo?.Id,
            RechazoLabel = entity.Rechazo is null ? null : Convert.ToString(entity.Rechazo.Motivo, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
            UsuarioCreadorId = entity.UsuarioCreador?.Id,
            UsuarioCreadorLabel = entity.UsuarioCreador is null ? null : Convert.ToString(entity.UsuarioCreador.Name, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(VideojuegoAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(WikiGames.Web.ViewModels.VideojuegoViewModel model, Videojuego entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Titulo = model.Titulo;
        entity.Enlace = model.Enlace;
        entity.Imagen = model.Imagen;
        entity.Estado = model.Estado;
        entity.PuntuacionMedia = model.PuntuacionMedia;
        entity.Texto = model.Texto;
        entity.FechaCreacion = model.FechaCreacion;
        entity.NumeroMeGusta = model.NumeroMeGusta;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(VideojuegoAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
