using Raccoon.Ninja.Application.MinimalApi.Endpoints;
using Raccoon.Ninja.AppServices.AppServices;
using Raccoon.Ninja.AppServices.Helpers;
using Raccoon.Ninja.AppServices.Interfaces;
using Raccoon.Ninja.Domain.Config;
using Raccoon.Ninja.Domain.Models;
using Raccoon.Ninja.Infra.DI.Extensions;
using Raccoon.Ninja.Infra.DI.Helpers;
using Raccoon.Ninja.Services.Helpers;

// Creating the app builder.
var builder = WebApplication.CreateBuilder(args);

// Basic API configuration.
builder.Services
    .AddEndpointsApiExplorer()
    .AddMemoryCache()
    .AddLogging();

// Adding swagger when running in dev only.
#if DEBUG
builder.Services.AddSwaggerGen();
#endif

// Adding AppSettings data to environment variables
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: false)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false);

// Binding app settings data to Options.
builder.Services.AddOptions<AppSettings>().Bind(builder.Configuration.GetSection("AppConfig"));

// Registering API adapter (app service layer) specific stuff.
builder.Services
    .RegisterMappingProfiles()
    .AddDecoratedWithStopWatch<IProductsAppService, ProductsAppService>(ServiceLifetime.Scoped)
    .AddScoped<IUserAppService, UsersAppService>();

// Registering the actual Services, Repositories and auxiliary engines.
builder.Services
    .RegisterRepositories()
    .RegisterServices()
    .RegisterAuxiliary();

// Building the app.
var app = builder.Build();

// Adding MethodTimer to log execution time of methods.
// Since it's a static class, we need to set the logger instance.
MethodTimeLogger.Logger = app.Logger;

// Adding swagger when running in dev only.
#if DEBUG
app.UseSwagger();
app.UseSwaggerUI();
#endif

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

app.MapGet($"{apiProductsEndpoint}/dev/populate-db", DevEndpoints.PopulateProducts);

app.MapGet(apiUsersEndpoint, UserEndpoints.GetUsers)
    .Produces<IList<UserModel>>()
    .Produces(StatusCodes.Status204NoContent);

app.MapGet($"{apiUsersEndpoint}/dev/populate-db", DevEndpoints.PopulateUsers);


// Running API
app.Run();

// Added so we can run the app from the test project. 
// Going to ignore warnings for this class, since it has nothing to do with the actual app.
#pragma warning disable CA1050
// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program { }
#pragma warning restore CA1050
