using AlertBakchich.Hubs;
using AlertBakchich.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.IO;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Alert Bakchich Server Starting...");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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
app.MapControllers();

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

app.Run();
