using FluentAssertions;
using SportAssistant.Domain.Models;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace Common;

public class DictionaryTest : BaseTest
{
    public DictionaryTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Get_Dictionary_Unauthorized_Fail()
    {
        Factory.Actions.UnAuthorize(Client);
        var response = Client.Get("/dictionary/getListByType/1");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Get_Dictionary_ByType_Success()
    {
        Factory.Actions.AuthorizeUser(Client);
        var response = Client.Get<List<DictionaryItem>>("/dictionary/getListByType/1");
        response.Should().NotBeNull();
        response.Count.Should().BeGreaterThan(1);
    }
}
