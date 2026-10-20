using System.Text.Json.Serialization;
using CleanTodo.Domain.Entities;

namespace CleanTodo.Domain.DTOS
{
    public class BrokerShipDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("goldCargo")]
        public decimal GoldCargo { get; set; }

        [JsonPropertyName("captain")]
        public string Captain { get; set; } = string.Empty;

        [JsonPropertyName("crewSize")]
        public int CrewSize { get; set; }

        [JsonPropertyName("createdBy")]
        public string CreatedBy { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonPropertyName("lastModified")]
        public DateTimeOffset LastModified { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        public BrokerShipDto() { }

        public BrokerShipDto(Ship ship)
        {
            Name = ship.Name;
            GoldCargo = ship.GoldCargo;
            Captain = ship.Captain;
            CrewSize = ship.CrewSize;
            CreatedBy = ship.CreatedBy;
            CreatedAt = ship.CreatedAt;
            LastModified = ship.LastModified;
            Status = "sailing";
        }
    }
}