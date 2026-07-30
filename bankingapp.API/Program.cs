using Microsoft.EntityFrameworkCore;
using bankingapp.API.Data;
using bankingapp.API.Repositories;
using bankingapp.API.Services;
// somut benzetme ornegi
// Controller (musteri): "bana bir tabak yemek lazim"
//DI Container (garson): "tamam, hemen mutfaga ileteyim"
//Service (asci): yemegi hazirliyor, malzemeleri almak icin Repository (market) ile konusuyor
//Repository (market): veritabanindan malzemeleri getiriyor


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();  
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMusteriRepository, MusteriRepository>();
builder.Services.AddScoped<IHesapRepository, HesapRepository>();
builder.Services.AddScoped<IIslemRepository, IslemRepository>();
builder.Services.AddScoped<IMusteriService, MusteriService>();
builder.Services.AddScoped<IHesapService, HesapService>();
builder.Services.AddScoped<IIslemService, IslemService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.MapControllers();  

app.Run();