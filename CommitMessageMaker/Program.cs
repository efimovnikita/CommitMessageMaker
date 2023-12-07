using System.Text;
using System.Text.Json;
using CommitMessageMaker.Shared;
using Microsoft.Extensions.Configuration;

namespace CommitMessageMaker;

internal static class Program
{
    private static IConfiguration? Configuration { get; set; }

    // ReSharper disable once UnusedParameter.Local
    static async Task Main(string[] args)
    {
        string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        string configPath = Path.Combine(homePath, ".config", "cmm", "appsettings.json");

        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(configPath, optional: true, reloadOnChange: true);
        
        Configuration = builder.Build();

        const string? keyParamName = "OpenAiApiKey";
        const string? proxyAddressParamName = "ProxyAddress";

        string? keyValue = Configuration[$"Settings:{keyParamName}"];
        string? proxyAddressValue = Configuration[$"Settings:{proxyAddressParamName}"];
        
        ValidateConfigParameters(keyValue, proxyAddressValue, configPath, keyParamName, proxyAddressParamName);

        StringBuilder inputBuilder = new();
        
        using (StreamReader reader = new(Console.OpenStandardInput(), Console.InputEncoding))
        {
            while (await reader.ReadLineAsync() is { } rawInput)
            {
                inputBuilder.AppendLine(rawInput);
            }
        }

        string allInput = inputBuilder.ToString();
        
        ApiRequestDto requestDto = new()
        {
            Prompt =
                $"I want you to act as a commit message generator. I will provide you with information from git diff command, and I would like you to generate an appropriate commit message using the conventional commit format. Do not write any explanations or other words, just reply with the commit message:\n{allInput}",
            ApiKey = keyValue!
        };
        string response = await PostRequest(proxyAddressValue!, requestDto);
        Console.Write(response);
    }
    
    private static async Task<string> PostRequest(string apiEndpoint, ApiRequestDto apiRequestDto)
    {
        using HttpClient client = new();
        string payload = JsonSerializer.Serialize(apiRequestDto);

        using HttpRequestMessage request = new();
        request.Method = HttpMethod.Post;
        request.RequestUri = new Uri(apiEndpoint);
        request.Content = new StringContent(content: payload, encoding: Encoding.UTF8, mediaType: "application/json");

        using HttpResponseMessage response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    
        return await response.Content.ReadAsStringAsync();
    }

    private static void ValidateConfigParameters(string? keyValue, string? proxyIpValue,
        string configPath, string? keyParamName, string? proxyIpParamName)
    {
        if (new[] {keyValue, proxyIpValue}.Any(String.IsNullOrWhiteSpace)) return;
        
        if (String.IsNullOrWhiteSpace(keyValue))
        {
            throw new ArgumentException($"Parameter must be set in '{configPath}'", keyParamName);
        }

        if (String.IsNullOrWhiteSpace(proxyIpValue))
        {
            throw new ArgumentException($"Parameter must be set in '{configPath}'", proxyIpParamName);
        }
    }
}