using Hotel.Application.Interfaces.Commons;
using Hotel.Application.Interfaces.Repositories;
using Hotel.Application.Interfaces.Services;
using Hotel.Application.Services;
using Hotel.Infrastructure.DbContext;
using Hotel.Infrastructure.Repositories;
using Hotel.Infrastructure.Repositories.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Hotel.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ===============================
// DbContext PostgreSQL
// ===============================
builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DBConn")
    )
);

// ===============================
// Registro de DIs
// ===============================
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IRoomAmenitiesImagesRepository, RoomAmenitiesImagesRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomAmenitiesImagesService, RoomAmenitiesImagesService>();
builder.Services.AddScoped<IAmenityService, AmenityService>();
builder.Services.AddScoped<IReservationStatusService, ReservationStatusService>();
builder.Services.AddScoped<IPaymentStatusService, PaymentStatusService>();

// ===============================
// CORS
// ===============================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// ===============================
// Controllers
// ===============================
builder.Services.AddControllers();

// ===============================
// API Versioning (URL)
// ===============================
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// ===============================
// OpenAPI (opcional)
// ===============================
builder.Services.AddOpenApi();

var app = builder.Build();

// ===============================
// Middleware de Manejo de Errores 
// ===============================
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAngularApp");

app.UseAuthorization();
app.MapControllers();

app.Run();
