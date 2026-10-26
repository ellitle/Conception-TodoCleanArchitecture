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

    public Task<DeliveryOutcome> Sail(
        string destination,
        BrokerShipDto ship)
    {
        // A finir plus tard avec la route et le contrat du broker.
        throw new NotImplementedException(
            "Le contrat HTTP de Sail doit être confirmé.");
    }
}