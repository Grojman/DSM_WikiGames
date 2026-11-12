using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Application.Services;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.UseCases_Categoria_crearSubcategoria) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.UseCases.Categoria;

public sealed partial class CategoriaUseCase
{
    public async Task<global::WikiGames.Domain.Entities.Categoria> CrearSubcategoriaAsync(int p_padre_oid, string p_name)
    {
        /*PROTECTED REGION ID(WikiGames.Application.UseCases_Categoria_crearSubcategoria) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        CategoriaService categoriaService = null!;
        global::WikiGames.Domain.Entities.Categoria result = default(global::WikiGames.Domain.Entities.Categoria)!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("CategoriaUseCase.CrearSubcategoriaAsync");
            categoriaService = new CategoriaService(_unitOfWork);

            if (string.IsNullOrWhiteSpace(p_name))
            {
                throw new ModelException("La categoría debe tener un nombre.");
            }

            global::WikiGames.Domain.Entities.Categoria padre = await _unitOfWork.CategoriaRepository.ReadAsync(p_padre_oid)
                ?? throw new ModelException($"No existe la categoría padre {p_padre_oid}.");

            // No puede haber dos hijas del mismo padre con el mismo nombre.
            bool nombreRepetido = padre.Hijos.Any(h => string.Equals(h.Name.Trim(), p_name.Trim(), StringComparison.OrdinalIgnoreCase));
            if (nombreRepetido)
            {
                throw new ModelException($"La categoría \"{padre.Name}\" ya tiene una subcategoría llamada \"{p_name}\".");
            }

            global::WikiGames.Domain.Entities.Categoria nueva = await categoriaService.NewAsync(p_name);

            // Relationer Categoria.addPadre: la nueva categoría pasa a ser hija de p_padre_oid.
            await categoriaService.AddPadreAsync(nueva.Id, padre.Id);
            result = nueva;

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Categoria.crearSubcategoria.", ex);
        }

        return result;
        /*PROTECTED REGION END*/
    }
}
