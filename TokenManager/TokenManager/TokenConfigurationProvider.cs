using NetDevPack.Security.Jwt.Core.Jwa;

namespace TokenManager;

public record TokenConfigurationProvider(string Issuer = "localhost", string Audience = "localhost", int ExpirationTimeInMinutes = 60, int SigningCertificateExpirationTimeInDays = 90, AlgorithmType SigningCertificateAlgorithmType = AlgorithmType.RSA);

