using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        public IRepositry<Resturant> restaurantRepo;
        public IRepositry<City> cityRepo;
        public IRepositry<District> districtRepo;
        public IRepositry<ResturantCities> resturantCitiesRepo;
        Context context;
       
        public CityController(IRepositry<Resturant> restaurantRepo, IRepositry<City> cityRepo,
            IRepositry<District> districtRepo, IRepositry<ResturantCities> resturantCitiesRepo)
        {
            this.restaurantRepo = restaurantRepo;
            this.cityRepo = cityRepo;
            this.districtRepo = districtRepo;
            this.resturantCitiesRepo = resturantCitiesRepo;
        }



        [HttpGet("GetAllCities")]
        public IActionResult getAll()
        {
            List<City> cityList = cityRepo.getall();
            return Ok(cityList);
        }


        [HttpGet("GetRestaurantbyCityId")]
        public IActionResult getRestaurantbyCityId(int cityID)
        {

            List<ResturantCities> resturantCitiesList = resturantCitiesRepo.getall("Resturant.ApplicationUser", "Resturant.ResturantCities").Where(c => c.CityID == cityID).ToList();

            List<ResturantsDTO> resturantsDTO = new List<ResturantsDTO>();
            foreach (ResturantCities res in resturantCitiesList)
            {
                ResturantsDTO resDTO = new ResturantsDTO();
                resDTO.Id = res.ID;
                resDTO.RestaurantImage = res.Resturant.Image;
                resDTO.RestaurantName = res.Resturant.ApplicationUser.UserName;
                resDTO.delivaryTime = res.DelivaryTime;
                resDTO.delivaryFee = res.DelivaryFee;
                resturantsDTO.Add(resDTO);
            }
            return Ok(resturantsDTO);

        }
        [HttpGet("GetDistrictByCityId")]
        public IActionResult getAllDistrictByCityID(int id)
        {
            City CityModel = cityRepo.getall("Districts").FirstOrDefault(c => c.ID == id);
            CityWithDistrictDTO CityDTO = new CityWithDistrictDTO();
            CityDTO.CityID = CityModel.ID;
            CityDTO.CityName = CityModel.Name;
            foreach (var item in CityModel.Districts)
            {
                DistrictDTO res = new DistrictDTO();
                res.Id = item.ID;
                res.DistrictName = item.Name;
                CityDTO.DistrictList.Add(res);
            }
            return Ok(CityDTO.DistrictList);
        }

        [HttpGet("GetAllDistrict")]
        public IActionResult getAllDistrict()
        {
            List<District> districtsList = districtRepo.getall().ToList();
          
            return Ok(districtsList);
        }





        //1
        [HttpGet("GetAllCitiesByResturantID/{ApplicationUserId}")]
        public IActionResult GetAllCitiesByResturantID(string ApplicationUserId)
        {
            Resturant resturent = restaurantRepo.getall().FirstOrDefault(r => r.ApplicationUserId == ApplicationUserId);
           
            List<ResturantCities> resturantCities= resturantCitiesRepo.getall("City")
                 .Where(rc => rc.ResturantID == resturent.ID).ToList();
            List<RestaurantWithCityDTO> restaurantWithCityDTOsList = new List<RestaurantWithCityDTO>();
            RestaurantWithCityDTO restaurantWithCityDTO;
            foreach (var item in resturantCities)
            {
                restaurantWithCityDTO = new RestaurantWithCityDTO();
                restaurantWithCityDTO.ResturentCityID = item.ID;
                restaurantWithCityDTO.Id = item.CityID;
                restaurantWithCityDTO.CityName = item.City.Name;
                restaurantWithCityDTO.DelivaryTime=item.DelivaryTime;
                restaurantWithCityDTO.DelivaryFee = item.DelivaryFee;
                restaurantWithCityDTOsList.Add(restaurantWithCityDTO);
            }
            return Ok(restaurantWithCityDTOsList);

        }


        //2
        [HttpGet("GetDefferentCities")]
        public IActionResult getAll(string ApplicationUserId)
        {
            List<City> CityList = cityRepo.getall();
            Resturant resturent = restaurantRepo.getall().FirstOrDefault(r => r.ApplicationUserId == ApplicationUserId);
            List<ResturantCities> resturantCities = resturantCitiesRepo.getall("City").Where(rc => rc.ResturantID == resturent.ID).ToList();

            List<City> cities = new List<City>();
            City City;
            foreach (var item in resturantCities)
            {
                City = new City();
                City.Name = item.City.Name;
                City.ID = item.CityID;
                cities.Add(City);
            }
            List<City> finalCities = CityList.Where(p => cities.All(x => x.ID != p.ID)).ToList();
            List<RestaurantWithCityDTO> citiesDTO = new List<RestaurantWithCityDTO>();
            RestaurantWithCityDTO cityDTO;
            foreach (var item in finalCities)
            {
                cityDTO = new RestaurantWithCityDTO();
                cityDTO.CityName = item.Name;
                cityDTO.Id = item.ID;
                citiesDTO.Add(cityDTO);
            }
            return Ok(citiesDTO);
        }


        //3
        [HttpPost("{ApplicationUserId}")]
        public IActionResult AddNewCityForResturent(string ApplicationUserId, List<RestaurantWithCityDTO> cities)
        {
            Resturant RestModel = restaurantRepo.getall().FirstOrDefault(r => r.ApplicationUserId == ApplicationUserId);

            ResturantCities resturantCities;
            foreach (RestaurantWithCityDTO item in cities)
            {
                resturantCities = new ResturantCities();
                resturantCities.ResturantID = RestModel.ID;
                resturantCities.CityID = item.Id;
                resturantCities.DelivaryTime = item.DelivaryTime;
                resturantCities.DelivaryFee = item.DelivaryFee;

                resturantCitiesRepo.create(resturantCities);
            }

            List<ResturantCities> resturantCitiesList = resturantCitiesRepo.getall("City")
                 .Where(rc => rc.ResturantID == RestModel.ID).ToList();
            List<RestaurantWithCityDTO> restaurantWithCityDTOsList = new List<RestaurantWithCityDTO>();
            RestaurantWithCityDTO restaurantWithCityDTO;
            foreach (var item in resturantCitiesList)
            {
                restaurantWithCityDTO = new RestaurantWithCityDTO();
                restaurantWithCityDTO.ResturentCityID = item.ID;
                restaurantWithCityDTO.Id = item.CityID;
                restaurantWithCityDTO.CityName = item.City.Name;
                restaurantWithCityDTO.DelivaryTime = item.DelivaryTime;
                restaurantWithCityDTO.DelivaryFee = item.DelivaryFee;
                restaurantWithCityDTOsList.Add(restaurantWithCityDTO);
            }
            return Ok(restaurantWithCityDTOsList);
        }


        //3
        [HttpDelete("{ResturentCityID}")]
        public IActionResult DeleteCityByResturentID(int ResturentCityID)
        {
            ResturantCities resturantCities = resturantCitiesRepo.getbyid(ResturentCityID);
            resturantCitiesRepo.delete(resturantCities);
            return Ok();
        }


        //4
        [HttpGet("GetCityDetails")]
        public IActionResult GetCityDetails(int ResturentCityID)
        {
            ResturantCities resturantCities = resturantCitiesRepo.getbyid(ResturentCityID);
            RestaurantWithCityDTO cityDTO = new RestaurantWithCityDTO();
            cityDTO.DelivaryTime = resturantCities.DelivaryTime;
            cityDTO.DelivaryFee = resturantCities.DelivaryFee;
            cityDTO.ResturentCityID = ResturentCityID; ;
            return Ok(cityDTO);
        }

        //5
        [HttpPut]
        public IActionResult EditCityDetails(string ApplicationUserId, int ResturentCityID, RestaurantWithCityDTO cityDTO)
        {
            ResturantCities resturantCities = resturantCitiesRepo.getbyid(ResturentCityID);
            resturantCities.DelivaryTime = cityDTO.DelivaryTime;
            resturantCities.DelivaryFee = cityDTO.DelivaryFee;
            resturantCitiesRepo.update(resturantCities);


            Resturant resturent = restaurantRepo.getall().FirstOrDefault(r => r.ApplicationUserId == ApplicationUserId);

            List<ResturantCities> resturantCitiesList = resturantCitiesRepo.getall("City")
                 .Where(rc => rc.ResturantID == resturent.ID).ToList();
            List<RestaurantWithCityDTO> restaurantWithCityDTOsList = new List<RestaurantWithCityDTO>();
            RestaurantWithCityDTO restaurantWithCityDTO;
            foreach (var item in resturantCitiesList)
            {
                restaurantWithCityDTO = new RestaurantWithCityDTO();
                restaurantWithCityDTO.ResturentCityID = item.ID;
                restaurantWithCityDTO.Id = item.CityID;
                restaurantWithCityDTO.CityName = item.City.Name;
                restaurantWithCityDTO.DelivaryTime = item.DelivaryTime;
                restaurantWithCityDTO.DelivaryFee = item.DelivaryFee;
                restaurantWithCityDTOsList.Add(restaurantWithCityDTO);
            }
            return Ok(restaurantWithCityDTOsList);
        }

    }
}