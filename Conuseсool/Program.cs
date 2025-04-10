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

// Login
app.MapPost("/login", async ([FromBody] UserRequest user, IUserRepository repository, HttpContext context, IPasswordHasherService hasher, IUserService service) =>
{
    var currentUser = await service.GetUserByNameAsync(user.Name);

    if(currentUser != null && hasher.HashVerify(user.Password, currentUser.Password))
    {
        var Claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Name) };
        ClaimsIdentity claimsIdentity = new ClaimsIdentity(Claims, "Cookies");
        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        return Results.Json(new { success = true });
    }
    return Results.Unauthorized();

});
// Logout
app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync();
    context.Response.Redirect("/index.html");
});
// Register
app.MapPost("/register", async ([FromBody] UserRequest user, IUserService service, HttpContext context, IPasswordHasherService hasher) =>
{
    var allUsers = await service.GetUsers();
    if (allUsers.Any(x => x.Name == user.Name))
    {
        return Results.BadRequest("User already exists");
    }
    service.AddUser(new User { Name = user.Name, Password = hasher.HashPassword(user.Password)});
    return Results.Ok("Added new user!");
});

//Colections
app.MapGet("/collection", [Authorize] async (HttpContext context, IUserRepository repository, IUserService service) =>
{
    var userName = context.User.Identity?.Name;
    var currentUser = await service.GetUserByNameAsync(userName);

    var list = currentUser?.Collections.Select(x => new CollectionDTO(name: x.Name, cardList: x.CardList, createdTime: x.CreatedTime, id: x.Id));

    await context.Response.WriteAsJsonAsync(new { collections = list, name = context.User.Identity?.Name});

});
app.MapPost("/collection", async ([FromBody] CollectionRequest collection, HttpContext context, ICollectionService collectionService,  IUserService userService) =>
{
    var userName = context.User.Identity?.Name;
    var currentUser = await userService.GetUserByNameAsync(userName);
    if (currentUser.Collections.Any(x => x.Name == collection.Name))
    {
        await context.Response.WriteAsJsonAsync(new {message="This item already exists!"});
        return;

    }
    await collectionService.UpdateCollection(currentUser, new CardCollection { Name = collection.Name, CreatedTime = DateTime.UtcNow });

    var newCollection = currentUser.Collections.Select(x => new CollectionDTO(name: x.Name, cardList: x.CardList, createdTime: x.CreatedTime, id: x.Id)).FirstOrDefault(x => x.Name == collection.Name);

    await context.Response.WriteAsJsonAsync(new { message = "item added", item = newCollection});
});
app.MapDelete("/collection/{id}", async (int id, ICollectionService collectionService, IUserService userService, HttpContext context) =>
{

    var userName = context.User.Identity?.Name;
    var currentUser = await userService.GetUserByNameAsync(userName);
    collectionService.DeleteCollection(currentUser, id);

});
app.MapGet("/collection/{id}/{sort=list}", [Authorize] async (int id, ICollectionService collectionService, IUserService userService, HttpContext context, string sort, ICollectionSortService sortService) =>
{
    //1 collection for game
    //2 new api request
    var userName = context.User.Identity?.Name;
    var currentUser = await userService.GetUserByNameAsync(userName);
    CardCollection collectionForSort = await collectionService.GetCollection(currentUser, id);
    if (sort == "list")
    {
        return await collectionService.GetCollection(currentUser, id);

    }
    return await sortService.SortForPlay(collectionForSort);

    //оцей результат треба буде повернути
    //отрмиати колекцію, прокрутити через сортувальений сервіс і повернути
});
app.MapPatch("/collection", async(int id, Card card, HttpContext context, IUserService service) =>
{
    var userName = context.User.Identity?.Name;
    var currentUser = await service.GetUserByNameAsync(userName);
    currentUser.Collections.FirstOrDefault(x => x.Id == id).CardList.Add(card);
});
app.MapPost("/words/{id}", async ([FromBody]CardRequest request, HttpContext context, int id, ICollectionService collectionService, IUserService userService) =>
{
    var userName = context.User.Identity?.Name;
    var currentUser = await userService.GetUserByNameAsync(userName);

    var collection = currentUser?.Collections?.FirstOrDefault(x => x.Id == id);
    var card = new Card {BackSideText = request.BackSideText, FrontSideText = request.FrontSideText, Decsription = request.Decsription, Priority = request.Priority, CreatedTime = DateTime.UtcNow};

    collectionService.AddCardToCollecton(currentUser, card, id);
});

app.MapPatch("/words/{id}", async (Card card, IUserService userService, HttpContext context, int id, ICollectionSortService sortService) =>
{
    var userName = context.User.Identity?.Name;
    var currentUser = await userService.GetUserByNameAsync(userName);

    var oldWord = currentUser.Collections.FirstOrDefault(x => x.Id == id).CardList.FirstOrDefault(x => x.Id == card.Id);
    oldWord.Priority = card.Priority;
    oldWord.ExpiresTime = await sortService.ExpiresDate(card);
    // Дописати алгортим, карточки повинні появлятись якщо їх мало

});
app.Run();
