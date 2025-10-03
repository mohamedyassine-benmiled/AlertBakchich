using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.SignalR;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.WebHost.UseUrls("http://0.0.0.0:5000");

var app = builder.Build();

app.MapHub<PaymentHub>("/paymentHub");

app.MapGet("/", async context =>
{
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync("wwwroot/index.html");
});

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

public class PaymentHub : Hub
{
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
