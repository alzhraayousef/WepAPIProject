namespace WebApplication1.DTO
{
    public class CategoryWithResturantDTO
    {
        public int Id { get; set; }
        public string ResturantName { get; set; }
        public string ApplicationUser { get; set; }
        public List<CategoriesDTO> CategoryList { get; set; } = new List<CategoriesDTO>();

    }
    public class CategoriesDTO
    {
        public int ResturentCategoryID { get; set; }
        public int Id { get; set; }
        public string CatergoryName { get; set; }
        public List<ProductsWithAllDetailsDTO> productlist { get; set; } = new List<ProductsWithAllDetailsDTO>();

    }


    public class ProductsWithAllDetailsDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public int ResturantID { get; set; }
    }
}
