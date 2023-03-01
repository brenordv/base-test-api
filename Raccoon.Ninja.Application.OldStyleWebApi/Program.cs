using Raccoon.Ninja.Infra.DI.Extensions.Web.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterCommon();
builder.Configuration.AddCommon(builder.Services);
builder.Services.AddControllers();

var app = builder.Build();

app.CommonConfig();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();