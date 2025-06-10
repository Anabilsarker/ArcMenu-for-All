using System;
using System.Diagnostics;
using System.IO;

namespace ArcMenu_for_All.Platform.Linux
{
    public static class Session
    {
        public static string? GetAccountPicturePath()
        {
            string username = Environment.UserName;

            // 1. Check ~/.face
            string home = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string facePath = Path.Combine(home, ".face");
            if (File.Exists(facePath))
                return facePath;

            // 2. Fallback: /var/lib/AccountsService/icons/username
            string accServiceIcon = Path.Combine("/var/lib/AccountsService/icons", username);
            if (File.Exists(accServiceIcon))
                return accServiceIcon;

            // 3. Not found
            return null;
        }
        public static void Lock()
        {
            // Try GNOME, KDE, XFCE, Cinnamon, MATE, Hyprland/Wayland lockers, then loginctl
            if (TryRun("gnome-screensaver-command", "-l")) return;
            if (TryRun("qdbus", "org.freedesktop.ScreenSaver /ScreenSaver Lock")) return;
            if (TryRun("xflock4", "")) return;
            if (TryRun("cinnamon-screensaver-command", "--lock")) return;
            if (TryRun("mate-screensaver-command", "--lock")) return;
            if (TryRun("swaylock", "")) return;
            if (TryRun("gtklock", "")) return;
            if (TryRun("waylock", "")) return;
            TryRun("loginctl", "lock-session");
        }

        public static void Logout()
        {
            // Try DE/compositor-specific logout or fallback to loginctl
            if (TryRun("gnome-session-quit", "--logout --no-prompt")) return;
            if (TryRun("qdbus", "org.kde.ksmserver /KSMServer logout 0 0 0")) return;
            if (TryRun("xfce4-session-logout", "--logout --fast")) return;
            if (TryRun("lxsession-logout", "--logout")) return;
            if (TryRun("cinnamon-session-quit", "--logout --no-prompt")) return;
            if (TryRun("mate-session-save", "--logout")) return;
            if (TryRun("pkill", "Hyprland")) return;
            if (TryRun("swaymsg", "exit")) return;
            if (TryRun("i3-msg", "exit")) return;
            // Force logout all user sessions (use with caution)
            TryRun("loginctl", $"terminate-user {System.Environment.UserName}");
        }

        public static void Shutdown()
        {
            TryRun("systemctl", "poweroff");
        }

        public static void Reboot(bool toUefi = false)
        {
            if (toUefi && TryRun("systemctl", "reboot --firmware-setup")) return;
            TryRun("systemctl", "reboot");
        }

        public static void Suspend()
        {
            TryRun("systemctl", "suspend");
        }

        public static void Hibernate()
        {
            TryRun("systemctl", "hibernate");
        }

        private static bool TryRun(string file, string arguments)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = file,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                var process = Process.Start(psi);
                if (process != null)
                    return true;
            }
            catch { /* Ignore and try next */ }
            return false;
        }
    }
}
