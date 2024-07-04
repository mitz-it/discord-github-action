// ReSharper disable ClassNeverInstantiated.Global
using Cocona;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace MitzIT.Actions.DiscordMessage.Commands;

public class DiscordMessageParameters : ICommandParameterSet
{
    [Option('t', Description = "The Discord bot application token")]
    public string Token { get; set; } = string.Empty;
    
    [Option('c', Description = "The text channel ID")]
    public ulong ChannelId { get; set; }
    
    [Option('m', Description = "The text message content")]
    public string MessageContent { get; set; } = string.Empty;

    [Option('e', Description = "The emote ID for the message reaction")]
    [HasDefaultValue]
    public ulong? EmoteId { get; set; } = null;

    [Option('n', Description = "The emote name for the message reaction, when emote ID is not provided use the following format: <:EMOTE_NAME:EMOTE_ID>")]
    [HasDefaultValue]
    public string? EmoteName { get; set; } = null;

    [Option('a', Description = "The emote animation mode for the message reaction")]
    [HasDefaultValue]
    public bool EmoteAnimated { get; set; } = false;
};

public class TextChannelMessageCommand
{
    private readonly ILogger<TextChannelMessageCommand> _logger;
    private readonly DiscordSocketClient _client;
    
    public TextChannelMessageCommand(ILogger<TextChannelMessageCommand> logger, DiscordSocketClient client)
    {
        _client = client;
        _logger = logger;
    }

    [Command("message")]
    public async Task SendMessageAsync(DiscordMessageParameters parameters)
    {
        try
        {
            await _client.LoginAsync(TokenType.Bot, parameters.Token);
            
            await _client.StartAsync();

            var channel = await _client.GetChannelAsync(parameters.ChannelId);
            
            if (channel is IMessageChannel messageChannel)
            {
                var message = await messageChannel.SendMessageAsync(parameters.MessageContent);
                
                await TryAddMessageReaction(message, parameters);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                message: "Failed to send message to text channel with id: {ChannelId}",
                parameters.ChannelId
            );
        }
        finally
        {
            await _client.StopAsync();
            await _client.LogoutAsync();    
        }
    }

    private async Task TryAddMessageReaction(IUserMessage? message, DiscordMessageParameters parameters)
    {
        try
        {
            if (message is not null)
            {
                var emote = TryParseEmote(parameters);
                
                if (emote is null) return;
                
                await message.AddReactionAsync(emote);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                message: "Failed to add emote with id: {EmoteId}, name: {EmoteName}, animated: {EmoteAnimated}",
                parameters.EmoteId,
                parameters.EmoteName,
                parameters.EmoteAnimated
            );
            
            await _client.StopAsync();
            await _client.LogoutAsync();

            throw new CommandExitedException(exitCode: 1);
        }
    }

    private static Emote? TryParseEmote(DiscordMessageParameters parameters)
    {
        if (parameters.EmoteId is not null && !string.IsNullOrWhiteSpace(parameters.EmoteName))
        {
            return new Emote(parameters.EmoteId.Value, parameters.EmoteName, parameters.EmoteAnimated);
        }

        _ = Emote.TryParse(parameters.EmoteName, out var emote);

        return emote;
    }
}