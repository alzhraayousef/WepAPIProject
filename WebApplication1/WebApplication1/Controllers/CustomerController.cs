using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private IRepositry<Customer> customerRepositry;
        private IRepositry<CustomerInfoDTO> customerInfosRepository;

        public CustomerController(IRepositry<Customer> customerRepositry)
        {
            this.customerRepositry = customerRepositry;
          
        }

        [HttpGet("{ApplicationUserId}")]
        public ActionResult GetCustomerDetails(string ApplicationUserId)
        {
            Customer customer = customerRepositry.getall("ApplicationUser").FirstOrDefault(c => c.ApplicationUserId == ApplicationUserId);

            CustomerInfoDTO CustomerDTO = new CustomerInfoDTO();
            CustomerDTO.CustomerFirstName = customer.FirstName;
            CustomerDTO.CustomerLastName = customer.LastName;
            CustomerDTO.Email = customer.ApplicationUser.Email;
            CustomerDTO.Gender = customer.Gender;

             return Ok(CustomerDTO);

        }

        [HttpPut("{Email:alpha}/{ApplicationUserId}")]
        public IActionResult UpdateEmail(string Email, string ApplicationUserId)
        {
            Customer customer = customerRepositry.getall("ApplicationUser").FirstOrDefault(c => c.ApplicationUserId == ApplicationUserId);

            CustomerInfoDTO CustomerDTO = new CustomerInfoDTO();
            CustomerDTO.CustomerFirstName = customer.FirstName;
            CustomerDTO.CustomerLastName = customer.LastName;
            CustomerDTO.Email = Email;
            CustomerDTO.Gender = customer.Gender;
            customer.ApplicationUser.Email = Email;
            customerRepositry.update(customer);
            return Ok(customer.ApplicationUser.Email);
          
        }

    }
}
