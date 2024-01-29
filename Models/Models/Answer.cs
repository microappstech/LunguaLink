namespace Data.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public bool IsOk { get; set; }
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
        public bool unserAnswer { get; set; }
    }
}