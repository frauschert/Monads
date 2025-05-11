using System;
using Monads;

namespace Playground.ReaderWriterExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input data
            string username = "JohnDoe";
            string password = "short";
            string email = "john.doe@invalid.com";

            // Define the validation configurations
            var userConfig = new UserValidationConfig
            {
                MinPasswordLength = 8
            };

            var emailConfig = new EmailValidationConfig
            {
                AllowedEmailDomain = "@example.com"
            };

            // Compose the validation chain
            var userValidation = ValidationRules.ValidateUsername(username)
                .SelectMany(
                    userNameWriter => ValidationRules.ValidatePassword(password),
                    (userNameWriter, passwordWriter) => 
                        Writer.Combine(userNameWriter, passwordWriter)                    
                );

            var emailValidation = ValidationRules.ValidateEmail(email);

            // Execute the validations
            var userResult = userValidation(userConfig);
            var emailResult = emailValidation(emailConfig);

            var (_, logs) = Writer.Combine(userResult, emailResult)();

            if (logs.Any())
            {
                Console.WriteLine("Validation failed with the following errors:");
                foreach (var log in logs)
                {
                    Console.WriteLine($"- {log}");
                }
            }
            else
            {
                Console.WriteLine($"Validation passed: {userResult.Value}, {emailResult.Value}");
            }
        }
    }
}
