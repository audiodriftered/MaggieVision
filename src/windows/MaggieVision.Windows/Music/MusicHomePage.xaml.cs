using MaggieVision.Windows.Music;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Media;

namespace MaggieVision_Windows.Music;

/// <summary>Music Home collections backed by fictional in-memory library data.</summary>
public sealed partial class MusicHomePage : Page
{
    private bool _ready;
    private bool _listMode;
    private double _availableWidth;

    public MusicHomePage()
    {
        InitializeComponent();
        _ready = true;
        LibrarySummary.Text = $"{DemoMusicCatalogue.AllTracks.Count:N0} tracks  ·  {DemoMusicCatalogue.AllAlbums.Count:N0} albums  ·  {DemoMusicCatalogue.AllArtists.Count:N0} artists · demonstration library";
        Populate();
    }

    public event EventHandler? ArtistsRequested;
    public event EventHandler<IReadOnlyList<DemoMusicTrack>>? TracksRequested;

    private void OnArtistsClick(object sender, RoutedEventArgs args) => ArtistsRequested?.Invoke(this, EventArgs.Empty);
    private void OnSearchClick(object sender, RoutedEventArgs args) => SearchBox.Visibility = SearchBox.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
    private void OnViewModeClick(object sender, RoutedEventArgs args) { _listMode = !_listMode; ViewModeGlyph.Glyph = _listMode ? "\uF0E2" : "\uE8A9"; Populate(); }
    private void OnSearchChanged(object sender, TextChangedEventArgs args) { if (_ready) Populate(); }
    private void OnPageSizeChanged(object sender, SizeChangedEventArgs args) { _availableWidth = args.NewSize.Width; if (_ready) Populate(); }

    private void Populate()
    {
        if (!_ready) return;
        RowsRoot.Children.Clear();
        var query = SearchBox.Text.Trim();
        var tracks = DemoMusicCatalogue.AllTracks.Where(track => Matches(query, track.Title, track.Artist, track.Album)).ToArray();
        var albums = DemoMusicCatalogue.AllAlbums.Where(album => Matches(query, album.Title, album.Artist)).ToArray();
        AddTrackRow("Recently Played", DemoMusicCatalogue.RecentlyPlayed.Where(track => Matches(query, track.Title, track.Artist, track.Album)).ToArray());
        AddAlbumRow("Recently Added Albums", albums.OrderByDescending(album => album.Year).Take(4).ToArray());
        AddAlbumRow("Favourite Albums", DemoMusicCatalogue.FavouriteAlbums.Where(album => Matches(query, album.Title, album.Artist)).ToArray());
        AddPlaylistRow("Your Playlists", tracks);
    }

    private void AddTrackRow(string title, IReadOnlyList<DemoMusicTrack> tracks)
    {
        var cards = tracks.Select(track => MakeCard(track.Title, track.Artist, track.Album, track.ArtworkFile,
            () => TracksRequested?.Invoke(this, [track]))).ToArray();
        AddSection(title, cards);
    }

    private void AddAlbumRow(string title, IReadOnlyList<DemoMusicAlbum> albums)
    {
        var cards = albums.Select(album => MakeCard(album.Title, album.Artist, $"{album.Tracks.Count} tracks · {album.Year}", album.ArtworkFile,
            () => TracksRequested?.Invoke(this, album.Tracks))).ToArray();
        AddSection(title, cards);
    }

    private void AddPlaylistRow(string title, IReadOnlyList<DemoMusicTrack> tracks)
    {
        var playlists = new[] { ("Evening Notes", 4), ("Road Light", 3), ("Quiet Focus", 5), ("New & Noted", 4) };
        var cards = playlists.Select((playlist, index) => MakeCard(playlist.Item1, "Demonstration playlist", $"{playlist.Item2} tracks",
            DemoMusicCatalogue.AllAlbums[index].ArtworkFile, () => TracksRequested?.Invoke(this, tracks.Take(Math.Min(playlist.Item2, tracks.Count)).ToArray()))).ToArray();
        AddSection(title, cards);
    }

    private void AddSection(string title, IReadOnlyList<FrameworkElement> cards)
    {
        var header = new TextBlock { Text = title, FontSize = 23, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, Margin = new Thickness(0, 6, 0, 2) };
        RowsRoot.Children.Add(header);
        if (cards.Count == 0)
        {
            RowsRoot.Children.Add(new TextBlock { Text = "No matching demonstration items.", Foreground = (Brush)Application.Current.Resources["ProvisionalSecondaryTextBrush"] });
            return;
        }

        var columns = _listMode ? 1 : Math.Clamp((int)Math.Floor(Math.Max(0, _availableWidth - 64) / 520), 1, 4);
        var rows = (int)Math.Ceiling(cards.Count / (double)columns);
        var grid = new Grid { ColumnSpacing = 16, RowSpacing = 10 };
        for (var column = 0; column < columns; column++) grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        for (var row = 0; row < rows; row++) grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        for (var index = 0; index < cards.Count; index++)
        {
            Grid.SetColumn(cards[index], index % columns);
            Grid.SetRow(cards[index], index / columns);
            grid.Children.Add(cards[index]);
        }
        RowsRoot.Children.Add(grid);
    }

    private Button MakeCard(string title, string subtitle, string detail, string artwork, Action invoke)
    {
        var imageSize = _listMode ? 54 : Math.Clamp((_availableWidth - 112) / Math.Max(1, Math.Min(4, (int)Math.Floor(Math.Max(0, _availableWidth - 64) / 520))) - 154, 92, 154);
        var content = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 14, VerticalAlignment = VerticalAlignment.Center };
        content.Children.Add(new Image
        {
            Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", artwork))),
            Width = imageSize,
            Height = imageSize,
            Stretch = Stretch.UniformToFill
        });
        var labels = new StackPanel { VerticalAlignment = VerticalAlignment.Center, Spacing = 5 };
        labels.Children.Add(new TextBlock { Text = title, FontSize = 16, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, TextTrimming = TextTrimming.CharacterEllipsis });
        labels.Children.Add(new TextBlock { Text = subtitle, Foreground = (Brush)Application.Current.Resources["ProvisionalSecondaryTextBrush"], TextTrimming = TextTrimming.CharacterEllipsis });
        labels.Children.Add(new TextBlock { Text = detail, FontSize = 13, Foreground = (Brush)Application.Current.Resources["ProvisionalSecondaryTextBrush"], TextTrimming = TextTrimming.CharacterEllipsis });
        content.Children.Add(labels);
        var button = new Button
        {
            Content = content,
            Padding = new Thickness(10),
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Background = new SolidColorBrush(Windows.UI.Color.FromArgb(30, 255, 255, 255)),
            BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(80, 180, 205, 230)),
            BorderThickness = new Thickness(1),
            Margin = new Thickness(0, 0, 0, 2)
        };
        AutomationProperties.SetName(button, $"Play {title} by {subtitle}");
        button.Click += (_, _) => invoke();
        return button;
    }

    private static bool Matches(string query, params string[] values) => string.IsNullOrEmpty(query) || values.Any(value => value.Contains(query, StringComparison.OrdinalIgnoreCase));
}
