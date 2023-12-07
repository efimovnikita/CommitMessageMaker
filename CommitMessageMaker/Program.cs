using Microsoft.Extensions.Configuration;

namespace CommitMessageMaker;

internal static class Program
{
    private static IConfiguration? Configuration { get; set; }

    private static void Main(string[] args)
    {
        string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        string configPath = Path.Combine(homePath, ".config", "cmm", "appsettings.json");

        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(configPath, optional: true, reloadOnChange: true);
        
        Configuration = builder.Build();

        const string? keyParamName = "OpenAiApiKey";
        const string? proxyModeParamName = "ProxyMode";
        const string? proxyIpParamName = "ProxyIp";

        string? keyValue = Configuration[$"Settings:{keyParamName}"];
        string? proxyModeValue = Configuration[$"Settings:{proxyModeParamName}"];
        string? proxyIpValue = Configuration[$"Settings:{proxyIpParamName}"];
        
        ValidateConfigParameters(keyValue, proxyIpValue, proxyModeValue, configPath, keyParamName, proxyIpParamName, proxyModeParamName);
        
        while (Console.ReadLine() is { } rawDiff)
        {
            
        }
    }

    private static void ValidateConfigParameters(string? keyValue, string? proxyIpValue, string? proxyModeValue,
        string configPath, string? keyParamName, string? proxyIpParamName, string? proxyModeParamName)
    {
        if (new[] {keyValue, proxyIpValue, proxyModeValue}.Any(String.IsNullOrWhiteSpace)) return;
        
        if (String.IsNullOrWhiteSpace(keyValue))
        {
            throw new ArgumentException($"Parameter must be set in '{configPath}'", keyParamName);
        }

        if (String.IsNullOrWhiteSpace(proxyIpValue))
        {
            throw new ArgumentException($"Parameter must be set in '{configPath}'", proxyIpParamName);
        }
            
        if (String.IsNullOrWhiteSpace(proxyModeValue))
        {
            throw new ArgumentException($"Parameter must be set in '{configPath}'", proxyModeParamName);
        }
    }
}