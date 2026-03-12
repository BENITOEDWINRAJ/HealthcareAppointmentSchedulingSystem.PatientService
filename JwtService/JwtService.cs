using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PatientService.Core.Entities;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;      

    public JwtService(IConfiguration config)
    {
        _config = config;
    }
    

    public string GenerateToken(User user)
    {
        var jwtKey = _config["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("JWT key is not configured.");
        }

        var claims = new[]
        {
           // new Claim("userid", user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Aud, "MyAppUsers")
        };

        var creds = new SigningCredentials(new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)
        ), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],          
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: creds
        );

         return new JwtSecurityTokenHandler().WriteToken(token);        
    }
}

public interface IJwtService
{
    string GenerateToken(User user);
}