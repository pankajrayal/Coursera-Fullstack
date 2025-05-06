using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillSnap.Api.Data;
using SkillSnap.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddCors(options => {
  options.AddPolicy("AllowClient", policy => {
    policy.WithOrigins("http://localhost:5226")
          .AllowAnyMethod()
          .AllowAnyHeader();
  });
});

// Configure MSSQL Server
builder.Services.AddDbContext<SkillSnapContext>(options => {
  options.UseSqlServer(builder.Configuration.GetConnectionString("SkillSnapDb"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddEntityFrameworkStores<SkillSnapContext>();

var app = builder.Build();
app.MapControllers();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
  app.MapOpenApi();
}
app.UseCors("AllowClient");

app.UseHttpsRedirection();

app.Run();
