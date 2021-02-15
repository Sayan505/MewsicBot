using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using DSharpPlus;
using DSharpPlus.Net;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;

using Newtonsoft.Json;

namespace MewsicBot_Core
{
    class MewsicBot
    {
        private DiscordClient discordClient     { get; set; }
        private CommandsNextExtension Commands  { get; set; }
        private LavalinkExtension lavaLink      { get; set; }


        // ENTRY:
        internal async Task ExecBotAsync()
        {
            // Config.json
            string Config_dot_json = string.Empty;
            Config Unmarshalled_Config_dot_json = new Config();

            using (var buffer = new StreamReader(File.OpenRead("./Config/Config.json"), new UTF8Encoding(true)))
            {
                Console.WriteLine($"[Y] MewsicBot: Loading \"Config.json\"");
                Config_dot_json = await buffer.ReadToEndAsync();

                Console.WriteLine($"[Y] MewsicBot: Un-marshalling \"Config.json\"");
                Unmarshalled_Config_dot_json = JsonConvert.DeserializeObject<Config>(Config_dot_json);
            }

            // Token.json
            string Token_dot_json = string.Empty;
            Token Unmarshalled_Token_dot_json = new Token();

            using (var buffer = new StreamReader(File.OpenRead("./Config/Token.json"), new UTF8Encoding(true)))
            {
                Console.WriteLine($"[Y] MewsicBot: Loading \"Token.json\"");
                Token_dot_json = await buffer.ReadToEndAsync();

                Console.WriteLine($"[Y] MewsicBot: Un-marshalling \"Token.json\"");
                Unmarshalled_Token_dot_json = JsonConvert.DeserializeObject<Token>(Token_dot_json);
            }

            // config bot
            DiscordConfiguration config = new DiscordConfiguration
            {
                Token           = Unmarshalled_Token_dot_json.Bot_Token,
                TokenType       = TokenType.Bot,
                AutoReconnect   = Unmarshalled_Config_dot_json.AutoReconnect,
                MinimumLogLevel = LogLevel.Error
            };
            discordClient = new DiscordClient(config); // init client

            // rig general events
            discordClient.Ready          += OnDiscordClient_Ready;
            discordClient.GuildAvailable += OnDiscordClient_GuildAvailable;
            discordClient.ClientErrored  += OnDiscordClient_ClientErrorHandler;

            // config commands
            CommandsNextConfiguration commands_config = new CommandsNextConfiguration
            {
                StringPrefixes      = Unmarshalled_Config_dot_json.StringPrefixes,
                EnableDms           = Unmarshalled_Config_dot_json.EnableDms,
                EnableMentionPrefix = Unmarshalled_Config_dot_json.EnableMentionPrefix
            };
            Commands = discordClient.UseCommandsNext(commands_config);  // apply commands config

            // config LavaLink
            ConnectionEndpoint connectionEndpoint = new ConnectionEndpoint
            {
                Hostname = "127.0.0.1",
                Port     = 2333
            };
            LavalinkConfiguration lavalinkConfiguration = new LavalinkConfiguration
            {
                Password       = "12345",    // don't care :p
                RestEndpoint   = connectionEndpoint,
                SocketEndpoint = connectionEndpoint
            };
            lavaLink = discordClient.UseLavalink();

            // rig command events
            Commands.CommandExecuted += OnCommands_CommandExecuted;
            Commands.CommandErrored  += OnCommands_CommandErrored;

            Console.WriteLine($"[Y] MewsicBot: Configs loaded.");

            // register commands
            Commands.RegisterCommands<Modules.Latency>();    // latency module
            Commands.RegisterCommands<Modules.Music>();      // audio module
            Console.WriteLine($"[Y] MewsicBot: Modules loaded.");

            // log in
            Console.WriteLine($"[-] MewsicBot: Connecting...");
            await discordClient.ConnectAsync();

            // init LavaLink
            await lavaLink.ConnectAsync(lavalinkConfiguration);


            // infinite await
            await Task.Delay(-1);
        }

        private Task OnCommands_CommandExecuted(CommandsNextExtension sender, CommandExecutionEventArgs e)
        {
            Console.WriteLine($"[Y] MewsicBot: Command \"{e.Command.QualifiedName}\" executed by \"{e.Context.User.Username}\" in \"{e.Context.Guild.Name}\" @ {DateTime.Now}.");  // local time

            return Task.CompletedTask;
        }

        private Task OnCommands_CommandErrored(CommandsNextExtension sender, CommandErrorEventArgs e)
        {
            Console.WriteLine($"[X] MewsicBot: Command failed exec by \"{e.Context.User.Username}\" in \"{e.Context.Guild.Name}\" @ {DateTime.Now}.");  // local time

            return Task.CompletedTask;
        }

        private Task OnDiscordClient_Ready(DiscordClient sender, ReadyEventArgs e)
        {
            Console.WriteLine($"[Y] MewsicBot: ONLINE @ {DateTime.Now}.");  // local time

            return Task.CompletedTask;
        }

        private Task OnDiscordClient_GuildAvailable(DiscordClient sender, GuildCreateEventArgs e)
        {
            Console.WriteLine($"[Y] MewsicBot: Connected to \"{e.Guild.Name}\" @ {DateTime.Now}.");  // local time

            return Task.CompletedTask;
        }

        private Task OnDiscordClient_ClientErrorHandler(DiscordClient sender, ClientErrorEventArgs e)
        {
            Console.WriteLine($"[X] MewsicBot: ERROR: {e.Exception.Message} @ {DateTime.Now}.");  // local time

            throw e.Exception;
        }
    }
}
