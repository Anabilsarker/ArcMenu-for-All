using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ArcMenu_for_All.Platform.Linux
{
    public class InstalledApp
    {
        public string Name { get; set; }
        public string Exec { get; set; }
        public string Icon { get; set; }
        public string Comment { get; set; }
        public string Categories { get; set; }
        public bool NoDisplay { get; set; }
        public string DesktopFilePath { get; set; }
    }

    public static class AppDiscovery
    {
        private static readonly string[] DesktopDirectories = {
            "/usr/share/applications",
            "/usr/local/share/applications",
            "~/.local/share/applications",
            "/var/lib/snapd/desktop/applications", // Snap apps
            "/var/lib/flatpak/exports/share/applications", // Flatpak system
            "~/.local/share/flatpak/exports/share/applications" // Flatpak user
        };

        public static List<InstalledApp> ListAllApps()
        {
            var apps = new List<InstalledApp>();

            foreach (string directory in DesktopDirectories)
            {
                string expandedPath = ExpandPath(directory);

                if (Directory.Exists(expandedPath))
                {
                    var desktopFiles = Directory.GetFiles(expandedPath, "*.desktop", SearchOption.TopDirectoryOnly);

                    foreach (string filePath in desktopFiles)
                    {
                        var app = ParseDesktopFile(filePath);
                        if (app != null && ShouldShowApp(app))
                        {
                            apps.Add(app);
                        }
                    }
                }
            }

            // Remove duplicates, prefer user applications over system ones
            return apps.GroupBy(app => app.Name)
                      .Select(group => group.OrderBy(app => GetApplicationPriority(app.DesktopFilePath)).First())
                      .ToList();
        }

        private static InstalledApp ParseDesktopFile(string filePath)
        {
            try
            {
                var app = new InstalledApp { DesktopFilePath = filePath };
                var lines = File.ReadAllLines(filePath);
                bool inDesktopEntry = false;

                foreach (string line in lines)
                {
                    var trimmedLine = line.Trim();

                    if (trimmedLine == "[Desktop Entry]")
                    {
                        inDesktopEntry = true;
                        continue;
                    }

                    if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                    {
                        inDesktopEntry = false;
                        continue;
                    }

                    if (!inDesktopEntry || !trimmedLine.Contains('='))
                        continue;

                    var parts = trimmedLine.Split('=', 2);
                    if (parts.Length != 2) continue;

                    string key = parts[0].Trim();
                    string value = parts[1].Trim();

                    switch (key)
                    {
                        case "Name":
                            app.Name = value;
                            break;
                        case "Exec":
                            app.Exec = value;
                            break;
                        case "Icon":
                            app.Icon = value;
                            break;
                        case "Comment":
                            app.Comment = value;
                            break;
                        case "Categories":
                            app.Categories = value;
                            break;
                        case "NoDisplay":
                            app.NoDisplay = value.ToLower() == "true";
                            break;
                    }
                }

                return app;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing {filePath}: {ex.Message}");
                return null;
            }
        }

        private static bool ShouldShowApp(InstalledApp app)
        {
            // Filter out apps that shouldn't be displayed
            if (string.IsNullOrEmpty(app.Name) ||
                string.IsNullOrEmpty(app.Exec) ||
                app.NoDisplay)
            {
                return false;
            }

            // Filter out system utilities and libraries
            if (app.Categories?.Contains("System") == true &&
                !app.Categories.Contains("Settings"))
            {
                return false;
            }

            return true;
        }

        private static string ExpandPath(string path)
        {
            if (path.StartsWith("~"))
            {
                return path.Replace("~", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            }
            return path;
        }

        private static int GetApplicationPriority(string desktopFilePath)
        {
            // Prioritize user apps over system apps
            if (desktopFilePath.Contains("/.local/"))
                return 0;
            if (desktopFilePath.Contains("/usr/local/"))
                return 1;
            return 2;
        }
    }
}
