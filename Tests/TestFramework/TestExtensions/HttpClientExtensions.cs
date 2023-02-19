using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using HttpClient = System.Net.Http.HttpClient;
using HttpResponseMessage = System.Net.Http.HttpResponseMessage;

namespace TestFramework.TestExtensions;

[ExcludeFromCodeCoverage]
public static class HttpClientExtensions
{
    private static HttpStatusCode[] allowedStatuses = new HttpStatusCode[] { HttpStatusCode.OK, HttpStatusCode.NoContent };

    public static HttpResponseMessage Get(this HttpClient httpClient, string path) => httpClient.GetAsync(path).GetAwaiter().GetResult();

    public static TResult Get<TResult>(this HttpClient httpClient, string path)
    {
        using HttpResponseMessage resp = httpClient.GetAsync(path).GetAwaiter().GetResult();
        string respContentString = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        if (!allowedStatuses.Contains(resp.StatusCode))
        {
            throw new ApplicationException(respContentString);
        }

        return HandleResult<TResult>(respContentString);
    }

    public static HttpResponseMessage Post(this HttpClient httpClient, string path, object body = null) => httpClient.PostAsJsonAsync(path, body).GetAwaiter().GetResult();

    public static TResult Post<TResult>(this HttpClient httpClient, string path, object body = null)
    {
        using HttpResponseMessage resp = httpClient.PostAsJsonAsync(path, body).GetAwaiter().GetResult();
        string respContentString = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        if (!allowedStatuses.Contains(resp.StatusCode))
        {
            throw new ApplicationException(respContentString);
        }

        return HandleResult<TResult>(respContentString);
    }

    public static TResult Put<TResult>(this HttpClient httpClient, string path, object body = null)
    {
        using HttpResponseMessage resp = httpClient.PutAsJsonAsync(path, body).GetAwaiter().GetResult();
        string respContentString = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        if (!allowedStatuses.Contains(resp.StatusCode))
        {
            throw new ApplicationException(respContentString);
        }

        return HandleResult<TResult>(respContentString);
    }

    private static TResult HandleResult<TResult>(string respContentString)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<TResult>(respContentString, options);
    }
}
