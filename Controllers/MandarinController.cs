using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OnlineAuction.Hubs;
using OnlineAuction.Interfaces;
using OnlineAuction.Models;
using OnlineAuction.Services;
using System.Security.Claims;

namespace OnlineAuction.Controllers
{
    public class MandarinController : Controller
    {
        private string message = "Мандаринка выкуплена!";
        private IHubContext<MandarinsHub> _hubContext;
        private IMandarinRepository _mandarinRepository;
        private IPurchaseRepository _purchaseRepository;
        private IUserRepository _userRepository;
        private EmailSender _emailSender;

        public MandarinController(IHubContext<MandarinsHub> hubContext, IMandarinRepository mandarinRepository, IPurchaseRepository purchaseRepository,
            IUserRepository userRepository, EmailSender emailSender)
        {
            _hubContext = hubContext;
            _mandarinRepository = mandarinRepository;
            _purchaseRepository = purchaseRepository;
            _userRepository = userRepository;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task BuyMandarinAsync([FromBody] BuyMandarinRequest request)
        {
            int id = request.Id;
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var userIdTask = await _userRepository.GetUserByFullEmailAsync(userEmail);

            var tasks = new List<Task>
            {
            _hubContext.Clients.All.SendAsync("RemoveMandarin", id),
            _mandarinRepository.DeleteMandarinAsync(id),
            _purchaseRepository.AddAsync(new Purchase
                {
                    UserId = userIdTask.UserId,
                    MandarinId = request.Id,
                    PurchasePrice = request.Price,
                    CreatedAt = DateTime.Now
                }),
            _emailSender.SendEmailAsync(userEmail, message)
            };
            await Task.WhenAll(tasks);
        }

        [HttpPost]
        public async Task PlaceBetAsync([FromBody] BuyMandarinRequest request)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            _mandarinRepository.UpdateFinalPriceMandarinAsync(request.Id, request.Price);
            _hubContext.Clients.All.SendAsync("UpdateMandarin", request.Id, request.Price);
            _emailSender.SendEmailAsync(userEmail, "Вы сделали ставку!");
        }
    }
}