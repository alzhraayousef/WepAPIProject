namespace WebApplication1.DTO
{
    public class AddNewProductDTO
    {
        public string name { get; set; }    
        public string description { get; set; } 
        public IFormFile Image { get; set; }   
        public int ResturantID { get; set; }
        public float Price { get; set; }
        public int CategoryID { get; set; }
    }
}
