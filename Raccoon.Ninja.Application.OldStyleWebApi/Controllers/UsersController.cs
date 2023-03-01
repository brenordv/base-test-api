using Microsoft.AspNetCore.Mvc;
using Raccoon.Ninja.AppServices.Interfaces;
using Raccoon.Ninja.Domain.Models;

namespace Raccoon.Ninja.Application.OldStyleWebApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController: ControllerBase
{
    private readonly IUserAppService _userAppService;

    public UsersController(IUserAppService userAppService)
    {
        _userAppService = userAppService;
    }

    [HttpGet(Name = "Get all users")]
    [ProducesResponseType(typeof(List<UserModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Get([FromQuery] int? limit = null)
    {
        var users = _userAppService.Get(limit ?? 42);
        return users.Any()
            ? Ok(users)
            : NoContent();
    }
}