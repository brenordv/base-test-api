using AutoMapper;
using Raccoon.Ninja.AppServices.Interfaces;
using Raccoon.Ninja.Domain.Entities;
using Raccoon.Ninja.Domain.Interfaces.Services;
using Raccoon.Ninja.Domain.Models;

namespace Raccoon.Ninja.AppServices.AppServices;

public class UsersAppService : IUserAppService
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UsersAppService(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public IList<UserModel> Get(int limit = 42)
    {
        var users = _userService.Get(limit);
        return _mapper.Map<IList<User>, IList<UserModel>>(users);
    }

    public void PopulateDevDb(int? quantity, int? toDeactivate)
    {
        _userService.PopulateDevDb(quantity, toDeactivate);
    }
}