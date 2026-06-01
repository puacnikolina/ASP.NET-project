using Microsoft.EntityFrameworkCore;
using Project.Models;

var builder = WebApplication.CreateBuilder(args);

//govori nasem db contextu kako da se poveze sa bazom
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); 

//dependency injection za db context, da bi mogli da koristimo db context u controllerima i drugim delovima aplikacije
//konfigurisemo identity da koristi nas db context za cuvanje korisnickih podataka
builder.Services.AddDefaultIdentity<ApplicationUser>(options => 
    options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDbContext>();

//dodajemo podrsku za session, da bi mogli da cuvamo podatke o korpi u sessionu
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseStaticFiles();
app.UseSession();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}"); //mapira da se otvori Product page kao pocetna stranica - default je da se otvori HomeController
app.MapRazorPages(); //dodajemo podrsku za razor pages, jer identity koristi razor pages za login, register i druge funkcionalnosti vezane za korisnike

app.Run();

