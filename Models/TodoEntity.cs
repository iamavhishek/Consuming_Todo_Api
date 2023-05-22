namespace APIConsumeAndCrud.Models
{
    public class TodoEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool isComplete { get; set; }
    }
}
