using AspNetCore.Identity.Mongo;
using Backend.Data;
using Backend.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Backend.Identity;
using Microsoft.AspNetCore.Identity;
using Backend.Utilities;
using Backend.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddSingleton<JwtTokenGenerator>();
builder.Services.AddSingleton<OtpService>();
builder.Services.AddSingleton<GoogleLogin>();

builder.Services.AddIdentityMongoDbProvider<ApplicationUser, ApplicationRole>(
    identityOptions =>
    {
        identityOptions.Password.RequiredLength = 6;
        identityOptions.User.RequireUniqueEmail = true;
    },
    mongoIdentityOptions =>
    {
        mongoIdentityOptions.ConnectionString = builder.Configuration["ConnectionStrings:DbConnection"];
    }
);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowCredentials()
            .AllowAnyHeader());
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowFrontend");

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var config = services.GetRequiredService<IConfiguration>();
    await Seeder.SeedRolesAndAdminAsync(services, config);
}


app.Run();


