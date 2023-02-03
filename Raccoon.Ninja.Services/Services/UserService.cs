using MethodTimer;
using Raccoon.Ninja.Domain.Entities;
using Raccoon.Ninja.Domain.Interfaces.Repositories;
using Raccoon.Ninja.Domain.Interfaces.Services;
using Raccoon.Ninja.Test.Helpers.Generators;
using Raccoon.Ninja.Test.Helpers.Helpers;

namespace Raccoon.Ninja.Services.Services;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    
    [Time("Fetching first {limit} users")]
    public IList<User> Get(int limit = 42)
    {
       return _userRepository
           .Get()
           .Take(limit)
           .ToList();
    }

    public void PopulateDevDb(int? quantity, int? toDeactivate)
    {
        var qty = quantity ?? 100;
        var qtyToDeactivate = toDeactivate ?? 10;
        
        var inserted = new List<User>();
        foreach (var user in UserGenerator.Generate(qty, false))
        {
            var insertedUser = _userRepository.Add(user);
            inserted.Add(insertedUser);
            Console.WriteLine($"Inserted: {insertedUser}");
        }

        for (var i = 0; i < qtyToDeactivate; i++)
        {
            var userToDeactivate = inserted.RandomPick();
            userToDeactivate = userToDeactivate with { IsActive = false };
            
            _userRepository.Update(userToDeactivate);
        }
    }
}