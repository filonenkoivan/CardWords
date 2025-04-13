using Api.Models;
using Api.Models.DTOs;
using Application.Configuration;
using Application.Extentsions;
using Application.Interfaces;
using Application.Services;
using Domain.Models;
using Infrastructure.AppDataContext;
using Infrastructure.Interfaces;
using Infrastructure.Providers;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICollectionService, CollectionService>();
builder.Services.AddScoped<ICollectionRepository, CollectionRepository>();
builder.Services.AddScoped<ICollectionSortService, CollectionSortService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<JwtProvider>();
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => {
    var security = new OpenApiSecurityScheme()
    {
        Name = HeaderNames.Authorization,
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "JWT Authorization header",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    x.AddSecurityDefinition(security.Reference.Id, security);
    x.AddSecurityRequirement(new OpenApiSecurityRequirement { { security, Array.Empty<string>() } });
});


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


app.Use(async (context, next) =>
{

    var path = context.Request.Path.ToString().ToLower();
    if (path == "/")
    {
        if (context.User.Identity.IsAuthenticated == true)
        {
            context.Response.Redirect("/profile.html");
        }
        else
        {
            context.Response.Redirect("/login.html");
        }
        return;
    }
    await next();

});

app.MapControllers();
//Redirect from login to main page
//app.MapGet("", (HttpContext context) =>
//{
//    Console.WriteLine("/");
//    if (context?.User?.Identity?.Name == null)
//    {
//        return Results.Redirect("/login");
//    }
//    return Results.Redirect("/profile.html");
//});

//app.MapGet("/login.html", () => Results.Redirect("/index.html"));


//// Authentication check
//app.MapGet("/index.html", async (HttpContext context) =>
//{
//    if (context?.User?.Identity?.IsAuthenticated == true)
//    {
//        context.Response.Redirect("profile.html");
//    }
//    else
//    {
//        context.Response.ContentType = "text/html; charset=utf-8";
//        await context.Response.SendFileAsync("wwwroot/index.html");
//    }
//});


app.Run();
