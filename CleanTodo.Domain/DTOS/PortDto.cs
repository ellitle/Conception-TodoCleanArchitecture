
namespace CleanTodo.Domain.DTOS
{
    public class PortDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public PortDto() { }

        public PortDto(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
