using Raccoon.Ninja.Domain.Entities;

namespace Raccoon.Ninja.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    IList<User> Get();
    User Add(User newUser);
    User Update(User user);
}