using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.J



using Microsoft.Extensions.Options;
using TodoApi.Data;
using TodoApi.MappingProfiles;
using TodoApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var Services = builder.Services;
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
Services.AddEndpointsApiExplorer();
Services.AddSwaggerGen();
Services.AddAutoMapper(typeof(UserMappings));
Services.AddAuthentication((options) =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        // ValidAudience = configuration["JWT:ValidAudience"]
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});
Services.AddTransient<IAuthService, AuthService>();

var conntextionString = builder.Configuration.GetConnectionString("DefaultConnection");
Services
    .AddEntityFrameworkNpgsql()
    .AddDbContext<TodoDbContext>(options => options.UseNpgsql(conntextionString));

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
