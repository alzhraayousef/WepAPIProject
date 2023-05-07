using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public IRepositry<Resturant> restaurantRepo;
        public IRepositry<Category> categoryRepo;
        public IRepositry<Product> ProductRepo;
        public IRepositry<ResturantCategories> resturantCategoriesRepo;

        public CategoryController(IRepositry<Resturant> restaurantRepo, IRepositry<Category> categoryRepo, IRepositry<Product> productRepo, IRepositry<ResturantCategories> resturantCategoriesRepo)
        {
            this.restaurantRepo = restaurantRepo;
            this.categoryRepo = categoryRepo;
            ProductRepo = productRepo;
            this.resturantCategoriesRepo = resturantCategoriesRepo;
        }

        //3
        [HttpPost("{ApplicationUserId}")]
        public IActionResult AddNewCategoryForResturent(string ApplicationUserId, List<int> categoriesIDs)
        {
            Resturant RestModel = restaurantRepo.getall().FirstOrDefault(r =>r.ApplicationUserId  == ApplicationUserId);
          
            ResturantCategories resturantCategories;
            foreach(int categoryID in categoriesIDs)
            {
                resturantCategories = new ResturantCategories();
                resturantCategories.ResturantID = RestModel.ID;
                resturantCategories.CategoryID=categoryID;
                resturantCategoriesRepo.create(resturantCategories);
            }

            List<ResturantCategories> resturantCategoriesList = resturantCategoriesRepo.getall("Category")
                 .Where(rc => rc.ResturantID == RestModel.ID).ToList();
            List<CategoriesDTO> categories = new List<CategoriesDTO>();
            CategoriesDTO categoriesDTO;
            foreach (var item in resturantCategoriesList)
            {
                categoriesDTO = new CategoriesDTO();
                categoriesDTO.CatergoryName = item.Category.Name;
                categoriesDTO.Id = item.Category.ID;
                categoriesDTO.ResturentCategoryID = item.ID;
                categories.Add(categoriesDTO);
            }
            return Ok(categories);
        }

        //2
        [HttpGet("GetAllCtegory")]
        public IActionResult getAll(string ApplicationUserId)
        {
            List<Category> categoryList = categoryRepo.getall();
            Resturant resturent = restaurantRepo.getall().FirstOrDefault(r => r.ApplicationUserId == ApplicationUserId);
            List<ResturantCategories> resturantCategories = resturantCategoriesRepo.getall("Category").Where(rc => rc.ResturantID == resturent.ID).ToList();

            List<Category> categories = new List<Category>();
            Category Category;
            foreach (var item in resturantCategories)
            {
                Category = new Category();
                Category.Name = item.Category.Name;
                Category.ID = item.Category.ID;
                categories.Add(Category);
            }
            List<Category> finalCategories = categoryList.Where(p => categories.All(x => x.ID != p.ID)).ToList();
            List<CategoriesDTO> categoriesDTO = new List<CategoriesDTO>();
            CategoriesDTO categoryDTO;
            foreach (var item in finalCategories)
            {
                categoryDTO = new CategoriesDTO();
                categoryDTO.CatergoryName = item.Name;
                categoryDTO.Id = item.ID;
                categoriesDTO.Add(categoryDTO);
            }
            return Ok(categoriesDTO);
        }

        [HttpGet("getCategorybyResturantId/{id:int}")]
        public IActionResult getCategorybyResturantId(int id)
        {
            Resturant RestModel = restaurantRepo.getall("ResturantCategories","ApplicationUser", "Products").FirstOrDefault(c => c.ID == id);
            Product ProdModel = ProductRepo.getall("Category", "Resturant.ApplicationUser").FirstOrDefault(c => c.ResturantID == id);
            CategoryWithResturantDTO RestDTO = new CategoryWithResturantDTO();
            RestDTO.Id = RestModel.ID;
            RestDTO.ResturantName = RestModel.ApplicationUser.UserName;
            RestDTO.ApplicationUser = RestModel.ApplicationUser.Id;
            foreach (var item in RestModel.ResturantCategories)
            {
                item.Category = categoryRepo.getall().FirstOrDefault(t => t.ID == item.CategoryID);

                CategoriesDTO cat = new CategoriesDTO();
                cat.Id = item.CategoryID;
                cat.CatergoryName = item.Category.Name;

                foreach (var val in item.Category.Products)
                {
                    ProductsWithAllDetailsDTO prod = new ProductsWithAllDetailsDTO();
                    prod.ID = val.ID;
                    prod.Name = val.Name;
                    prod.Description = val.Description;
                    prod.Price = val.Price;
                    prod.ResturantID = item.ResturantID;
                    prod.Image = val.Image;
                    prod.IsDeleted = val.IsDeleted;
                    prod.CategoryID = val.CategoryID;
                    cat.productlist.Add(prod);

                }

                RestDTO.CategoryList.Add(cat);

            }
            return Ok(RestDTO);
        }



        [HttpGet("GetAllCategoriesNameByResturantID/{ApplicationUserId}")]
        public IActionResult GetAllCategoriesNameByResturantID(string ApplicationUserId)
        {
           Resturant resturent = restaurantRepo.getall().FirstOrDefault(r => r.ApplicationUserId == ApplicationUserId);
           List<ResturantCategories> resturantCategories = resturantCategoriesRepo.getall("Category")
                .Where(rc => rc.ResturantID == resturent.ID).ToList();
           List<CategoriesDTO> categories = new List<CategoriesDTO>();
            CategoriesDTO categoriesDTO;
            foreach (var item in resturantCategories)
            {
                categoriesDTO = new CategoriesDTO();
                categoriesDTO.CatergoryName = item.Category.Name;
                categoriesDTO.Id = item.Category.ID;
                categoriesDTO.ResturentCategoryID = item.ID;
                categories.Add(categoriesDTO);
            }
            return Ok(categories);
        }


        //1
        [HttpDelete("{ResturentCategoryID}")]
        public IActionResult DeleteCategoryByResturentID(int ResturentCategoryID)
        {
            ResturantCategories resturantCategories = resturantCategoriesRepo.getbyid(ResturentCategoryID);
            resturantCategoriesRepo.delete(resturantCategories);
            return Ok();
        }




    }
}
