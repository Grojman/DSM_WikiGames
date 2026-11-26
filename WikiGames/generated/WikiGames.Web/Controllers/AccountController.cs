// HARIA-WEB GENERATED sha256:0e6ae0e6f7a70e00c4f3c84c563e71b37a2ba207b600fce0217c5e606b5ee1d5
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WikiGames.Web.Assemblers;
using WikiGames.Web.Utils.Security;
using WikiGames.Web.ViewModels;

namespace WikiGames.Web.Controllers;

public sealed class AccountController : Controller
{
    private readonly WikiGames.Application.Services.UsuarioService _userService;
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _userRepository;

    public AccountController(WikiGames.Application.Services.UsuarioService userService,
        WikiGames.Application.Abstractions.IUnitOfWorkFactory userRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login(string? returnUrl = null) =>
        View(new LoginViewModel { ReturnUrl = returnUrl });

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Usuario.login customized: el identificador es el nombre o el email.
        var rawIdentifier = model.Identifier ?? string.Empty;
        var token = await _userService.LoginAsync(rawIdentifier, model.Password);
        if (string.IsNullOrWhiteSpace(token))
        {
            ModelState.AddModelError(string.Empty, "Credenciales incorrectas.");
            return View(model);
        }

        var entity = await _userService.FindByNombreOEmailAsync(rawIdentifier);
        if (entity is null)
        {
            ModelState.AddModelError(string.Empty, "Credenciales incorrectas.");
            return View(model);
        }
        var user = UsuarioAssembler.ToViewModel(entity);

        await HariaWebAuthentication.SignInAsync(HttpContext, "Usuario", entity.Id, entity);
        HttpContext.Session.SetUser(user);

        return Url.IsLocalUrl(model.ReturnUrl)
            ? LocalRedirect(model.ReturnUrl!)
            : RedirectToAction("Index", "Home");
    }
   
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.ClearClasses();
        return RedirectToAction("Login");
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult AccessDenied() => View();
}
