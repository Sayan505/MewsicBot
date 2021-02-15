using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;


namespace MewsicBot_Core.Modules
{
    class Latency : BaseCommandModule
    {
        [Command("ping"), Aliases("Ping","latency", "ms", "delay")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync($"Pong: {ctx.Client.Ping} ms");

            Console.WriteLine($"[Y] MewsicBot: Latency: {ctx.Client.Ping} ms");
        }
    }
}
