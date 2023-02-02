using Raccoon.Ninja.Domain.Entities;

namespace Raccoon.Ninja.Domain.Interfaces.Services;

public interface IUserService
{
    IList<User> Get(int limit = 42);
    void PopulateDevDb(int? quantity, int? toDeactivate);
}