namespace CleanTodo.Domain.Entities;

public class Ship
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Captain { get; set; } = string.Empty;
    public decimal GoldCargo { get; set; }
    public int CrewSize { get; set; }
    public string Status { get; set; } = "docked";
    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    public Ship(string name, string captain, decimal goldCargo, int crewSize, string createdBy)
    {
        var now = DateTime.UtcNow;

        Id = Guid.NewGuid();
        Name = name.Trim();
        Captain = captain.Trim();
        GoldCargo = goldCargo;
        CrewSize = crewSize;
        CreatedBy = createdBy;
        Status = "docked";
        CreatedAt = now;
        LastModified = now;
    }

    public Ship() { }
}