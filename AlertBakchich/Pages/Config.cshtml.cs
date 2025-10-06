using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlertBakchich.Pages
{
    public class ConfigModel(AlertConfigurationService configService) : PageModel {
        private readonly AlertConfigurationService _configService = configService;
        [BindProperty]
        public AlertConfig Config { get; set; } = new AlertConfig();

        public void OnGet()
        {
            Config = _configService.GetConfiguration();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine( "ModelState is invalid:");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"- {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            _configService.SaveConfiguration(Config);
            TempData["SuccessMessage"] = "Configuration saved successfully!";
            return RedirectToPage();
        }

        public IActionResult OnPostReset()
        {
            _configService.SaveConfiguration(new AlertConfig
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
                customJs = ""
            });
            TempData["SuccessMessage"] = "Configuration reset to defaults!";
            return RedirectToPage();
        }
    }
}
