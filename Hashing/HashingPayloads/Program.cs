using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

/*
 * Given a class or JSON payload, generate a token that contains:
 * Expiration time
 * Hashed representation of the payload, to avoid manipulation of the token or payload
 * 
 * This code is a simulation of a process where a user submits an initial payload for validation.
 * If the payload is invalid, the user receives a recommended payload with a validation token.
 * The user must resubmit the payload with the validation token to save the data without revalidating it with a third-party API.
 * It uses a JWT token to validate the recommended payload and the token submitted by the user.
 * The JWT token is hashed and concatenated with the expiration date to create a validation hash.
 * The validation hash is compared with the token submitted by the user to validate the token.
 */

var tokenService = new TokenService("Yp2s5v8y/B?E(H+MbQeThWmZq4t6w9z$");

// Simulate user submitting an initial payload
var initialPayload = new PayloadDto
{
    City = "Niu York",
    Country = "US"
};

// Step 1: Try to store the invalid payload
var validationResult = TryStorePayload(initialPayload);
Console.WriteLine($"1st attempt, storing an invalid payload: {validationResult}");          // Invalid

// Step 2: Try to store with the recommended payload
validationResult = TryStorePayload(validationResult.RecommendedPayload);
Console.WriteLine($"2nd attempt, storing the recommended payload: {validationResult}");     // Valid

// Step 3: Try to manipulated payload
var manipulatedPayload = validationResult.RecommendedPayload with { City = "Another city" };
validationResult = TryStorePayload(manipulatedPayload);     // Invalid
Console.WriteLine($"3rd attempt, storing a manipulated payload: {validationResult}");        // Valid

// Step 4: Try to store an expired token
var expiredToken = tokenService.GenerateToken(validationResult.RecommendedPayload, DateTime.UtcNow.AddSeconds(-1));
validationResult = TryStorePayload(validationResult.RecommendedPayload with { ValidationToken = expiredToken });
Console.WriteLine($"4th attempt, storing an expired token: {validationResult}");            // Valid (because fallback to revalidate)


ValidationResult TryStorePayload(PayloadDto payload)
{
    var token = payload.ValidationToken;
    if (token is null)
    {
        var validatedPayload = GetValidatedAddressFromThirdPartyApi(payload);
        token = tokenService.GenerateToken(validatedPayload, DateTime.UtcNow.AddSeconds(15));
        return new ValidationResult
        {
            IsValid = validatedPayload == payload,
            RecommendedPayload = validatedPayload with { ValidationToken = token }
        };
    }

    var payloadWithoutToken = payload with { ValidationToken = null };

    bool isValid = tokenService.ValidateToken(payloadWithoutToken, token);
    if (!isValid)
        return TryStorePayload(payloadWithoutToken);

    return new ValidationResult
    {
        IsValid = true,
        RecommendedPayload = payload
    };
}


static PayloadDto GetValidatedAddressFromThirdPartyApi(PayloadDto payload)
{
    // Simulate calling a third-party API
    return new PayloadDto
    {
        City = "New York",
        Country = "US",
    };
}

public class TokenService
{
    private readonly string _secretKey;

    public TokenService(string secretKey)
    {
        _secretKey = secretKey;
    }

    public string GenerateToken<T>(T recommendedPayload, DateTime expirationTime)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var payloadJson = JsonSerializer.Serialize(recommendedPayload);

        var claims = new[]
        {
                new Claim("payload", payloadJson)
            };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expirationTime,
            signingCredentials: creds);

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        // Hash the JWT
        using (var sha256 = SHA256.Create())
        {
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(jwtToken));
            var hashedToken = Convert.ToBase64String(hashBytes);
            var expirationDateString = expirationTime.ToString("o"); // ISO 8601 format
            return $"{hashedToken}|{expirationDateString}";

        }
    }


    public bool ValidateToken<T>(T recommendedPayload, string token)
    {
        // Regenerate the JWT token
        var tokenParts = token.Split('|');
        var expirationDateString = tokenParts[1];

        if (!DateTime.TryParse(expirationDateString, null, System.Globalization.DateTimeStyles.RoundtripKind, out var expirationDate))
        {
            return false;
        }

        // Check if the token has expired
        if (DateTime.UtcNow > expirationDate)
        {
            return false;
        }

        var validationHash = GenerateToken(recommendedPayload, expirationDate);
        return validationHash == token;
    }

}

public record PayloadDto
{
    public string City { get; init; }
    public string Country { get; init; }
    public string? ValidationToken { get; init; } // New property for the validation token
}

public record ValidationResult
{
    public bool IsValid { get; set; }
    public PayloadDto? RecommendedPayload { get; set; }
}