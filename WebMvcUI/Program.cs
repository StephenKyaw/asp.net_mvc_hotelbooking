using Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddApplicationServices();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/admin/account/login";
    options.Cookie.Name = "PandaHotelBookingCookie";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/admin/account/login";
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var userManager = scopedProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scopedProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var identityContext = scopedProvider.GetRequiredService<ApplicationDbContext>();
        await IdentitySeed.SeedAsync(identityContext, userManager, roleManager);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}");


app.MapAreaControllerRoute(name: "Admin", areaName: "Admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{area=admin}/{controller=dashboard}/{action=index}/{id?}");




app.Run();
