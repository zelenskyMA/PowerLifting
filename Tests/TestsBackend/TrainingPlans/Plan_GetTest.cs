using FluentAssertions;
using SportAssistant.Domain.Models.TrainingPlan;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingPlans;

public class Plan_GetTest : BaseTest
{
    public Plan_GetTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Get_Plan_WrongId_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);

        //Act
        var plan = Client.Get<Plan>($"/trainingPlan/-1");

        //Assert
        plan.Should().NotBeNull();
        plan.UserId.Should().Be(0);
        plan.Id.Should().Be(0);
        plan.TrainingDays.Count.Should().Be(0);
    }

    [Fact]
    public void Get_Plan_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var planId = Factory.Data.PlanDays[0].PlanId;

        //Act
        var response = Client.Get($"/trainingPlan/{planId}");

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет прав на просмотр данной информации*");
    }

    [Fact]
    public void Get_Plan_ByCoach_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var planId = Factory.Data.PlanDays[0].PlanId;

        //Act
        var response = Client.Get<Plan>($"/trainingPlan/{planId}");

        //Assert
        response.Should().NotBeNull();
        response.IsMyPlan.Should().BeFalse();
        VerifyPlanCheck(response, planId);
    }

    [Fact]
    public void Get_Plan_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var planId = Factory.Data.PlanDays[0].PlanId;

        //Act
        var response = Client.Get<Plan>($"/trainingPlan/{planId}");

        //Assert
        response.Should().NotBeNull();
        response.IsMyPlan.Should().BeTrue();
        VerifyPlanCheck(response, planId);
    }


    [Fact]
    public void Get_PlanList_WrongId_Fail()
    {
        Factory.Actions.AuthorizeUser(Client);
        var response = Client.Get($"/trainingPlan/getList/-1");
        response.ReadErrorMessage().Should().Match("У вас нет прав на просмотр данной информации*");
    }

    [Fact]
    public void Get_PlanList_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var userId = Factory.Data.GetUserId(Constants.UserLogin);

        //Act
        var response = Client.Get($"/trainingPlan/getList/{userId}");

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет прав на просмотр данной информации*");
    }

    [Fact]
    public void Get_PlanList_ByCoach_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var userId = Factory.Data.GetUserId(Constants.UserLogin);

        //Act
        var response = Client.Get<Plans>($"/trainingPlan/getList/{userId}");

        //Assert
        response.Should().NotBeNull();
        response.ActivePlans.Should().HaveCount(1);
        response.ExpiredPlans.Should().HaveCount(2);
    }

    [Fact]
    public void Get_PlanList_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var userId = Factory.Data.GetUserId(Constants.UserLogin);

        //Act
        var response = Client.Get<Plans>($"/trainingPlan/getList/{userId}");

        //Assert
        response.Should().NotBeNull();
        response.ActivePlans.Should().HaveCount(1);
        response.ExpiredPlans.Should().HaveCount(2);

        var plan = response.ActivePlans[0];
        plan.UserId.Should().Be(Factory.Data.Users.First(t => t.Email == Constants.UserLogin).Id);
        plan.StartDate.Date.Should().BeCloseTo(DateTime.Now.Date, new TimeSpan(1, 1, 1));
        plan.FinishDate.Date.Should().BeCloseTo(plan.StartDate.AddDays(6).Date, new TimeSpan(1, 1, 1));
        plan.TrainingDays.Should().HaveCount(0); // в списке планов нам нужны только заголовки, никаких деталей.

        plan = response.ExpiredPlans[0];
        plan.UserId.Should().Be(Factory.Data.Users.First(t => t.Email == Constants.UserLogin).Id);
        plan.TrainingDays.Should().HaveCount(0); // в списке планов нам нужны только заголовки, никаких деталей.
    }


    private void VerifyPlanCheck(Plan plan, int? planId)
    {
        // план
        plan.Should().NotBeNull();
        plan.Id.Should().Be(planId);
        plan.UserId.Should().Be(Factory.Data.Users.First(t => t.Email == Constants.UserLogin).Id);
        plan.StartDate.Date.Should().BeCloseTo(DateTime.Now.Date, new TimeSpan(1, 1, 1));
        plan.FinishDate.Date.Should().BeCloseTo(plan.StartDate.AddDays(6).Date, new TimeSpan(1, 1, 1));

        plan.TypeCountersSum.Should().NotBeEmpty();
        plan.TypeCountersSum.Where(t => t.Value > 0).Should().HaveCount(4);

        // тренировочный день плана
        plan.TrainingDays.Should().HaveCount(7);
        var day = plan.TrainingDays[0];
        day.Id.Should().BeGreaterThan(0);
        day.PlanId.Should().Be(planId);
        day.ActivityDate.Should().Be(plan.StartDate);

        day.WeightLoadSum.Should().BeGreaterThan(0);
        day.IntensitySum.Should().BeGreaterThan(0);
        day.LiftCounterSum.Should().BeGreaterThan(0);

        day.ExerciseTypeCounters.Should().NotBeEmpty();
        day.ExerciseTypeCounters[0].Value.Should().BeGreaterThan(0);
        day.ExerciseTypeCounters[0].Name.Should().NotBeNullOrEmpty();

        // Упражнения в тренировочном дне        
        day.Exercises.Should().HaveCount(2);
        var dayExercise = day.Exercises[0];
        dayExercise.Id.Should().BeGreaterThan(0);
        dayExercise.PlanDayId.Should().Be(plan.TrainingDays[0].Id);
        dayExercise.Intensity.Should().BeGreaterThan(0);
        dayExercise.LiftCounter.Should().BeGreaterThan(0);
        dayExercise.WeightLoad.Should().BeGreaterThan(0);
        dayExercise.Order.Should().BeGreaterThan(0);

        dayExercise.SettingsTemplate.Should().NotBeNull(); // шаблон не назначен, но заглушка
        dayExercise.SettingsTemplate.Id.Should().Be(0);

        dayExercise.LiftIntensities.Should().NotBeEmpty();
        dayExercise.LiftIntensities[0].Percentage.Should().NotBeNull();

        // Упражнение из справочника, назначенное в план
        var exercise = dayExercise.Exercise;
        exercise.Should().NotBeNull();
        exercise.Id.Should().BeGreaterThan(0);
        exercise.ExerciseTypeId.Should().BeGreaterThan(0);
        exercise.ExerciseSubTypeId.Should().BeGreaterThan(0);

        // Настройки поднятий в упражнении
        dayExercise.Settings.Should().NotBeEmpty();
        var settings = dayExercise.Settings[0];
        settings.Id.Should().BeGreaterThan(0);
        settings.Completed.Should().BeFalse();
        settings.Weight.Should().BeGreaterThan(0);
        settings.Iterations.Should().BeGreaterThan(0);
        settings.ExercisePart1.Should().BeGreaterThan(0);
        settings.ExercisePart2.Should().BeGreaterThan(0);
        settings.ExercisePart3.Should().BeGreaterThan(0);
        settings.PlanExerciseId.Should().BeGreaterThan(0);

        // процентовка поднятия
        settings.Percentage.Should().NotBeNull();
        settings.Percentage.Id.Should().BeGreaterThan(0);
        settings.Percentage.MaxValue.Should().BeGreaterThan(0);
        settings.Percentage.MinValue.Should().BeGreaterThan(0);
        settings.Percentage.Name.Should().NotBeNullOrEmpty();
        settings.Percentage.Description.Should().BeNullOrEmpty();
    }
}

