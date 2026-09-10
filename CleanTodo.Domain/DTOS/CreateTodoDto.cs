using System;
using System.Collections.Generic;
using System.Text;

namespace CleanTodo.Domain.DTOS
{
    public class CreateTodoDto
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
    }
}
