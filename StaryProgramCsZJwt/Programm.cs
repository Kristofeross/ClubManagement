using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ClubManagement.ApplicationDbContext;
using ClubManagement.Models;
using ClubManagement.Services;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using ClubManagement.Models.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Builder;
using ClubManagement;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ClubManagement.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*builder.Services.AddControllersWithViews();*/
builder.Services.AddControllersWithViews().AddFluentValidation();
// Do swaggera
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Do Autentykacji
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});
// Do EntityFramework
builder.Services.AddDbContext<ClubDbContext>(options => options.UseSqlServer("name=ConnectionStrings:con"));
// Do Autentykacji cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // tutaj dodaj �cie�k� na kt�r� zostanie przekierowany u�ytkownik je�li spr�buje dosta� si� do zasob�w bez uwierzytelnienia
        options.LoginPath = "/User/Login";
    });
/*Podsumowuj�c, kod ten definiuje dwie polityki autoryzacji: jedn� wymagaj�c�, aby u�ytkownik by� zalogowany (RequireLoggedIn), a drug� wymagaj�c�, aby u�ytkownik by� zalogowany i mia� przypisan� rol� "Admin" (AdminAccess). Te polityki s� wykorzystywane do kontrolowania dost�pu u�ytkownik�w do r�nych zasob�w w aplikacji, w zale�no�ci od ich stanu uwierzytelnienia i roli.*/
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireLoggedIn", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
    options.AddPolicy("AdminAccess", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Admin");
    });
});

// Nie wiem czy na pewno z filmik�w
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
builder.Services.AddScoped<IValidator<AccountRegisterDto>, AccountRegisterDtoValidator>();
// Do wywalania wyj�tk� ale nie dzia�a
builder.Services.AddScoped<ErrorHandlingMiddleware>();
// koniec

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

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
