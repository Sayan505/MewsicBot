using System;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MewsicBot_Core
{
    class Init
    {
        static ProcessStartInfo processStartInfo;
        static Process Lavalink;

        static int Main(string[] args)
        {
            // init Lavalink (requires Java exec in path)
            processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Arguments = $@"/c cd {AppDomain.CurrentDomain.BaseDirectory}Lavalink & java -jar Lavalink.jar ",
                CreateNoWindow = false,
                UseShellExecute = true
            };

            Lavalink = Process.Start(processStartInfo);

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(Exit);

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

        static void Exit(object sender, EventArgs e)
        {
            Lavalink.Kill();
        }
    }
}
