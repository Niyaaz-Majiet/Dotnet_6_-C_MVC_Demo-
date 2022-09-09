using Microsoft.EntityFrameworkCore;
using Product.Registry.Admin.Data;
using Product.Registry.Admin.Logic;
using Product.Registry.Admin.Repository;
using Product.Registry.Admin.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ProductContext>();
builder.Services.AddDbContext<ProductRegistryContext>();

builder.Services.AddDefaultIdentity<AdminUser>(options => {
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0987654321-._@+";
    options.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<ProductRegistryContext>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductLogic, ProductLogic>();
var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    var ctx = services.GetRequiredService<ProductContext>();
    ctx.Database.Migrate();

    var userCtx = services.GetRequiredService<ProductRegistryContext>();
    userCtx.Database.Migrate(); 

    if (app.Environment.IsDevelopment()) {
        ctx.SeedInitialData();
    }
}

app.UseExceptionHandler("/Products/Error");
app.UseHsts();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();
