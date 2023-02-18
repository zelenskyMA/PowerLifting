using FluentAssertions;
using SportAssistant.Application.TrainingPlan.PlanCommands;
using SportAssistant.Domain.Models.TrainingPlan;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingPlans;

public class Plan_CreateDeleteTest : BaseTest
{
    public Plan_CreateDeleteTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Create_Plan_UnAuthorized_Fail()
    {
        //Arrange
        Factory.Actions.UnAuthorize(Client);
        var userId = Factory.Data.GetUserId(Constants.UserLogin);
        var request = new PlanCreateCommand.Param() { CreationDate = DateTime.Now.AddDays(10), UserId = userId };

        //Act
        var response = Client.Post($"/trainingPlan/create", request);

        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Create_Plan_CrossDates_Fail() // пересечение с предсозданным планом по DateTime.Now
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var userId = Factory.Data.GetUserId(Constants.UserLogin);

        //Act + Assert
        var request = new PlanCreateCommand.Param() { CreationDate = DateTime.Now.AddDays(6), UserId = userId };
        var response = Client.Post($"/trainingPlan/create", request);
        response.ReadErrorMessage().Should().Match("Найдены пересекающийся по датам планы. Даты начала*");

        request = new PlanCreateCommand.Param() { CreationDate = DateTime.Now, UserId = userId };
        response = Client.Post($"/trainingPlan/create", request);
        response.ReadErrorMessage().Should().Match("Найдены пересекающийся по датам планы. Даты начала*");
    }

    [Fact]
    public void Create_Plan_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var userId = Factory.Data.GetUserId(Constants.UserLogin);
        var request = new PlanCreateCommand.Param() { CreationDate = DateTime.Now.AddDays(10), UserId = userId };

        //Act
        var response = Client.Post($"/trainingPlan/create", request);

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет права планировать тренировки данного пользователя*");
    }

    [Fact]
    public void Delete_Plan_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var request = new PlanDeleteCommand.Param() { Id = Factory.Data.PlanDays[0].PlanId.Value };

        //Act
        var response = Client.Post($"/trainingPlan/delete", request);

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет права планировать тренировки данного пользователя*");
    }

    [Fact]
    public void CreateDelete_Plan_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var userId = Factory.Data.GetUserId(Constants.NoCoachUserLogin);
        var request = new PlanCreateCommand.Param() { CreationDate = DateTime.Now, UserId = userId };

        //Act + Assert создание
        var planId = Client.Post<int>($"/trainingPlan/create", request);
        planId.Should().BeGreaterThan(0);

        var plan = Client.Get<Plan>($"/trainingPlan/get?id={planId}"); // план создан
        plan.Should().NotBeNull();
        plan.Id.Should().Be(planId);
        plan.TrainingDays.Count.Should().Be(7);

        //Act + Assert удаление
        var requestDelete = new PlanDeleteCommand.Param() { Id = planId };
        var response = Client.Post<bool>($"/trainingPlan/delete", requestDelete);
        response.Should().BeTrue();

        plan = Client.Get<Plan>($"/trainingPlan/get?id={planId}"); // план удален
        plan.Should().NotBeNull();
        plan.Id.Should().Be(0);
        plan.TrainingDays.Should().BeEmpty();
    }

    [Fact]
    public void CreateDelete_Plan_ByCoach_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var userId = Factory.Data.GetUserId(Constants.UserLogin);
        var request = new PlanCreateCommand.Param() { CreationDate = DateTime.Now.AddDays(20), UserId = userId };

        //Act + Assert создание
        var planId = Client.Post<int>($"/trainingPlan/create", request);
        planId.Should().BeGreaterThan(0);

        var plan = Client.Get<Plan>($"/trainingPlan/get?id={planId}"); // план создан
        plan.Should().NotBeNull();
        plan.Id.Should().Be(planId);
        plan.TrainingDays.Count.Should().Be(7);

        //Act + Assert удаление
        var requestDelete = new PlanDeleteCommand.Param() { Id = planId };
        var response = Client.Post<bool>($"/trainingPlan/delete", requestDelete);
        response.Should().BeTrue();

        plan = Client.Get<Plan>($"/trainingPlan/get?id={planId}"); // план удален
        plan.Should().NotBeNull();
        plan.Id.Should().Be(0);
        plan.TrainingDays.Should().BeEmpty();
    }
}
