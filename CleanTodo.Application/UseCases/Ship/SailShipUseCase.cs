using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Exceptions;
using CleanTodo.Domain.Interfaces.Gateways;
using CleanTodo.Domain.Interfaces.Repositories;

namespace CleanTodo.Application.UseCase;

public class SailShipUseCase
{
    private readonly IShipTransferRepository _shipTransferRepository;
    private readonly IBrokerGateway _brokerGateway;

    public SailShipUseCase(IShipTransferRepository shipTransferRepository, IBrokerGateway brokerGateway)
    {
        _shipTransferRepository = shipTransferRepository;
        _brokerGateway = brokerGateway;
    }

    public async Task<ShipTransferDto> Execute( Guid id, SailShipDto sailShipDto)
    {
        if (string.IsNullOrWhiteSpace(sailShipDto.Destination)|| sailShipDto.Destination.Length > 200)
        {
            throw new ShipOperationException(ShipErrorCode.InvalidDestination);
        }

        var ports = await _brokerGateway.GetPorts();

        bool destinationExists = ports.Contains(sailShipDto.Destination);

        if (!destinationExists)
        {
            throw new ShipOperationException(ShipErrorCode.InvalidDestination);
        }

        var startedTransfer = await _shipTransferRepository.BeginTransfer(id, sailShipDto.Destination);

        BrokerShipDto brokerShipDto = new BrokerShipDto(startedTransfer.Ship);

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

        var completedTransfer = await _shipTransferRepository.FinishTransfer(startedTransfer.Transfer.Id, outcome);

        return new ShipTransferDto(completedTransfer);
    }
}
