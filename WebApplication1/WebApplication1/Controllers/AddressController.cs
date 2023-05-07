using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Controllers

{ 
    //action//api/Customer
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private  Context context;
        private  IRepositry<Address> AddressRepositry;
        private IRepositry<Customer> customerRepositry;

        public AddressController(IRepositry<Address> AddressRepositry,IRepositry<Customer> CustomerRepositry,
            Context context) {
         
            this.AddressRepositry = AddressRepositry;
            this.customerRepositry = CustomerRepositry;
            this.context = context;

        }


        [HttpGet]
        public IActionResult GetAddressByCustomerId(string ApplicationUserId)
        {
            Customer custModel = customerRepositry.getall("Addresses.City").FirstOrDefault(c => c.ApplicationUserId == ApplicationUserId);

            CustomerWithAddressDTO custDTO = new CustomerWithAddressDTO();
            custDTO.CustomerID = custModel.ID;
            custDTO.CustomerFirstName = custModel.FirstName;
            custDTO.CustomerLastName = custModel.LastName;
            foreach (var item in custModel.Addresses)
            {
                AddressDTO add = new AddressDTO();
                add.StreetName= item.StreetName;
                add.ApartmentNo = item.ApartmentNo;
                add.FloorNo = item.FloorNo;
                add.DistrictID = item.DistrictID;
                add.CityName = item.City.Name;
                custDTO.Address.Add(add);
            }
            return Ok(custDTO);
        }
       


        [HttpPost]
        public IActionResult PostAddress(string ApplicationUserId,AddressDTO add)
        {
            Customer customer = customerRepositry.getall("Addresses.City").FirstOrDefault(c=>c.ApplicationUserId== ApplicationUserId);

            Address address = new Address();
             address.StreetName= add.StreetName ;
             address.ApartmentNo= add.ApartmentNo ;
            address.FloorNo = add.FloorNo;
            address.DistrictID = add.DistrictID;
            address.CityID = add.CityID;
            address.CustomerID = customer.ID;
            AddressRepositry.create(address);
            

             CustomerWithAddressDTO custDTO = new CustomerWithAddressDTO();
            custDTO.CustomerID = customer.ID;
            custDTO.CustomerFirstName = customer.FirstName;
            custDTO.CustomerLastName = customer.LastName;
            foreach (var item in customer.Addresses)
            {
                AddressDTO addressDTO = new AddressDTO();
                addressDTO.StreetName = item.StreetName;
                addressDTO.ApartmentNo = item.ApartmentNo;
                addressDTO.FloorNo = item.FloorNo;
                addressDTO.DistrictID = item.DistrictID;
                addressDTO.CityName = item.City.Name;
                custDTO.Address.Add(addressDTO);
            }
            return Ok(custDTO);


        }
    }
}
