// HARIA-WEB GENERATED sha256:6b0f0a81af6d353d9c7f692235cac38f5617f60a4a87aa998fb35a56696b0d97
using System.ComponentModel.DataAnnotations;

namespace WikiGames.Web.ViewModels;

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
