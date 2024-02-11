using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace server.Utilities;

public static class JwtGenerator
{

   /* private static readonly IConfiguration configuration;

    // static constructor
    static JwtGenerator()
    {
        configuration = new ConfigurationBuilder()
                                              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                              .Build();
    }
 */
    public static string GenerateUserToken(string userName)
    {
        var claims = new Claim[]
        {
        new(ClaimTypes.Name, userName)
        };
        return GenerateToken(claims, DateTime.UtcNow.AddDays(1));
    }

    private static string GenerateToken(Claim[] claims, DateTime expires)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = Environment.GetEnvironmentVariable("JWT_SECRET");
        var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");

        var key = Encoding.ASCII.GetBytes(secret!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
