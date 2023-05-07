using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication1.DTO;
using WebApplication1.Helper;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResturantController : ControllerBase
    {
        public IRepositry<Resturant> restaurantRepo;
        public IRepositry<City> cityRepo;
        Context context;
        public  ResturantController(IRepositry<Resturant> restaurantRepo, IRepositry<City> cityRepo) 
        {
           this.restaurantRepo= restaurantRepo;
            this.cityRepo= cityRepo;
        }



        [HttpGet("GetAllRestaurant")]
        public IActionResult getAll()
        {
            List<Resturant> restaurantList = restaurantRepo.getall("ApplicationUser", "ResturantCities");
            List<ResturantsDTO> resturantsDTO = new List<ResturantsDTO>();
            foreach (Resturant res in restaurantList)
            {
                ResturantsDTO resDTO = new ResturantsDTO();
                resDTO.Id = res.ID;
                resDTO.RestaurantImage = res.Image;
                resDTO.RestaurantName = res.ApplicationUser.UserName;
                resDTO.delivaryTime = res.ResturantCities.FirstOrDefault().DelivaryTime;
                resDTO.delivaryFee = res.ResturantCities.FirstOrDefault().DelivaryFee;
                resturantsDTO.Add(resDTO);
            }
            return Ok(resturantsDTO);
        }

        [HttpGet("{id:int}")]
        //[Authorize]//jwt
        public ActionResult GetResturantDetails(int id)
        {
            Resturant resturant = restaurantRepo.getall("ApplicationUser").FirstOrDefault(r=>r.ID==id);

            return Ok(resturant);

        }
 

        [HttpGet("{ApplicationUserId}")]
        public ActionResult GetResturentDetails(string ApplicationUserId)
        {
            Resturant Resturant = restaurantRepo.getall("ApplicationUser").FirstOrDefault(c => c.ApplicationUserId == ApplicationUserId);

            ResturantRegistrationDTO resturantRegistrationDTO= new ResturantRegistrationDTO();
            resturantRegistrationDTO.UserName = Resturant.ApplicationUser.UserName;
            resturantRegistrationDTO.Email = Resturant.ApplicationUser.Email;
            resturantRegistrationDTO.WorkingHours = Resturant.WorkingHours;
            resturantRegistrationDTO.ImageName = Resturant.Image;
            resturantRegistrationDTO.MinOrderAmmount = Resturant.MinOrderAmmount;
            return Ok(resturantRegistrationDTO);
        }


        [HttpPut("{ApplicationUserId}")]
        public ActionResult EditResturentDetails(string ApplicationUserId,[FromForm] ResturantRegistrationDTO resturantRegistrationDTO)
        {
            Resturant Resturant = restaurantRepo.getall("ApplicationUser").FirstOrDefault(c => c.ApplicationUserId == ApplicationUserId);
            Resturant.ApplicationUser.UserName = resturantRegistrationDTO.UserName;
            Resturant.ApplicationUser.Email = resturantRegistrationDTO.Email;
            Resturant.WorkingHours = resturantRegistrationDTO.WorkingHours;
           if(resturantRegistrationDTO.ImageNameFile!=null)
                 Resturant.Image = ImagesHelper.UploadImg(resturantRegistrationDTO.ImageNameFile, "ResturantIMG");
            Resturant.MinOrderAmmount = resturantRegistrationDTO.MinOrderAmmount;
            restaurantRepo.update(Resturant);
            return Ok(new { image=Resturant.Image });
        }

    }
}
