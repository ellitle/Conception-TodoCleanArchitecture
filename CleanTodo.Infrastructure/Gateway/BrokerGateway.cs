using System.Net.Http.Json;
using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Interfaces.Gateways;
using CleanTodo.Infrastructure.Configuration;

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

    public async Task<List<PortDto>> GetPorts()
    {
        using var response = await _httpClient.GetAsync("ports");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<PortDto>>() ?? throw new InvalidOperationException("Le broker a retourné une réponse vide.");
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