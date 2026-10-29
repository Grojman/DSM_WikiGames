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
    public async Task<Categoria> NewCustomizedAsync(string p_name, int p_videojuego_oid)
    {
        /*PROTECTED REGION ID(WikiGames.Application.Services_Categoria_newCustomized) ENABLED START*/
        Videojuego videojuegoEntity = await _unitOfWork.VideojuegoRepository.ReadAsync(p_videojuego_oid)
            ?? throw new ModelException("Videojuego not found for Categoria.Videojuego.");

        Categoria entity = new Categoria
        {
            Name = p_name,
            Videojuego = videojuegoEntity,
        };

        await _repository.SaveAsync(entity);
        return entity;
        /*PROTECTED REGION END*/
    }
}
