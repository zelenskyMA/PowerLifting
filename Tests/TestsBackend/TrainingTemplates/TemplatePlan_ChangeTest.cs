using FluentAssertions;
using SportAssistant.Application.TrainingTemplate.TemplatePlanCommands;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingTemplates;

public class TemplatePlan_ChangeTest : BaseTest
{
    public TemplatePlan_ChangeTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Create_TmpltPlan_EmptyName_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var request = new TemplatePlanCreateCommand.Param() { SetId = Factory.Data.TemplateSet.Id, Name = string.Empty };

        //Act
        var response = Client.Post($"/templatePlan", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Необходимо указать название шаблона*");
    }

    [Fact]
    public void Create_TmpltPlan_DuplicateName_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var tmplt = Client.Get<TemplatePlan>($"/templatePlan/{Factory.Data.TemplateSet.Templates[0].Id}");

        var request = new TemplatePlanCreateCommand.Param() { SetId = Factory.Data.TemplateSet.Id, Name = tmplt.Name };

        //Act
        var response = Client.Post($"/templatePlan", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Тренировочный шаблон с указанным именем уже существует*");
    }

    [Fact]
    public void Create_TmpltPlan_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeSecondCoach(Client);
        var request = new TemplatePlanCreateCommand.Param() { SetId = Factory.Data.TemplateSet.Id, Name = "xzczvbv" };

        //Act
        var response = Client.Post($"/templatePlan", request);

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет права изменять данные в выбранном тренировочном цикле*");
    }

    [Fact]
    public void Create_TmpltPlan_NotCoach_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var request = new TemplatePlanCreateCommand.Param() { SetId = Factory.Data.TemplateSet.Id, Name = "test name" };

        //Act
        var response = Client.Post($"/templatePlan", request);

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет прав на выполнение данной операции*");
    }

    [Fact]
    public void Create_TmpltPlan_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var name = "coach template set";
        var request = new TemplatePlanCreateCommand.Param() { SetId = Factory.Data.TemplateSet.Id, Name = name };

        //Act
        var response = Client.Post<int>($"/templatePlan", request);

        //Assert
        response.Should().BeGreaterThan(0);

        var tmplt = Client.Get<TemplatePlan>($"/templatePlan/{response}");
        tmplt.Should().NotBeNull();
    }


    [Fact]
    public void Update_TmpltPlan_EmptyName_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var tmplt = Factory.Data.TemplateSet.Templates[0];
        var request = new TemplatePlan { Id = tmplt.Id, Name = string.Empty, TrainingDays = tmplt.TrainingDays };

        //Act
        var response = Client.Put($"/templatePlan", request);

        //assert
        response.ReadErrorMessage().Should().Match("Необходимо указать новое название шаблона*");
    }

    [Fact]
    public void Update_TmpltPlan_WrongId_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var tmplt = Factory.Data.TemplateSet.Templates[0];
        var request = new TemplatePlan { Id = 0, Name = string.Empty, TrainingDays = tmplt.TrainingDays };

        //Act
        var response = Client.Put($"/templatePlan", request);

        //assert
        response.ReadErrorMessage().Should().Match("У вас нет шаблона с ид 0*");
    }

    [Fact]
    public void Update_TmpltPlan_Duplicate_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var newName = "duplicate name";
        var createReq = new TemplatePlanCreateCommand.Param() { SetId = Factory.Data.TemplateSet.Id, Name = newName };
        Client.Post<int>($"/templatePlan", createReq); // создаем 1 экземпляр

        var tmplt = Client.Get<TemplatePlan>($"/templatePlan/{Factory.Data.TemplateSet.Templates[0].Id}");
        var request = new TemplatePlan { Id = tmplt.Id, Name = newName, TrainingDays = tmplt.TrainingDays };

        //Act
        var response = Client.Put($"templatePlan", request);

        //assert
        response.ReadErrorMessage().Should().Match("Тренировочный шаблон с указанным именем уже существует в выбранном цикле*");
    }

    [Fact]
    public void Update_TmpltPlan_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var newName = "new name";
        var planId = Factory.Data.TemplateSet.Templates[0].Id;
        var tmplt = Client.Get<TemplatePlan>($"/templatePlan/{planId}"); // берем для апейта
        tmplt.Name = newName;

        Factory.Actions.AuthorizeSecondCoach(Client);

        //Act
        var response = Client.Put($"/templatePlan", tmplt);

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет права изменять данные в выбранном тренировочном цикле*");
    }

    [Fact]
    public void Update_TmpltPlan_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var newName = "new name";
        var planId = Factory.Data.TemplateSet.Templates[0].Id;
        var tmplt = Client.Get<TemplatePlan>($"/templatePlan/{planId}"); // берем для апейта
        tmplt.Name = newName;

        //Act
        var response = Client.Put<int>($"/templatePlan", tmplt);

        //Assert
        response.Should().Be(Factory.Data.TemplateSet.Id);

        var plan = Client.Get<TemplatePlan>($"/templatePlan/{planId}");
        plan.Name.Should().Be(newName);
    }
}
