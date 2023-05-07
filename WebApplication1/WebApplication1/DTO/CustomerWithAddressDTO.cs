namespace WebApplication1.DTO
{
    public class CustomerWithAddressDTO
    {
        public int? CustomerID { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }

        public List<AddressDTO> Address { get; set; } = new List<AddressDTO>();
    }

    public class AddressDTO
    {

        public string StreetName { get; set; }
        public int ApartmentNo { get; set; }
        public int FloorNo { get; set; }
         public int DistrictID { get; set; }
        public int CityID { get; set; }
        public string? CityName { get; set; }


    }
}
