using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_GarroRojasRosa.Data;
using ProyectoFinal_GarroRojasRosa.Models;

var builder = WebApplication.CreateBuilder(args);

// ===============================
// CONEXIÓN A LA BASE DE DATOS
// ===============================

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


// ===============================
// ASP.NET IDENTITY + ROLES
// ===============================

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();


// ===============================
// MVC
// ===============================

builder.Services.AddControllersWithViews();


var app = builder.Build();


// ===============================
// CREAR ROLES AUTOMÁTICAMENTE
// ===============================

using (var scope = app.Services.CreateScope())
{
    var roleManager =
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var userManager =
        scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles =
    {
        "Administrador",
        "Estudiante"
    };

    foreach (var rol in roles)
    {
        if (!await roleManager.RoleExistsAsync(rol))
        {
            await roleManager.CreateAsync(
                new IdentityRole(rol));
        }
    }

    // Crear usuario administrador inicial
    string correoAdmin = "admin@universidad.com";
    string claveAdmin = "Admin123!";

    var administrador =
        await userManager.FindByEmailAsync(correoAdmin);

    if (administrador == null)
    {
        administrador = new ApplicationUser
        {
            UserName = correoAdmin,
            Email = correoAdmin,
            Nombre = "Administrador",
            Apellido = "Sistema",
            EmailConfirmed = true
        };

        var resultado =
            await userManager.CreateAsync(
                administrador,
                claveAdmin);

        if (resultado.Succeeded)
        {
            await userManager.AddToRoleAsync(
                administrador,
                "Administrador");
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(
            administrador,
            "Administrador"))
        {
            await userManager.AddToRoleAsync(
                administrador,
                "Administrador");
        }
    }
}

// ===============================
// PIPELINE
// ===============================

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();