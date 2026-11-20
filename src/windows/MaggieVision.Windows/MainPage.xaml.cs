using MaggieVision.Windows.Navigation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;

namespace MaggieVision_Windows;

/// <summary>Native module-scoped navigation and non-functional page placeholders.</summary>
public sealed partial class MainPage : Page
{
    private readonly NavigationController _navigation = new();
    private readonly NavigationView _shell = new();
    private readonly TextBlock _pageTitle = new();
    private readonly TextBlock _pageDescription = new();
    private readonly ComboBox _modulePicker = new();

    public MainPage()
    {
        InitializeComponent();
        BuildShell();
        RenderNavigation();
    }

    private void BuildShell()
    {
        _shell.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftCompact;
        _shell.IsSettingsVisible = true;
        _shell.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
        _shell.IsPaneToggleButtonVisible = true;
        _shell.SelectionChanged += OnNavigationSelectionChanged;
        _shell.PaneClosing += (_, _) => _navigation.SetPaneCollapsed(true);
        _shell.PaneOpened += (_, _) => _navigation.SetPaneCollapsed(false);
        AutomationProperties.SetName(_shell, "MaggieVision primary navigation");

        _modulePicker.Items.Add(new ComboBoxItem { Content = "Video" });
        _modulePicker.Items.Add(new ComboBoxItem { Content = "Music" });
        _modulePicker.SelectedIndex = 0;
        _modulePicker.SelectionChanged += OnModuleSelectionChanged;
        AutomationProperties.SetName(_modulePicker, "Media module");
        _shell.PaneHeader = _modulePicker;

        var pageContent = new StackPanel { Spacing = 12, MaxWidth = 760, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };
        _pageTitle.FontSize = 28;
        _pageDescription.TextWrapping = TextWrapping.Wrap;
        pageContent.Children.Add(_pageTitle);
        pageContent.Children.Add(_pageDescription);
        pageContent.Children.Add(new InfoBar
        {
            IsOpen = true,
            IsClosable = false,
            Severity = InfoBarSeverity.Informational,
            Title = "Navigation shell",
            Message = "This page is a structural placeholder. No media has been scanned or opened."
        });
        _shell.Content = new Grid { Padding = new Thickness(32), Children = { pageContent } };
        RootGrid.Children.Add(_shell);
    }

    private void OnModuleSelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        if (_modulePicker.SelectedIndex is < 0 or > 1) return;
        _navigation.SwitchModule(_modulePicker.SelectedIndex == 0 ? MediaModule.Video : MediaModule.Music);
        RenderNavigation();
    }

    private void OnNavigationSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            _navigation.SelectSettings();
            ShowPage("Settings", "Application settings will be available here.");
            return;
        }
        if (args.SelectedItemContainer?.Tag is not string destinationId || !_navigation.NavigateTo(destinationId)) return;
        ShowDestination(_navigation.Destinations.First(item => item.Id == destinationId));
    }

    private void RenderNavigation()
    {
        _shell.MenuItems.Clear();
        foreach (var destination in _navigation.Destinations)
        {
            var item = new NavigationViewItem
            {
                Content = destination.Label,
                Tag = destination.Id,
                Icon = new SymbolIcon(ToSymbol(destination.Symbol))
            };
            AutomationProperties.SetName(item, destination.Label);
            _shell.MenuItems.Add(item);
        }
        _shell.SelectedItem = _shell.MenuItems.OfType<NavigationViewItem>()
            .First(item => item.Tag as string == _navigation.State.SelectedDestinationId);
        ShowDestination(_navigation.Destinations.First(item => item.Id == _navigation.State.SelectedDestinationId));
    }

    private void ShowDestination(NavigationDestination destination)
    {
        var moduleName = _navigation.State.Module == MediaModule.Video ? "Video" : "Music";
        ShowPage(destination.Label, $"{moduleName} · {destination.Label}. This view is a navigation placeholder.");
    }

    private void ShowPage(string title, string description)
    {
        _pageTitle.Text = title;
        _pageDescription.Text = description;
    }

    private static Symbol ToSymbol(string symbol) => symbol switch
    {
        "Home" => Symbol.Home,
        "Video" => Symbol.Video,
        "TV" => Symbol.Video,
        "Folder" => Symbol.Folder,
        "Favorite" => Symbol.Favorite,
        "Like" => Symbol.Like,
        "Find" => Symbol.Find,
        "Contact" => Symbol.Contact,
        "MusicInfo" => Symbol.MusicInfo,
        "PlaylistMusic" => Symbol.List,
        "Calendar" => Symbol.Calendar,
        "Filter" => Symbol.Filter,
        "Play" => Symbol.Play,
        _ => Symbol.Library
    };
}
