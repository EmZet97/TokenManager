using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using Newtonsoft.Json;
using System.Collections.Immutable;

namespace TokenManager;

public class TokenConstructor
{
    private readonly IJwtService _jwtService;
    private readonly TokenConfigurationProvider _tokenConfigurationProvider;

    public TokenConstructor(IJwtService jwtService, TokenConfigurationProvider tokenConfigurationProvider)
    {
        _jwtService = jwtService;
        _tokenConfigurationProvider = tokenConfigurationProvider;
    }

    public async Task<Token> GenerateTokenAsync(IEnumerable<Claim> tokenClaims)
    {
        var tokenHandler = new JsonWebTokenHandler();
        var key = await _jwtService.GetCurrentSigningCredentials();

        var claims = tokenClaims.ToImmutableDictionary(x => x.Name, x => JsonConvert.SerializeObject(x.Value) as object);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _tokenConfigurationProvider.Issuer,
            Audience = _tokenConfigurationProvider.Audience,

            Claims = claims,

            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow + TimeSpan.FromMinutes(_tokenConfigurationProvider.ExpirationTimeInMinutes),

            SigningCredentials = key,
        };

        return new Token(tokenHandler.CreateToken(tokenDescriptor));
    }
}
