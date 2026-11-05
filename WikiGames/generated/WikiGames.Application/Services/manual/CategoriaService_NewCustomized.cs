using WikiGames.Application.Abstractions;
using WikiGames.Application.Exceptions;
using WikiGames.Domain.Entities;
using WikiGames.Domain.Enums;

        /*PROTECTED REGION ID(usingWikiGames.Application.Services_Categoria_newCustomized) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace WikiGames.Application.Services;

public sealed partial class CategoriaService
{
    public async Task<Categoria> NewCustomizedAsync(string p_name)
    {
        var c = await _repository.FindByNameLinQAsync(p_name);

        if (c is not null)
        {
            throw new ModelException($"Category with name {p_name} already exists.");
        }

        Categoria entity = new Categoria
        {
            Name = p_name,
        };

        await _repository.SaveAsync(entity);
        return entity;
        /*PROTECTED REGION END*/
    }
}
