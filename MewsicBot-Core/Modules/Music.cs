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

            LavalinkExtension Lavalink = ctx.Client.GetLavalink();
            if (!Lavalink.ConnectedNodes.Any())
            {
                Console.WriteLine($"[Y] MewsicBot: ERROR: Lavalink connection not established. \"{ctx.Guild.Name}\" @ {DateTime.Now}.");

                return;
            }

            var lavalinkNode = Lavalink.ConnectedNodes.Values.First();

            if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
            {
                await ctx.RespondAsync("You're not in a voice channel.");

                return;
            }

            // assign channel
            DiscordChannel channel = ctx.Member.VoiceState?.Channel;

            // connect to channel
            await lavalinkNode.ConnectAsync(channel);

            Console.WriteLine($"[Y] MewsicBot: joined channel \"{channel.Name}\" of \"{ctx.Guild.Name}\" @ {DateTime.Now}.");

            return;
        }


        // MAKESHIFT PLAY FEATURE. IN PROGRESS.
        [Command("play")]
        public async Task Play(CommandContext ctx, [RemainingText] string search_term)
        {
            await Join(ctx);    // join if not already in

            LavalinkExtension lavalinkExtension = ctx.Client.GetLavalink();
            LavalinkNodeConnection lavalinkNode = lavalinkExtension.ConnectedNodes.Values.First();
            LavalinkGuildConnection lavalinkGuildConnection = lavalinkNode.GetGuildConnection(ctx.Member.VoiceState.Guild);

            if (lavalinkGuildConnection == null)
            {
                Console.WriteLine($"[Y] MewsicBot: Lavalink server is not connected. @ {DateTime.Now}.");
                return;
            }

            // make search
            LavalinkLoadResult lavalinkloadResult = await lavalinkNode.Rest.GetTracksAsync(search_term);

            if (lavalinkloadResult.LoadResultType == LavalinkLoadResultType.LoadFailed
                || lavalinkloadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
            {
                await ctx.RespondAsync($"Track search failed for {search_term}.");
                Console.WriteLine($"[Y] MewsicBot: Track search failed for \"{search_term}\" in \"{ctx.Channel.Name}\" of \"{ctx.Guild.Name}\" @ {DateTime.Now}.");
                return;
            }

            LavalinkTrack track = lavalinkloadResult.Tracks.First();

            // play!
            await lavalinkGuildConnection.PlayAsync(track);

            await ctx.RespondAsync($"Now playing: {track.Title}!");
            Console.WriteLine($"[Y] MewsicBot: Now playing: {track.Title} in \"{ctx.Channel.Name}\" of \"{ctx.Guild.Name}\" @ {DateTime.Now}.");

            return;
        }
    }
}
