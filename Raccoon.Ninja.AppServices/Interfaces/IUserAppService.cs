using Raccoon.Ninja.Domain.Models;

namespace Raccoon.Ninja.AppServices.Interfaces;

public interface IUserAppService
{
    IList<UserModel> Get();
    void PopulateDevDb(int? quantity, int? toDeactivate);
}