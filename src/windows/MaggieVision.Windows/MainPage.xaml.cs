using MaggieVision.Windows.Navigation;
using MaggieVision.Windows.Video;
using MaggieVision.Windows.Music;
using MaggieVision_Windows.Themes;
using MaggieVision_Windows.Music;
using Microsoft.UI.Dispatching;
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
    private readonly MusicPlaybackSession _music = MusicPlaybackSession.Current;
    private bool _updatingMiniProgress;
    private bool _updatingMiniVolume;
    private DispatcherQueueTimer? _playerTimer;

    public MainPage()
    {
        InitializeComponent();
        _shell = ShellNavigation;
        _pageHost = PageHost;
        BuildShell();
        RenderNavigation();
        _music.StateChanged += OnMusicStateChanged;
        _playerTimer = DispatcherQueue.CreateTimer();
        _playerTimer.Interval = TimeSpan.FromMilliseconds(250);
        _playerTimer.Tick += (_, _) => RefreshMusicControls();
        _playerTimer.Start();
    }

    private void BuildShell()
    {
        AutomationProperties.SetName(_shell, "MaggieVision primary navigation");

    }

    public void SwitchModule(MediaModule module)
    {
        _navigation.SwitchModule(module);
        RenderNavigation();
        RefreshMusicControls();
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
        RefreshMusicControls();
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

        if (destination.Id == "video.tv")
        {
            ShowTvProgrammesLanding();
            return;
        }

        if (destination.Id == "music.home")
        {
            ShowMusicHome();
            return;
        }

        if (destination.Id == "music.artists")
        {
            ShowMusicArtists();
            return;
        }

        if (destination.Id == "music.now-playing")
        {
            ShowNowPlaying();
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

    private void ShowTvProgrammesLanding()
    {
        var page = new MaggieVision_Windows.Video.Programmes.TvProgrammesPage();
        page.ProgrammeSelected += OnProgrammeSelected;
        _pageHost.Children.Clear();
        _pageHost.Children.Add(page);
    }

    private void OnProgrammeSelected(object? sender, DemoTvProgramme programme)
    {
        var page = new MaggieVision_Windows.Video.Programmes.ProgrammeDetailsPage(programme);
        page.BackRequested += OnProgrammeDetailsBackRequested;
        page.SeasonSelected += OnSeasonSelected;
        _pageHost.Children.Clear();
        _pageHost.Children.Add(page);
    }

    private void OnProgrammeDetailsBackRequested(object? sender, EventArgs args) => ShowTvProgrammesLanding();

    private void OnSeasonSelected(object? sender, int seasonNumber)
    {
        var programme = (sender as MaggieVision_Windows.Video.Programmes.ProgrammeDetailsPage)?.Programme;
        if (programme is null || DemoTvCatalogue.FindSeason(programme.Id, seasonNumber) is not { } season) return;
        var page = new MaggieVision_Windows.Video.Programmes.SeasonDetailsPage(programme, season);
        page.BackRequested += (_, _) => OnProgrammeSelected(null, programme);
        _pageHost.Children.Clear();
        _pageHost.Children.Add(page);
    }

    private void ShowMusicHome()
    {
        var page = new MusicHomePage();
        page.ArtistsRequested += (_, _) => NavigateTo("music.artists");
        page.TracksRequested += (_, tracks) => { if (tracks.Count > 0) _music.PlayNow(tracks); };
        _pageHost.Children.Clear();
        _pageHost.Children.Add(page);
    }

    private void ShowMusicArtists()
    {
        var page = new MusicArtistsPage();
        page.ArtistSelected += (_, tracks) => { if (tracks.Count > 0) _music.PlayNow(tracks); };
        _pageHost.Children.Clear();
        _pageHost.Children.Add(page);
    }

    private void ShowNowPlaying()
    {
        var page = new MusicNowPlayingPage();
        _pageHost.Children.Clear();
        _pageHost.Children.Add(page);
    }

    private void NavigateTo(string destinationId)
    {
        if (!_navigation.NavigateTo(destinationId)) return;
        _shell.SelectedItem = _shell.MenuItems.OfType<NavigationViewItem>().First(item => item.Tag as string == destinationId);
        ShowDestination(_navigation.Destinations.First(item => item.Id == destinationId));
    }

    private void OnMusicStateChanged(object? sender, EventArgs args) => DispatcherQueue.TryEnqueue(RefreshMusicControls);

    private void RefreshMusicControls()
    {
        MusicMiniPlayer.Visibility = _navigation.State.Module == MediaModule.Music ? Visibility.Visible : Visibility.Collapsed;
        var track = _music.Track;
        MiniTrackTitle.Text = track?.Title ?? "Nothing playing";
        MiniTrackArtist.Text = _music.PlaybackError ?? (track is null ? "Choose a synthetic demo track" : $"{track.Artist}  ·  {track.Album}");
        MiniArtwork.Source = track is null ? null : new Microsoft.UI.Xaml.Media.Imaging.BitmapImage(new Uri(System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", track.ArtworkFile)));
        MiniPlayGlyph.Glyph = _music.IsPlaying ? "\uE769" : "\uE768";
        MiniFavouriteGlyph.Glyph = track is not null && _music.IsFavourite(track) ? "\uEB52" : "\uEB51";
        MiniRepeatGlyph.Glyph = _music.RepeatMode == DemoRepeatMode.Off ? "\uE8EE" : "\uE8ED";
        AutomationProperties.SetName(MiniRepeatGlyph, $"Repeat mode: {_music.RepeatMode}");
        MiniElapsed.Text = FormatTime(_music.Position);
        MiniDuration.Text = FormatTime(_music.Duration);
        _updatingMiniProgress = true;
        MiniProgress.Maximum = Math.Max(1, _music.Duration.TotalSeconds);
        MiniProgress.Value = Math.Clamp(_music.Position.TotalSeconds, MiniProgress.Minimum, MiniProgress.Maximum);
        _updatingMiniProgress = false;
        _updatingMiniVolume = true;
        MiniVolume.Value = _music.Volume;
        _updatingMiniVolume = false;
        if (_pageHost.Children.OfType<MusicNowPlayingPage>().FirstOrDefault() is { } nowPlaying)
            nowPlaying.RefreshPlaybackState();
    }

    private void OnMiniTrackClick(object sender, RoutedEventArgs args) => NavigateTo("music.now-playing");
    private void OnPlayPauseClick(object sender, RoutedEventArgs args) => _music.TogglePlayback();
    private void OnPreviousClick(object sender, RoutedEventArgs args) => _music.Previous();
    private void OnNextClick(object sender, RoutedEventArgs args) => _music.Next();
    private void OnShuffleClick(object sender, RoutedEventArgs args) => _music.ToggleShuffle();
    private void OnRepeatClick(object sender, RoutedEventArgs args) => _music.CycleRepeatMode();
    private void OnMiniFavouriteClick(object sender, RoutedEventArgs args) { if (_music.Track is { } track) _music.ToggleFavourite(track); }
    private void OnQueueClick(object sender, RoutedEventArgs args)
    {
        NavigateTo("music.now-playing");
        if (_pageHost.Children.OfType<MusicNowPlayingPage>().FirstOrDefault() is { } nowPlaying) nowPlaying.OpenQueue();
    }
    private void OnMiniProgressChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs args)
    {
        if (!_updatingMiniProgress && _music.Track is not null) _music.Seek(TimeSpan.FromSeconds(args.NewValue));
    }

    private void OnMiniVolumeChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs args)
    {
        if (!_updatingMiniVolume) _music.Volume = args.NewValue;
    }

    private static string FormatTime(TimeSpan time) => $"{(int)time.TotalMinutes}:{time.Seconds:00}";

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
