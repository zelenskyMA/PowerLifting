using FluentAssertions;
using SportAssistant.Application.TrainingTemplate.TemplateSetCommands;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingTemplates;

public class TemplateSet_ChangeTest : BaseTest
{
    public TemplateSet_ChangeTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Create_TmpltSet_EmptyName_Fail()
    {
        Factory.Actions.AuthorizeCoach(Client);
        var request = new TemplateSetCreateCommand.Param() { Name = string.Empty };
        var response = Client.Post($"/templateSet", request);
        response.ReadErrorMessage().Should().Match("Необходимо указать название цикла*");
    }

    [Fact]
    public void Create_TmpltSet_DuplicateName_Fail()
    {
        Factory.Actions.AuthorizeCoach(Client);
        var name = Factory.Data.TemplateSet.Name;

        var request = new TemplateSetCreateCommand.Param() { Name = name };
        var response = Client.Post($"/templateSet", request);
        response.ReadErrorMessage().Should().Match("Тренировочный цикл с указанным именем уже существует*");
    }

    [Fact]
    public void Create_TmpltSet_NotCoach_Fail()
    {
        Factory.Actions.AuthorizeUser(Client);
        var request = new TemplateSetCreateCommand.Param() { Name = "user template set" };
        var response = Client.Post($"/templateSet", request);
        response.ReadErrorMessage().Should().Match("У вас нет прав на выполнение данной операции*");
    }

    [Fact]
    public void Create_TmpltSet_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var name = "coach template set";
        var request = new TemplateSetCreateCommand.Param() { Name = name };

        //Act
        var response = Client.Post<bool>($"/templateSet", request);

        //Assert
        response.Should().BeTrue();

        var sets = Client.Get<List<TemplateSet>>($"/templateSet/getList");
        sets.FirstOrDefault(t => t.Name == name).Should().NotBeNull();
    }



    [Fact]
    public void Update_TmpltSet_EmptyName_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var setId = Factory.Data.TemplateSet.Id;
        var request = new TemplateSet()
        {
            Id = setId,
            CoachId = Factory.Data.GetUserId(TestConstants.CoachLogin),
            Name = string.Empty
        };

        //Act
        var response = Client.Put($"/templateSet", request);

        //assert
        response.ReadErrorMessage().Should().Match("Необходимо указать новое название цикла*");
    }

    [Fact]
    public void Update_TmpltSet_WrongId_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var request = new TemplateSet()
        {
            Id = 0,
            CoachId = Factory.Data.GetUserId(TestConstants.CoachLogin),
            Name = "asdfsd"
        };

        //Act
        var response = Client.Put($"/templateSet", request);

        //assert
        response.ReadErrorMessage().Should().Match("У вас нет тренировочного цикла с ид 0*");
    }

    [Fact]
    public void Update_TmpltSet_Duplicate_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var newName = "duplicate name";
        Client.Post<bool>($"/templateSet", new TemplateSetCreateCommand.Param() { Name = newName }); // создаем 1 экземпляр

        var setItem = Client.Get<List<TemplateSet>>($"/templateSet/getList").First(t=> t.Name != newName); // готовим старый цикл
        var request = new TemplateSet()
        {
            Id = setItem.Id,
            CoachId = Factory.Data.GetUserId(TestConstants.CoachLogin),
            Name = newName // пробуем сделать дубликат
        };

        //Act
        var response = Client.Put($"/templateSet", request);

        //assert
        response.ReadErrorMessage().Should().Match("Тренировочный цикл с указанным именем уже существует*");
    }


    [Fact]
    public void Update_TmpltSet_NoChange_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var setItem = Client.Get<List<TemplateSet>>($"/templateSet/getList").First();

        //Act
        var response = Client.Put<bool>($"/templateSet", setItem);

        //assert
        response.Should().BeTrue();
    }

    [Fact]
    public void Update_TmpltSet_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var newName = "new name";
        var setItem = Client.Get<List<TemplateSet>>($"/templateSet/getList").First(); // берем цикл для апейта
        var request = new TemplateSet()
        {
            Id = setItem.Id,
            CoachId = Factory.Data.GetUserId(TestConstants.CoachLogin),
            Name = newName
        };

        //Act
        var response = Client.Put<bool>($"/templateSet", request);

        //assert
        response.Should().BeTrue();

        var sets = Client.Get<List<TemplateSet>>($"/templateSet/getList");
        sets.FirstOrDefault(t => t.Name == newName).Should().NotBeNull(); // новое имя есть
        sets.FirstOrDefault(t => t.Name == setItem.Name).Should().BeNull(); // старое исчезло
    }
}
