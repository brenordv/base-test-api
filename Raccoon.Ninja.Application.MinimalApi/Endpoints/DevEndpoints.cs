using Microsoft.AspNetCore.Mvc;
using Raccoon.Ninja.AppServices.Interfaces;

namespace Raccoon.Ninja.Application.MinimalApi.Endpoints;

public static class DevEndpoints
{
    public static IResult PopulateProducts(IProductsAppService productsAppService, [FromQuery] int? quantity,
        [FromQuery] int? archive)
    {
        // None of this should be here, but it's just a test.        
        productsAppService.PopulateDevDb(quantity, archive);
        
        return Results.Ok();
    }
    
    public static IResult PopulateUsers(IUserAppService usersAppService, [FromQuery] int? quantity, 
        [FromQuery] int? archive)
    {
        // None of this should be here, but it's just a test.        
        usersAppService.PopulateDevDb(quantity, archive);
        
        return Results.Ok();
    }
}