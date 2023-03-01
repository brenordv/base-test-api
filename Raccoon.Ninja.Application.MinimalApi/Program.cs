using Raccoon.Ninja.Application.MinimalApi.Endpoints;
using Raccoon.Ninja.Infra.DI.Extensions.Web.Common.Extensions;

// Creating the app builder.
var builder = WebApplication.CreateBuilder(args);

// Moved a lot of the common stuff to extensions.
builder.Services.RegisterCommon();
builder.Configuration.AddCommon(builder.Services);

// Building the app.
var app = builder.Build();

// Moved a lot of the common stuff to extensions, including the route mapping.
app.CommonConfig();
app.MapEndpoints();

// Running API
app.Run();


// ReSharper disable once ClassNeverInstantiated.Global
#pragma warning disable CA1050
public partial class Program
#pragma warning restore CA1050
{
    protected Program()
    {
    }
}