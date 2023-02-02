using Raccoon.Ninja.Domain.Models;

namespace Raccoon.Ninja.AppServices.Interfaces;

public interface IUserAppService
{
    IList<UserModel> Get(int limit = 42);
    void PopulateDevDb(int? quantity, int? toDeactivate);
}