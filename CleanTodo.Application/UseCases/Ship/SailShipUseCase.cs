using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Exceptions;
using CleanTodo.Domain.Interfaces.Gateways;
using CleanTodo.Domain.Interfaces.Repositories;

namespace CleanTodo.Application.UseCase;

public class SailShipUseCase
{
    private readonly IShipRepository _shipRepository;
    private readonly IBrokerGateway _brokerGateway;

    public SailShipUseCase(IShipRepository shipRepository, IBrokerGateway brokerGateway)
    {
        _shipRepository = shipRepository;
        _brokerGateway = brokerGateway;
    }

    public async Task<ShipDto> Execute(Guid id, SailShipDto sailShipDto)
    {
        if (string.IsNullOrWhiteSpace(sailShipDto.Destination) || sailShipDto.Destination.Length > 200)
        {
            throw new ShipOperationException(ShipErrorCode.InvalidDestination);
        }

        List<string> ports;
        try
        {
            ports = await _brokerGateway.GetPorts();
        }
        catch (HttpRequestException)
        {
            throw new ShipOperationException(ShipErrorCode.BrokerUnavailable);
        }
        catch (OperationCanceledException)
        {
            throw new ShipOperationException(ShipErrorCode.BrokerUnavailable);
        }

        bool destinationExists = ports.Contains(sailShipDto.Destination);

        if (!destinationExists)
        {
            throw new ShipOperationException(ShipErrorCode.InvalidDestination);
        }

        var ship = await _shipRepository.BeginSail(id);
        var brokerShipDto = new BrokerShipDto(ship);

        DeliveryOutcome outcome;

        try
        {
            outcome = await _brokerGateway.Sail(sailShipDto.Destination, brokerShipDto);
        }
        catch (HttpRequestException)
        {
            outcome = DeliveryOutcome.Unknown;
        }
        catch (OperationCanceledException)
        {
            outcome = DeliveryOutcome.Unknown;
        }

        if (outcome == DeliveryOutcome.Rejected)
        {
            ship.Status = "docked";
            ship.LastModified = DateTime.UtcNow;
            await _shipRepository.Update(ship);
            throw new ShipOperationException(ShipErrorCode.BrokerRejected);
        }

        if (outcome != DeliveryOutcome.Confirmed)
        {
            // Keep the ship sailing when delivery is uncertain to prevent a duplicate send.
            throw new ShipOperationException(ShipErrorCode.DeliveryUnknown);
        }

        await _shipRepository.Delete(ship);
        return new ShipDto(ship);
    }
}
