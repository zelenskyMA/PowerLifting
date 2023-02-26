using FluentAssertions;
using SportAssistant.Application.TrainingTemplate.TemplatePlanCommands;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingTemplates;

public class TemplatePlan_DeleteTest : BaseTest
{
    public TemplatePlan_DeleteTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Delete_TmpltPlan_ByOthers_Fail()
    {
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var response = Client.Delete($"/templatePlan/{Factory.Data.TemplateSet.Templates[0].Id}");
        response.ReadErrorMessage().Should().Match("У вас нет прав на удаление шаблона*");
    }

    [Fact]
    public void Delete_TmpltPlan_WrongId_Fail()
    {
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var response = Client.Delete($"/templatePlan/-1");
        response.ReadErrorMessage().Should().Match("Шаблон с ид -1 не найден*");
    }

    [Fact]
    public void Delete_TmpltPlan_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var name = "coach template set1";
        var createReq = new TemplatePlanCreateCommand.Param() { SetId = Factory.Data.TemplateSet.Id, Name = name };
        var planId = Client.Post<int>($"/templatePlan", createReq);

        //Act
        var response = Client.Delete<int>($"/templatePlan/{planId}");

        //Assert
        response.Should().Be(createReq.SetId);

        var newTmplt = Client.Get($"/templatePlan/{planId}");
        newTmplt.ReadErrorMessage().Should().Match("Шаблон не найден*");
    }
}
