using CommitMessageMaker.Shared;
using OpenAiNg;
using OpenAiNg.Chat;
using OpenAiNg.Models;

namespace GptMiddlewareApi;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors();

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapPost("/AskLlm", async (ApiRequestDto input, HttpContext _, ILogger<Program> logger) =>
        {
            logger.LogInformation("Received a request with prompt: {Prompt}", input.Prompt);

            try
            {
                OpenAiApi api = new(input.ApiKey);
                ChatRequest request = new()
                {
                    Model = Model.GPT4_1106_Preview,
                    Messages = new[] {new ChatMessage(ChatMessageRole.User, input.Prompt)}
                };

                logger.LogInformation("Sending request to OpenAI API with prompt: {Prompt}", input.Prompt);

                ChatResult result = await api.Chat.CreateChatCompletionAsync(request);

                logger.LogInformation("Received response from OpenAI API for prompt: {Prompt}", input.Prompt);

                string? response = result.Choices?[0].Message?.Content;

                logger.LogInformation("Returning response: {Response}", response);

                return Results.Ok(response ?? "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while processing the request with prompt: {Prompt}",
                    input.Prompt);
                return Results.Problem(detail: $"An unexpected error occurred: {ex}",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        });

        app.Run();
    }
}