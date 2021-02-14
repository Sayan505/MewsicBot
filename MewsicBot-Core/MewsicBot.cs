using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace MewsicBot_Core
{
    class MewsicBot
    {
        private DiscordClient discordClient { get; set; }
        public CommandsNextModule Commands  { get; set; }

        internal MewsicBot()
        {
            // TODO: init & load config.json
        }

        // ENTRY:
        internal async Task ExecBotAsync()
        {
            // TODO: load (Deserialize) config.json

            DiscordConfiguration config = new DiscordConfiguration
            {
                Token = "[REDACTED]",  // TODO: load token from config.json
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LogLevel = DSharpPlus.LogLevel.Error
            };

            // init client
            discordClient = new DiscordClient(config);

            // rig general events
            discordClient.Ready += OnDiscordClient_Ready;
            discordClient.GuildAvailable += OnDiscordClient_GuildAvailable;
            discordClient.ClientErrored += OnDiscordClient_ClientErrorHandler;

            // rig commands
            CommandsNextConfiguration command_config = new CommandsNextConfiguration
            {
                StringPrefix = "#",
                EnableDms = true,
                EnableMentionPrefix = true
            };
            Commands = discordClient.UseCommandsNext(command_config);

            // rig command events
            Commands.CommandExecuted += OnCommands_CommandExecuted;
            Commands.CommandErrored  += OnCommands_CommandErrored;

            // register commands
            Commands.RegisterCommands<MewsicBot_Core.Modules.Modules>();

            // log in
            await discordClient.ConnectAsync();

            // infinite await
            await Task.Delay(-1);
        }

        private Task OnCommands_CommandExecuted(CommandExecutionEventArgs e)
        {
            Console.WriteLine($"[Y] MewsicBot: Command \"{e.Command.QualifiedName}\" executed by \"{e.Context.User.Username}\" in \"{e.Context.Guild.Name}\" @ {DateTime.Now}, {TimeZoneInfo.Local.Id}");  // local time
            // TODO: log it

            return Task.CompletedTask;
        }

        private Task OnCommands_CommandErrored(CommandErrorEventArgs e)
        {
            Console.WriteLine($"[Y] MewsicBot: Command \"{e.Command.QualifiedName}\" failed exec by \"{e.Context.User.Username}\" in \"{e.Context.Guild.Name}\" @ {DateTime.Now}, {TimeZoneInfo.Local.Id}");  // local time
            // TODO: log it

            return Task.CompletedTask;
        }

        private Task OnDiscordClient_Ready(ReadyEventArgs e)
        {
            Console.WriteLine($"[Y] MewsicBot: ONLINE @ {DateTime.Now}, {TimeZoneInfo.Local.Id}");  // local time
            // TODO: log it

            return Task.CompletedTask;
        }

        private Task OnDiscordClient_GuildAvailable(GuildCreateEventArgs e)
        {
            Console.WriteLine($"[Y] MewsicBot: Connected to \"{e.Guild.Name}\" @ {DateTime.Now}, {TimeZoneInfo.Local.Id}");  // local time
            // TODO: log it

            return Task.CompletedTask;
        }

        private Task OnDiscordClient_ClientErrorHandler(ClientErrorEventArgs e)
        {
            Console.WriteLine($"[Y] MewsicBot: ERROR: {e.Exception.Message} @ {DateTime.Now}, {TimeZoneInfo.Local.Id}");  // local time
            // TODO: log it

            throw e.Exception;

            //return Task.CompletedTask;
        }
    }
}
