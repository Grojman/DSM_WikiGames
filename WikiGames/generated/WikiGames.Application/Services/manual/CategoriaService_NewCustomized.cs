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
        /*PROTECTED REGION ID(WikiGames.Application.Services_Categoria_newCustomized) ENABLED START*/
        Categoria entity = new Categoria
        {
            Name = p_name,
        };

        await _repository.SaveAsync(entity);
        return entity;
        /*PROTECTED REGION END*/
    }
}
