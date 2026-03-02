namespace ChineseSaleApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Gift> gifts { get; set; }=new List<Gift>();
    }
}