using System.ComponentModel.DataAnnotations;

namespace WikiGames.WebEjemplo.ViewModels;

public sealed class VideojuegoViewModel : IValidatableObject
{
    /*PROTECTED REGION ID(VideojuegoViewModel_generated) START*/
    [UIHint("HariaReadOnly:Create")]
    [Required(ErrorMessage = "Debes indicar {0}.")]
    public string Titulo { get; set; } = string.Empty;

    [UIHint("HariaReadOnly:Create")]
    [Required(ErrorMessage = "Debes indicar {0}.")]
    public string Enlace { get; set; } = string.Empty;

    [UIHint("HariaReadOnly:Create")]
    [Required(ErrorMessage = "Debes indicar {0}.")]
    public string Imagen { get; set; } = string.Empty;

    [UIHint("HariaReadOnly:Create")]
    [EnumDataType(typeof(WikiGames.Domain.Enums.EstadoContenido), ErrorMessage = "El valor de {0} no es válido.")]
    public WikiGames.Domain.Enums.EstadoContenido Estado { get; set; }

    [UIHint("HariaReadOnly:Create")]
    public float PuntuacionMedia { get; set; }

    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Debes indicar {0}.")]
    public string Texto { get; set; } = string.Empty;

    [UIHint("HariaReadOnly:Create")]
    [DataType(DataType.Date)]
    public DateTime FechaCreacion { get; set; }

    [UIHint("HariaReadOnly:Create")]
    public int NumeroMeGusta { get; set; }

    // HARIA-WEB COMPOSITION {"property":"Valoraciones","fullName":"WikiGames.WebEjemplo.ViewModels.ValoracionViewModel","fields":[{"name":"Puntuacion","type":"float","key":false},{"name":"Id","type":"int","key":true},{"name":"Texto","type":"string","key":false},{"name":"FechaCreacion","type":"DateTime","key":false,"dataType":"Date"},{"name":"NumeroMeGusta","type":"int","key":false},{"name":"UsuarioCreadorId","type":"int?","association":true},{"name":"UsuarioCreadorLabel","type":"string?","readOnly":true}]}
    [UIHint("HariaComposition")]
    [MinLength(0)]
    public List<ValoracionViewModel> Valoraciones { get; set; } = new();
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public ValoracionViewModel ValoracionesTemplate { get; set; } = new();

    [UIHint("HariaAssociation")]
    [Display(Name = "Rechazo")]
    public int? RechazoId { get; set; }
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> RechazoOptions { get; set; } = new();
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, UIHint("HariaAssociationLabel"), Display(Name = "Rechazo")]
    public string? RechazoLabel { get; set; }

    [Required(ErrorMessage = "Debes indicar {0}.")]
    [UIHint("HariaAssociation")]
    [Display(Name = "UsuarioCreador")]
    public int? UsuarioCreadorId { get; set; }
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> UsuarioCreadorOptions { get; set; } = new();
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, UIHint("HariaAssociationLabel"), Display(Name = "UsuarioCreador")]
    public string? UsuarioCreadorLabel { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in ValidateAdditional(validationContext)) yield return result;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(VideojuegoViewModel_manual) ENABLED START*/
    // Add custom properties and validation here.
    private IEnumerable<ValidationResult> ValidateAdditional(ValidationContext validationContext)
    {
        yield break;
    }

    /*PROTECTED REGION END*/
}
