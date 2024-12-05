using Microsoft.EntityFrameworkCore;
using NZWalk.API.Repositories;
using NZWalk.Data;
using NZWalk.Mappings;
using NZWalk.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NZWalkDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalkConnectionString")));
builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>(); //Indicates that SQLRegionRepository is the implementation of IRegionRepository.
                                                                      //This "SQLRegionRepository" can be changed to some other implementation like "InMemoryRegionRepository" based on requirement.

builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
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
