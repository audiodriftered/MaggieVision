using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;
using MaggieVision_Windows.Themes;

namespace MaggieVision_Windows.VideoHome;

/// <summary>Native Video Home composition populated only with fictional demonstration content.</summary>
public sealed partial class VideoHomePage : Page
{
    public VideoHomePage()
    {
        InitializeComponent();
        FeaturedImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", "demo-featured-amber-horizon.png")));
        PopulateCards(ContinueWatchingCards, ContinueWatching);
        PopulateCards(RecentFilmCards, RecentFilms);
        PopulateCards(RecentProgrammeCards, RecentProgrammes);
    }

    private static readonly DemoCard[] ContinueWatching =
    [
        new("Coastal Echoes", "Film  ·  1 h 12 min left", 0, 0.58),
        new("Silver District", "S1 E4  ·  18 min left", 1, 0.61),
        new("The Northern Range", "Film  ·  46 min left", 2, 0.22),
        new("Garden of Stone", "S1 E2  ·  27 min left", 3, 0.48),
        new("Orbit 7", "Film  ·  1 h 34 min left", 4, 0.15)
    ];

    private static readonly DemoCard[] RecentFilms =
    [
        new("Midnight Valley", "2024  ·  Thriller  ·  1 h 54 min", 5, null, "4K"),
        new("Riverside Summer", "2024  ·  Drama  ·  1 h 46 min", 6, null, "4K"),
        new("The Last Signal", "2023  ·  Sci-Fi  ·  2 h 1 min", 7, null, "4K  HDR"),
        new("Map of Tomorrow", "2024  ·  Sci-Fi  ·  1 h 56 min", 8, null, "4K"),
        new("Hidden Harbour", "2023  ·  Mystery  ·  1 h 42 min", 9, null, "4K")
    ];

    private static readonly DemoCard[] RecentProgrammes =
    [
        new("Evergreen", "Season 1  ·  8 Episodes", 10),
        new("Cityline", "Season 2  ·  10 Episodes", 11),
        new("Frontier Road", "Season 1  ·  6 Episodes", 12),
        new("The Institute", "Season 1  ·  8 Episodes", 13),
        new("Brightwater", "Season 1  ·  6 Episodes", 14)
    ];

    private void OnContinueWatchingNextClick(object sender, RoutedEventArgs args) => ScrollRowToNextCard(ContinueWatchingScrollViewer);

    private void OnRecentFilmsNextClick(object sender, RoutedEventArgs args) => ScrollRowToNextCard(RecentFilmsScrollViewer);

    private void OnRecentProgrammesNextClick(object sender, RoutedEventArgs args) => ScrollRowToNextCard(RecentProgrammesScrollViewer);

    private static void ScrollRowToNextCard(ScrollViewer row)
    {
        var nextOffset = Math.Min(row.HorizontalOffset + row.ViewportWidth * 0.8, row.ScrollableWidth);
        row.ChangeView(nextOffset, null, null);
    }

    private void PopulateCards(Panel row, IReadOnlyList<DemoCard> cards)
    {
        foreach (var card in cards)
        {
            row.Children.Add(CreateCard(card));
        }
    }

    private FrameworkElement CreateCard(DemoCard card)
    {
        var foreground = GetThemeBrush("ProvisionalPrimaryTextBrush");
        var cardBorder = new Border
        {
            Width = 252,
            Height = 139,
            CornerRadius = new CornerRadius(10),
            BorderThickness = new Thickness(1),
            BorderBrush = GetThemeBrush("ProvisionalCardBorderBrush"),
            Background = GetThemeBrush("ProvisionalCardBrush"),
            Margin = new Thickness(0, 0, 0, 1),
            Tag = card.ArtworkIndex
        };

        var content = new Grid();
        var artworkPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", $"demo-video-{card.ArtworkIndex:D2}.png");
        var artwork = new Image
        {
            Source = new BitmapImage(new Uri(artworkPath)),
            Stretch = Stretch.UniformToFill,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };
        AutomationProperties.SetName(artwork, $"Demonstration artwork for {card.Title}");
        content.Children.Add(artwork);
        var shade = new Rectangle
        {
            VerticalAlignment = VerticalAlignment.Bottom,
            Height = 66,
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
        };
        content.Children.Add(shade);

        var playMark = new Border
        {
            Width = 28,
            Height = 25,
            CornerRadius = new CornerRadius(4),
            Background = new SolidColorBrush(Windows.UI.Color.FromArgb(210, 8, 25, 41)),
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 7, 7, 0),
            Child = new TextBlock { Text = "▶", FontSize = 13, Foreground = foreground, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center }
        };
        content.Children.Add(playMark);

        var labels = new StackPanel { VerticalAlignment = VerticalAlignment.Bottom, Spacing = 2, Padding = new Thickness(11, 0, 10, card.Progress is null ? 8 : 17) };
        labels.Children.Add(new TextBlock { Text = card.Title, FontSize = 14, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, Foreground = foreground, TextTrimming = TextTrimming.CharacterEllipsis });
        labels.Children.Add(new TextBlock { Text = card.Subtitle, FontSize = 12, Foreground = foreground, TextTrimming = TextTrimming.CharacterEllipsis });
        content.Children.Add(labels);

        if (card.Progress is double progress)
        {
            var progressTrack = new Grid { Height = 5, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(11, 0, 11, 4), Background = GetThemeBrush("ProvisionalProgressTrackBrush") };
            progressTrack.Children.Add(new Border
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 230 * progress,
                Background = GetThemeBrush("ProvisionalAccentBrush")
            });
            content.Children.Add(progressTrack);
        }
        else if (!string.IsNullOrWhiteSpace(card.Badge))
        {
            var badge = new Border
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 7, 7),
                Padding = new Thickness(5, 1, 5, 1),
                CornerRadius = new CornerRadius(3),
                BorderThickness = new Thickness(1),
                BorderBrush = GetThemeBrush("ProvisionalBadgeBorderBrush"),
                Child = new TextBlock { Text = card.Badge, FontSize = 10, FontWeight = Microsoft.UI.Text.FontWeights.Bold, Foreground = GetThemeBrush("ProvisionalBadgeTextBrush") }
            };
            content.Children.Add(badge);
        }

        cardBorder.Child = content;
        return cardBorder;
    }

    private Brush GetThemeBrush(string key)
    {
        return ProvisionalPalette.GetBrush(key, ActualTheme);
    }

    private void OnHomeContentSizeChanged(object sender, SizeChangedEventArgs args)
    {
        var availableWidth = Math.Max(0, args.NewSize.Width - HomeContent.Padding.Left - HomeContent.Padding.Right);
        if (availableWidth <= 0)
        {
            return;
        }

        var scale = Math.Clamp(availableWidth / 1370d, 1d, 1.8d);
        FeaturedPanel.Height = Math.Max(278, availableWidth * (278d / 1370d));
        FeaturedCopy.Width = Math.Min(900, Math.Max(570, availableWidth * 0.39));
        FeaturedTitle.FontSize = 38 * scale;
        FeaturedMetadata.FontSize = 14 * scale;
        FeaturedDescription.FontSize = 15 * scale;

        var cardWidth = Math.Max(220, (availableWidth - 3 * 14) / 4);
        foreach (var row in new[] { ContinueWatchingCards, RecentFilmCards, RecentProgrammeCards })
        {
            foreach (var card in row.Children.OfType<Border>())
            {
                card.Width = cardWidth;
                card.Height = cardWidth * 9d / 16d;
                if (card.Child is Grid panel)
                {
                    foreach (var text in FindTextBlocks(panel))
                    {
                        text.FontSize = text.Tag is double baseFontSize ? baseFontSize * scale : text.FontSize;
                        text.Tag ??= text.FontSize;
                    }
                }
            }
        }
    }

    private static IEnumerable<TextBlock> FindTextBlocks(DependencyObject parent)
    {
        var count = VisualTreeHelper.GetChildrenCount(parent);
        for (var index = 0; index < count; index++)
        {
            var child = VisualTreeHelper.GetChild(parent, index);
            if (child is TextBlock text)
            {
                yield return text;
            }
            foreach (var descendant in FindTextBlocks(child))
            {
                yield return descendant;
            }
        }
    }

    private sealed record DemoCard(string Title, string Subtitle, int ArtworkIndex, double? Progress = null, string? Badge = null);
}
