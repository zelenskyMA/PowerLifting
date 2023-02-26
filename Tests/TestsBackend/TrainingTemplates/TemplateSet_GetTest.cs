using FluentAssertions;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingTemplates;

public class TemplateSet_GetTest : BaseTest
{
    public TemplateSet_GetTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Get_TmpltSet_UnAuthorized_Fail()
    {
        //Arrange
        Factory.Actions.UnAuthorize(Client);
        var setId = Factory.Data.TemplateSet.Id;

        //Act
        var response = Client.Get($"/templateSet/{setId}");

        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Get_TmpltSet_WrongId_Fail()
    {
        Factory.Actions.AuthorizeCoach(Client);
        var response = Client.Get($"/templateSet/-1");
        response.ReadErrorMessage().Should().Match("У вас нет тренировочного цикла с ид*");
    }

    [Fact]
    public void Get_TmpltSet_ByOthers_Fail() // шаблон есть, но никто кроме автора его не видит
    {
        //Arrange
        Factory.Actions.AuthorizeAdmin(Client);
        var setId = Factory.Data.TemplateSet.Id;

        //Act
        var response = Client.Get($"/templateSet/{setId}");

        //Assert
        response.ReadErrorMessage().Should().Match($"У вас нет тренировочного цикла с ид {setId}*");
    }

    [Fact]
    public void Get_TmpltSet_ByCoach_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var setId = Factory.Data.TemplateSet.Id;

        //Act
        var response = Client.Get<TemplateSet>($"/templateSet/{setId}");

        //Assert
        response.Should().NotBeNull();
        VerifySetCheck(response, setId);
    }

    [Fact]
    public void Get_TmpltSetList_ByOthers_Nothing_Fail()
    {
        Factory.Actions.AuthorizeAdmin(Client);
        var response = Client.Get<List<TemplateSet>>($"/templateSet/getList");
        response.Should().BeEmpty();
    }

    [Fact]
    public void Get_TmpltSetList_ByCoach_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var setId = Factory.Data.TemplateSet.Id;

        //Act
        var response = Client.Get<List<TemplateSet>>($"/templateSet/getList");

        //Assert
        response.Should().NotBeNull();
        VerifySetCheck(response[0], setId);
    }

    private void VerifySetCheck(TemplateSet tmpltSet, int? setId) // в циклю не грузится вся иерархия
    {
        // тренировочный цикл
        tmpltSet.Should().NotBeNull();
        tmpltSet.Id.Should().Be(setId);
        tmpltSet.CoachId.Should().Be(Factory.Data.GetUserId(Constants.CoachLogin));
        tmpltSet.Name.Should().NotBeNullOrEmpty();

        // шаблон
        tmpltSet.Templates.Should().NotBeEmpty();
        var template = tmpltSet.Templates[0];

        template.Id.Should().BeGreaterThan(0);
        template.Name.Should().NotBeNullOrEmpty();
        template.TypeCountersSum.Should().BeEmpty();

        template.TrainingDays.Should().BeEmpty();
    }
}
