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
    private TodoDbContext _context;

    public JwtMiddleware(RequestDelegate next, IOptions<Config> config, TodoDbContext context)
    {
        _next = next;
        _config = config.Value;
        _context = context;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            await AttachUserToContext(context, token);

        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, string token)
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
            context.Items["User"] = _context.Users.Where((x) => x.Id == userId);
        }
        catch
        {
            //Do nothing if JWT validation fails
            // user is not attached to context so the request won't have access to secure routes
        }
    }
}
