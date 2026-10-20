using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Enums;

namespace CleanTodo.Domain.DTOS;

public class ShipTransferDto
{
    public Guid Id { get; set; }
    public Guid ShipId { get; set; }
    public string Destination { get; set; } = string.Empty;
    public ShipStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public ShipTransferDto() { }

    public ShipTransferDto(ShipTransfer transfer)
    {
        Id = transfer.Id;
        ShipId = transfer.ShipId;
        Destination = transfer.Destination;
        Status = transfer.Status;
        CreatedAt = transfer.CreatedAt;
        CompletedAt = transfer.CompletedAt;
    }
}
