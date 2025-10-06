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
            return new AlertConfig();
        }
    }
}
