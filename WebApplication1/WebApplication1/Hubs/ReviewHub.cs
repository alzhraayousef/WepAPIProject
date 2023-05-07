using Microsoft.AspNetCore.SignalR;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Hubs
{
    public class ReviewHub:Hub
    {
        private IRepositry<Customer> CustomerRepo;
        public ReviewHub(IRepositry<Customer> CustomerRepo) { 
            this.CustomerRepo= CustomerRepo;
        }
        public void NewReview(ReviewDTO reviewDTO,string applicationUserId)
        {
            Customer customer = CustomerRepo.getall().FirstOrDefault(c => c.ApplicationUserId == applicationUserId);
            reviewDTO.customer = new Customer();
            reviewDTO.customer.FirstName = customer.FirstName;
            reviewDTO.customer.LastName=customer.LastName;
            Clients.All.SendAsync("NewReviewNotify", reviewDTO);
        }
        //public void Connect(string name)
        //{

        //    Clients.All.SendAsync("NewUser", name, Context.ConnectionId);
        //}
        //public override Task OnConnectedAsync()
        //{
        //    string name = "No Name";
        //    Clients.All.SendAsync("NewUser",name, Context.ConnectionId);
        //    return base.OnConnectedAsync();
        //}

      
    }
}
