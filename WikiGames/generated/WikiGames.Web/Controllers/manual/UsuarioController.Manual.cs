// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(UsuarioController_manual_usings) ENABLED START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WikiGames.Domain.Entities;
using WikiGames.Application.Services;
using WikiGames.Web.Assemblers;
using WikiGames.Web.Utils.Security;

using WikiGames.Domain.Enums;
using WikiGames.Web.Models.Usuarios;
using WikiGames.Application.Exceptions;
using WikiGames.Web.Utils.Paginas;
using WikiGames.Web.ViewModels.Paginas;
/*PROTECTED REGION END*/

namespace WikiGames.Web.Controllers;

// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION ID(UsuarioController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class UsuarioController
{
    [HttpGet]
    /*PROTECTED REGION ID(UsuarioController_Create_GET_attributes) ENABLED START*/
    [Authorize(Roles = UserRoles.Administrador)]
    /*PROTECTED REGION END*/
    public IActionResult Create()
    {
    /*PROTECTED REGION ID(UsuarioController_Create_GET_body) ENABLED START*/

        return View(new WikiGames.Web.ViewModels.UsuarioViewModel());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(UsuarioController_Create_POST_attributes) ENABLED START*/
    [Authorize(Roles = UserRoles.Administrador)]
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(WikiGames.Web.ViewModels.UsuarioViewModel model)
    {
    /*PROTECTED REGION ID(UsuarioController_Create_POST_body) ENABLED START*/


        if (string.IsNullOrEmpty(model.Password)) ModelState.AddModelError(nameof(model.Password), "Debes indicar una contraseña.");
        if (!ModelState.IsValid) return View(model);

        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var useCase = new global::WikiGames.Application.UseCases.Usuario.UsuarioUseCase(unitOfWork);
        try
        {
            // Web -> backend: the ViewModel becomes a typed UsuarioModel, which gives the Rol.
            var usuario = UsuarioModelMapper.FromViewModel(model);
            await useCase.NewAsync(usuario.Name, usuario.Email, model.Password!, UsuarioModelMapper.ToRol(usuario));
        }
        catch (Exception error) when (error is not OperationCanceledException)
        {
            _logger.LogError(error, "Could not create the record.");
            ModelState.AddModelError(string.Empty, "No se ha podido guardar el registro. Inténtalo de nuevo.");
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(UsuarioController_Delete_GET_attributes) ENABLED START*/
    [Authorize(Roles = UserRoles.Administrador)]
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(UsuarioController_Delete_GET_body) ENABLED START*/

        var entity = await _usuarioService.ReadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(UsuarioAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(UsuarioController_DeleteConfirmed_POST_attributes) ENABLED START*/
    [Authorize(Roles = UserRoles.Administrador)]
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(UsuarioController_DeleteConfirmed_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return View("Delete", new WikiGames.Web.ViewModels.UsuarioViewModel());
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var useCase = new global::WikiGames.Application.UseCases.Usuario.UsuarioUseCase(unitOfWork);
        await useCase.DestroyAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    // =====================================================================
    // Página WikiGames "Usuarios" (admin-usuarios.html). Solo administradores.
    // Acciones enteramente manuales: HARIA las conserva al regenerar.
    // =====================================================================

    [HttpGet]
    [Authorize(Roles = UserRoles.Administrador)]
    public async Task<IActionResult> Lista(string? q)
    {
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var usuarios = await unitOfWork.UsuarioRepository.ReadAllAsync();
        var juegosPorUsuario = (await unitOfWork.VideojuegoRepository.ReadAllAsync())
            .Where(v => v.UsuarioCreador is not null)
            .GroupBy(v => v.UsuarioCreador.Id)
            .ToDictionary(g => g.Key, g => g.Count());
        // Usuario.denunciasRecibidas (Denuncia.denunciado)
        var denunciasPorUsuario = (await unitOfWork.DenunciaRepository.ReadAllAsync())
            .Where(d => d.Denunciado is not null)
            .GroupBy(d => d.Denunciado!.Id)
            .ToDictionary(g => g.Key, g => g.Count());

        var filtro = q?.Trim();
        var es = StringComparer.Create(new System.Globalization.CultureInfo("es-ES"), true);
        var model = new ListaUsuariosViewModel
        {
            Q = filtro,
            AdministradorActualId = WikiGamesPaginas.UsuarioActualId(User) ?? 0,
            Usuarios = usuarios
                .Where(u => string.IsNullOrEmpty(filtro) || u.Name.Contains(filtro, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(u => u.Name, es)
                .Select(u =>
                {
                    var vm = UsuarioAssembler.ToViewModel(u);
                    vm.NumeroVideojuegos = juegosPorUsuario.GetValueOrDefault(u.Id);
                    vm.NumeroDenunciasRecibidas = denunciasPorUsuario.GetValueOrDefault(u.Id);
                    return vm;
                })
                .ToList(),
        };
        return View(model);
    }

    /// <summary>Cambia el rol entre Registrado y Moderador (UsuarioUseCase.actualizarRol).</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = UserRoles.Administrador)]
    public async Task<IActionResult> CambiarRol(int id, Rol rol, string? q)
    {
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var entity = await unitOfWork.UsuarioRepository.ReadAsync(id);
        if (entity is null) return NotFound();
        var nombre = entity.Name;

        try
        {
            // Las reglas (no se modifica a un administrador; solo Registrado <-> Moderador) están en el caso de uso.
            var useCase = new global::WikiGames.Application.UseCases.Usuario.UsuarioUseCase(unitOfWork);
            await useCase.ActualizarRolAsync(id, rol);
        }
        catch (ModelException error)
        {
            _logger.LogWarning(error, "Could not change the role of Usuario {Id}.", id);
            TempData["Toast"] = error.InnerException is ModelException regla ? regla.Message : $"No se ha podido cambiar el rol de {nombre}";
            return RedirectToAction(nameof(Lista), new { q });
        }

        TempData["Toast"] = $"{nombre} ahora es {(rol == Rol.Moderador ? "moderador" : "usuario normal")}";
        return RedirectToAction(nameof(Lista), new { q });
    }

    /// <summary>Elimina un usuario (UsuarioUseCase.destroy). Los administradores no se pueden eliminar aquí.</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = UserRoles.Administrador)]
    public async Task<IActionResult> Eliminar(int id, string? q)
    {
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var entity = await unitOfWork.UsuarioRepository.ReadAsync(id);
        if (entity is null) return NotFound();
        var nombre = entity.Name;
        if (entity.RolUsuario == Rol.Administrador || entity.Id == WikiGamesPaginas.UsuarioActualId(User))
        {
            TempData["Toast"] = "No se puede eliminar a un administrador";
            return RedirectToAction(nameof(Lista), new { q });
        }

        try
        {
            var useCase = new global::WikiGames.Application.UseCases.Usuario.UsuarioUseCase(unitOfWork);
            await useCase.DestroyAsync(id);
        }
        catch (ModelException error)
        {
            _logger.LogError(error, "Could not delete Usuario {Id}.", id);
            TempData["Toast"] = $"No se ha podido eliminar a «{nombre}»";
            return RedirectToAction(nameof(Lista), new { q });
        }

        TempData["Toast"] = $"Usuario «{nombre}» eliminado";
        return RedirectToAction(nameof(Lista), new { q });
    }

    // =====================================================================
    // Página WikiGames "Ajustes". Usuario autenticado: cambia su propia contraseña.
    // Acciones enteramente manuales: HARIA las conserva al regenerar.
    // =====================================================================

    [HttpGet]
    public IActionResult Ajustes()
    {
        if (WikiGamesPaginas.UsuarioActualId(User) is null) return Challenge();
        return View(new AjustesViewModel());
    }

    /// <summary>Cambia la contraseña del usuario actual (Usuario.modify) tras verificar la actual.</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ajustes(AjustesViewModel model)
    {
        var usuarioId = WikiGamesPaginas.UsuarioActualId(User);
        if (usuarioId is null) return Challenge();

        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var entity = await unitOfWork.UsuarioRepository.ReadAsync(usuarioId.Value);
        if (entity is null) return Challenge();

        if (ModelState.IsValid && !unitOfWork.PasswordHasher.VerifyPassword(entity.Password, model.PasswordActual))
            ModelState.AddModelError(nameof(model.PasswordActual), "La contraseña actual no es correcta.");
        if (ModelState.IsValid && unitOfWork.PasswordHasher.VerifyPassword(entity.Password, model.PasswordNueva))
            ModelState.AddModelError(nameof(model.PasswordNueva), "La nueva contraseña debe ser distinta de la actual.");
        if (!ModelState.IsValid) return View(model); // los <input type="password"> nunca reenvían su valor

        try
        {
            await unitOfWork.BeginTransactionAsync("MVC.Usuario.Ajustes");
            // Se hashea aquí para que un texto con forma de hash nunca se guarde tal cual.
            await new UsuarioService(unitOfWork).ModifyAsync(entity.Id, entity.Name, entity.Email,
                unitOfWork.PasswordHasher.HashPassword(model.PasswordNueva), entity.RolUsuario);
            await unitOfWork.CommitAsync();
        }
        catch (Exception error) when (error is not OperationCanceledException)
        {
            await unitOfWork.RollbackAsync();
            _logger.LogError(error, "Could not change the password of Usuario {Id}.", entity.Id);
            ModelState.AddModelError(string.Empty, "No se ha podido cambiar la contraseña. Inténtalo de nuevo.");
            return View(model); // los <input type="password"> nunca reenvían su valor
        }

        TempData["Toast"] = "Contraseña actualizada";
        return RedirectToAction(nameof(Ajustes));
    }
}
