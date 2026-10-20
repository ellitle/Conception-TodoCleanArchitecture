namespace CleanTodo.Domain.DTOS
{
    public class UpdateShipDto : CreateShipDto
    {
        public UpdateShipDto() { }

        public UpdateShipDto(string name, decimal goldCargo, string captain, int crewSize) : base(name, goldCargo, captain, crewSize)
        {
        }
    }
}