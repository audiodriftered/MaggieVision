using Microsoft.UI.Xaml;

namespace MaggieVision_Windows;

/// <summary>Creates the main native window when Windows launches the application.</summary>
public partial class App : Application
{
    private Window? _window;

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = new MainWindow();
        _window.Activate();
    }
}
