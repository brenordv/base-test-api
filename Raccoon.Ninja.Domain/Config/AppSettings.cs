using Newtonsoft.Json;

namespace Raccoon.Ninja.Domain.Config;

public class AppSettings
{
    [JsonProperty("testDbFolder")] public string TestDbFolder { get; set; }
}