using DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using ProductionApi.Authentication;
using ProductionApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ProductionDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("WebApiContextConnection")));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IFacilityRepository, FacilityRepository>();
builder.Services.AddScoped<IEquipmentTypeRepository, EquipmentTypeRepository>();

builder.Services.AddScoped<ContractService>();

builder.Services.AddSingleton<BackgroundProcessingService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<BackgroundProcessingService>());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ApiKeyAuthMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
