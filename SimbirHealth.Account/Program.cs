using Microsoft.EntityFrameworkCore;
using SimbirHealth.Account.Services.AuthenticationService;
using SimbirHealth.Common;
using SimbirHealth.Common.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure db context
builder.Services.AddDbContext<SimbirHealthContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("MyPrivateConnection")
        )
    );

#region DI
builder.Services.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
