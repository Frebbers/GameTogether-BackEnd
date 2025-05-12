namespace GameTogetherAPI.Test.Util;

public static class Constants
{
    internal const string TestUserEmail = "user@example.com";
    internal const string TestUserPassword = "password123";
    internal const string TestUserName = "TestUser";
    internal const string WeakPassword = "weak";
    internal const string InvalidEmail = "invalid-email";
    internal const string NonExistentEmail = "user1@example.com";
    internal const string WrongPassword = "wrongpassword123";
    internal static readonly Dictionary<string, string> TestEnvironment = new()
    {
        {"JwtSettings:SecretKey", "TestSecretKeyWithAtLeast32Characters!!"},
        {"JwtSettings:Issuer", "TestIssuer"},
        {"JwtSettings:Audience", "TestAudience"},
        {"ASPNETCORE_ENVIRONMENT", "Development"}
    };
}