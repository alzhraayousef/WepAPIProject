using WebApplication1.Models;

namespace WebApplication1.DTO
{
    public class RestaurantWithCityDTO
    {
        public int? ResturentCityID { get; set; }
        public int Id { get; set; }
        public string? CityName { get; set; }
        public string DelivaryTime { get; set; }
       public float DelivaryFee { get; set; }
        public List<ResturantsDTO>? RestList { get; set; } = new List<ResturantsDTO>();
    }
    public class ResturantsDTO
    {
        public int Id { get; set;}
        public string RestaurantImage { get; set; }
        public string RestaurantName { get; set; }
         public float delivaryFee { get; set; }
        public string delivaryTime { get; set; }

    }
}
