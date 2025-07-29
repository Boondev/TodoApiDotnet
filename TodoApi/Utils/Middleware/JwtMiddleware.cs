using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Utils.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Config _config;

    public JwtMiddleware(RequestDelegate next, IOptions<Config> config)
    {
        _next = next;
        _config = config.Value;
    }

    public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<TodoDbContext>();
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
            await AttachUserToContext(context, token, dbContext);

        await _next(context);
    }

    private async Task AttachUserToContext(
        HttpContext context,
        string token,
        TodoDbContext dbContext
    )
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.JwtConfig.Secret);
            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero,
                },
                out SecurityToken validatedToken
            );

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(
                jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value
            );

            //Attach user to context on successful JWT validation
            User user = dbContext.Users.First((x) => x.Id == userId);
            context.Items["User"] = user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //Do nothing if JWT validation fails
            // user is not attached to context so the request won't have access to secure routes
        }
    }
}
