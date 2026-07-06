using MarketWatch.Data;
using MarketWatch.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// disables cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // Allows requests from any origin (like local HTML files)
              .AllowAnyMethod()    // Allows GET, POST, PUT, DELETE, etc.
              .AllowAnyHeader();   // Allows any headers
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton<IStockService, MockStockService>();
//builder.Services.AddSingleton<IStockService, FinnhubStockService>();
builder.Services.AddHttpClient<IStockService, FinnhubStockService>();
//builder.Services.AddSingleton<IWatchlistService, WatchlistService>(); //can no longer be singleton once it has a reference to the stock service
builder.Services.AddScoped<IWatchlistService, WatchlistService>();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

// IMPORTANT line to disable cors
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.Run();