using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Claims;

namespace TokenManager;

public class TokenDeconstructor
{
    private ClaimsPrincipal Claims { get; init; }

    public TokenDeconstructor(IHttpContextAccessor accessor)
    {
        Claims = accessor.HttpContext!.User;
    }

    public bool TryExtractClaim<T>(out T extractedClaim, string claimName)
    {
        var jsonValue = Claims.FindFirstValue(claimName);

        if (jsonValue is null)
        {
            extractedClaim = default!;

            return false;
        }

        extractedClaim = GetValue<T>(jsonValue) ?? default!;

        return extractedClaim is not null;
    }

    private static T? GetValue<T>(string jsonValue)
    {
        return JsonConvert.DeserializeObject<T>(jsonValue);
    }
}
