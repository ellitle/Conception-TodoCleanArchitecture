using CleanTodo.Domain.Enums;

namespace CleanTodo.Domain.Entities;

public class ShipTransfer
{
    public Guid Id { get; set; }
    public Guid ShipId { get; set; }
    public string Destination { get; set; } = string.Empty;
    public ShipStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public ShipTransfer(Guid shipId, string destination)
    {
        Id = Guid.NewGuid();
        ShipId = shipId;
        Destination = destination;
        Status = ShipStatus.Sailing;
        CreatedAt = DateTime.UtcNow;
        CompletedAt = null;
    }

    public ShipTransfer() { }
}