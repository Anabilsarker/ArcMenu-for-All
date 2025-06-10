using ArcMenu_for_All.Platform.Linux;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace ArcMenu_for_All;

public partial class ArcMenu : UserControl
{
    public ArcMenu()
    {
        InitializeComponent();

        accountName.Text = System.Environment.UserName;
        string accountPicturePath = Session.GetAccountPicturePath();
        if (!string.IsNullOrEmpty(accountPicturePath) && File.Exists(accountPicturePath))
        {
            accountImage.Source = new Bitmap(accountPicturePath);
        }

        appScrollView.Content = AppDiscovery.ListAllApps();
    }

    private void Shutdown_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Session.Shutdown();
        }
    }

    private void Reboot_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        bool toUefi = false;
        if ((e.KeyModifiers & KeyModifiers.Shift) == KeyModifiers.Shift)
        {
            toUefi = true;
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Session.Reboot(toUefi);
        }
    }

    private void Lock_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Session.Lock();
        }
    }

    private void Logout_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Session.Logout();
        }
    }
}