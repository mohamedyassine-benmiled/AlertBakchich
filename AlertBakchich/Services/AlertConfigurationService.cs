using AlertBakchich.Models;
using Microsoft.AspNetCore.SignalR;
using AlertBakchich.Hubs;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlertBakchich.Services
{
    public class AlertConfigurationService
    {
        private readonly string _configPath = "alertConfig.json";
        private readonly IHubContext<PaymentHub> _hubContext;

        public AlertConfigurationService(IHubContext<PaymentHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public AlertConfig GetConfiguration()
        {
            if (File.Exists(_configPath))
            {
                var json = File.ReadAllText(_configPath);
                return JsonSerializer.Deserialize<AlertConfig>(json) ?? GetDefaultConfig();
            }
            return GetDefaultConfig();
        }

        public async Task SaveConfiguration(AlertConfig config)
        {
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_configPath, json);
            // Notify all clients config was updated
            await _hubContext.Clients.All.SendAsync("ConfigUpdated");
        }

        private AlertConfig GetDefaultConfig()
        {
            return new AlertConfig
            {
                alertDuration = 5,
                messagePosition = "inside",
                animationType = "slide",
                animationDirection = "bottom",
                mediaType = "gif",
                mediaUrl = "",
                mediaWidth = 400,
                mediaHeight = 300,
                fontFamily = "'Arial', sans-serif",
                donorFontSize = 32,
                donorColor = "#ffffff",
                amountFontSize = 48,
                amountColor = "#ffd700",
                messageFontSize = 24,
                messageColor = "#ffffff",
                backgroundColor = "#000000",
                backgroundOpacity = 80,
                showBorder = false,
                borderColor = "#ffd700",
                borderWidth = 3,
                textShadow = true,
                playSound = false,
                soundUrl = "",
                queueAlerts = true,
                maxQueue = 10,
                zIndexMedia = 1,
                zIndexText = 2,
                textVertical = "50%",
                textHorizontal = "50%",
                customCss = "",
                customJs = "",
                textWidth = "",
                loopVideo = false
            };
        }
    }
}
