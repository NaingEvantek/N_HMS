using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
using N_HMS.Database;
using N_HMS.Interfaces;
using N_HMS.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

//var password = "P@ssword123";
//var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
}); 

builder.Services.AddDbContext<N_HMSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<ILicenseService,LicenseService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFloorService, FloorService>();
builder.Services.AddScoped<IGuestService, GuestService>();
builder.Services.AddScoped<IRoomService, RoomService>();

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new Exception("JWT key missing");
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", policy =>
//    {
//        policy.AllowAnyOrigin()
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});

var Issuer = builder.Configuration["Jwt:Issuer"]?? throw new InvalidOperationException("Jwt:Issuer is missing in configuration");
// Add CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalUI", policy =>
    {
        policy.WithOrigins(Issuer)  // UI domain
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidIssuer = Issuer,
        ValidateAudience = false,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//app.UseCors("AllowAll");
app.UseCors("LocalUI");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
