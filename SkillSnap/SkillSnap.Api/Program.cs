using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SkillSnap.Api.Data;
using SkillSnap.Api.Models;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["JwtSettings:SecretKey"];
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
      options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = key
      };
    });

// Add services to the container.
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
    .AddEntityFrameworkStores<SkillSnapContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();
app.MapControllers();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
  app.MapOpenApi();
}
app.UseCors("AllowClient");

app.UseHttpsRedirection();

app.Run();
