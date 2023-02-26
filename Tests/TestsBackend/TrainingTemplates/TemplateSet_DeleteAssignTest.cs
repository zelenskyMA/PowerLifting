using FluentAssertions;
using SportAssistant.Application.TrainingTemplate.TemplateSetCommands;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Models.Coaching;
using SportAssistant.Domain.Models.TrainingPlan;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingTemplates;

public class TemplateSet_DeleteAssignTest : BaseTest
{
    public TemplateSet_DeleteAssignTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Delete_TmpltSet_WrongId_Fail()
    {
        Factory.Actions.AuthorizeCoach(Client);
        var response = Client.Delete($"/templateSet/0");
        response.ReadErrorMessage().Should().Match("У вас нет тренировочного цикла с ид 0*");
    }

    [Fact]
    public void Delete_TmpltSet_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var name = "set to delete";
        Client.Post<bool>($"/templateSet", new TemplateSetCreateCommand.Param() { Name = name });

        var sets = Client.Get<List<TemplateSet>>($"/templateSet/getList");
        var setId = sets.First(t => t.Name == name).Id;

        //Act
        var response = Client.Delete<bool>($"/templateSet/{setId}");

        //Assert
        response.Should().BeTrue();

        sets = Client.Get<List<TemplateSet>>($"/templateSet/getList");
        sets.FirstOrDefault(t => t.Name == name).Should().BeNull();
    }

    [Fact]
    public void Assign_TmpltSet_WrongId_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var request = new TemplateSetAssignCommand.Param() { SetId = 0, GroupId = 10, StartDate = DateTime.Now };

        //Act
        var response = Client.Post($"/templateSet/assign", request);

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет тренировочного цикла с ид 0*");
    }

    [Fact]
    public void Assign_TmpltSet_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeSecondCoach(Client);
        var setId = Factory.Data.TemplateSet.Id; // чужой тренировочный цикл
        var group = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList").First(); // своя группа
        var request = new TemplateSetAssignCommand.Param() { SetId = setId, GroupId = group.Id, StartDate = DateTime.Now };

        //Act
        var response = Client.Post($"/templateSet/assign", request);

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет тренировочного цикла с ид *");
    }

    [Fact]
    public void Assign_TmpltSet_WrongGroup_Fail()
    {
        //Arrange
        var setId = Factory.Data.TemplateSet.Id; // свой тренировочный цикл

        Factory.Actions.AuthorizeSecondCoach(Client);
        var group = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList").First(); // чужая группа
        var request = new TemplateSetAssignCommand.Param() { SetId = setId, GroupId = group.Id, StartDate = DateTime.Now };

        Factory.Actions.AuthorizeCoach(Client);

        //Act
        var response = Client.Post($"/templateSet/assign", request);

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет прав назначать тренировки группе*");
    }

    [Fact]
    public void Assign_TmpltSet_UserWithNoRecord_Fail() // нельзя назначить план, когда нет рекорда соотв. упражнения у спортсмена
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var setId = Factory.Data.TemplateSet.Id;
        var group = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList").First();
        var request = new TemplateSetAssignCommand.Param() { SetId = setId, GroupId = group.Id, StartDate = DateTime.Now };

        //Act
        var response = Client.Post($"/templateSet/assign", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Назначение спортсмену * не удалось*");
    }

    [Fact]
    public void Assign_TmpltSet_OverExistingPlan_Success() // назначение поверх плана
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var setId = Factory.Data.TemplateSet.Id;
        var group = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList").First(t=> t.Name == Constants.SecondGroupName);
        var request = new TemplateSetAssignCommand.Param() { SetId = setId, GroupId = group.Id, StartDate = DateTime.Now };        

        //Act
        var response = Client.Post<bool>($"/templateSet/assign", request);

        //Assert
        response.Should().BeTrue();

        var userId = Factory.Data.GetUserId(Constants.User2Login);
        var plans = Client.Get<Plans>($"/trainingPlan/getList/{userId}");
        plans.ActivePlans.FirstOrDefault(t=> t.StartDate.Date == DateTime.Now.Date).Should().NotBeNull();
    }

    [Fact]
    public void Assign_TmpltSet_FreeDate_Success() // назначение на свободные даты
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var setId = Factory.Data.TemplateSet.Id;
        var group = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList").First(t => t.Name == Constants.SecondGroupName);
        var request = new TemplateSetAssignCommand.Param() { SetId = setId, GroupId = group.Id, StartDate = DateTime.Now.AddDays(20) };

        //Act
        var response = Client.Post<bool>($"/templateSet/assign", request);

        //Assert
        response.Should().BeTrue();

        var userId = Factory.Data.GetUserId(Constants.User2Login);
        var plans = Client.Get<Plans>($"/trainingPlan/getList/{userId}");
        plans.ActivePlans.FirstOrDefault(t => t.StartDate.Date == DateTime.Now.AddDays(20).Date).Should().NotBeNull();
    }
}
