using Microsoft.EntityFrameworkCore;
using SistemaCarcel.Models;
using Rotativa.AspNetCore;
using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

// Registrar DbContext con la cadena de conexión
builder.Services.AddDbContext<ProyectoCarcelContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Habilitar sesión y MVC
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Cifrar contraseñas si no lo están
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProyectoCarcelContext>();
    var usuarios = context.Users.ToList();

    foreach (var user in usuarios)
    {
        if (!user.PasswordHash.StartsWith("$2"))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        }
    }

    context.SaveChanges(); 
}

// Configurar pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cuenta}/{action=Login}/{id?}");

RotativaConfiguration.Setup("wwwroot", "Rotativa");

app.Run();
