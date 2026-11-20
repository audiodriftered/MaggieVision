using MaggieVision.Windows.Navigation;
using MaggieVision.Windows.Video;
using MaggieVision_Windows.Themes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace MaggieVision_Windows;

/// <summary>Native module-scoped navigation and non-functional page placeholders.</summary>
public sealed partial class MainPage : Page
{
    private readonly NavigationController _navigation = new();
    private readonly NavigationView _shell;
    private readonly Grid _pageHost;

    public MainPage()
    {
        InitializeComponent();
        _shell = ShellNavigation;
        _pageHost = PageHost;
        BuildShell();
        RenderNavigation();
    }

    private void BuildShell()
    {
        AutomationProperties.SetName(_shell, "MaggieVision primary navigation");

    }

    public void SwitchModule(MediaModule module)
    {
        _navigation.SwitchModule(module);
        RenderNavigation();
    }

    public void TogglePane()
    {
        _navigation.TogglePane();
        ApplySidebarLayout();
    }

    private void ApplySidebarLayout()
    {
        var isCollapsed = _navigation.State.IsPaneCollapsed;
        const double compactWidth = 64;
        _shell.CompactPaneLength = compactWidth;
        _shell.PaneDisplayMode = isCollapsed ? NavigationViewPaneDisplayMode.LeftCompact : NavigationViewPaneDisplayMode.Left;
        _shell.IsPaneOpen = !isCollapsed;
    }

    private void OnMainPageSizeChanged(object sender, SizeChangedEventArgs args)
    {
        // Match the supplied desktop reference's roughly 15.5% sidebar while keeping labels readable.
        _shell.OpenPaneLength = Math.Max(248, args.NewSize.Width * 0.155);
    }

    private void OnNavigationSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            _navigation.SelectSettings();
            ShowPlaceholder("Settings", "Application settings will be available here.");
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
        if (destination.Id == "video.home")
        {
            _pageHost.Children.Clear();
            _pageHost.Children.Add(new MaggieVision_Windows.VideoHome.VideoHomePage());
            return;
        }

        if (destination.Id == "video.films")
        {
            ShowFilmsLanding();
            return;
        }

        var moduleName = _navigation.State.Module == MediaModule.Video ? "Video" : "Music";
        ShowPlaceholder(destination.Label, $"{moduleName} · {destination.Label}. This view is a navigation placeholder. No media has been scanned or opened.");
    }

    private void ShowFilmsLanding()
    {
        var page = new MaggieVision_Windows.Video.Films.VideoFilmsPage();
        page.FilmSelected += OnFilmSelected;
        _pageHost.Children.Clear();
        _pageHost.Children.Add(page);
    }

    private void OnFilmSelected(object? sender, DemoFilm film)
    {
        var page = new MaggieVision_Windows.Video.Films.FilmDetailsPage(film);
        page.BackRequested += OnFilmDetailsBackRequested;
        _pageHost.Children.Clear();
        _pageHost.Children.Add(page);
    }

    private void OnFilmDetailsBackRequested(object? sender, EventArgs args) => ShowFilmsLanding();

    private void ShowPlaceholder(string title, string description)
    {
        var panel = new StackPanel { Spacing = 12, MaxWidth = 760, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };
        panel.Children.Add(new TextBlock { Text = title, FontSize = 30, Foreground = ProvisionalPalette.GetBrush("ProvisionalPrimaryTextBrush", ActualTheme) });
        panel.Children.Add(new TextBlock
        {
            Text = description,
            TextWrapping = TextWrapping.Wrap,
            Foreground = ProvisionalPalette.GetBrush("ProvisionalSecondaryTextBrush", ActualTheme)
        });
        panel.Children.Add(new InfoBar
        {
            IsOpen = true,
            IsClosable = false,
            Severity = InfoBarSeverity.Informational,
            Title = "Navigation placeholder",
            Message = "This destination is represented for navigation only. No media has been scanned or opened."
        });

        _pageHost.Children.Clear();
        _pageHost.Children.Add(new ScrollViewer
        {
            Padding = new Thickness(32),
            Content = panel
        });
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
