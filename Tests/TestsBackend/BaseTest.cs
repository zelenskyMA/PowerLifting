using Microsoft.AspNetCore.Mvc.Testing;
using TestFramework;
using Xunit;

namespace TestsBackend
{
    public class BaseTest : IClassFixture<ServiceTestFixture<Program>>
    {
        public HttpClient Client { get; }
        public ServiceTestFixture<Program> Factory { get; }

        public BaseTest(ServiceTestFixture<Program> factory)
        {
            Factory = factory;
            Client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect= false,
            });
        }
    }
}
