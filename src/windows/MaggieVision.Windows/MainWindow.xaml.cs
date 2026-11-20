using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Controls.Primitives;
using MaggieVision_Windows.Themes;
using MaggieVision.Windows.Navigation;

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
        UpdateModuleSwitchStyle();
    }

    private void OnNavigationToggleClick(object sender, RoutedEventArgs args)
    {
        if (RootFrame.Content is MainPage page)
        {
            page.TogglePane();
        }
    }

    private void OnModuleSelectionChanged(object sender, RoutedEventArgs args)
    {
        if (sender is not ToggleButton selector || selector.IsChecked != true) return;
        if (VideoModuleSelector is null || MusicModuleSelector is null) return;
        VideoModuleSelector.IsChecked = selector == VideoModuleSelector;
        MusicModuleSelector.IsChecked = selector == MusicModuleSelector;
        UpdateModuleSwitchStyle();
        if (RootFrame.Content is MainPage page)
        {
            page.SwitchModule(selector == MusicModuleSelector ? MediaModule.Music : MediaModule.Video);
        }
    }

    private void UpdateModuleSwitchStyle()
    {
        var theme = AppTitleBar.ActualTheme;
        var accent = ProvisionalPalette.GetBrush("ProvisionalAccentStrongBrush", theme);
        var header = ProvisionalPalette.GetBrush("ProvisionalHeaderBrush", theme);
        var selectedBorder = ProvisionalPalette.GetBrush("ProvisionalAccentBrush", theme);
        VideoSegmentSurface.Background = VideoModuleSelector.IsChecked == true ? accent : header;
        MusicSegmentSurface.Background = MusicModuleSelector.IsChecked == true ? accent : header;
        VideoSegmentSurface.BorderBrush = selectedBorder;
        MusicSegmentSurface.BorderBrush = selectedBorder;
        VideoSegmentSurface.BorderThickness = new Thickness(VideoModuleSelector.IsChecked == true ? 1 : 0);
        MusicSegmentSurface.BorderThickness = new Thickness(MusicModuleSelector.IsChecked == true ? 1 : 0);
    }
}
