using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RazorPagesProject.Tests;
using SportAssistant.Domain.Models;
using TestFramework.TestExtensions;
using Xunit;

namespace TestsBackend.Common
{
    public class DictionaryTest : IClassFixture<ServiceTestFixture<Program>>
    {
        private readonly HttpClient _client;
        private readonly ServiceTestFixture<Program> _factory;

        public DictionaryTest(ServiceTestFixture<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        }

        [Fact]
        public void Get_Dictionary_Unauthorized_Fail()
        {
            _factory.UnAuthorize(_client);
            var response = _client.Get("/dictionary/getListByType?typeId=1");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public void Get_Dictionary_ByType_Success()
        {
            _factory.AuthorizeUser(_client);
            var response = _client.Get<List<DictionaryItem>>("/dictionary/getListByType?typeId=1");
            response.Should().NotBeNull();
            response.Count.Should().BeGreaterThan(1);
        }
    }
}
