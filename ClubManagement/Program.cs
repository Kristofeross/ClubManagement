using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ClubManagement.ApplicationDbContext;
using ClubManagement.Models;
using ClubManagement.Services;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
//using ClubManagement.Models.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Builder;
using ClubManagement;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ClubManagement.Middleware;

var builder = WebApplication.CreateBuilder(args);

/* Add services to the container.*/

builder.Services.AddControllersWithViews(); 
//builder.Services.AddControllersWithViews().AddFluentValidation(); // Ten jest z wykorzystaniem Fluent Validation
// Do Swaggera
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Do EntityFramework
builder.Services.AddDbContext<ClubDbContext>(options => options.UseSqlServer("name=ConnectionStrings:con"));
// Do Autentykacji cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // tutaj dodaj œcie¿kê na któr¹ zostanie przekierowany u¿ytkownik jeœli spróbuje dostaæ siê do zasobów bez uwierzytelnienia
        options.LoginPath = "/account/login";
        options.AccessDeniedPath = "/home/accessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });
/*Podsumowuj¹c, kod ten definiuje dwie polityki autoryzacji: jedn¹ wymagaj¹c¹, aby u¿ytkownik by³ zalogowany (RequireLoggedIn), a drug¹ wymagaj¹c¹, aby u¿ytkownik by³ zalogowany i mia³ przypisan¹ rolê "Admin" (AdminAccess). Te polityki s¹ wykorzystywane do kontrolowania dostêpu u¿ytkowników do ró¿nych zasobów w aplikacji, w zale¿noœci od ich stanu uwierzytelnienia i roli.*/

// Do autoryzacji cookie
builder.Services.AddAuthorization(options =>
{
    // Prawdopodobnie bêdzie skasowany LoggedInAccess
    options.AddPolicy("LoggedInAccess", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
    options.AddPolicy("PlayerAccess", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Player");
    });
    options.AddPolicy("CoachAccess", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Coach");
    });
    options.AddPolicy("AdminAccess", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Admin");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
