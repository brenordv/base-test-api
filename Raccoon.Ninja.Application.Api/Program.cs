using Microsoft.AspNetCore.Mvc;
using Raccoon.Ninja.AppServices.AppServices;
using Raccoon.Ninja.AppServices.Helpers;
using Raccoon.Ninja.AppServices.Interfaces;
using Raccoon.Ninja.Domain.Config;
using Raccoon.Ninja.Domain.Models;
using Raccoon.Ninja.Infra.DI.Extensions;
using Raccoon.Ninja.Infra.DI.Helpers;
using Raccoon.Ninja.Services.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Basic API stuff.
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddMemoryCache()
    .AddLogging();

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

var app = builder.Build();

// Adding MethodTimer to log execution time of methods.
// Since it's a static class, we need to set the logger instance.
MethodTimeLogger.Logger = app.Logger;

// Adding swagger when running in dev.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

const string apiProductsEndpoint = "/api/products";
const string apiUsersEndpoint = "/api/users";

// API Routes
app.MapGet("/",
    () => "Hello World!");

app.MapGet(apiProductsEndpoint,
    (IProductsAppService productsAppService) => productsAppService.Get());

app.MapGet($"{apiProductsEndpoint}/{{id:Guid}}",
    (IProductsAppService productsAppService, Guid id) => productsAppService.Get(id));

app.MapPost(apiProductsEndpoint,
    (IProductsAppService productsAppService, AddProductModel newProd) => productsAppService.Add(newProd));

app.MapPut(apiProductsEndpoint,
    (IProductsAppService productsAppService, UpdateProductModel prod) => productsAppService.Update(prod));


app.MapGet($"{apiUsersEndpoint}/{{limit:int}}",
    (IUserAppService usersAppService, int? limit) => usersAppService.Get(limit ?? 42));

#if DEBUG
app.MapGet($"{apiProductsEndpoint}/dev/populate-db",
    (IProductsAppService productsAppService, [FromQuery] int? quantity, [FromQuery] int? archive) =>
    {
        // None of this should be here, but it's just a test.        
        productsAppService.PopulateDevDb(quantity, archive);
        return "ok";
    });


app.MapGet($"{apiUsersEndpoint}/dev/populate-db",
    (IUserAppService usersAppService, [FromQuery] int? quantity, [FromQuery] int? archive) =>
    {
        // None of this should be here, but it's just a test.        
        usersAppService.PopulateDevDb(quantity, archive);
        return "ok";
    });
#endif

// Running API
app.Run();