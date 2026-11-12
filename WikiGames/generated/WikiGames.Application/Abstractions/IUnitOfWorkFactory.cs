namespace WikiGames.Application.Abstractions;

public interface IUnitOfWorkFactory
{
    IUnitOfWork Create();
}