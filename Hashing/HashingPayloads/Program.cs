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
 * The user must resubmit the payload with the validation token to save the data without revalidating it with a  *third-party API.
 * It uses a JWT token to validate the recommended payload and the token submitted by the user.
 * The JWT token is hashed and concatenated with the expiration date to create a validation hash.
 * The validation hash is compared with the token submitted by the user to validate the token.
 */

var tokenService = new TokenService("2j55AFBjqr7zKVyElDXqD5YoW");

// Simulate user submitting an initial payload
var initialPayload = new ValidatedAddressDto
{
    AddressOne = "123 Main St",
    City = "Anytown2",
    ZipCode = "12345",
    CountryIsoCode = "US"
};

// Call third-party API for validation (simulated here)
var validationResult = ValidateWithThirdPartyApi(initialPayload);

if (validationResult.IsValid)
{
    // Save the data
    SaveData(initialPayload);
    Console.WriteLine("Initial payload saved.");
}
else
{
    var token = tokenService.GenerateToken(
        validationResult.RecommendedPayload, DateTime.UtcNow.AddSeconds(5));
    var payloadWithToken = validationResult.RecommendedPayload with { ValidationToken = token };

    // RETURN THIS TO USER

    // NOW WE RECEIVE THE payloadWithToken FROM THE USER

    var payloadWithoutToken = payloadWithToken with { ValidationToken = null };
    // Validate the token
    if (tokenService.ValidateToken(payloadWithoutToken, payloadWithToken.ValidationToken))
    {
        // Save the data without third-party API validation
        SaveData(payloadWithoutToken);
        Console.WriteLine("Resubmitted payload saved.");
    }
    else
    {
        Console.WriteLine("Invalid token or payload does not match the recommended payload.");
    }
}

static ValidationResult ValidateWithThirdPartyApi(ValidatedAddressDto payload)
{
    // Simulate third-party API validation
    var isValid = payload.City == "Anytown" && payload.ZipCode == "12345";

    if (isValid)
    {
        return new ValidationResult { IsValid = true };
    }
    else
    {
        var recommendedPayload = new ValidatedAddressDto
        {
            AddressOne = payload.AddressOne,
            AddressTwo = payload.AddressTwo,
            City = "Anytown",
            ZipCode = "12345",
            CountryIsoCode = "US",
            CountryDivisionIsoCode = payload.CountryDivisionIsoCode
        };
        return new ValidationResult { IsValid = false, RecommendedPayload = recommendedPayload };
    }
}

static void SaveData(ValidatedAddressDto payload)
{
    // Simulate saving data
    Console.WriteLine("Data saved:");
    Console.WriteLine(JsonSerializer.Serialize(payload));
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

public interface IPayloadWithValidationToken
{
    string ValidationToken { get; }
}

public record ValidatedAddressDto
{
    public string AddressOne { get; init; }
    public string? AddressTwo { get; init; }
    public string City { get; init; }
    public string ZipCode { get; init; }
    public string CountryIsoCode { get; init; }
    public string? CountryDivisionIsoCode { get; init; }
    public string? ValidationToken { get; init; } // New property for the validation token

}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public ValidatedAddressDto? RecommendedPayload { get; set; }
}




