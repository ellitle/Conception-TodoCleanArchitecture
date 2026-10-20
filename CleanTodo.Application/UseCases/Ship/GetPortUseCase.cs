using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Interfaces.Gateways;

namespace CleanTodo.Application.UseCase;

public class GetPortUseCase
{
    private readonly IBrokerGateway _brokerGateway;

    public GetPortUseCase(IBrokerGateway brokerGateway)
    {
        _brokerGateway = brokerGateway;
    }

    public async Task<List<PortDto>> Execute()
    {
        return await _brokerGateway.GetPorts();
    }
}