using SportAssistant.Domain.CustomExceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;

namespace TestFramework.TestExtensions;

[ExcludeFromCodeCoverage]
public static class HttpResponseMessageExtensions
{
    public static T ReadContentAs<T>(this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return response.Content.ReadFromJsonAsync<T>().GetAwaiter().GetResult();
        }
        else
        {
            static TError ReadAsAnonymousType<TError>(HttpContent content, TError template) => content.ReadFromJsonAsync<TError>().GetAwaiter().GetResult();

            throw new ApplicationException(ReadAsAnonymousType(response.Content, new { errorMessage = default(string) }).errorMessage);
        }
    }

    public static string ReadErrorMessage(this HttpResponseMessage response) => ReadAsJson(response.Content).Message;
    private static CustomError ReadAsJson(HttpContent content) => content.ReadFromJsonAsync<CustomError>().GetAwaiter().GetResult();
}
