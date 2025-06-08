using ArcMenu_for_All.Platform.Linux;
using Avalonia.Controls;
using System.Collections.Generic;

namespace ArcMenu_for_All;

public partial class ArcMenu : UserControl
{
    private List<InstalledApp> _apps;
    public ArcMenu()
    {
        InitializeComponent();
        _apps = AppDiscovery.ListAllApps();
        appScrollView.Content = _apps;
    }
}