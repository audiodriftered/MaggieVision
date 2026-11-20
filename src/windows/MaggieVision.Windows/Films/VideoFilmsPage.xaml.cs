using MaggieVision.Windows.Video;
using MaggieVision_Windows.Themes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;

namespace MaggieVision_Windows.Video.Films;

/// <summary>Film browsing surface backed only by deterministic fictional demonstration records.</summary>
public sealed partial class VideoFilmsPage : Page
{
    private readonly DemoFilm _featured = DemoFilmCatalogue.All.Single(film => film.IsFeatured);
    private bool _isReady;
    private bool _isListMode;

    public VideoFilmsPage()
    {
        InitializeComponent();
        FeaturedImage.Source = LoadArtwork(_featured.ArtworkFile);
        FeaturedTitle.Text = _featured.Title;
        FeaturedMetadata.Text = _featured.Metadata;
        FeaturedSynopsis.Text = _featured.Synopsis;

        var genres = DemoFilmCatalogue.All.Where(film => !film.IsFeatured).Select(film => film.Genre).Distinct().OrderBy(value => value);
        foreach (var genre in genres)
        {
            GenreFilter.Items.Add(new ComboBoxItem { Content = genre });
            GenreButtons.Items.Add(CreateGenreButton(genre));
        }

        foreach (var film in DemoFilmCatalogue.All.Where(film => film.IsResumeCandidate))
        {
            ResumeCards.Children.Add(CreateFilmCard(film, isResume: true));
        }

        foreach (var film in DemoFilmCatalogue.All.Where(film => !film.IsFeatured).OrderByDescending(film => film.Year).Take(4))
        {
            PopularCards.Children.Add(CreateFilmCard(film));
        }

        _isReady = true;
        RefreshResults();
    }

    public event EventHandler<DemoFilm>? FilmSelected;

    private static BitmapImage LoadArtwork(string fileName) => new(new Uri(System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", fileName)));

    private Button CreateGenreButton(string genre)
    {
        var button = new Button
        {
            Content = genre,
            Margin = new Thickness(0, 0, 8, 0),
            Padding = new Thickness(14, 9, 14, 9),
            Tag = genre
        };
        AutomationProperties.SetName(button, $"Filter films by {genre}");
        button.Click += OnGenreButtonClick;
        return button;
    }

    private Button CreateFilmCard(DemoFilm film, bool isResume = false)
    {
        if (_isListMode && !isResume)
        {
            return CreateFilmListRow(film);
        }

        var width = CardWidth();
        var height = width * 9d / 16d;
        var content = new Grid();
        var artwork = new Image { Source = LoadArtwork(film.ArtworkFile), Stretch = Stretch.UniformToFill };
        AutomationProperties.SetName(artwork, $"Synthetic demonstration artwork for {film.Title}");
        content.Children.Add(artwork);
        content.Children.Add(new Rectangle
        {
            Height = 58,
            VerticalAlignment = VerticalAlignment.Bottom,
            Fill = new LinearGradientBrush
            {
                StartPoint = new Windows.Foundation.Point(0.5, 0),
                EndPoint = new Windows.Foundation.Point(0.5, 1),
                GradientStops =
                {
                    new GradientStop { Color = Windows.UI.Color.FromArgb(0, 0, 0, 0), Offset = 0 },
                    new GradientStop { Color = Windows.UI.Color.FromArgb(235, 0, 0, 0), Offset = 1 }
                }
            }
        });

        var labels = new StackPanel { VerticalAlignment = VerticalAlignment.Bottom, Spacing = 2, Padding = new Thickness(10, 0, 10, isResume ? 17 : 8) };
        labels.Children.Add(new TextBlock { Text = film.Title, FontSize = 14, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, Foreground = ProvisionalPalette.GetBrush("ProvisionalPrimaryTextBrush", ActualTheme), TextTrimming = TextTrimming.CharacterEllipsis });
        labels.Children.Add(new TextBlock { Text = isResume ? "Resume demonstration · synthetic progress" : film.Metadata, FontSize = 12, Foreground = ProvisionalPalette.GetBrush("ProvisionalPrimaryTextBrush", ActualTheme), TextTrimming = TextTrimming.CharacterEllipsis });
        content.Children.Add(labels);

        if (isResume && film.ResumeProgress is double progress)
        {
            var track = new Grid { Height = 5, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(10, 0, 10, 4), Background = ProvisionalPalette.GetBrush("ProvisionalProgressTrackBrush", ActualTheme) };
            track.Children.Add(new Border
            {
                Width = Math.Max(0, width - 20) * progress,
                HorizontalAlignment = HorizontalAlignment.Left,
                Background = ProvisionalPalette.GetBrush("ProvisionalAccentBrush", ActualTheme)
            });
            content.Children.Add(track);
        }

        var button = new Button
        {
            Width = width,
            Height = height,
            Padding = new Thickness(0),
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
            Background = ProvisionalPalette.GetBrush("ProvisionalCardBrush", ActualTheme),
            BorderBrush = ProvisionalPalette.GetBrush("ProvisionalCardBorderBrush", ActualTheme),
            BorderThickness = new Thickness(1),
            Content = new Border { CornerRadius = new CornerRadius(8), Child = content },
            Tag = film
        };
        AutomationProperties.SetName(button, $"Open film details for {film.Title}");
        button.Click += OnFilmCardClick;
        return button;
    }

    private Button CreateFilmListRow(DemoFilm film)
    {
        var row = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(176) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            }
        };
        row.Children.Add(new Image { Source = LoadArtwork(film.ArtworkFile), Stretch = Stretch.UniformToFill, Width = 160, Height = 90, Margin = new Thickness(8), VerticalAlignment = VerticalAlignment.Center });
        var details = new StackPanel { VerticalAlignment = VerticalAlignment.Center, Spacing = 6, Margin = new Thickness(12, 8, 16, 8) };
        details.Children.Add(new TextBlock { Text = film.Title, FontSize = 18, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, Foreground = ProvisionalPalette.GetBrush("ProvisionalPrimaryTextBrush", ActualTheme) });
        details.Children.Add(new TextBlock { Text = film.Metadata, FontSize = 13, Foreground = ProvisionalPalette.GetBrush("ProvisionalSecondaryTextBrush", ActualTheme) });
        details.Children.Add(new TextBlock { Text = film.Synopsis, FontSize = 13, MaxLines = 2, TextTrimming = TextTrimming.CharacterEllipsis, TextWrapping = TextWrapping.Wrap, Foreground = ProvisionalPalette.GetBrush("ProvisionalPrimaryTextBrush", ActualTheme) });
        Grid.SetColumn(details, 1);
        row.Children.Add(details);

        var button = new Button
        {
            Width = Math.Max(520, FilmsContent.ActualWidth - FilmsContent.Padding.Left - FilmsContent.Padding.Right),
            Height = 122,
            Padding = new Thickness(0),
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
            Background = ProvisionalPalette.GetBrush("ProvisionalCardBrush", ActualTheme),
            BorderBrush = ProvisionalPalette.GetBrush("ProvisionalCardBorderBrush", ActualTheme),
            BorderThickness = new Thickness(1),
            Content = new Border { CornerRadius = new CornerRadius(8), Child = row },
            Tag = film
        };
        AutomationProperties.SetName(button, $"Open film details for {film.Title}");
        button.Click += OnFilmCardClick;
        return button;
    }

    private void RefreshResults()
    {
        var genre = (GenreFilter.SelectedItem as ComboBoxItem)?.Content?.ToString();
        var orderText = (SortFilter.SelectedItem as ComboBoxItem)?.Tag?.ToString();
        var sort = Enum.TryParse<FilmSortOrder>(orderText, out var value) ? value : FilmSortOrder.RecentlyAdded;
        var films = DemoFilmCatalogue.Search(FilmSearch.Text, genre, sort);

        FilmCards.Children.Clear();
        EmptyResults.Visibility = films.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        foreach (var film in films)
        {
            FilmCards.Children.Add(CreateFilmCard(film));
        }

        var gridWidth = CardWidth();
        FilmCards.Orientation = _isListMode ? Orientation.Vertical : Orientation.Horizontal;
        ResultsScroll.HorizontalScrollBarVisibility = _isListMode ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto;
        ResultsScroll.HorizontalScrollMode = _isListMode ? ScrollMode.Disabled : ScrollMode.Enabled;
        foreach (var card in FilmCards.Children.OfType<Button>())
        {
            card.Width = _isListMode ? Math.Max(520, FilmsContent.ActualWidth - FilmsContent.Padding.Left - FilmsContent.Padding.Right) : gridWidth;
            card.Height = _isListMode ? 150 : gridWidth * 9d / 16d;
        }
    }

    private void OnPageSizeChanged(object sender, SizeChangedEventArgs args)
    {
        if (!_isReady) return;

        ResumeCards.Children.Clear();
        foreach (var film in DemoFilmCatalogue.All.Where(film => film.IsResumeCandidate))
        {
            ResumeCards.Children.Add(CreateFilmCard(film, isResume: true));
        }

        PopularCards.Children.Clear();
        foreach (var film in DemoFilmCatalogue.All.Where(film => !film.IsFeatured).OrderByDescending(film => film.Year).Take(4))
        {
            PopularCards.Children.Add(CreateFilmCard(film));
        }

        RefreshResults();
    }

    private void OnFeaturedSizeChanged(object sender, SizeChangedEventArgs args)
    {
        var width = args.NewSize.Width;
        if (width > 0)
        {
            FeaturedPanel.Height = Math.Clamp(width * 0.205, 230, 330);
            FeaturedCopy.Width = Math.Min(680, width * 0.48);
        }
    }

    private void OnGridModeClick(object sender, RoutedEventArgs args)
    {
        _isListMode = false;
        GridModeButton.IsChecked = true;
        ListModeButton.IsChecked = false;
        RefreshResults();
    }

    private void OnListModeClick(object sender, RoutedEventArgs args)
    {
        _isListMode = true;
        GridModeButton.IsChecked = false;
        ListModeButton.IsChecked = true;
        RefreshResults();
    }

    private void OnFilterChanged(object sender, SelectionChangedEventArgs args)
    {
        if (_isReady) RefreshResults();
    }

    private void OnSearchChanged(object sender, TextChangedEventArgs args)
    {
        if (_isReady) RefreshResults();
    }

    private void OnGenreButtonClick(object sender, RoutedEventArgs args)
    {
        var genre = (sender as Button)?.Tag?.ToString();
        GenreFilter.SelectedItem = GenreFilter.Items.OfType<ComboBoxItem>().FirstOrDefault(item => string.Equals(item.Content?.ToString(), genre, StringComparison.OrdinalIgnoreCase));
    }

    private void OnFilmCardClick(object sender, RoutedEventArgs args)
    {
        if ((sender as Button)?.Tag is DemoFilm film) FilmSelected?.Invoke(this, film);
    }

    private void OnFeaturedDetailsClick(object sender, RoutedEventArgs args) => FilmSelected?.Invoke(this, _featured);

    private void OnUnavailableActionClick(object sender, RoutedEventArgs args)
    {
        ActionMessage.Title = "Playback is not implemented";
        ActionMessage.Message = "This screen uses synthetic catalogue entries. No media file was opened.";
        ActionMessage.IsOpen = true;
    }

    private double CardWidth() => Math.Max(210, (Math.Max(0, FilmsContent.ActualWidth - FilmsContent.Padding.Left - FilmsContent.Padding.Right) - 3 * 14) / 4);
}
