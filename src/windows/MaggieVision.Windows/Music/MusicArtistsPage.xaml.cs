using MaggieVision.Windows.Music;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace MaggieVision_Windows.Music;

/// <summary>Artist browse view with working title, letter, sort, and grid/list filters.</summary>
public sealed partial class MusicArtistsPage : Page
{
    private bool _ready;
    private bool _listMode;
    private string? _letter;
    private double _availableWidth;

    public MusicArtistsPage()
    {
        InitializeComponent();
        _ready = true;
        ArtistSummary.Text = $"{DemoMusicCatalogue.AllArtists.Count:N0} demonstration artists  ·  {DemoMusicCatalogue.AllAlbums.Count:N0} albums  ·  {DemoMusicCatalogue.AllTracks.Count:N0} tracks";
        BuildLetterBar();
        Populate();
    }

    public event EventHandler<IReadOnlyList<DemoMusicTrack>>? ArtistSelected;

    private void OnPageSizeChanged(object sender, SizeChangedEventArgs args)
    {
        _availableWidth = args.NewSize.Width;
        if (!_ready) return;
        BuildLetterBar();
        Populate();
    }
    private void OnSearchClick(object sender, RoutedEventArgs args) { SearchBox.Visibility = SearchBox.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; if (SearchBox.Visibility == Visibility.Collapsed) SearchBox.Text = string.Empty; }
    private void OnSearchChanged(object sender, TextChangedEventArgs args) { if (_ready) Populate(); }
    private void OnSortChanged(object sender, SelectionChangedEventArgs args) { if (_ready) Populate(); }
    private void OnViewModeClick(object sender, RoutedEventArgs args) { _listMode = !_listMode; ViewModeGlyph.Glyph = _listMode ? "\uF0E2" : "\uE8A9"; Populate(); }

    private void BuildLetterBar()
    {
        LetterBar.Children.Clear();
        LetterBar.ColumnDefinitions.Clear();
        var letters = new string?[] { null }.Concat("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Select(letter => (string?)letter.ToString())).ToArray();
        var buttonWidth = Math.Max(25, (_availableWidth - 64 - (letters.Length - 1) * 3) / letters.Length);
        for (var index = 0; index < letters.Length; index++)
        {
            LetterBar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            var button = LetterButton(letters[index] ?? "All", letters[index], buttonWidth);
            Grid.SetColumn(button, index);
            LetterBar.Children.Add(button);
        }
    }

    private Button LetterButton(string label, string? letter, double width)
    {
        var button = new Button { Content = label, Padding = new Thickness(3, 5, 3, 5), MinWidth = 22, Width = width };
        AutomationProperties.SetName(button, letter is null ? "All artist names" : $"Artists beginning with {label}");
        button.Click += (_, _) => { _letter = letter; Populate(); };
        return button;
    }

    private void Populate()
    {
        if (!_ready) return;
        var matches = DemoMusicCatalogue.SearchArtists(SearchBox.Text.Trim());
        if (_letter is not null) matches = matches.Where(artist => artist.Name.StartsWith(_letter, StringComparison.OrdinalIgnoreCase)).ToArray();
        matches = SortSelector.SelectedIndex switch
        {
            1 => matches.OrderByDescending(artist => artist.Name, StringComparer.OrdinalIgnoreCase).ToArray(),
            2 => matches.OrderByDescending(artist => artist.Albums.Count).ThenBy(artist => artist.Name, StringComparer.OrdinalIgnoreCase).ToArray(),
            _ => matches.OrderBy(artist => artist.Name, StringComparer.OrdinalIgnoreCase).ToArray()
        };
        ArtistCards.Children.Clear();
        if (matches.Count == 0)
        {
            ArtistCards.Children.Add(new TextBlock { Text = "No matching demonstration artists.", Foreground = (Brush)Application.Current.Resources["ProvisionalSecondaryTextBrush"] });
            return;
        }
        var columns = _listMode ? 1 : Math.Clamp((int)Math.Floor(Math.Max(0, _availableWidth - 64) / 245), 1, 7);
        var grid = new Grid { ColumnSpacing = 16, RowSpacing = 18 };
        for (var column = 0; column < columns; column++) grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        for (var row = 0; row < Math.Ceiling(matches.Count / (double)columns); row++) grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        for (var index = 0; index < matches.Count; index++)
        {
            var artist = matches[index];
            var imageSize = _listMode ? 64 : Math.Clamp((_availableWidth - 64 - (columns - 1) * 16) / columns - 12, 110, 208);
            var stack = new StackPanel { Spacing = 8 };
            stack.Children.Add(new Image
            {
                Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", artist.Albums[0].ArtworkFile))),
                Width = imageSize,
                Height = imageSize,
                Stretch = Stretch.UniformToFill
            });
            stack.Children.Add(new TextBlock { Text = artist.Name, FontSize = 16, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, TextTrimming = TextTrimming.CharacterEllipsis });
            stack.Children.Add(new TextBlock { Text = $"{artist.Albums.Count} album{(artist.Albums.Count == 1 ? "" : "s")} · {artist.TrackCount} tracks · {artist.Genre}", FontSize = 13, Foreground = (Brush)Application.Current.Resources["ProvisionalSecondaryTextBrush"], TextWrapping = TextWrapping.Wrap });
            var button = new Button { Content = stack, HorizontalContentAlignment = HorizontalAlignment.Left, HorizontalAlignment = HorizontalAlignment.Stretch, Background = new SolidColorBrush(Windows.UI.Color.FromArgb(25, 255, 255, 255)), Padding = new Thickness(10) };
            AutomationProperties.SetName(button, $"Play {artist.Name}, {artist.Albums.Count} albums");
            button.Click += (_, _) => ArtistSelected?.Invoke(this, DemoMusicCatalogue.FindArtistTracks(artist.Id));
            Grid.SetColumn(button, index % columns);
            Grid.SetRow(button, index / columns);
            grid.Children.Add(button);
        }
        ArtistCards.Children.Add(grid);
    }
}
