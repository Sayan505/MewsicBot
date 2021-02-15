using System;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MewsicBot_Core
{
    class Init
    {
        static int Main(string[] args)
        {
            // init Lavalink (requires Java exec in path)
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Arguments = $@"/c cd {AppDomain.CurrentDomain.BaseDirectory}Lavalink & java -jar Lavalink.jar ",
                CreateNoWindow = false,
                UseShellExecute = true
            };

            Process Lavalink = Process.Start(processStartInfo);

            Console.WriteLine($"[Y] MewsicBot: init @ {DateTime.Now}.");  // local time

            try
            {
                new MewsicBot().ExecBotAsync().GetAwaiter().GetResult(); // exec
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            Console.ReadKey();

            return 0;
        }
    }
}
