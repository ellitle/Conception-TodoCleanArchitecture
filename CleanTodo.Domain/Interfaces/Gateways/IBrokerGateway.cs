using CleanTodo.Domain.DTOS;
namespace CleanTodo.Domain.Interfaces.Gateways;

public enum DeliveryOutcome { Confirmed, Rejected, Unknown }

public interface IBrokerGateway
{
    string CreatorId { get; }
    Task<List<PortDto>> GetPorts();
    Task<DeliveryOutcome> Sail(string destination, BrokerShipDto ship);
}
