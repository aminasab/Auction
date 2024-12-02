using Microsoft.AspNetCore.SignalR;
using OnlineAuction.Hubs;
using OnlineAuction.Models;
using System.Timers;
using Timer = System.Timers.Timer;


namespace OnlineAuction.Services
{
    public class MandarinService
    {
        private static readonly Random _random = new();
        private readonly IHubContext<MandarinsHub> _hubContext;
        private static List<Mandarin> _mandarins = [];
        private static Timer? _timer;

        public MandarinService(IHubContext<MandarinsHub> hubContext)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            // Генерация каждые 2 секунды.
            _timer = new Timer(2000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            // Генерация первой мандаринки при старте сервиса.
            GenerateMandarinsAsync();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
         {
            GenerateMandarinsAsync();
            // Удаляем испорченные мандаринки.
            RemoveSpoiledMandarinsAsync();
        }

        /// <summary>
        /// Логика генерации новой мандаринки.
        /// </summary>
        public async void GenerateMandarinsAsync()
        {
            var mandarin = new Mandarin
            {
                ImageUrl = "/images/mandarin.png",
                Id = _mandarins.Count + 1,
                CreatedAt = DateTime.Now,
                IsSpoiled = false,
                Price = _random.Next(50, 100)
            };
            _mandarins.Add(mandarin);
            await _hubContext.Clients.All.SendAsync("ReceiveMandarinUpdate", mandarin);
        }

        /// <summary>
        /// Получить список мандаринок.
        /// </summary>
        public List<Mandarin> GetMandarins()
        {
            return _mandarins;
        }

        /// <summary>
        /// Удаление мандаринок, которые испортились (старше 24 часов).
        /// </summary>
        public async Task RemoveSpoiledMandarinsAsync()
        {
            var threshold = DateTime.Now.AddHours(-24);
            var spoiledMandarins = _mandarins.Where(m => m.IsSpoiled || m.CreatedAt < threshold).ToList();

            foreach (var mandarin in spoiledMandarins)
            {
                _mandarins.Remove(mandarin);
                // Уведомляем клиентов о том, что мандаринка была удалена.
                await _hubContext.Clients.All.SendAsync("RemoveMandarin", mandarin.Id);
            }
        }
    }
}