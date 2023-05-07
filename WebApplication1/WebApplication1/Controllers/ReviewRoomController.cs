using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewRoomController : ControllerBase
    {
        private IHubContext<Hub> _hub;
        private IRepositry<Review> reviewRepo;
        private IRepositry<Customer> CustomerRepo;
        public ReviewRoomController(IRepositry<Review> _reviewRepo, IHubContext<Hub> hub, IRepositry<Customer> CustomerRepo)
        {
            reviewRepo = _reviewRepo;
            _hub = hub;
            this.CustomerRepo = CustomerRepo;
        }

        [HttpGet]
        public IActionResult GetAllReviews()
        {
            List<Review> reviews = reviewRepo.getall("Customer").ToList();
            reviews.Reverse();
            //_hub.Clients.All.SendAsync("NewReviewNotify", reviews);
            return Ok(reviews);
        }
        [HttpGet("display")]
        public IActionResult GetThreeReviews()
        {
            List<Review> reviews = reviewRepo.getall("Customer").TakeLast(3).ToList();
            reviews.Reverse();
            return Ok(reviews);
        }

        [HttpPost("ApplicationUserId")]
        public ActionResult<ReviewDTO> AddReview(string ApplicationUserId,ReviewDTO reviewDTO)
        {
            Customer customer = CustomerRepo.getall().FirstOrDefault(c => c.ApplicationUserId == ApplicationUserId);
            Review review = new Review();
            review.ResturantID = reviewDTO.ResturantId;
            review.CustomerID = customer.ID;
            review.Date= DateTime.Now;
            review.Text = reviewDTO.text;
            reviewRepo.create(review);
            return Ok();
        }
        }
}
