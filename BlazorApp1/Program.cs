using BlazorApp1.Components;
using BlazorApp1.Data;
using BlazorApp1.Models;
using BlazorApp1.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// ===== Services =====

// Your scoped app state
builder.Services.AddScoped<TicketState>();

// Database context for Identity
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
// Identity with roles
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure cookies to redirect to your Blazor login page
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/loginpage";    // Your LoginPage.razor
    options.AccessDeniedPath = "/notfound"; // Optional: show notfound page if unauthorized
});

// Razor Components with interactive server render mode
builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents();

// Authorization policy example
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireManagerRole",
        policy => policy.RequireRole("SupportManager"));
});


var app = builder.Build();

// ===== Initialize DB with seed data =====
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await FillDB.InitializeAsync(services); // Make sure FillDB seeds roles/users
}

// ===== Middleware =====
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapPost("/login", async (
    HttpContext context,
    SignInManager<ApplicationUser> signInManager) =>
{
    var form = await context.Request.ReadFormAsync();
    var username = form["username"];
    var password = form["password"];

    var result = await signInManager.PasswordSignInAsync(
        username!,
        password!,
        true,
        false);

    if (result.Succeeded)
        return Results.Redirect("/");

    return Results.Redirect("/loginpage?error=1");
});

// ===== Endpoint mapping =====
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
