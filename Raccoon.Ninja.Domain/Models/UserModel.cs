using Newtonsoft.Json;
using Raccoon.Ninja.Domain.Enums;

namespace Raccoon.Ninja.Domain.Models;

public class UserModel
{
    [JsonProperty("id")]public Guid Id { get; set; }
    [JsonProperty("firstName")]public string FirstName { get; set; }
    [JsonProperty("lastName")]public string LastName { get; set; }
    [JsonProperty("email")]public string Email { get; set; }
    [JsonProperty("mobile")]public string Mobile { get; set; }
    [JsonProperty("role")]public UserType Role { get; set; }
    [JsonProperty("createdAt")]public DateTime CreatedAt { get; set; }
    [JsonProperty("modifiedAt")]public DateTime UpdatedAt { get; set; }
    [JsonProperty("isActive")]public bool IsActive { get; set; }
    
    [JsonProperty("fullName")] public string FullName => $"{FirstName} {LastName}";
}