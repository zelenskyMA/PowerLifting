using SportAssistant.Application.UserData.UserCommands.UserCommands;
using SportAssistant.Domain.Models.UserData.Auth;
using System.Net.Http;
using TestFramework.TestExtensions;

namespace TestFramework.Presets;

public class ActionsPreset
{
    public void AuthorizeAdmin(HttpClient client) => Authorize(client, TestConstants.AdminLogin);
    public void AuthorizeCoach(HttpClient client) => Authorize(client, TestConstants.CoachLogin);
    public void AuthorizeSecondCoach(HttpClient client) => Authorize(client, TestConstants.SecondCoachLogin);
    public void AuthorizeUser(HttpClient client) => Authorize(client, TestConstants.UserLogin);
    public void AuthorizeNoCoachUser(HttpClient client) => Authorize(client, TestConstants.NoCoachUserLogin);
    public void AuthorizeManager(HttpClient client) => Authorize(client, TestConstants.ManagerLogin);
    public void AuthorizeOrgOwner(HttpClient client) => Authorize(client, TestConstants.OrgOwnerLogin);

    public void UnAuthorize(HttpClient client) => client.DefaultRequestHeaders.Remove("Authorization");

    private void Authorize(HttpClient client, string login)
    {
        var response = client.Post<TokenModel>("/user/login", new UserLoginCommand.Param() { Login = login, Password = TestConstants.Password });

        UnAuthorize(client);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {response.Token}");
    }
}
