using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using AlertBakchich;
using AlertBakchich.Services;
using AlertBakchich.Models;

namespace AlertBakchich.Pages
{
    public class AlertModel : PageModel
    {
        private readonly AlertConfigurationService _configService;

        public AlertModel(AlertConfigurationService configService)
        {
            _configService = configService;
        }

        public string ConfigJson { get; set; } = string.Empty;

        public AlertConfig Config { get; set; } = new AlertConfig();

        public void OnGet()
        {
            Config = _configService.GetConfiguration();
            ConfigJson = JsonSerializer.Serialize(Config);
        }
    }
}
