using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Commons.Models;
using Microsoft.IdentityModel.Tokens;

namespace Services.Authentication;

public static class AuthenticationHelpers
{
    public static string GenerateJwtToken(Account user, string bearerKey)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(bearerKey);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = AssembleClaimsIdentity(user),
            Expires = DateTime.UtcNow.AddYears(10),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static ClaimsIdentity AssembleClaimsIdentity(Account account)
    {
        var subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) });
        return subject;
    }

    public static void GenerateSaltAndHash(this Account account)
    {
        var salt = GenerateSalt();
        account.PasswordSalt = Convert.ToBase64String(salt);
        account.PasswordHash = ComputeHash(account.PasswordHash, account.PasswordSalt);
    }

    private static byte[] GenerateSalt()
    {
        var rng = RandomNumberGenerator.Create();
        var salt = new byte[24];
        rng.GetBytes(salt);
        return salt;
    }

    public static string ComputeHash(string password, string saltString)
    {
        var salt = Convert.FromBase64String(saltString);

        using var hashGenerator = new Rfc2898DeriveBytes(password, salt, 10101);
        var bytes = hashGenerator.GetBytes(24);
        return Convert.ToBase64String(bytes);
    }
}