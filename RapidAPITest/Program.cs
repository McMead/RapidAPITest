using Eway.Rapid.Abstractions.Interfaces;
using Eway.Rapid;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Net.Http.Headers;
using RapidAPIProject.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddHttpClient("RapidClientHttpClient", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["RapidApi:BaseAddress"]);  // Retrieved from appsettings.json gitignore applied for security, an improvement will be use of Azure key vault
    c.DefaultRequestHeaders.Add("Accept", "application/json");
    //Adding basic authentication
    var byteArray = Encoding.ASCII.GetBytes(builder.Configuration["RapidApi:Credentials"]); // Retrieved from appsettings.json 
    c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));  
});

builder.Services.AddTransient<IRapidClient, RapidClient>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("RapidClientHttpClient");
    return new RapidClient(httpClient);
});

builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddLogging(config =>
{
    // Configure your logging pipeline
    config.AddConsole();
    config.AddDebug();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler("/error");  

app.MapControllers();

app.Run();
