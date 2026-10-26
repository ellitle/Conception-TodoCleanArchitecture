using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Interfaces.Gateways;
using CleanTodo.Infrastructure.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace CleanTodo.Infrastructure.Gateways;

public class BrokerGateway : IBrokerGateway
{
    private readonly HttpClient _httpClient;
    private readonly BrokerConfiguration _configuration;

    public BrokerGateway(HttpClient httpClient, BrokerConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public string CreatorId => _configuration.CreatorId;
    
    //Merci a Mathis pour l'aide 
    public async Task<List<string>> GetPorts()
    {
        var response = await _httpClient.GetAsync("users");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        return json.GetProperty("users").Deserialize<List<string>>()
            ?? new List<string>();
    }

   
    public async Task<DeliveryOutcome> Sail(string destination, BrokerShipDto ship)
    {
        var payload = new
        {
            name = ship.Name,
            captain = ship.Captain,
            goldCargo = ship.GoldCargo,
            crewSize = ship.CrewSize,
            createdBy = ship.CreatedBy,
            createdAt = ship.CreatedAt,
            lastModified = DateTime.UtcNow,
            status = "sailing"
        };

        var path = $"ship/sail/{Uri.EscapeDataString(destination)}";

        using var response = await _httpClient.PostAsJsonAsync(
            path,
            payload
        );

        if (response.IsSuccessStatusCode)
        {
            return DeliveryOutcome.Confirmed;
        }

        // A timeout or server error can occur after the broker has accepted the ship.
        var statusCode = (int)response.StatusCode;
        return statusCode >= 400 && statusCode < 500 && statusCode != 408
            ? DeliveryOutcome.Rejected
            : DeliveryOutcome.Unknown;
    }
}
