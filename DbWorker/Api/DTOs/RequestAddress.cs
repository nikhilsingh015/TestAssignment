using System.Text.Json.Serialization;

namespace Api.DTOs;

public class RequestAddress
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("street")]
    public string? Street { get; set; }

    [JsonPropertyName("city")]
    public required string City { get; set; }

    [JsonPropertyName("country")]
    public required string Country { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("zipcode")]
    public string? ZipCode { get; set; }
    [JsonPropertyName("email")]
    public required string Email { get; set; }
    [JsonPropertyName("phonenumber")]
    public string? PhoneNumber { get; set; }  // nullable since it's optional

}
