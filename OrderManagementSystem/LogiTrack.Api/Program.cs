using LogiTrack.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with SQLite configuration
builder.Services.AddDbContext<LogiTrackContext>(options =>
    options.UseSqlite("Data Source=logitrack.db"));

// Add Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<LogiTrackContext>()
    .AddDefaultTokenProviders();

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
      options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
      };
    });

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed roles and admin user
using(var scope = app.Services.CreateScope()) {
  var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
  var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

  // Create "Manager" role if it doesn't exist
  if(!await roleManager.RoleExistsAsync("Manager")) {
    await roleManager.CreateAsync(new IdentityRole("Manager"));
  }

  // Create an admin user and assign the "Manager" role
  var adminEmail = "admin@logitrack.com";
  var adminUser = await userManager.FindByEmailAsync(adminEmail);
  if(adminUser == null) {
    adminUser = new ApplicationUser {
      UserName = adminEmail,
      Email = adminEmail,
      FirstName = "Admin",
      LastName = "User"
    };
    await userManager.CreateAsync(adminUser, "Admin@123");
    await userManager.AddToRoleAsync(adminUser, "Manager");
  }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
