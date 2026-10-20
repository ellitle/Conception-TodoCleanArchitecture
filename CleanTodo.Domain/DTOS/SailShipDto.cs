namespace CleanTodo.Domain.DTOS
{
    public class SailShipDto
    {
        public string Destination { get; set; } = string.Empty;

        public SailShipDto() { }

        public SailShipDto(string destination)
        {
            Destination = destination;
        }
    }
}