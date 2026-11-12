// HARIA-WEB GENERATED sha256:1ddf8d354e4f035e312ae0b2d571f3cc9979f14a3c2a629428d71a14fa6c854a
using System.ComponentModel.DataAnnotations;

namespace WikiGames.WebEjemplo.ViewModels;

public sealed class LoginViewModel
{
    public string UserClass { get; set; } = "Usuario";
    public string? Identifier { get; set; }
    [Display(Name = "id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Debes indicar {0}.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}
