using SkillSnap.Components;
using Microsoft.EntityFrameworkCore;
using SkillSnap.Data;
using SkillSnap.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<SkillSnapContext>(options => {
  options.UseSqlServer(builder.Configuration.GetConnectionString("SkillSnapContext"));
});

builder.Services.AddScoped<PortfolioUserService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<SkillService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
