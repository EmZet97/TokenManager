using Microsoft.AspNetCore.Builder;

namespace TokenManager;

public static class TokenWebApplicationBuilderExtensions
{
    public static WebApplication AddTokenServices(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}

