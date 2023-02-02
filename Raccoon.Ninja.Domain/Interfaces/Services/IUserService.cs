using Raccoon.Ninja.Domain.Entities;

namespace Raccoon.Ninja.Domain.Interfaces.Services;

public interface IUserService
{
    IList<User> Get();
    void PopulateDevDb(int? quantity, int? toDeactivate);
}