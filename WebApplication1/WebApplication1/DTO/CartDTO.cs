using WebApplication1.Models;

namespace WebApplication1.DTO
{
    public class CartDTO
    {
        public int cartID { get; set; }
        public int productID { get; set; }
        public string productName { get; set; }
        public float totalPrice { get; set; }
        public int quantity { get; set; }
        public float delivaryFee { get; set; }
        public string customerAddress { get; set; }
    }
}
