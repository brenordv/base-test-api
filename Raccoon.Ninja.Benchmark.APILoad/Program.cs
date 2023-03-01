using NBomber.CSharp;
using NBomber.Http.CSharp;

using var httpClientMinApi = new HttpClient();
using var httpClientWebApi = new HttpClient();

var testLoad = Simulation.Inject(rate: 24,
    interval: TimeSpan.FromMilliseconds(50),
    during: TimeSpan.FromSeconds(60));

var scenarioMinApi = Scenario.Create("Minimal API", async context =>
    {
        // "https://localhost:7105/api/products/2c3b852c-5b84-4369-b64f-7468a9c06061"
        var request =
            Http.CreateRequest("GET", "https://localhost:7105/api/dev/info")
                .WithHeader("Accept", "application/json");

        var response = await Http.Send(httpClientMinApi, request);
        return response;
    })
    .WithLoadSimulations(testLoad);

var scenarioWebApi = Scenario.Create("Web API", async context =>
    {
        //https://localhost:7290/api/products/068c7939-e989-4589-87c3-4bac5b973e0b
        var request =
            Http.CreateRequest("GET", "https://localhost:7290/api/dev/info")
                .WithHeader("Accept", "application/json");

        var response = await Http.Send(httpClientWebApi, request);
        return response;
    })
    .WithLoadSimulations(testLoad);

NBomberRunner
    .RegisterScenarios(new [] {scenarioMinApi, scenarioWebApi})
    .Run();