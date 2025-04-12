using Api.Models;
using Api.Models.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Models;
using Infrastructure.AppDataContext;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath = "/login");
builder.Services.AddAuthorization();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICollectionService, CollectionService>();
builder.Services.AddScoped<ICollectionRepository, CollectionRepository>();
builder.Services.AddScoped<ICollectionSortService, CollectionSortService>();
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();


string? connectionString = builder.Configuration.GetConnectionString("ConuseDb");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();



//Redirect from login to main page
app.MapGet("/", (HttpContext context) =>
{
    if(context?.User?.Identity?.Name == null)
    {
        return Results.Redirect("/login");
    }
    return Results.Redirect("/profile.html");
});
app.MapGet("/login", () => Results.Redirect("/index.html"));
app.MapGet("/login.html", () => Results.Redirect("/index.html"));


// Authentication check
app.MapGet("/index.html", async (HttpContext context) =>
{
    if (context?.User?.Identity?.IsAuthenticated == true)
    {
        context.Response.Redirect("profile.html");
    }
    else
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.SendFileAsync("wwwroot/index.html");
    }
});


app.Run();
