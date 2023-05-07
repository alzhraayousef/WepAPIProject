using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        IRepositry<Cart> cartRepo;
        IRepositry<Product> productRepo;
        IRepositry<Customer> customerRepo;
        public CartController(IRepositry<Cart> _cartRepo, IRepositry<Product> _productRepo,
            IRepositry<Customer> _customerRepo)
        {
            productRepo = _productRepo;
            cartRepo = _cartRepo;
            customerRepo = _customerRepo;

        }


        [HttpPost("{ProductID:int}/{ApplicationUserId}")]
        public ActionResult<CartDTO> AddToCart(int ProductID, string ApplicationUserId)
        {
            Customer customer = customerRepo.getall().FirstOrDefault(c => c.ApplicationUserId == ApplicationUserId);
            Product product = productRepo.getall().FirstOrDefault(p => p.ID == ProductID);

            List<Cart> cartList = cartRepo.getall("Product").Where(c => c.CustomerID == customer.ID).ToList();
            Cart cart;
            bool flagResturentDound = false;
            bool flagProductFound = false;
            if (cartList.Count > 0)
            {
                foreach (Cart item in cartList)
                {
                    if (item.Product.ResturantID == product.ResturantID)
                    {
                        flagResturentDound = true;
                    }
                    if (item.ProductID == ProductID)
                    {
                        flagProductFound = true;
                    }
                }
            }
            if (cartList.Count <= 0 || flagResturentDound)
            {
                if (flagProductFound)
                {
                    cart = cartRepo.getall().FirstOrDefault(c => c.ProductID == ProductID && c.CustomerID == customer.ID);
                    cart.Quantity += 1;
                    cart.TotalPrice = cart.Quantity * product.Price;
                    cartRepo.update(cart);
                }
                else
                {
                    cart = new Cart
                    {
                        CustomerID = customer.ID,
                        ProductID = product.ID,
                        Quantity = 1,
                        TotalPrice = product.Price
                    };
                    cartRepo.create(cart);
                }
                // return Ok();
                //cartList = cartRepo.getall("Product").Where(c => c.CustomerID == customer.ID).ToList();
                // return Ok(cartList);
                cartList = cartRepo.getall("Product").Where(c => c.CustomerID == customer.ID).ToList();
                List<CartDTO> cartDTOList = new List<CartDTO>();
                CartDTO cartDTO;
                foreach (Cart item in cartList)
                {
                    cartDTO = new CartDTO();
                    cartDTO.cartID = item.ID;
                    cartDTO.productName = item.Product.Name;
                    cartDTO.productID = item.Product.ID;
                    cartDTO.quantity = item.Quantity;
                    cartDTO.totalPrice = item.TotalPrice;
                    cartDTO.customerAddress = "Assyut University";
                    cartDTO.delivaryFee = 15;
                    cartDTOList.Add(cartDTO);
                }
                return Ok(cartDTOList);
            }
            else
            {
                return BadRequest("Resturant Not Exist");
            }

        }




        [HttpGet("ApplicationUserId")]
        public IActionResult GetCartByApplicationUserId(string ApplicationUserId)
        {
            Customer customer = customerRepo.getall().FirstOrDefault(c => c.ApplicationUserId == ApplicationUserId);

            List<Cart> cartList = cartRepo.getall("Product").Where(c => c.CustomerID == customer.ID).ToList();
            List<CartDTO> cartDTOList = new List<CartDTO>();
            CartDTO cartDTO;
            foreach (Cart item in cartList)
            {
                cartDTO = new CartDTO();
                cartDTO.cartID = item.ID;
                cartDTO.productName = item.Product.Name;
                cartDTO.productID = item.Product.ID;
                cartDTO.quantity = item.Quantity;
                cartDTO.totalPrice = item.TotalPrice;
                cartDTO.customerAddress = "Assyut University";
                cartDTO.delivaryFee = 15;
                cartDTOList.Add(cartDTO);
            }
            return Ok(cartDTOList);
        }


        [HttpPut("{ProductID:int}/{quantity:int}/{ApplicationUserId}")]
        public IActionResult EditCart(int ProductID, int quantity, string ApplicationUserId)
        {
            Customer customer = customerRepo.getall().FirstOrDefault(c => c.ApplicationUserId == ApplicationUserId);
            Cart cart = cartRepo.getall().FirstOrDefault(c => c.ProductID == ProductID && c.CustomerID == customer.ID);
            Product product = productRepo.getall().FirstOrDefault(p => p.ID == ProductID);
            cart.Quantity = quantity;
            cart.TotalPrice = product.Price * cart.Quantity;
            cartRepo.update(cart);
            return Ok(cart.TotalPrice);
        }


        [HttpDelete("{id}")]
        public IActionResult deleteFromCart(int id)
        {
            Cart cart = cartRepo.getbyid(id);
            cartRepo.delete(cart);

            return Ok();

        }



        [HttpDelete("ApplicationUserId")]
        public IActionResult deleteAllCustomerCart(string ApplicationUserId)
        {
            Customer customer = customerRepo.getall().FirstOrDefault(c => c.ApplicationUserId == ApplicationUserId);
            List<Cart> cartList = cartRepo.getall().Where(c => c.CustomerID == customer.ID).ToList();
            foreach(Cart item in cartList)
            {
                cartRepo.delete(item);

            }
            return Ok();

        }

    }
}
