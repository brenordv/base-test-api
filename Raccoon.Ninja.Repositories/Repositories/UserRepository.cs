using LiteDB;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Raccoon.Ninja.Domain.Config;
using Raccoon.Ninja.Domain.Entities;
using Raccoon.Ninja.Domain.Interfaces.Repositories;
using Raccoon.Ninja.Repositories.DbHelpers;

namespace Raccoon.Ninja.Repositories.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly ILiteCollection<User> _collection;

    public UserRepository(ILogger<UserRepository> logger, IOptions<AppSettings> appSettings)
    {
        var database = new LiteDatabase(DbFileHelper.FromFile(appSettings.Value.TestDbFolder));
        _collection = database.GetCollection<User>();
        _logger = logger;
    }

    public IList<User> Get()
    {
        var users = _collection
            .Query()
            .OrderBy(user => user.CreatedAt)
            .ToList();

        _logger.LogTrace("Returning '{UserCount}' from database", users.Count);

        return users;
    }

    public User Add(User newUser)
    {
        var preparedProduct = newUser with
        {
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        var insertedId = (Guid)_collection.Insert(preparedProduct);

        _logger.LogTrace("New user to the database: {FirstName} {LastName} ({Id})",
            newUser.FirstName,
            newUser.FirstName, insertedId);

        return preparedProduct with
        {
            Id = insertedId
        };
    }

    public User Update(User user)
    {
        var preparedUser = user with
        {
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Version = user.Version + 1
        };

        if (_collection.Update(preparedUser))
            _logger.LogTrace("User updated successfully! Id: {Id}", user.Id);
        else
            _logger.LogTrace("Failed to update user! Id: {Id}", user.Id);

        return preparedUser;
    }
}