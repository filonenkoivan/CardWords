using Api.Models;
using Api.Models.DTOs;
using Api.Validation;
using Application.Configuration;
using Application.Extentsions;
using Application.Interfaces;
using Application.Interfaces.Repository;
using Application.Services;
using Domain.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.AppDataContext;
using Infrastructure.Configuration;
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Security.Claims;

DotNetEnv.Env.Load("../.env");

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<AiConfiguration>(builder.Configuration);
builder.Services.Configure<ApiTranslationConfiguration>(builder.Configuration);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICollectionService, CollectionService>();
builder.Services.AddScoped<ICollectionRepository, CollectionRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IValidator<UserRequest>, CustomValidator>();
builder.Services.AddScoped<UserStatsService>();
builder.Services.AddScoped<JwtProvider>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IdentificationService>();
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddScoped<IValidator<UserRequest>, CustomValidator>();
builder.Services.AddScoped<IAiTranslateService, AiTranslateService>();
builder.Services.AddScoped<IApiTranslationService, ApiTranslationService>();
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<ICommunityRepository, CommunityRepository>();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen(x =>
{
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

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("react", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:3000", "http://localhost:3001");
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowCredentials();
    });
});

string? connectionString = builder.Configuration.GetValue<string>("DB_CONNECTION");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
app.UseRouting();

app.UseCors("react");

app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();


app.MapControllers();


app.Run();
