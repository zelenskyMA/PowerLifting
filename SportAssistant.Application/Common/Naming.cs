using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Application.Common
{
    public static class Naming
    {
        public static string GetLegalShortName(string? firstName, string? surname, string? patronimic, string defaultValue = "")
        {
            string firstLetter = string.IsNullOrEmpty(firstName) ? string.Empty : $" {firstName?.ToUpper()?.First()}.";
            string secondLetter = string.IsNullOrEmpty(patronimic) ? string.Empty : $" {patronimic.ToUpper().First()}.";

            return string.IsNullOrEmpty(surname) ? defaultValue : $"{surname}{firstLetter}{secondLetter}";
        }

        public static string GetLegalFullName(UserInfo info, string defaultValue = "") =>
            GetLegalFullName(info.FirstName, info.Surname, info.Patronimic, defaultValue);

        public static string GetLegalFullName(string? firstName, string? surname, string? patronimic, string defaultValue = "")
        {
            var fullName = string.Join(" ", new[] { surname, firstName, patronimic }).Trim();

            return string.IsNullOrEmpty(fullName) ? defaultValue : fullName;
        }

    }
}
