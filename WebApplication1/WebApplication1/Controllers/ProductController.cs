using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Helper;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase

    {
        public IRepositry<Product> ProductRepo;
        
        public IRepositry<ResturantCities> ResturantCitiesRepo;
        public IRepositry<Resturant> restaurantRepo;

        public ProductController(IRepositry<Product> ProductRepo, IRepositry<ResturantCities> ResturantCitiesRepo,IRepositry<Resturant> restaurantRepo)
        {
            this.ProductRepo = ProductRepo;
            this.restaurantRepo = restaurantRepo;
           
        }
        [HttpGet("GetProductDetailsbbyID")]
        public ActionResult GetProductDetailsbbyID(int id)
        {
            Product ProdModel = ProductRepo.getall("Resturant.ResturantCities", "Resturant.ApplicationUser").FirstOrDefault(c => c.ID == id);
            ProductCartDTO ProdDTO = new ProductCartDTO();
            ProdDTO.ID = ProdModel.ID;
            ProdDTO.Price = ProdModel.Price;    
            ProdDTO.ResturantID = ProdModel.Resturant.ID;
            ProdDTO.Name = ProdModel.Name;
            ProdDTO.Quanttity = 1;
            ProdDTO.ApplicationUser = ProdModel.Resturant.ApplicationUser.UserName;
            var DeliveryFee = ProdModel.Resturant.ResturantCities.FirstOrDefault(c=>c.ResturantID==ProdModel.Resturant.ID).DelivaryFee;
            ProdDTO.Deliveryfee = DeliveryFee;
    
            return Ok(ProdDTO);

        }


        [HttpPost("{ApplicationUserId}")]
        public IActionResult New(string ApplicationUserId,[FromForm]AddNewProductDTO productDTO)
        {
            Resturant resturent = restaurantRepo.getall().FirstOrDefault(r => r.ApplicationUserId == ApplicationUserId);

            if (ModelState.IsValid)
            {
                Product newProd = new Product();
                newProd.Name = productDTO.name;
                newProd.Description = productDTO.description;
                newProd.Image = ImagesHelper.UploadImg(productDTO.Image, "ProductIMG");
                newProd.IsDeleted = false;
                newProd.CategoryID = productDTO.CategoryID;
                newProd.ResturantID = resturent.ID;
                newProd.Price = productDTO.Price;
                ProductRepo.create(newProd);
                return new StatusCodeResult(StatusCodes.Status201Created);

            }
            return BadRequest(ModelState);

        }


    }
}
