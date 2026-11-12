using System.ComponentModel.DataAnnotations;

namespace WikiGames.WebEjemplo.ViewModels;

public sealed class NotificacionViewModel : IValidatableObject
{
    /*PROTECTED REGION ID(NotificacionViewModel_generated) START*/
    [Key]
    public int Id { get; set; }

    [UIHint("HariaReadOnly:Create")]
    [Required(ErrorMessage = "Debes indicar {0}.")]
    public string Texto { get; set; } = string.Empty;

    [UIHint("HariaReadOnly:Create")]
    [DataType(DataType.Date)]
    public DateTime Fecha { get; set; }

    [UIHint("HariaAssociation")]
    [Display(Name = "Denuncia")]
    public int? DenunciaId { get; set; }
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> DenunciaOptions { get; set; } = new();
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, UIHint("HariaAssociationLabel"), Display(Name = "Denuncia")]
    public string? DenunciaLabel { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in ValidateAdditional(validationContext)) yield return result;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(NotificacionViewModel_manual) ENABLED START*/
    // Add custom properties and validation here.
    private IEnumerable<ValidationResult> ValidateAdditional(ValidationContext validationContext)
    {
        yield break;
    }

    /*PROTECTED REGION END*/
}
