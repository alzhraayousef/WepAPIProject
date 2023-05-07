using WebApplication1.Models;

namespace WebApplication1.DTO
{
    public class ReviewDTO
    {
        public string text { get; set; }   
        public DateTime date { get; set; }
        public Customer? customer { get; set; }
        public int ResturantId { get; set; }
    }
}
