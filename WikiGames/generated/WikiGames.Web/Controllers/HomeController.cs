using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WikiGames.Domain.Enums;
using WikiGames.Web.Assemblers;
using WikiGames.Web.Utils.Paginas;
using WikiGames.Web.ViewModels;
using WikiGames.Web.ViewModels.Paginas;

namespace WikiGames.Web.Controllers;

public sealed class HomeController : Controller
{
    private readonly WikiGames.Application.Abstractions.IUnitOfWorkFactory _unitOfWorkFactory;

    public HomeController(WikiGames.Application.Abstractions.IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    /// <summary>
    /// Página principal WikiGames (index.html): videojuegos aprobados con búsqueda por título,
    /// orden (popular / valorado / recientes) y filtro por categoría (incluye subcategorías). Pública.
    /// </summary>
    public async Task<IActionResult> Index(string? q, string? orden, int? categoria)
    {
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var categorias = await unitOfWork.CategoriaRepository.ReadAllAsync();
        var videojuegos = (await unitOfWork.VideojuegoRepository.ReadAllAsync())
            .Where(v => v.Estado == EstadoContenido.Aprobado);

        var filtro = q?.Trim();
        if (!string.IsNullOrEmpty(filtro))
            videojuegos = videojuegos.Where(v => v.Titulo.Contains(filtro, StringComparison.CurrentCultureIgnoreCase));

        if (categoria is int categoriaId && categorias.Any(c => c.Id == categoriaId))
        {
            var ids = WikiGamesPaginas.ConDescendientes(categorias, categoriaId);
            videojuegos = videojuegos.Where(v => v.Categorias.Any(c => ids.Contains(c.Id)));
        }
        else categoria = null;

        orden = orden is InicioViewModel.OrdenValorado or InicioViewModel.OrdenRecientes ? orden : InicioViewModel.OrdenPopular;
        videojuegos = orden switch
        {
            InicioViewModel.OrdenValorado => videojuegos.OrderByDescending(v => v.PuntuacionMedia),
            InicioViewModel.OrdenRecientes => videojuegos.OrderByDescending(v => v.FechaCreacion),
            _ => videojuegos.OrderByDescending(v => v.NumeroMeGusta),
        };

        return View("Inicio", new InicioViewModel
        {
            Q = filtro,
            Orden = orden,
            Categoria = categoria,
            Categorias = WikiGamesPaginas.ArbolCategorias(categorias),
            Videojuegos = videojuegos.Select(VideojuegoAssembler.ToViewModel).ToList(),
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
