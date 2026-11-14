using System.ComponentModel.DataAnnotations;

namespace WikiGames.Web.ViewModels;

public sealed class DenunciaViewModel : IValidatableObject
{
    /*PROTECTED REGION ID(DenunciaViewModel_generated) START*/
    [Key]
    public int Id { get; set; }

    [UIHint("HariaReadOnly:Create")]
    [DataType(DataType.Date)]
    public DateTime Fecha { get; set; }

    [UIHint("HariaReadOnly:Create")]
    [EnumDataType(typeof(WikiGames.Domain.Enums.EstadoContenido), ErrorMessage = "El valor de {0} no es válido.")]
    public WikiGames.Domain.Enums.EstadoContenido Estado { get; set; }

    [UIHint("HariaAssociation")]
    [Display(Name = "Notificacion")]
    public int? NotificacionId { get; set; }
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> NotificacionOptions { get; set; } = new();
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, UIHint("HariaAssociationLabel"), Display(Name = "Notificacion")]
    public string? NotificacionLabel { get; set; }

    [Required(ErrorMessage = "Debes indicar {0}.")]
    [UIHint("HariaAssociation")]
    [Display(Name = "Denunciante")]
    public int? DenuncianteId { get; set; }
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> DenuncianteOptions { get; set; } = new();
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, UIHint("HariaAssociationLabel"), Display(Name = "Denunciante")]
    public string? DenuncianteLabel { get; set; }

    [UIHint("HariaAssociation")]
    [Display(Name = "Denunciado")]
    public int? DenunciadoId { get; set; }
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> DenunciadoOptions { get; set; } = new();
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, UIHint("HariaAssociationLabel"), Display(Name = "Denunciado")]
    public string? DenunciadoLabel { get; set; }

    [UIHint("HariaAssociation")]
    [Display(Name = "ContenidoDenunciado")]
    public int? ContenidoDenunciadoId { get; set; }
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> ContenidoDenunciadoOptions { get; set; } = new();
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, UIHint("HariaAssociationLabel"), Display(Name = "ContenidoDenunciado")]
    public string? ContenidoDenunciadoLabel { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in ValidateAdditional(validationContext)) yield return result;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(DenunciaViewModel_manual) ENABLED START*/
    // Add custom properties and validation here.
    private IEnumerable<ValidationResult> ValidateAdditional(ValidationContext validationContext)
    {
        yield break;
    }

    /*PROTECTED REGION END*/
}
