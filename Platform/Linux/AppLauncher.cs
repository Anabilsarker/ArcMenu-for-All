using System;
using System.Diagnostics;

namespace ArcMenu_for_All.Platform.Linux
{
    public static class AppLauncher
    {
        public static void LaunchApp(InstalledApp app)
        {
            try
            {
                if (string.IsNullOrEmpty(app.Exec))
                    return;

                // Parse the Exec field (remove field codes like %f, %u, etc.)
                // string command = ParseExecCommand(app.Exec);
                //
                // var processInfo = new ProcessStartInfo
                // {
                //     FileName = "/bin/bash",
                //     Arguments = $"-c \"{command}\"",
                //     UseShellExecute = false,
                //     CreateNoWindow = true
                // };
                var processInfo = new ProcessStartInfo
                {
                    FileName = app.Exec.Split(' ')[0],
                    UseShellExecute = false,
                    CreateNoWindow = false
                };

                Process.Start(processInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error launching {app.Name}: {ex.Message}");
            }
        }

        private static string ParseExecCommand(string exec)
        {
            // Remove desktop entry field codes
            return exec.Replace("%f", "")
                      .Replace("%F", "")
                      .Replace("%u", "")
                      .Replace("%U", "")
                      .Replace("%d", "")
                      .Replace("%D", "")
                      .Replace("%n", "")
                      .Replace("%N", "")
                      .Replace("%i", "")
                      .Replace("%c", "")
                      .Replace("%k", "")
                      .Replace("%v", "")
                      .Replace("%m", "")
                      .Trim();
        }
    }
}
