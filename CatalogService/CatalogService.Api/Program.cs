using CatalogService.Api.Models;
using CatalogService.Api.Models.Interfaces;
using CatalogService.Api.Setup;

var builder = WebApplication.CreateBuilder(args);

// The following line enables Application Insights telemetry collection.
builder.Services.AddApplicationInsightsTelemetry(opt => { opt.EnableAdaptiveSampling = false; opt.EnableDebugLogger = true; });

// Add services to the container.
builder.Services.ConfigureUrlHelper();
builder.Services.AddScoped<IItemResourceBuilder, ItemResourceBuilder>();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.ConfigureServiceBus(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigurSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
