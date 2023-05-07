namespace WebApplication1.DTO
{
    public class CityWithDistrictDTO
    {
        public int ? CityID { get; set; }
        public string CityName { get; set; }
        public List<DistrictDTO> DistrictList { get; set; } = new List<DistrictDTO>();

    }
    public class DistrictDTO
    {
        public int? Id { get; set; }
        public string DistrictName { get; set; }
    }
}
