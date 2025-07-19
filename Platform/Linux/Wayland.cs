using System;
using System.Runtime.InteropServices;

namespace ArcMenu_for_All.Platform.Linux
{
    internal class Wayland
    {
        private const string LibWaylandClient = "libwayland-client.so.0"; // Not how it is done.

        // Core display functions
        [DllImport(LibWaylandClient)]
        public static extern IntPtr wl_display_connect(string name);
    }
}
