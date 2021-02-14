using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MewsicBot_Core
{
    class Init
    {
        static int Main(string[] args)
        {
            Console.WriteLine($"[Y] MewsicBot: init @ {DateTime.Now}.");  // local time
            // TODO: log it

            try
            {
                new MewsicBot().ExecBotAsync().GetAwaiter().GetResult(); // exec
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // TODO: log the exception
            }
            finally
            {
                // TODO: clean up
                // TODO: log exit after crash
            }


            Console.ReadKey();

            return 0;
        }
    }
}
