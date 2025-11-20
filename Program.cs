using Microsoft.EntityFrameworkCore;
using APIAutoservice156.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<APIAutoservice156.Repositories.IClientsRepository, APIAutoservice156.Repositories.ClientsRepository>();
builder.Services.AddScoped<APIAutoservice156.Repositories.IVehicleRepository, APIAutoservice156.Repositories.VehicleRepository>();
builder.Services.AddScoped<APIAutoservice156.Repositories.IServiceRepository, APIAutoservice156.Repositories.ServiceRepository>();

// Services  
builder.Services.AddScoped<APIAutoservice156.Services.IAuthService, APIAutoservice156.Services.AuthService>();
builder.Services.AddScoped<APIAutoservice156.Services.IClientsService, APIAutoservice156.Services.ClientsService>();
builder.Services.AddScoped<APIAutoservice156.Services.IVehiclesService, APIAutoservice156.Services.VehiclesService>();
builder.Services.AddScoped<APIAutoservice156.Services.IServicesService, APIAutoservice156.Services.ServicesService>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<APIAutoservice156.Services.IAuthService, APIAutoservice156.Services.AuthService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        context.Database.EnsureCreated();

        DbInitializer.Initialize(context);

        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Database created and seeded successfully!");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, " An error occurred while creating the database.");
    }
}

app.Run();