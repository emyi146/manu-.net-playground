namespace Auth.WithClaims;

public class AuthSchemes
{
    public const string LocalCookieAuthSchema = "local-cookie";
    public const string VisitorCookieAuthSchema = "visitor-cookie";
    public const string PresidentOauthAuthSchema = "president-oauth";   // This schema represents the OAuth protocol
    public const string PresidentCookieAuthSchema = "president-cookie";  // This schema represents the cookie to store the result from OAuth
}

public class AuthPolicies
{
    public const string AmericanLocalPassportPolicy = "american passport";
    public const string AmericanVisitorPolicy = "american visitor";
    public const string AmericanPresidentPolicy = "american president";
    public const string EuropeanLocalPassportPolicy = "eu passport";
}