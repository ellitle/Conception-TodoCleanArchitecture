using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Enums;

namespace CleanTodo.Domain.DTOS
{
    public class ShipDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal GoldCargo { get; set; }
        public string Captain { get; set; } = string.Empty;
        public int CrewSize { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public ShipStatus Status { get; set; }

        public ShipDto() { }

        public ShipDto(Ship ship)
        {
            Id = ship.Id;
            Name = ship.Name;
            GoldCargo = ship.GoldCargo;
            Captain = ship.Captain;
            CrewSize = ship.CrewSize;
            CreatedBy = ship.CreatedBy;
            CreatedAt = ship.CreatedAt;
            LastModified = ship.LastModified;
            Status = ship.Status;
        }
    }
}