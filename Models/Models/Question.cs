namespace Data.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }
        public List<Answer> Answers { get; set; }
    }
}