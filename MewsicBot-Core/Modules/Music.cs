using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Lavalink;

namespace MewsicBot_Core.Modules
{
    class Music : BaseCommandModule
    {
        [Command("join"), Aliases("Join")]
        public async Task Join(CommandContext ctx, DiscordChannel channel)
        {
            //Lavalink: https://github.com/Frederikam/Lavalink/releases

            if (channel.Type != ChannelType.Voice)
            {
                await ctx.RespondAsync("You're not in a voice channel.");
                return;
            }

            //else
            //await 
        }
    }
}
