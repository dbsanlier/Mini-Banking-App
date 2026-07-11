using Microsoft.EntityFrameworkCore;
using bankingapp.API.Data;
using bankingapp.API.Repositories;
using bankingapp.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();  // <-- BUNU EKLE

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMusteriRepository, MusteriRepository>();
builder.Services.AddScoped<IMusteriService, MusteriService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();  // <-- BUNU EKLE

app.Run();