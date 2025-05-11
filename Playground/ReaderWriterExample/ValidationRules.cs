using Monads;
using Playground.ReaderWriterExample;

public static class ValidationRules
{
    public static Reader<UserValidationConfig, Writer<string, string>> ValidateUsername(string username) =>
        config =>
        {
            var logs = new List<string>();
            if (string.IsNullOrWhiteSpace(username))
                logs.Add("Username cannot be empty.");
            return () => (username, logs);
        };

    public static Reader<UserValidationConfig, Writer<string, string>> ValidatePassword(string password) =>
        config =>
        {
            var logs = new List<string>();
            if (password.Length < config.MinPasswordLength)
                logs.Add($"Password must be at least {config.MinPasswordLength} characters long.");
            return () => (password, logs);   
        };

    public static Reader<EmailValidationConfig, Writer<string, string>> ValidateEmail(string email) =>
        config =>
        {
            var logs = new List<string>();
            if (!email.EndsWith(config.AllowedEmailDomain))
                logs.Add($"Email must end with {config.AllowedEmailDomain}.");
            return () => (email, logs);
        };
}
