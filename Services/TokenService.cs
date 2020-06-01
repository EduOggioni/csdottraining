using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using csdottraining.Models;

namespace csdottraining.Services
{
  public class TokenService : ITokenService
  {
    private readonly ISettings _settings;

    public TokenService(ISettings settings)
    {
      _settings = settings;

    }

    public string GenerateToken(User user)
    {
      var tokenHadler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_settings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
        {
          new Claim(ClaimTypes.Name, user.email)
        }),
        Expires = DateTime.UtcNow.AddSeconds(_settings.Expires),
        SigningCredentials = new SigningCredentials(
          new SymmetricSecurityKey(key),
          SecurityAlgorithms.HmacSha256Signature
        )
      };
      var token = tokenHadler.CreateToken(tokenDescriptor);
      return tokenHadler.WriteToken(token);
    }
  }

  public interface ITokenService {
    string GenerateToken(User user);

  }
}