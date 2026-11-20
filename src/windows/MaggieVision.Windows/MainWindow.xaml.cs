using Microsoft.UI.Xaml;

namespace MaggieVision_Windows;

/// <summary>Top-level native window hosting the application page frame.</summary>
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        RootFrame.Navigate(typeof(MainPage));
    }
}
