using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

var Services=builder.Services;
// Add services to the container.

Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
Services.AddEndpointsApiExplorer();
Services.AddSwaggerGen();
Services.AddAutoMapper(typeof(UserMappings));

var conntextionString = builder.Configuration.GetConnectionString("DefaultConnection");
Services.AddEntityFrameworkNpgsql().AddDbContext<TodoDbContext>(options=>options.UseNpgsql(conntextionString));

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
