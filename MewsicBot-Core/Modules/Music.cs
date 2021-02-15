using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Lavalink;

namespace MewsicBot_Core.Modules
{
    class Music : BaseCommandModule
    {
        [Command("join")]
        public async Task Join(CommandContext ctx)
        {
            //Lavalink: https://github.com/Frederikam/Lavalink/releases
            // JDK 15
            
            // assign channel
            DiscordChannel channel = ctx.Member.VoiceState?.Channel;

            LavalinkExtension Lavalink = ctx.Client.GetLavalink();
            if (!Lavalink.ConnectedNodes.Any())
            {
                await ctx.RespondAsync("Connection not established");
                Console.WriteLine($"[Y] MewsicBot: ERROR: Lavalink connection not established. \"{ctx.Guild.Name}\" @ {DateTime.Now}.");

                return;
            }

            var lavalinkNode = Lavalink.ConnectedNodes.Values.First();

            if (channel.Type != ChannelType.Voice)
            {
                await ctx.RespondAsync("You're not in a voice channel.");

                return;
            }

            //else
            await lavalinkNode.ConnectAsync(channel);
            Console.WriteLine($"[Y] MewsicBot: joined channel \"{channel.Name}\" in \"{ctx.Guild.Name}\" @ {DateTime.Now}.");
        }
    }
}
