namespace PowerLifting.Application.Common
{
    public static class Naming
    {
        public static string GetLegalName(string firstName, string surname, string patronimic, string defaultValue)
        {
            string firstLetter = string.IsNullOrEmpty(firstName) ? string.Empty : $" {firstName?.ToUpper()?.First()}.";
            string secondLetter = string.IsNullOrEmpty(patronimic) ? string.Empty : $" {patronimic.ToUpper().First()}.";

            return string.IsNullOrEmpty(surname) ? defaultValue : $"{surname}{firstLetter}{secondLetter}";
        }
    }
}
