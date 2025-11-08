using System.Collections.Generic;
using System.Net.Http;
#if NET6_0_OR_GREATER
using System.Net.Http.Json;
#endif
using System.Text.Json;
using System.Threading.Tasks;
using VelocipedeUtils.Shared.Models.Network;

namespace VelocipedeUtils.Shared.Communication.NetworkAPIs;

/// <summary>
/// Class for sending objects via HTTP
/// </summary>
public class HttpSender 
{
#if NET5_0_OR_GREATER
    /// <summary>
    /// Synchronous method for sending an object via HTTP
    /// </summary>
    public static string Send(string url, object parameter)
    {
        var client = new HttpClient();
        var webRequest = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(JsonSerializer.Serialize(parameter), System.Text.Encoding.UTF8, "application/json")
        };
        var response = client.Send(webRequest);
        using var reader = new StreamReader(response.Content.ReadAsStream());
        return reader.ReadToEnd();
    }
#endif

    /// <summary>
    /// Method for sending an object via HTTP asynchronously
    /// </summary>
    public static async Task<ApiOperation> SendAsync(string url, object parameter, string methodName = "")
    {
        if (string.IsNullOrEmpty(url))
            throw new Exception("URL could not be null or empty");
        if (parameter == null)
            throw new Exception("Parameter could not be null");
        // 
        var httpClient = new HttpClient();
        var requestStr = JsonSerializer.Serialize(parameter);
        var dt1 = DateTime.Now;
        HttpResponseMessage response;
#if NET6_0_OR_GREATER
        response = await httpClient.PostAsJsonAsync(url, parameter);
#else
        response = await httpClient.PostAsync(url, new StringContent(requestStr));
#endif
        var dt2 = DateTime.Now;
        return new ApiOperation
        {
            ClientAppUid = string.Empty,
            MethodDescription = string.Empty,
            MicroserviceCommunicationConfiguration = new() { EndpointFrom = new(), EndpointTo = new(), NetworkInteractionDetails = new() },
            ServerAppUid = string.Empty,
            RedirectToAppUid = string.Empty,
            RequestObject = new(),
            ResponseObject = new(),
            Status = string.Empty,
            StatusDescription = string.Empty,
            MethodName = methodName,
            Request = requestStr,
            Response = await response.Content.ReadAsStringAsync(),
            DateTimeBegin = dt1,
            DateTimeEnd = dt2
        };
    }

    /// <summary>
    /// Method for sending multiple objects via HTTP asynchronously
    /// </summary>
    public static async Task<List<ApiOperation>> SendMultipleAsync(string url, List<object> parameters, string methodName = "")
    {
        if (string.IsNullOrEmpty(url))
            throw new Exception("URL could not be null or empty");
        if (parameters == null || parameters.Count == 0)
            throw new Exception("The list of parameters could not be null or empty");
        // 
        var httpClient = new HttpClient();
        Task<HttpResponseMessage>[] tasks = new Task<HttpResponseMessage>[parameters.Count];
        var requestStrings = new string[parameters.Count];
        for (int i = 0; i < parameters.Count; i++)
        {
            var requestStr = JsonSerializer.Serialize(parameters[i]);
            requestStrings[i] = requestStr;
#if NET6_0_OR_GREATER
            tasks[i] = httpClient.PostAsJsonAsync(url, parameters[i]);
#else
            tasks[i] = httpClient.PostAsync(url, new StringContent(requestStr));
#endif
        }
        var dt1 = DateTime.Now;
        await Task.WhenAll(tasks);
        var dt2 = DateTime.Now;
        // 
        var result = new List<ApiOperation>();
        for (int i = 0; i < parameters.Count; i++)
        {
            result.Add(new ApiOperation
            {
                ClientAppUid = string.Empty,
                MethodDescription = string.Empty,
                MicroserviceCommunicationConfiguration = new() { EndpointFrom = new(), EndpointTo = new(), NetworkInteractionDetails = new() },
                ServerAppUid = string.Empty,
                RedirectToAppUid = string.Empty,
                RequestObject = new(),
                ResponseObject = new(),
                Status = string.Empty,
                StatusDescription = string.Empty,
                MethodName = methodName,
                Request = requestStrings[i],
                Response = await tasks[i].Result.Content.ReadAsStringAsync(),
                DateTimeBegin = dt1,
                DateTimeEnd = dt2
            });
        }
        return result;
    }
}