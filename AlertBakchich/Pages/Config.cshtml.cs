using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AlertBakchich.Services;
using AlertBakchich.Models;

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

        public async Task<IActionResult> OnPost()
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
                TempData["ModelStateErrors"] = 
                    string.Join("\n", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return Page();
            }

            await _configService.SaveConfiguration(Config);
            TempData["SuccessMessage"] = "Configuration saved successfully!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostReset()
        {
            await _configService.SaveConfiguration(new AlertConfig());
            TempData["SuccessMessage"] = "Configuration reset to defaults!";
            return RedirectToPage();
        }
    }
}
