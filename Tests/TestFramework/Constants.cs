using System.Diagnostics.CodeAnalysis;

namespace TestFramework;

[ExcludeFromCodeCoverage]
public static class Constants
{
    public static string AdminLogin => "admin@mail.ru";
    public static string CoachLogin => "coach@mail.ru";
    public static string SecondCoachLogin => "secondCoach@mail.ru";
    public static string UserLogin => "user@mail.ru";
    public static string User2Login => "user2@mail.ru";
    public static string NoCoachUserLogin => "noCoach@mail.ru";
    public static string BlockedUserLogin => "blocked@mail.ru";
    public static string Password => "asdf123";
    public static string Salt => "1CCFD3C98A";
    public static string EncriptedPwd => "BD-40-DC-2F-2B-E1-A6-EC-E0-27-63-65-BD-0C-E6-AB-57-54-8F-65";


    public static string GroupName => "Predefined Group";
    public static string SecondGroupName => "Second Predefined Group";

    public static int ExType1Id => 40;
    public static int ExType2Id => 1;
    public static int SubTypeId => 50;
}
