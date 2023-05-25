using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using NetDevPack.Security.Jwt.Core.Jwa;

namespace TokenManager;

public static class TokenServiceCollectionExtensions
{
    public static IServiceCollection AddTokenServices(this IServiceCollection services, TokenConfigurationProvider tokenConfigurationProvider)
    {
        services.AddMemoryCache();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = tokenConfigurationProvider.Issuer,
                    ValidateIssuer = true,
                    ValidAudience = tokenConfigurationProvider.Audience,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                };
            });
        services.AddAuthorization();

        //TODO: add configuration from settings
        services.AddJwksManager(opt =>
        {
            opt.Jwe = Algorithm.Create(AlgorithmType.RSA, JwtType.Jwe);
            opt.Jws = Algorithm.Create(AlgorithmType.RSA, JwtType.Jws);
            opt.DaysUntilExpire = tokenConfigurationProvider.SigningCertificateExpirationTimeInDays;
        }).UseJwtValidation();

        services.AddScoped<TokenConstructor>(x => new (x.GetRequiredService<IJwtService>(), tokenConfigurationProvider));

        services.AddHttpContextAccessor();
        services.AddScoped<TokenDeconstructor>();

        return services;
    }
}

