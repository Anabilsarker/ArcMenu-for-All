using System;
using System.IO;

namespace ArcMenu_for_All.Platform.Linux
{
    public static class IconResolver
    {
        private static readonly string[] IconDirectories = {
            "/usr/share/icons",
            "/usr/share/pixmaps",
            "~/.local/share/icons",
            "~/.icons"
        };

        public static string ResolveIconPath(string iconName)
        {
            if (string.IsNullOrEmpty(iconName))
                return null;

            // If it's already a full path
            if (iconName.StartsWith("/") && File.Exists(iconName))
                return iconName;

            // Search in icon directories
            foreach (string iconDir in IconDirectories)
            {
                string expandedDir = ExpandPath(iconDir);
                if (!Directory.Exists(expandedDir))
                    continue;

                // Try different extensions and sizes
                string[] extensions = { ".png", ".svg", ".xpm", ".ico" };
                string[] sizes = { "48x48", "32x32", "24x24", "16x16", "scalable" };

                foreach (string size in sizes)
                {
                    foreach (string ext in extensions)
                    {
                        string iconPath = Path.Combine(expandedDir, "hicolor", size, "apps", iconName + ext);
                        if (File.Exists(iconPath))
                        {
                            return iconPath;
                        }
                    }
                }

                // Try pixmaps directory
                foreach (string ext in extensions)
                {
                    string iconPath = Path.Combine(expandedDir, iconName + ext);
                    if (File.Exists(iconPath))
                        return iconPath;
                }
            }

            return null;
        }

        private static string ExpandPath(string path)
        {
            if (path.StartsWith("~"))
            {
                return path.Replace("~", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            }
            return path;
        }
    }
}
