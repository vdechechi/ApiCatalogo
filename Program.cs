using ApiCatalogo.Context;
using ApiCatalogo.DTO.Mappings;
using ApiCatalogo.Extensions;
using ApiCatalogo.Filters;
using ApiCatalogo.Repositorys.Categorias;
using ApiCatalogo.Repositorys.Generico;
using ApiCatalogo.Repositorys.Produtos;
using ApiCatalogo.Repositorys.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiCatalogo.Models;
using ApiCatalogo.Services;
using APICatalogo.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var secretKey = builder.Configuration["JWT:SecretKey"]
    ?? throw new ArgumentException("Invalid secret key!!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey))
    };
});


string? mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(mySqlConnection,
                    ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));

}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddNewtonsoftJson();

builder.Services.AddScoped<ICategoriasRepository, CategoriasRepository>();
builder.Services.AddScoped<IProdutosRepository, ProdutosRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddAutoMapper(typeof(DtoMappingProfile));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}
app.ConfigureExceptionHandler();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
