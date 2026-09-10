namespace CleanTodo.Domain.Entities;

public class Todo
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime Date { get; set; }
    public DateTime CompetedOn { get; set; }

    /// <summary>
    /// Constructeur qui crée mon guid, date
    /// </summary>
    /// <param name="text">Le texte du todo.</param>
    public Todo(string text)
    {
        Text = text;
        Id = Guid.NewGuid();
        Date = DateTime.Now;
        IsCompleted = false;
    }
}