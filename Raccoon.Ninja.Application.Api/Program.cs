using Microsoft.AspNetCore.Mvc;
using Raccoon.Ninja.AppServices.AppServices;
using Raccoon.Ninja.AppServices.Helpers;
using Raccoon.Ninja.AppServices.Interfaces;
using Raccoon.Ninja.Domain.Config;
using Raccoon.Ninja.Domain.Models;
using Raccoon.Ninja.Infra.DI.Extensions;
using Raccoon.Ninja.Infra.DI.Helpers;

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
    .AddDecoratedWithStopWatch<IProductsAppService, ProductsAppService>(ServiceLifetime.Scoped);

// Registering the actual Services, Repositories and auxiliary engines.
builder.Services
    .RegisterRepositories()
    .RegisterServices()
    .RegisterAuxiliary();

var app = builder.Build();

// Adding swagger when running in dev.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

const string apiEndpoint = "/api/products";

// API Routes
app.MapGet("/",
    () => "Hello World!");

app.MapGet(apiEndpoint,
    (IProductsAppService productsAppService) => productsAppService.Get());

app.MapGet($"{apiEndpoint}/{{id:Guid}}",
    (IProductsAppService productsAppService, Guid id) => productsAppService.Get(id));

app.MapPost(apiEndpoint,
    (IProductsAppService productsAppService, AddProductModel newProd) => productsAppService.Add(newProd));

app.MapPut(apiEndpoint,
    (IProductsAppService productsAppService, UpdateProductModel prod) => productsAppService.Update(prod));

#if DEBUG
app.MapGet("/dev/populate-db",
    (IProductsAppService productsAppService, [FromQuery] int? quantity, [FromQuery] int? archive) =>
    {
        // None of this should be here, but it's just a test.        
        productsAppService.PopulateDevDb(quantity, archive);
        return "ok";
    });
#endif

// Running API
app.Run();