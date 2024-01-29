namespace Data.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Category? category { get; set; }
    }
}