using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityDbContext>(options => options.UseInMemoryDatabase("AuthDemoDb"));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
  .AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorizationBuilder();

var app = builder.Build();

app.MapGet("/", () => "I am root!");

app.MapGet("/admin-only", () => "Admin access only!")
  .RequireAuthorization();

app.MapGet("/user-claims-check", () => "User!")
   .RequireAuthorization();

var roles = new[] { "Admin", "User" };

app.Run();