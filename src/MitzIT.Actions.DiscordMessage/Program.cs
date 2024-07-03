using Cocona;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MitzIT.Actions.DiscordMessage.Commands;

var builder = CoconaApp.CreateBuilder(args);

builder.Services.AddTransient<DiscordSocketClient>();

using var app = builder.Build();

app.AddCommands<TextChannelMessageCommand>();

await app.RunAsync();