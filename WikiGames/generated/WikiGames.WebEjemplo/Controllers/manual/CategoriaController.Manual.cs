// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(CategoriaController_manual_usings) ENABLED START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WikiGames.Domain.Entities;
using WikiGames.Application.Services;
using WikiGames.WebEjemplo.Assemblers;

using WikiGames.Domain.Enums;
/*PROTECTED REGION END*/

namespace WikiGames.WebEjemplo.Controllers;

// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION ID(CategoriaController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class CategoriaController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CategoriaController_CrearSubcategoria_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> CrearSubcategoria(int p_padre_oid, string p_name)
    {
    /*PROTECTED REGION ID(CategoriaController_CrearSubcategoria_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var useCase = new WikiGames.Application.UseCases.Categoria.CategoriaUseCase(unitOfWork);
        var result = await useCase.CrearSubcategoriaAsync(p_padre_oid, p_name);
        return Ok(result);
    /*PROTECTED REGION END*/
    }
}
