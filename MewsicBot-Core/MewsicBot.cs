﻿using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;

using Newtonsoft.Json;

namespace MewsicBot_Core
{
    class MewsicBot
    {
        private DiscordClient discordClient  { get; set; }
        private CommandsNextModule Commands  { get; set; }

        internal MewsicBot() { }

        // ENTRY:
        internal async Task ExecBotAsync()
        {
            // Config.json
            string Config_dot_json = string.Empty;
            Config Unmarshalled_Config_dot_json = new Config();

            using (var buffer = new StreamReader(File.OpenRead("./Config/Config.json")))
            {
                Console.WriteLine($"[Y] MewsicBot: Loading \"Config.json\"");
                Config_dot_json = await buffer.ReadToEndAsync();

                Console.WriteLine($"[Y] MewsicBot: Un-marshalling \"Config.json\"");
                Unmarshalled_Config_dot_json = JsonConvert.DeserializeObject<Config>(Config_dot_json);
            }

            // Token.json
            string Token_dot_json = string.Empty;
            Token Unmarshalled_Token_dot_json = new Token();

            using (var buffer = new StreamReader(File.OpenRead("./Config/Token.json")))
            {
                Console.WriteLine($"[Y] MewsicBot: Loading \"Token.json\"");
                Token_dot_json = await buffer.ReadToEndAsync();

                Console.WriteLine($"[Y] MewsicBot: Un-marshalling \"Config.json\"");
                Unmarshalled_Token_dot_json = JsonConvert.DeserializeObject<Token>(Token_dot_json);
            }

            // config bot
            DiscordConfiguration config = new DiscordConfiguration
            {
                Token         = Unmarshalled_Token_dot_json.Bot_Token,
                TokenType     = TokenType.Bot,
                AutoReconnect = Unmarshalled_Config_dot_json.AutoReconnect,
                LogLevel      = DSharpPlus.LogLevel.Error
            };
            discordClient = new DiscordClient(config); // init client

            // rig general events
            discordClient.Ready          += OnDiscordClient_Ready;
            discordClient.GuildAvailable += OnDiscordClient_GuildAvailable;
            discordClient.ClientErrored  += OnDiscordClient_ClientErrorHandler;

            // config commands
            CommandsNextConfiguration command_config = new CommandsNextConfiguration
            {
                StringPrefix        = Unmarshalled_Config_dot_json.StringPrefix,
                EnableDms           = Unmarshalled_Config_dot_json.EnableDms,
                EnableMentionPrefix = Unmarshalled_Config_dot_json.EnableMentionPrefix
            };
            Commands = discordClient.UseCommandsNext(command_config);

            // rig command events
            Commands.CommandExecuted += OnCommands_CommandExecuted;
            Commands.CommandErrored  += OnCommands_CommandErrored;

            // register commands
            Commands.RegisterCommands<MewsicBot_Core.Modules.Modules>();

            Console.WriteLine($"[Y] MewsicBot: Configs loaded.");

            // log in
            Console.WriteLine($"[-] MewsicBot: Connecting...");
            await discordClient.ConnectAsync();

            // infinite await
            await Task.Delay(-1);
        }

        private Task OnCommands_CommandExecuted(CommandExecutionEventArgs e)
        {
            Console.WriteLine($"[Y] MewsicBot: Command \"{e.Command.QualifiedName}\" executed by \"{e.Context.User.Username}\" in \"{e.Context.Guild.Name}\" @ {DateTime.Now}.");  // local time
            // TODO: log it

            return Task.CompletedTask;
        }

        private Task OnCommands_CommandErrored(CommandErrorEventArgs e)
        {
            Console.WriteLine($"[X] MewsicBot: Command \"{e.Command.QualifiedName}\" failed exec by \"{e.Context.User.Username}\" in \"{e.Context.Guild.Name}\" @ {DateTime.Now}.");  // local time
            // TODO: log it

            return Task.CompletedTask;
        }

        private Task OnDiscordClient_Ready(ReadyEventArgs e)
        {
            Console.WriteLine($"[Y] MewsicBot: ONLINE @ {DateTime.Now}.");  // local time
            // TODO: log it

            return Task.CompletedTask;
        }

        private Task OnDiscordClient_GuildAvailable(GuildCreateEventArgs e)
        {
            Console.WriteLine($"[Y] MewsicBot: Connected to \"{e.Guild.Name}\" @ {DateTime.Now}.");  // local time
            // TODO: log it

            return Task.CompletedTask;
        }

        private Task OnDiscordClient_ClientErrorHandler(ClientErrorEventArgs e)
        {
            Console.WriteLine($"[X] MewsicBot: ERROR: {e.Exception.Message} @ {DateTime.Now}.");  // local time
            // TODO: log it

            throw e.Exception;
        }
    }
}
