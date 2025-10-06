using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.SignalR;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Alert Bakchich Server Starting...");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddRazorPages();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Add configuration service
builder.Services.AddSingleton<AlertConfigurationService>();

builder.WebHost.UseUrls("http://0.0.0.0:5000");

var app = builder.Build();

app.UseCors();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapHub<PaymentHub>("/paymentHub");

// Serve index page
app.MapGet("/", async context =>
{
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync("wwwroot/index.html");
});

// Serve configuration CSS
app.MapGet("/configuration.css", async context =>
{
    context.Response.ContentType = "text/css";
    await context.Response.SendFileAsync("wwwroot/configuration.css");
});

// Webhook endpoint
app.MapPost("/webhook", async (HttpContext context, IHubContext<PaymentHub> hubContext) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var rawJson = await reader.ReadToEndAsync();
    
    Console.WriteLine("=== Received Webhook ===");
    Console.WriteLine($"Raw JSON: {rawJson}");
    Console.WriteLine();
    
    try
    {
        var payment = JsonSerializer.Deserialize<Payment>(rawJson);
        if (payment != null)
        {
            Console.WriteLine("=== Parsed Payment ===");
            Console.WriteLine($"Payment ID: {payment.paymentID}");
            Console.WriteLine($"Amount: {payment.amount}");
            Console.WriteLine($"Asset: {payment.asset.name} ({payment.asset.id})");
            Console.WriteLine($"Message: {payment.message ?? "(none)"}");
            Console.WriteLine($"Donor: {payment.donor?.username ?? "(anonymous)"}");
            if (!string.IsNullOrEmpty(payment.donor?.fullname))
            {
                Console.WriteLine($"Donor Full Name: {payment.donor.fullname}");
            }
            Console.WriteLine($"Webhook URL: {payment.webhookURL ?? "(none)"}");
            
            // Broadcast to all connected clients
            await hubContext.Clients.All.SendAsync("ReceivePayment", payment);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error deserializing: {ex.Message}");
    }
    
    Console.WriteLine("========================");
    return Results.Ok();
});

app.Run();

// Move classes to global namespace so they're accessible
public class PaymentHub : Hub
{
}

public class AlertConfigurationService
{
    private readonly string _configPath = "alertConfig.json";
    
    public AlertConfig GetConfiguration()
    {
        if (File.Exists(_configPath))
        {
            var json = File.ReadAllText(_configPath);
            return JsonSerializer.Deserialize<AlertConfig>(json) ?? GetDefaultConfig();
        }
        return GetDefaultConfig();
    }
    
    public void SaveConfiguration(AlertConfig config)
    {
        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_configPath, json);
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
            textWidth = ""
        };
    }
}

public class AlertConfig
{
    public int alertDuration { get; set; }
    public string messagePosition { get; set; } = "inside";
    public string animationType { get; set; } = "slide";
    public string animationDirection { get; set; } = "bottom";
    public string mediaType { get; set; } = "gif";
    public string mediaUrl { get; set; } = "";
    public int? mediaWidth { get; set; }
    public int? mediaHeight { get; set; }
    public string fontFamily { get; set; } = "'Arial', sans-serif";
    public int donorFontSize { get; set; }
    public string donorColor { get; set; } = "#ffffff";
    public int amountFontSize { get; set; }
    public string amountColor { get; set; } = "#ffd700";
    public int messageFontSize { get; set; }
    public string messageColor { get; set; } = "#ffffff";
    public string backgroundColor { get; set; } = "#000000";
    public int backgroundOpacity { get; set; }
    public bool showBorder { get; set; }
    public string borderColor { get; set; } = "#ffd700";
    public int borderWidth { get; set; }
    public bool textShadow { get; set; }
    public bool playSound { get; set; }
    public string soundUrl { get; set; } = "";
    public bool queueAlerts { get; set; }
    public int maxQueue { get; set; }
    public int? zIndexMedia { get; set; } = 1;
    public int? zIndexText { get; set; } = 2;
    public string textVertical { get; set; } = "50%";
    public string textHorizontal { get; set; } = "50%";
    public string textWidth { get; set; } = string.Empty;
    public string? customCss { get; set; } = string.Empty;
    public string? customJs { get; set; } = string.Empty;
}

public class Payment
{
    public int paymentID { get; set; }
    public string? message { get; set; }
    public Donor? donor { get; set; }
    public double amount { get; set; }
    public Asset asset { get; set; } = new Asset();
    public string? webhookURL { get; set; }
}

public class Donor
{
    public string? username { get; set; }
    public string? fullname { get; set; }
}

public class Asset
{
    public string id { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
}
