using Microsoft.EntityFrameworkCore;
using UdemySignalR.API.Hubs;
using UdemySignalR.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opts => { opts.UseSqlServer(builder.Configuration["SqlDbConnect"]); });

builder.Services.AddCors(opts =>
{
    opts.AddPolicy("CorsPolicy", builder =>
    {

        builder.WithOrigins("https://localhost:7170").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();
//https://localhost:7104/SignalRHub
app.MapHub<SignalRHub>("/SignalRHub");

app.Run();
