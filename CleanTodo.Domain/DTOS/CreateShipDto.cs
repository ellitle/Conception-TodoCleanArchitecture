namespace CleanTodo.Domain.DTOS
{
    public class CreateShipDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal GoldCargo { get; set; }
        public string Captain { get; set; } = string.Empty;
        public int CrewSize { get; set; }

        public CreateShipDto() { }

        public CreateShipDto(string name, decimal goldCargo, string captain, int crewSize)
        {
            Name = name;
            GoldCargo = goldCargo;
            Captain = captain;
            CrewSize = crewSize;
        }
    }
}