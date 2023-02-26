using Microsoft.AspNetCore.Mvc;
using Raccoon.Ninja.AppServices.Interfaces;

namespace Raccoon.Ninja.Application.MinimalApi.Endpoints;

public static class UserEndpoints
{
    public static IResult GetUsers(IUserAppService userAppService, [FromQuery]int? limit)
    {
        var users = userAppService.Get(limit ?? 42);
        return users is not null && users.Any()
            ? Results.Ok(users)
            : Results.NoContent();
    }
}