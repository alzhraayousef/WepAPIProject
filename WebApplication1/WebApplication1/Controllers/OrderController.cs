using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {

        IRepositry<OrderProducts> _OrderProducts;
        IRepositry<Order> _order;
        IRepositry<Product> _Product;
        IRepositry<Cart> _cart;
        IRepositry<Customer> _Customer;

        public OrderController(IRepositry<Cart> cart, IRepositry<Order> order, IRepositry<OrderProducts> OrderProducts, IRepositry<Product> Product, IRepositry<Customer> customer)
        {
            _OrderProducts = OrderProducts;
            _Product = Product;
            _Customer = customer;
            _order = order;
            _cart = cart;
        }

        [HttpGet("GetOrdersByUserID")]
        public IActionResult GetOrdersByUserID(string ApplicationUserId)
        {
            //List<OrderProducts> orderProducts = _OrderProducts.getall("");
            Customer custmer= _Customer.getall("ApplicationUser").FirstOrDefault(c=>c.ApplicationUserId==ApplicationUserId);
            List<Order> orders = _order.getall("Resturant.ApplicationUser").Where(o=>o.CustomerID==custmer.ID).ToList();
            List<OrderDTO> orderDTO = new List<OrderDTO>();
            foreach (Order order in orders)
            {
                orderDTO.Add(new OrderDTO()
                {
                    OrderId = order.ID,
                    RestaurantName = order.Resturant.ApplicationUser.UserName,
                    RestaurantImage = order.Resturant.Image,
                    Date = order.Date,
                    State = order.OrderState

                });
            }

            return Ok(orderDTO);
        }
        [HttpGet("id")]
        public IActionResult GetById(int id)
        {
            List<OrderProducts> orderProducts = _OrderProducts.getall("Product").Where(op=>op.OrderID==id).ToList();
            List<OrderDTO> orderDTOs= new List<OrderDTO>();
            OrderDTO orderDTO;
            foreach (OrderProducts orderProduct in orderProducts)
            {
                orderDTO = new OrderDTO();
                orderDTO.OrderId = id;
                orderDTO.ProductName = orderProduct.Product.Name;
                orderDTO.Price = orderProduct.Product.Price;
                orderDTO.Quantity = orderProduct.Quantity;
                orderDTO.TotalPrice = orderProduct.TotalPrice;
                orderDTOs.Add(orderDTO);
            }
            return Ok(orderDTOs);



            //Order order = _order.getall("OrderProducts.Product.Resturant.ResturantCities", "OrderProducts.Product.Resturant.ApplicationUser").FirstOrDefault(o => o.ID == id);

            //OrderDTO orderDTO = new OrderDTO();
            //orderDTO.OrderId = order.ID;
            //orderDTO.RestaurantName = order.OrderProducts.FirstOrDefault().Product.Resturant.ApplicationUser.UserName;
            //orderDTO.ProductName = order.OrderProducts.FirstOrDefault().Product.Name;
            //orderDTO.Quantity = order.OrderProducts.FirstOrDefault().Quantity;
            //orderDTO.Price = order.OrderProducts.FirstOrDefault().Product.Price;
            //orderDTO.TotalPrice = orderDTO.Quantity * orderDTO.Price;
            //orderDTO.Date = order.Date;
            //orderDTO.DelivaryFee = order.OrderProducts.FirstOrDefault().Product.Resturant.ResturantCities.FirstOrDefault().DelivaryFee;
            //orderDTO.DelivaryTime = order.OrderProducts.FirstOrDefault().Product.Resturant.ResturantCities.FirstOrDefault().DelivaryTime;
            //return Ok(orderDTO);

        }




        [HttpPost("{ResturentId:int}/{Total}/{ApplicationUserId}")]
        public IActionResult NewOrder(int ResturentId,float Total,string ApplicationUserId)
        {
            Customer customer = _Customer.getall().FirstOrDefault(o => o.ApplicationUserId == ApplicationUserId);
            List<Cart> CartItems = _cart.getall().Where(c => c.CustomerID==customer.ID).ToList();
            Order order = new Order()
            {
                Date = DateTime.Now,
                TotalPrice = Total,
                OrderState = "bending",
                CustomerID = customer.ID,
                IsDeleted = false,
                ResturantID= ResturentId
            };
            _order.create(order);

            OrderProducts orderProductsorg;
            foreach (Cart CartItem in CartItems)
            {
                orderProductsorg = new OrderProducts();
                orderProductsorg.OrderID = order.ID;
                orderProductsorg.ProductID = CartItem.ProductID;
                orderProductsorg.TotalPrice = CartItem.TotalPrice;
                orderProductsorg.Quantity = CartItem.Quantity;
                _cart.delete(CartItem);
                _OrderProducts.create(orderProductsorg);
            }

            return Ok();
        }

    }
}


