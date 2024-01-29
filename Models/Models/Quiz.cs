namespace Data.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public List<Question> questions { get; set; }

    }
}
