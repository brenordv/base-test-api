using Microsoft.AspNetCore.Mvc;
using Raccoon.Ninja.AppServices.Interfaces;

namespace Raccoon.Ninja.Application.OldStyleWebApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class DevController : ControllerBase
{
    private readonly IProductsAppService _productsAppService;
    private readonly IUserAppService _userAppService;

    public DevController(IProductsAppService productsAppService, IUserAppService userAppService)
    {
        _productsAppService = productsAppService;
        _userAppService = userAppService;
    }

    [HttpGet("populate-users-db", Name = "Populate Users database")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult PopulateUsersDb([FromQuery] int? quantity, [FromQuery] int? archive)
    {
        _userAppService.PopulateDevDb(quantity, archive);
        return Ok();
    }

    [HttpGet("populate-products-db", Name = "Populate Products database")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult PopulateProductsDb([FromQuery] int? quantity, [FromQuery] int? archive)
    {
        _productsAppService.PopulateDevDb(quantity, archive);
        return Ok();
    }

    [HttpGet("info", Name = "Just a test route.")]
    [ProducesResponseType(typeof(DateTime), StatusCodes.Status200OK)]
    public IActionResult TestRoute()
    {
        return Ok(DateTime.Now);
    }
}