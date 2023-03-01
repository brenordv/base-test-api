using Raccoon.Ninja.Domain.Models;

namespace Raccoon.Ninja.Application.MinimalApi.Endpoints;

public static class GlobalEndpointMapper
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        // Just to organize things a little, we'll define the API endpoint prefixes here.
        const string apiProductsEndpoint = "/api/products";
        const string apiUsersEndpoint = "/api/users";

        // API Routes
        app.MapGet(apiProductsEndpoint, ProductEndpoints.GetProducts)
            .Produces<IList<ProductModel>>()
            .Produces(StatusCodes.Status204NoContent);

        app.MapGet($"{apiProductsEndpoint}/{{id:Guid}}", ProductEndpoints.GetProductById)
            .Produces<ProductModel>()
            .Produces(StatusCodes.Status404NotFound);

        app.MapPost(apiProductsEndpoint, ProductEndpoints.CreateProduct)
            .Produces<ProductModel>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        app.MapPut(apiProductsEndpoint, ProductEndpoints.UpdateProduct)
            .Produces<ProductModel>()
            .Produces(StatusCodes.Status404NotFound);

        app.MapGet("/api/dev/populate-products-db", DevEndpoints.PopulateProducts);

        app.MapGet(apiUsersEndpoint, UserEndpoints.GetUsers)
            .Produces<IList<UserModel>>()
            .Produces(StatusCodes.Status204NoContent);

        app.MapGet("/api/dev/populate-users-db", DevEndpoints.PopulateUsers);

        app.MapGet("/api/dev/info", DevEndpoints.TestRoute);

        return app;
    }
}