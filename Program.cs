using Microsoft.EntityFrameworkCore;
using Project.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

//govori nasem db contextu kako da se poveze sa bazom
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); 

//dependency injection za db context, da bi mogli da koristimo db context u controllerima i drugim delovima aplikacije
//konfigurisemo identity da koristi nas db context za cuvanje korisnickih podataka
builder.Services.AddDefaultIdentity<ApplicationUser>(options => 
    options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>(); 

builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages(); //dodajemo podrsku za razor pages, jer identity koristi razor pages za login, register i druge funkcionalnosti vezane za korisnike
app.Run();

