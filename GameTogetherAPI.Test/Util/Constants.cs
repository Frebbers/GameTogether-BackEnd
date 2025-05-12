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

    // User profile constants
    internal const int TestUserId = 1;
    internal const string TestUserDescription = "This is a test description";
    internal static readonly string TooLongDescription = new string('A', 5001);
    internal const string DescriptionWithLinks = "Check out my website at https://example.com";
    internal const string TestRegion = "Europe";
    
    // DateTime constants
    internal static readonly DateTime ValidBirthDate = DateTime.UtcNow.AddYears(-20);
    internal static readonly DateTime TooYoungBirthDate = DateTime.UtcNow.AddYears(-12);
    internal static readonly DateTime TooOldBirthDate = DateTime.UtcNow.AddYears(-140);
}
