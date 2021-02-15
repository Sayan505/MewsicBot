using System;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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


        /* MAKESHIFT PLAY FEATURE. IN PROGRESS.
        [Command("play")]
        public async Task Play(CommandContext ctx, [RemainingText] string search)
        {
            if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
            {
                await ctx.RespondAsync("You are not in a voice channel.");
                return;
            }

            var lava = ctx.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

            if (conn == null)
            {
                await ctx.RespondAsync("Lavalink is not connected.");
                return;
            }

            var loadResult = await node.Rest.GetTracksAsync(search);

            if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed
                || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
            {
                await ctx.RespondAsync($"Track search failed for {search}.");
                return;
            }

            var track = loadResult.Tracks.First();

            await conn.PlayAsync(track);

            await ctx.RespondAsync($"Now playing {track.Title}!");
        }

        */
    }
}
