using Azure.Core;
using FluentAssertions;
using SportAssistant.Application.Settings;
using SportAssistant.Domain.Models.Basic;
using System.Net;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace AdminConsole;

public class SettingsTest : BaseTest
{
    public SettingsTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Get_Settings_Unauthorized_Fail()
    {
        var response = Client.Get("appSettings");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Get_Settings_Success()
    {
        Factory.Actions.AuthorizeUser(Client);
        var response = Client.Get<AppSettings>("/appSettings");

        //Assert
        response.Should().NotBeNull();
        response.MaxExercises.Should().Be(10);
    }

    [Fact]
    public void Update_Settings_User_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var settings = Client.Get<AppSettings>("/appSettings");
        settings.MaxActivePlans = 200;
        var request = new SettingsUpdateCommand.Param() { Settings = settings };

        //Act
        var response = Client.Post("/appSettings", request);

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет прав на выполнение данной операции*");
    }

    [Fact]
    public void Update_Settings_Coach_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var settings = Client.Get<AppSettings>("/appSettings");
        settings.MaxActivePlans = 200;
        var request = new SettingsUpdateCommand.Param() { Settings = settings };

        //Act
        var response = Client.Post("/appSettings", request);

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет прав на выполнение данной операции*");
    }

    [Fact]
    public void Update_Settings_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeAdmin(Client);
        var settings = Client.Get<AppSettings>("/appSettings");
        var backup = settings.MaxActivePlans;

        //Act
        settings.MaxActivePlans = 200;
        var response = Client.Post<bool>("/appSettings", new SettingsUpdateCommand.Param() { Settings = settings });

        //Assert
        response.Should().BeTrue();

        settings = Client.Get<AppSettings>("/appSettings");
        settings.Should().NotBeNull();
        settings.MaxActivePlans.Should().Be(200);

        //Откат изменений
        settings.MaxActivePlans = backup;
        Client.Post<bool>("/appSettings", new SettingsUpdateCommand.Param() { Settings = settings });
    }    
}
