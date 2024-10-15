using BoxnMove.Business.Services.Account;
using BoxnMove.Business.Services.Interface;
using BoxnMove.Business.Services.Order;
using BoxnMove.Business.Services.Shared;
using BoxnMove.Database;
using BoxnMove.Shared.Utilities;
using BoxnMoveAPI.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("BoxnMoveConnection");

// DBContext
builder.Services.AddDbContext<BoxnMoveDBContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Authentication
string key = builder.Configuration.GetSection("Jwt:Key").Value ?? "";
AuthenticationBuilder authenticationBuilder = builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        //RoleClaimType = ClaimTypes.Role,
        RoleClaimType = "RoleName",
    };
});

builder.Services.AddScoped<IAuthorizationHandler, CustomAuthorizationHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CustomPolicy", policy =>
    policy.Requirements.Add(new CustomAuthRequirement("CustomClaimType", "CustomClaimValue", "RequiredPermission")));

    options.AddPolicy("RequireCustomerRole", policy =>
    policy.RequireClaim("RoleName", "Customer"));
});
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

builder.Services.AddHttpClient();
// Business Containers
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISharedService, SharedService>();
builder.Services.AddScoped<SMSService>();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.UseMemberCasing();
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
}
);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500; 

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        // Log the exception
        Log.Error(exception, "Unhandled exception: {Message}", exception?.Message);
        await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
    });
});

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 401)
    {
        Log.Error("Unhandled exception: {Message}", "401");
    }
});
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Unhandled exception: {Message}", ex.Message);
        throw;
    }
});

app.MapControllers();
app.Run();
